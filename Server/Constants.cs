using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanity.Server
{
    /// <summary>
    /// A class containing constants related to Apps Against Humanity.
    /// </summary>
    public static class Constants
    {
#if DEBUG
        /// <summary>
        /// The minimum number of players required for a game to begin.
        /// </summary>
        public const int MinimumPlayers = 2;
#else
        /// <summary>
        /// The minimum number of players required for a game to begin.
        /// </summary>
        public const int MinimumPlayers = 3;
#endif
        /// <summary>
        /// The address of the Apps Against Humanity website.
        /// </summary>
        public static Uri WebAddress
        {
            get { return new Uri("http://getaah.net"); }
        }

        /// <summary>
        /// The name of the file containing server configuration data.
        /// </summary>
        public const string SettingsFile = "Server.xml";

        /// <summary>
        /// The default port used for communication.
        /// </summary>
        public static int DefaultPort { get { return 11235; } }

        public const string DefaultDeckPath = "./Decks";

        public const string DefaultGameMode = "Standard";
    }
}
