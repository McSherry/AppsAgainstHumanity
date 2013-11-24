using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanity.Server.Commands
{
    public static class Handlers
    {
        /// <summary>
        /// A delegate for representing commands which do not have arguments.
        /// </summary>
        public delegate void CommandNoArgs();
        /// <summary>
        /// A delegate for representing commands which do have arguments.
        /// </summary>
        /// <param name="args">The arguments to pass to the command.</param>
        public delegate void CommandArgs(params string[] args);
    }
}
