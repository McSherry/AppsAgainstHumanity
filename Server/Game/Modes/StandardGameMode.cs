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
        /// True when the server is expecting to receive
        /// a CZPK command. When false, the command handler
        /// for CZPKs will not function.
        /// </summary>
        private bool _allowCzpk = false;
        /// <summary>
        /// The player to be returned as the winner of the round.
        /// </summary>
        private Player _roundWinner = null;

        public StandardGameMode(Game parent)
            : base(parent)
        {

        }

        public override string Name
        {
            get { return "Standard"; }
        }

        public override Player Start()
        {


            throw new NotImplementedException();
        }

        public override void CommandCzpkHandler(long sender, string[] arguments)
        {
            if (arguments == null)
            {
                // If the client has attempted to submit a Czar Pick
                // without specifying any cards, we'll return an error
                // message to them regarding this, and then exit the function.
                base.Parent.SendCommand(
                    CommandType.UNRG,
                    "CZPK sent without arguments; invalid.",
                    sender
                );
                return;
            }

            // If the game mode has indicated that this is currently a period
            // in which CZPK commands are accepted and should be parsed.
            if (_allowCzpk)
            {
                // The player represented by the client identifier which sent this
                // command.
                Player pSender = base.Players.Single(p => p.ClientIdentifier == sender);
                
                // If the sender of the CZPK is the Card Czar for this round, we can
                // proceed as normal, since they're the one that's meant to be picking.
                if (sender == base.Parent.CurrentRound.CardCzar.ClientIdentifier)
                {
                    int cardId = 0;
                    // We must verify that the client sent a card identifier which
                    // can be parsed into an int. If they have not, an invalid
                    // card identifier has been sent.
                    if (int.TryParse(arguments[0], out cardId))
                    {
                        // We don't know, at this point, whether the card ID the
                        // Czar has sent us actually exists, so we have to wrap
                        // our check in a try-catch block to prevent crashes.
                        try
                        {
                            // If the provided card ID exists, we select the player who
                            // sent it in and set _roundWinner to indicate that they have
                            // won. If the card doesn't exist, an exception is thrown,
                            // and _roundWinner will remain null.
                            this._roundWinner = base.PlayedCards.Single(k => k.Value.ContainsKey(cardId)).Key;
                            // Since we've now selected a winner, we no longer need to
                            // execute this handler, and so we can set _allowCzpk to false,
                            // causing this function to exit much earlier if called again.
                            this._allowCzpk = false;
                        }
                        catch (InvalidOperationException)
                        {
                            // If the card doesn't exist, this block will be executed and
                            // the Card Czar will be informed that the card they attempted
                            // to select does not exist or, rather, hasn't been played by
                            // any players.
                            base.Parent.SendCommand(
                                CommandType.UNRG,
                                String.Format(
                                    "Card has not been played ({0}).",
                                    cardId
                                ),
                                sender
                            );
                        }
                        // Exit out of the function.
                        return;
                    }
                    else
                    {
                        // If the card identifier is invalid, we inform
                        // the client of this, and provide the identifier
                        // which was invalid.
                        base.Parent.SendCommand(
                            CommandType.UNRG,
                            String.Format(
                                "Invalid card identifier sent ({0}).",
                                arguments[0]
                            ),
                            sender
                        );
                    }
                }
                else
                {
                    // If they aren't the Card Czar, however, we'll inform them of this via
                    // an UNRG and then exit the function, taking no further action on their
                    // command and continue waiting for the actual Card Czar to make a pick.
                    base.Parent.SendCommand(
                        CommandType.CZPK,
                        "You are not the Card Czar.",
                        sender
                    );
                    return;
                }
            }
            else
            {
                // If CZPKs are not currently being accepted, we'll
                // respond with an UNRG and exit out of the function.
                // Since card picks aren't persistent across rounds in
                // the same way drawn cards are, we won't do anything
                // to "return" the card to the client.
                base.Parent.SendCommand(
                    CommandType.UNRG,
                    "CZPK commands are not being accepted at this time.",
                    sender
                );
                return;
            }
        }
    }
}
