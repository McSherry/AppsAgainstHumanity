using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CsNetLib2;

namespace AppsAgainstHumanity.Server.Game
{
    public class Round
    {
        // Whether new white cards should be given to players.
        // May be disabled if a round is skipped and played
        // cards are returned to players.
        private bool _drawNewWhites;
        private Random _cardSelectorRNG;
        /// <summary>
        /// Generates a random number within the bounds of the white card pool's length.
        /// </summary>
        private int _cardSelectorGenerate()
        {
            return _cardSelectorRNG.Next(this.WhiteCardPool.Count - 1);
        }
        /// <summary>
        /// The game this round is a part of.
        /// </summary>
        private Game _parent;

        /// <summary>
        /// Create a new Round.
        /// </summary>
        /// <param name="blackCard">The black card to play this round.</param>
        /// <param name="pool">The pool of white cards to use this round.</param>
        /// <param name="cardCzar">The player chosen as Card Czar for this round.</param>
        /// <param name="parent">The instance of game which instantiated this round.</param>
        /// <param name="drawNewWhites">Whether to deal new white cards to players. True by default.</param>
        public Round(
            BlackCard blackCard,
            Dictionary<int, WhiteCard> pool,
            Game parent,
            Player cardCzar,
            bool drawNewWhites = true
            ) 
        {
            this.BlackCard = blackCard;
            this.WhiteCardPool = pool;
            this.RoundSeed = new Crypto.SRand();
            this._parent = parent;
            this.CardCzar = cardCzar;

            this._drawNewWhites = drawNewWhites;
            this._cardSelectorRNG = new Random((int)this.RoundSeed);
            this.HasPlayedList = new Dictionary<Player, bool>();
            this.PlayedCards = new Dictionary<Player, Dictionary<int, WhiteCard>>();

            foreach (Player p in Players.ToList())
            {
                // If the player is this iteration is the Czar, we won't need
                // to set up a place for them in dictionaries, as they won't
                // be playing any cards.
                if (p == cardCzar) continue;
                this.HasPlayedList.Add(p, false);
                this.PlayedCards.Add(p, new Dictionary<int, WhiteCard>());
                /* TODO:
                 * 1. Send the black card for this round to each player
                 * 2. Use NetLib wrapper's "BLCK" command.
                 */
            }
        }

        /// <summary>
        /// The black card that was played this round.
        /// </summary>
        public BlackCard BlackCard { get; private set; }
        /// <summary>
        /// The list of currently connected players.
        /// </summary>
        public List<Player> Players
        {
            get { return _parent.Players; }
        }
        /// <summary>
        /// The Card Czar for this round.
        /// </summary>
        public Player CardCzar { get; private set; }
        /// <summary>
        /// The white cards available to send to players this round.
        /// </summary>
        public Dictionary<int, WhiteCard> WhiteCardPool { get; private set; }

        /// <summary>
        /// The randomly-generated seed for this round.
        /// </summary>
        public ulong RoundSeed { get; private set; }

        /// <summary>
        /// A list of players and whether they've played a card this round.
        /// </summary>
        public Dictionary<Player, bool> HasPlayedList { get; internal set; }
        /// <summary>
        /// The white cards played by players.
        /// </summary>
        public Dictionary<Player, Dictionary<int, WhiteCard>> PlayedCards { get; internal set; }

        /// <summary>
        /// Sends a card to a given player. Randomly selected from pool.
        /// </summary>
        /// <param name="p">The player to send the card to.</param>
        internal void SendRandomCards(Player p)
        {
            List<KeyValuePair<int, WhiteCard>> sendCards = new List<KeyValuePair<int, WhiteCard>>();
            for (int i = 0; i < BlackCard.Draw; i++)
            {
                // Select a single ID/Card pair using the generator
                KeyValuePair<int, WhiteCard> wc = WhiteCardPool.ElementAt(_cardSelectorGenerate());
                // Remove it from this round's pool so it isn't selected again.
                WhiteCardPool.Remove(wc.Key);
                // Add it to the Game class's list of drawn cards for this player. It was already
                // removed from the Game's master pool when the card was sent to this Round class,
                // so we don't need to remove it from there.
                this._parent.DrawnCards[p].Add(wc.Key, wc.Value);
                // Add to this function's list of cards to send.
                sendCards.Add(wc);
            }

            foreach (KeyValuePair<int, WhiteCard> kvp in sendCards)
            {
                _parent.SendCommand(
                    CommandType.WHTE,
                    new string[2] { kvp.Key.ToString(), kvp.Value.Text },
                    p.ClientIdentifier
                );
            }
            /* TODO:
             * 1. Replace Player.Client with a wrapper over NetLib
             * 2. Send "WHTE" command to player, with the cards selected
             * 
             * NOTE: 1 WHTE command per card. Up to 3 WHTE commands will be issued,
             *       as the maximum "Pick" number is 3.
             */
        }

        /// <summary>
        /// Starts this round.
        /// </summary>
        /// <returns>The player who has won this round.</returns>
        public Player Start()
        {
            bool allPlayersSubmitted = false;
            // Players are limited in the time they have available to choose.
            // This timeout is in seconds in the parameters, so we have to multiply by 1000
            // to get it into milliseconds, Timer's accepted unit of time.
            // This timer will also be used to limit the Card Czar's choice time.
            Timer timeoutTimer = new Timer(this._parent.Parameters.TimeoutLimit * 1000);
            timeoutTimer.Elapsed += (s, e) =>
            {
                var hasntPlayed =
                    from p in HasPlayedList
                    where p.Value == false
                    select p;

                foreach (KeyValuePair<Player, bool> pl in hasntPlayed)
                {
                    _parent.SendCommand(CommandType.CRTO, (string[])null, pl.Key.ClientIdentifier);
                    if (_parent.Parameters.KickOnTimeout)
                    {
                        _parent.Disconnect(pl.Key.Nickname, "Disconnected for inactivity.");
                    }
                }

                allPlayersSubmitted = true;
            };

            foreach (Player p in Players.ToList())
            {
                _parent.SendCommand(CommandType.RSTR, (string[])null, p.ClientIdentifier);
                
                // Send information regarding the number of points each player has
                // to the player currently represented by 'p'. This includes player
                // 'p', as the server is the only source of authoritative information.
                foreach (Player pl in Players.ToList())
                {
                    _parent.SendCommand(
                        CommandType.PNTS,
                        new string[2] { pl.Nickname, pl.AwesomePoints.ToString() },
                        p.ClientIdentifier
                    );
                }

                _parent.SendCommand(
                    CommandType.BLCK,
                    new string[2] { this.BlackCard.Text, this.BlackCard.Pick.ToString() },
                    p.ClientIdentifier);

                // Sends the CZAR command with nickname to all players, informing them of who the
                // Card Czar is for this round.
                _parent.SendCommand(CommandType.CZAR, CardCzar.Nickname, p.ClientIdentifier);

            }

            foreach (Player p in Players.ToList().Where(pl => pl != CardCzar))
            {
                // Send new white cards to all players who are not the Card Czar
                // unless we've stopped drawing new whites for this round.
                if (_drawNewWhites) SendRandomCards(p);
            }

            
            // TODO: UNCOMMENT ME FOR PRODUCTION!
            timeoutTimer.Start();

            // Handles a player's pick.
            // Must determine whether the card ID is valid, and
            // whether the player sending the ID actually has
            // the card.
            Game.PlayerCardsEventHandler pickHandler = (player, ids) =>
            {
                foreach (int id in ids)
                {
                    if (_parent.DrawnCards[player].ContainsKey(id) && !HasPlayedList[player])
                    {
                        // Add this card to the list of cards played by this
                        // player.
                        PlayedCards[player].Add(id, _parent.DrawnCards[player][id]);
                        // Remove it from the list of cards the player currently
                        // has available to them.
                        _parent.DrawnCards[player].Remove(id);
                        // If the number of cards the player has played is equal to
                        // the required amount, set the boolean indicating whether
                        // they have completed playing cards to true.
                        if (PlayedCards[player].Count == this.BlackCard.Pick)
                            HasPlayedList[player] = true;

                        // Select every player that is not the player
                        // sending the PICK
                        var otherPlayers = from pl in Players.ToList()
                                           where pl != player
                                           select pl;
                        foreach (Player p in otherPlayers)
                        {
                            // Send a BLNK to each of those players
                            _parent.SendCommand(
                                CommandType.BLNK,
                                (string[])null,
                                p.ClientIdentifier
                            );
                        }

                        //System.Windows.Forms.MessageBox.Show(
                        //    String.Format(
                        //        "Player {0} picked ID {1} ({2}).",
                        //        player.Nickname,
                        //        id,
                        //        PlayedCards[player].First(c => c.Key == id)
                        //    ));
                    }
                    else if (HasPlayedList[player])
                    {
                        _parent.SendCommand(CommandType.UNRG, "You have already played your cards.", player.ClientIdentifier);
                    }
                    else if (!_parent.DrawnCards[player].ContainsKey(id))
                    {
                        _parent.SendCommand(
                            CommandType.UNRG,
                            "You do not have this card, or the card you attempted to play was invalid.",
                            player.ClientIdentifier
                        );
                    }
                    else
                    {
                        _parent.SendCommand(
                            CommandType.UNRG,
                            "Unable to accept given pick.",
                            player.ClientIdentifier
                        );
                    }
                }

                int falseCtr = 0;
                foreach (KeyValuePair<Player, bool> hP in HasPlayedList.ToList())
                {
                    if (!hP.Value) ++falseCtr;
                }
                if (falseCtr == 0) allPlayersSubmitted = true;
            };

            // Allows PICKs to be received and handled
            _parent.OnPlayerPick += pickHandler;
            // Wait for all players to submit cards, or until the timeout elapses.
            while (!allPlayersSubmitted) ;
            // Stop the timeout timer, as another will be created next round and
            // we no longer require this one.
            // TODO: UNCOMMENT ME FOR PRODUCTION
            timeoutTimer.Stop();
            // Removes PICK handler, so any received PICKs are now dropped.
            _parent.OnPlayerPick -= pickHandler;

            if (HasPlayedList.Count > 0)
            {
                foreach (Player p in Players.ToList())
                {
                    // These are some fucking ugly types, but we will persevere
                    List<KeyValuePair<Player, Dictionary<int, WhiteCard>>> playedCardsList = PlayedCards.ToList();
                    // Get rid of that list and key value pair, as we don't need the player for this, as
                    // nobody is to know the identity of who chose each card.
                    List<Dictionary<int, WhiteCard>> cardPairs = (from cards in playedCardsList
                                                                  select cards.Value).ToList();

                    for (
                        // In order to randomise the order in which pairs of cards are sent,
                        // we'll use our handy instance of Random to generate a random number
                        // between 0 and the length of the list containing pairs whilst
                        // removing pairs from said list, thus making the list smaller and
                        // eventually ending the loop.
                        int rnd = _cardSelectorRNG.Next(0, cardPairs.Count);
                        cardPairs.Count > 0;
                        cardPairs.Remove(cardPairs[rnd]), rnd = _cardSelectorRNG.Next(0, cardPairs.Count)
                        )
                    {

                        // This is where it gets a bit ugly.
                        // We're going to put the ID-Text pairs into an enumerable of string arrays,
                        // as we can easily extract these pairs using LINQ.
                        IEnumerable<string[]> IDs = from card in cardPairs.ToList()[rnd]
                                                    select new string[2] { card.Key.ToString(), card.Value.Text };
                        // Now we have to make this enumerable of arrays on array, and to do that
                        // we need to figure out the total length.
                        int lengCtr = 0;
                        // This first loop finds out our length for us.
                        foreach (string[] sa in IDs) lengCtr += sa.Length;
                        // We then use this length to define a string array which will, eventually,
                        // contain all our bundled cards.
                        string[] finalIds = new string[lengCtr];
                        // Might as well reuse this variable.
                        lengCtr = 0;
                        // Similarly to the previous foreach loop, we iterate through, using lengCtr
                        // to keep track of the length of the array. Only this time, we copy the contents
                        // of string arrays in our enumerable in-between additions to the tracked length.
                        foreach (string[] sa in IDs)
                        {
                            sa.CopyTo(finalIds, lengCtr);
                            lengCtr += sa.Length;
                        }

                        // Since this code spits out a nice string array, all that's left is to send
                        // the REVL command to each player.
                        _parent.SendCommand(
                            CommandType.REVL,
                            finalIds,
                            p.ClientIdentifier
                        );
                    }
                }
            }
            else
            {
                // If the number of players that have played cards is zero,
                // it'll be a null round, so we'll return a null player,
                // which indicates that the round is null.
                return (Player)null;
            }

            bool cardCzarHasPicked = false;
            Player roundWinner = null;
            Game.PlayerCardEventHandler czpkHandler = (player, cardId) =>
            {
                // Ensure that the player sending CZPK is actually the Card Czar.
                if (this.CardCzar != player)
                {
                    // If they aren't, respond appropriately and then exit this function.
                    _parent.SendCommand(CommandType.UNRG, "You are not the Card Czar.", player.ClientIdentifier);
                    return;
                }

                // Select the player who played the winning card,
                // using the card's ID to locate said player.
                    roundWinner = this.PlayedCards.First(pl => pl.Value.ContainsKey(cardId)).Key;

                foreach (Player p in Players.ToList())
                {
                    _parent.SendCommand(
                        CommandType.RWIN,
                        new string[2] { roundWinner.Nickname, cardId.ToString() },
                        p.ClientIdentifier
                    );
                }

                // We've received the Czar's pick, so we can stop the waiting loop.
                cardCzarHasPicked = true;
            };
            // Card Czars are allowed twice the time given to players to choose
            // a card as winner. Min: 30s Max: 120s
            Timer czarTimeout = new Timer(_parent.Parameters.TimeoutLimit * 2000);
            czarTimeout.Elapsed += (s, e) =>
            {
                cardCzarHasPicked = true;
                _parent.SendCommand(CommandType.CZTO, "You did not pick within adequate time.", CardCzar.ClientIdentifier);
                if (_parent.Parameters.KickOnTimeout) _parent.Disconnect(CardCzar.Nickname);
            };

            // TODO: Uncomment me and my Stop() call for production!
            czarTimeout.Start();
            _parent.OnCzarPick += czpkHandler;
            while (!cardCzarHasPicked) ;
            _parent.OnCzarPick -= czpkHandler;
            czarTimeout.Stop();

            /* TODO:
             * 1. Bind handlers to events for receiving "PICK" commands from players.
             * 2. Bind handlers to events for timeout timer, ensuring no picks are accepted after timeout.
             * 3. Reset timer and move to Card Czar picking stage.
             * 4. Wait for czar pick.
             * 5. Return the player whose card was chosen by the card czar.
             * 6. Remove all played cards from Game.DrawnCards.
             */

            return roundWinner;
        }

        /// <summary>
        /// Forcibly ends the round.
        /// </summary>
        public void End()
        {
            // TODO: implement
        }
    }
}
