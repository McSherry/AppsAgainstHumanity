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
        /// a PICK command. When false, the command handler
        /// for PICKs will not function.
        /// </summary>
        private bool _allowPicks = false, _allowCzpk = false;

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
                if (pSender.ClientIdentifier == base.Parent.CurrentRound.CardCzar.ClientIdentifier)
                {

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
