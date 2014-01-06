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
        public StandardGameMode(Game parent) : base(parent) { }

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

        }
    }
}
