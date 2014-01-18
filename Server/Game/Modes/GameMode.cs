using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsNetLib2;

namespace AppsAgainstHumanity.Server.Game.Modes
{
    /// <summary>
    /// An abstract class from which game mode implementations should inherit.
    /// </summary>
    public abstract class GameMode
    {
        /// <summary>
        /// The instance of a Game class to which this game-mode instance belongs.
        /// </summary>
        protected internal virtual Game Parent { get; set; }
        /// <summary>
        /// Whether the handler for PICK commands is currently
        /// allowed to operate. False by default.
        /// </summary>
        protected internal bool AllowPicks { get; set; }
        /// <summary>
        /// Whether the handler for CZPK commands should be
        /// allowed to execute. Recommended, but not required,
        /// that CZPK handlers implement support for this.
        /// </summary>
        protected internal bool AllowCzpk { get; set; }
        /// <summary>
        /// Informs each player currently in the game of the points
        /// held by every other player.
        /// </summary>
        protected virtual void InformPlayersPoints()
        {
            var players = this.Players.ToList();
            foreach (Player p in players)
            {
                foreach (Player pl in players)
                {
                    this.Parent.SendCommand(
                        CommandType.PNTS,
                        new string[2] { pl.Nickname, pl.AwesomePoints.ToString() },
                        p.ClientIdentifier
                    );
                }
            }
        }
        /// <summary>
        /// Pulls cards from the parent instance of Game's
        /// pool for use in the current round.
        /// </summary>
        protected virtual Dictionary<int, WhiteCard> CreateCardPool()
        {
            // The generator which will be used to select random cards
            // from our pools. Seeded with bytes from a CSPRNG.
            Random rng = new Random(new Crypto.SRand());
            // The dictionary which will contain our final pool of cards.
            Dictionary<int, WhiteCard> pool = new Dictionary<int, WhiteCard>();

            // First, we'll add to our pool the number of cards to be sent to
            // each player at the beginning of the round. This is determined
            // by the Draw number of the black card which will be played.
            for (int i = 0; i < this.BlackCard.Draw * this.Players.Count; i++)
            {
                // Select a card from the white card pool of the parent Game instance.
                // Since indices are zero-based, we have to subtract one from the total
                // count as, if we did not do this, an IndexOutOfRangeException could
                // occur.
                KeyValuePair<int, WhiteCard> wc = this.Parent.WhiteCardPool
                    .ElementAt(
                        rng.Next(this.Parent.WhiteCardPool.Count - 1)
                    );
                // We then remove the card from the pool. This prevents the card being
                // dealt to two people.
                this.Parent.WhiteCardPool.Remove(wc.Key);
                // The card is then added to our pool. The pool will then be returned at
                // the end of the function.
                pool.Add(wc.Key, wc.Value);
            }

            // We return the pool that we created.
            return pool;
        }
        /// <summary>
        /// Distributes cards from the pool to players.
        /// </summary>
        protected virtual void DistributeCards()
        {
            // If there are no cards to distribute, throw an exception
            // suggesting the likely reasons for this.
            if (this.WhiteCards.Count <= 0)
                throw new InvalidOperationException
                ("Cards already distributed or pool depleted.");

            foreach (Player p in this.Players.ToList())
            {
                Random rng = new Random(new Crypto.SRand());
                for (int i = 0; i < this.BlackCard.Draw; i++)
                {
                    // Select a random white card from the pool to send to the player.
                    KeyValuePair<int, WhiteCard> wc = this.WhiteCards.ElementAt(rng.Next(0, this.WhiteCards.Count - 1));
                    // Remove the white card from our pool so it cannot be dealt to another player.
                    this.WhiteCards.Remove(wc.Key);
                    // Add the selected card to the player's list of drawn cards.
                    this.Parent.DrawnCards[p].Add(wc.Key, wc.Value);
                    // Send the contents of the card, along with its identifier, to the player.
                    this.Parent.SendCommand(
                        CommandType.WHTE,
                        new string[2] { wc.Key.ToString(), wc.Value.Text },
                        p.ClientIdentifier
                    );
                }
            }
        }
        /// <summary>
        /// Handler for when a player is disconnected.
        /// </summary>
        /// <param name="p">The disconnected player.</param>
        protected virtual void OnPlayerDisconnectedHandler(Player p)
        {
            foreach (Player pl in Players.ToList())
            {
                // Inform each player that another player has disconnected.
                this.Parent.SendCommand(
                    CommandType.CLEX,
                    String.Format(
                        "Player {0} disconnected.",
                        p.Nickname
                    ),
                    pl.ClientIdentifier
                );

                // Inform each player that the round ended without a winner.
                this.Parent.SendCommand(
                    CommandType.RWIN,
                    (string[])null,
                    pl.ClientIdentifier
                );
            }

            // The round cannot continue in this state, so we inform
            // players that the round will be ending.
            this.Parent.Broadcast("Player disconnected mid-round. Terminating round.");

            this.Stop();
        }

        /// <summary>
        /// Initialise an instance of GameMode with a parent Game instance.
        /// </summary>
        /// <param name="parent">The Game instance to treat as a parent.</param>
        public GameMode(Game parent)
        {
            this.Parent = parent;
            this.Parent.OnPlayerDisconnected += OnPlayerDisconnectedHandler;
            // Remove any previous handlers
            this.Parent._serverWrapper.UnregisterCommandHandler(CommandType.PICK);
            this.Parent._serverWrapper.UnregisterCommandHandler(CommandType.CZPK);
            // Bind our own handlers
            this.Parent._serverWrapper.RegisterCommandHandler(CommandType.PICK, this.CommandPickHandler);
            this.Parent._serverWrapper.RegisterCommandHandler(CommandType.CZPK, this.CommandCzpkHandler);

            this.Players = parent.Players;
            this.BlackCard = parent.CurrentRound.BlackCard;
            this.WhiteCards = this.CreateCardPool();
            this.AllowPicks = false;

            this.PickedList = new Dictionary<Player, bool>();
            this.PlayedCards = new Dictionary<Player, Dictionary<int, WhiteCard>>();
            this.Gamblers = new List<Player>();

            foreach (Player p in this.Players.ToList())
            {
                this.PickedList.Add(p, false);
                this.PlayedCards.Add(p, new Dictionary<int, WhiteCard>());
            }

            if (this.Parent.CurrentRound.DrawNewWhites)
                this.DistributeCards();
        }

        /// <summary>
        /// The name of the represented game mode.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// A list of players which the implementation can modify. Will
        /// be copied back to the game's dictionary at the cessation of
        /// a round.
        /// </summary>
        public virtual List<Player> Players { get; internal set; }
        /// <summary>
        /// A dictionary containing all available white cards. Copied
        /// back to the game's pool at the end of a round.
        /// </summary>
        public virtual Dictionary<int, WhiteCard> WhiteCards { get; internal set; }
        /// <summary>
        /// The list of currently available black cards. Copied back
        /// into the game's pool when the round ends.
        /// </summary>
        public virtual BlackCard BlackCard { get; internal set; }
        /// <summary>
        /// A list of players and associated boolean values indicating
        /// whether the player has currently submitted the required number
        /// of white card picks.
        /// </summary>
        public virtual Dictionary<Player, bool> PickedList { get; set; }
        /// <summary>
        /// A list of players who have gambled in order to play additional cards.
        /// </summary>
        public virtual List<Player> Gamblers { get; set; }
        /// <summary>
        /// Dictionary containing a dictionary of cards played by each player.
        /// Card dictionary is keyed using the card's unique identifier, as
        /// generated at the beginning of the game.
        /// </summary>
        public virtual Dictionary<Player, Dictionary<int, WhiteCard>> PlayedCards { get; internal set; }

        /// <summary>
        /// Fired when a player wins the round.
        /// </summary>
        public abstract event Game.PlayerCardEventHandler OnPlayerWin;

        /// <summary>
        /// This game mode's handler for PICK commands.
        /// </summary>
        /// <param name="sender">The Client ID of the client sending the command.</param>
        /// <param name="arguments">Any arguments sent by the client with the command.</param>
        public virtual void CommandPickHandler(long sender, string[] arguments)
        {
            // If this array is null, it indicates that the client sent the command
            // without any arguments. Since the PICK command requires arguments,
            // this is invalid.
            // We don't check for the number of arguments provided, as the client is
            // free to provide its user's picks over a series of PICK commands rather
            // than as a single command.
            if (arguments == null)
            {
                // As this is invalid, we return an UNRG (unrecognised) to the client,
                // and exit out of the function. No further action is taken.
                this.Parent.SendCommand(
                    CommandType.UNRG,
                    "PICK received without arguments; invalid.",
                    sender
                );
                return;
            }

            // The variable _allowPicks being set to true indicates that players can
            // currently submit cards. If this is false, we should not accept picks
            // sent in by players.
            if (this.AllowPicks)
            {
                // The sender of the PICK command, as a Player class rather than
                // their numeric identifier.
                Player pSender = this.Players.Single(pl => pl.ClientIdentifier == sender);
                List<int> pSenderPicks = new List<int>();

                // Parse the IDs sent by the client into integers, which will
                // later be used to try and retrieve cards from the list of
                // drawn cards.
                // Note: Due to checks being performed in a loop, multiple
                //       invalid cards will result in multiple error messages
                //       being sent back to the client.
                foreach (string id in arguments)
                {
                    int tempAdd = 0;
                    if (int.TryParse(id, out tempAdd))
                    {
                        pSenderPicks.Add(tempAdd);
                    }
                }

                foreach (int i in pSenderPicks.ToList())
                {
                    // Before we accept the client's PICK, we'll check if the ID
                    // sent exists within the player represented by the client's
                    // hand. If it does, we can accept it and move on. If it doesn't,
                    // the server will report, via UNRG, that they do not have
                    // the card they tried to PICK.
                    if (this.Parent.DrawnCards[pSender].ContainsKey(i))
                    {
                        // If the player has not picked the number of cards
                        // required for this round, add their pick to the
                        // dictionary containing drawn cards.
                        if (!this.PickedList[pSender])
                        {
                            // Add the card the player played to the
                            // dictionary containing the cards they have
                            // played.
                            this.PlayedCards[pSender].Add(i, this.Parent.DrawnCards[pSender][i]);
                            // As the player no longer has the card in their "hand,"
                            // we'll remove it from the list of drawn cards to prevent
                            // the card being played again.
                            this.Parent.DrawnCards[pSender].Remove(i);
                            // If the player has played the required number of cards,
                            // we set the value in the dictionary indicating as much to
                            // true. As this code will not be executed during gambling,
                            // we don't need to perform any checks for it.
                            if (this.PlayedCards[pSender].Count == this.BlackCard.Pick)
                                this.PickedList[pSender] = true;

                            // We now select every player who is /not/ the player who just sent a
                            // PICK command, and we inform them that another player has submitted
                            // a choice of card via the BLNK command. Upon receiving this command,
                            // clients should show some indication of another player having played.
                            foreach (Player p in this.Players.Where(pl => pl != pSender).ToList())
                            {
                                this.Parent.SendCommand(
                                    CommandType.BLNK,
                                    (string[])null,
                                    p.ClientIdentifier
                                );
                            }
                        }
                        // This code block handles gambling by players. Won't be executed
                        // Unless gambling is enabled and the player has played all cards
                        // required of them.
                        else if (this.PickedList[pSender] && this.Parent.Parameters.AllowGambling)
                        {
                            // If the player has more than zero Awesome Points, they will be able
                            // to gamble. This was implemented in this manner as I doubt negative
                            // Awesome Points, whilst they would work, are really the best choice.
                            if (pSender.AwesomePoints > 0)
                            {
                                // Players are allowed to gamble only once per round, so if the player
                                // is already within the list, they should be prevented from gambling
                                // once again.
                                if (this.Gamblers.Contains(pSender))
                                {
                                    // Inform the player that they have already gambled this round,
                                    // and so their gamble has not been accepted.
                                    this.Parent.SendCommand(
                                        CommandType.UNRG,
                                        "You have already gambled this round.",
                                        pSender.ClientIdentifier
                                    );

                                    // Return the played cards to the sender, as they were
                                    // not accepted.
                                    this.Parent.SendCommand(
                                        CommandType.WHTE,
                                        new string[2] { i.ToString(), pSender.DrawnCards[i].Text },
                                        pSender.ClientIdentifier
                                    );
                                }
                                else
                                {
                                    // Add the card the player's gambled to the list of drawn cards.
                                    this.PlayedCards[pSender].Add(i, this.Parent.DrawnCards[pSender][i]);
                                    // Remove the card from the player's hand since they've just played
                                    // it and therefore no longer have it drawn.
                                    this.Parent.DrawnCards[pSender].Remove(i);
                                    // As players must spend an Awesome Point to gamble, we'll subtract
                                    // one from their current total, provided it is greater than zero
                                    // (this particular condition was checked in the above conditional).
                                    pSender.AwesomePoints--;
                                    // We'll then add the player to our list of gamblers. At the end of
                                    // the round, this list will be checked to determine whether the
                                    // winning player is present within it. If they are, they will be
                                    // returned the Awesome Point they gambled.
                                    this.Gamblers.Add(pSender);
                                    // Once again, we inform all other players that a choice of card
                                    // has been made, via the BLNK command.
                                    foreach (Player p in this.Players.Where(p => p != pSender).ToList())
                                    {
                                        this.Parent.SendCommand(
                                            CommandType.BLNK,
                                            (string[])null,
                                            p.ClientIdentifier
                                        );
                                    }
                                    // We inform other players that a gamble has been made via a broadcast.
                                    // We won't, however, send out any commands to tell clients to update
                                    // the number of points they are showing for this player.
                                    this.Parent.Broadcast(
                                        String.Format(
                                            "Player {0} has gambled an Awesome Point in order to play another card!",
                                            pSender.Nickname
                                        )
                                    );

                                    return;
                                }
                            }
                            else
                            {
                                // First, we inform the user that they have too few Awesome Points
                                // to gamble any, via UNRG.
                                this.Parent.SendCommand(
                                    CommandType.UNRG,
                                    "You have too few Awesome Points to gamble.",
                                    sender
                                );
                                // We then return the card to this player via a WHTE command. As the
                                // card is never removed from any of the server's dictionaries, no
                                // further action must be taken.
                                this.Parent.SendCommand(
                                    CommandType.WHTE,
                                    new string[2] {
                                        i.ToString(),
                                        this.Parent.DrawnCards[pSender][i].Text
                                    },
                                    sender
                                );
                                return;
                            }
                        }
                        // If the player has already played all cards required of them,
                        // and if gambling is disabled by the server, this block of
                        // code will be executed.
                        else
                        {
                            // The client is informed, via UNRG, that gambling is disabled.
                            this.Parent.SendCommand(
                                CommandType.UNRG,
                                "Gambling is disabled on this server.",
                                sender
                            );
                            // The client is then returned their white card, and the function
                            // is exited.
                            this.Parent.SendCommand(
                                CommandType.WHTE,
                                new string[2] {
                                        i.ToString(),
                                        this.Parent.DrawnCards[pSender][i].Text
                                    },
                                sender
                            );
                            return;
                        }
                    }
                    else
                    {
                        // Informs the client that they card they tried to play
                        // was not present in their hand, and so the PICK was not
                        // accepted by the server.
                        this.Parent.SendCommand(
                            CommandType.UNRG,
                            String.Format(
                                "You do not have the card {0}.",
                                i
                            ),
                            sender
                        );
                        return;
                    }
                }
            }
            else
            {
                foreach (string id in arguments)
                {
                    int cardId = 0;
                    // We have to determine whether the client actually sent
                    // us valid card identifiers first. If it has, the identifier
                    // should be able to be parsed into an int.
                    if (int.TryParse(id, out cardId))
                    {
                        // Since there's no confirmation of whether a PICK was successful in the networking spec,
                        // it is reasonable to assume that a client will cease to display the card sent, even if
                        // said card was sent outwith the time period it should have been sent in. Since we're
                        // making that assumption, we will have to confirm the existence of the card which was
                        // sent within the player's hand, which is the condition here.
                        if (this.Parent.DrawnCards.Single(pl => pl.Key.ClientIdentifier == sender).Value.ContainsKey(cardId))
                        {
                            // If the card did, in fact, exist, we'll send it back to the player and then continue
                            // with the loop.
                            this.Parent.SendCommand(
                                CommandType.WHTE,
                                new string[2] { 
                                    cardId.ToString(),
                                    // Rather ugly, but this little statement here selects the list of cards drawn by the
                                    // player who sent the PICK, and then selects the white card with the ID sent by the
                                    // client, and retrieves the property Card.Text.
                                    this.Parent.DrawnCards.Single(pl => pl.Key.ClientIdentifier == sender).Value[cardId].Text
                                },
                                sender
                            );
                            continue;
                        }
                        else continue;
                    }
                    // If they haven't sent a valid identifier, there isn't anything
                    // we can do about that, so we'll iterate on the arguments sent
                    // and check any further arguments.
                    // Since we aren't accepting PICKs at this time, we won't reply
                    // with an UNRG, as that would imply the operation would have
                    // otherwise succeeded had the identifier been valid.
                    else continue;
                }
            }
        }
        /// <summary>
        /// This game mode's handler for CZPK commands.
        /// </summary>
        /// <param name="sender">The Client ID of the client sending the command.</param>
        /// <param name="arguments">Any arguments sent by the client with the command.</param>
        public abstract void CommandCzpkHandler(long sender, string[] arguments);
        /// <summary>
        /// Starts a round of the specified game mode.
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// Forces the current game-mode to stop.
        /// </summary>
        public virtual void Stop()
        {
            this.Parent._serverWrapper.UnregisterCommandHandler(CommandType.PICK);
            this.Parent._serverWrapper.UnregisterCommandHandler(CommandType.CZPK);
        }
    }
}
