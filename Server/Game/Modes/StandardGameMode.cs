using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsNetLib2;

namespace AppsAgainstHumanity.Server.Game.Modes
{
    public class StandardGameMode : GameMode
    {
        /// <summary>
        /// Dictionary containing a dictionary of cards played by each player.
        /// Card dictionary is keyed using the card's unique identifier, as
        /// generated at the beginning of the game.
        /// </summary>
        private Dictionary<Player, Dictionary<int, WhiteCard>> _playedCards;
        /// <summary>
        /// True when the server is expecting to receive
        /// a PICK command. When false, the command handler
        /// for PICKs will not function.
        /// </summary>
        private bool _allowPicks = false;

        public StandardGameMode(Game parent)
            : base(parent)
        {
            // In order to avoid putting the instantiation of this
            // ugly type somewhere weird, it's been thrown into the
            // constructor. Hopefully base.Players is instantiated
            // before this code is executed.
            foreach (Player p in base.Players.ToList())
            {
                // Instantiates the dictionary that will track the cards
                // played by each player for the duration of the round.
                _playedCards.Add(p, new Dictionary<int, WhiteCard>());
            }
        }

        public override string Name
        {
            get { return "Standard"; }
        }

        public override void CommandPickHandler(long sender, string[] arguments)
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
                base.Parent.SendCommand(
                    CommandType.UNRG,
                    "PICK received without arguments; invalid.",
                    sender
                );
                return;
            }

            // The variable _allowPicks being set to true indicates that players can
            // currently submit cards. If this is false, we should not accept picks
            // sent in by players.
            if (_allowPicks)
            {
                // The sender of the PICK command, as a Player class rather than
                // their numeric identifier.
                Player pSender = base.Players.Single(pl => pl.ClientIdentifier == sender);
                List<int> pSenderPicks = new List<int>();

                // Parse the IDs sent by the client into integers, which will
                // later be used to try and retrieve cards from the list of
                // drawn cards.
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
                    if (base.Parent.DrawnCards[pSender].ContainsKey(i))
                    {
                        // If the player has not picked the number of cards
                        // required for this round, add their pick to the
                        // dictionary containing drawn cards.
                        if (!base.PickedList[pSender])
                        {
                            // Add the card the player played to the
                            // dictionary containing the cards they have
                            // played.
                            this._playedCards[pSender].Add(i, base.Parent.DrawnCards[pSender][i]);
                            // As the player no longer has the card in their "hand,"
                            // we'll remove it from the list of drawn cards to prevent
                            // the card being played again.
                            base.Parent.DrawnCards[pSender].Remove(i);
                            // If the player has played the required number of cards,
                            // we set the value in the dictionary indicating as much to
                            // true. As this code will not be executed during gambling,
                            // we don't need to perform any checks for it.
                            if (this._playedCards[pSender].Count == base.BlackCard.Pick)
                                base.PickedList[pSender] = true;

                            // We now select every player who is /not/ the player who just sent a
                            // PICK command, and we inform them that another player has submitted
                            // a choice of card via the BLNK command. Upon receiving this command,
                            // clients should show some indication of another player having played.
                            foreach (Player p in base.Players.Where(pl => pl != pSender).ToList())
                            {
                                base.Parent.SendCommand(
                                    CommandType.BLNK,
                                    (string[])null,
                                    p.ClientIdentifier
                                );
                            }
                        }
                        // This code block handles gambling by players.
                        else if (base.PickedList[pSender] && base.Parent.Parameters.AllowGambling)
                        {

                        }
                    }
                    else
                    {
                        // Informs the client that they card they tried to play
                        // was not present in their hand, and so the PICK was not
                        // accepted by the server.
                        base.Parent.SendCommand(
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
                        if (base.Parent.DrawnCards.Single(pl => pl.Key.ClientIdentifier == sender).Value.ContainsKey(cardId))
                        {
                            // If the card did, in fact, exist, we'll send it back to the player and then continue
                            // with the loop.
                            base.Parent.SendCommand(
                                CommandType.WHTE,
                                new string[2] { 
                                    cardId.ToString(),
                                    // Rather ugly, but this little statement here selects the list of cards drawn by the
                                    // player who sent the PICK, and then selects the white card with the ID sent by the
                                    // client, and retrieves the property Card.Text.
                                    base.Parent.DrawnCards.Single(pl => pl.Key.ClientIdentifier == sender).Value[cardId].Text
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
    }
}
