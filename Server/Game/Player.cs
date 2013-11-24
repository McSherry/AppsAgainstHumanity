using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using CsNetLib2;

namespace AppsAgainstHumanity.Server.Game
{
    /// <summary>
    /// A class to store basic information about each player.
    /// </summary>
    public class Player
    {
        private char[] validNickChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_|".ToCharArray();

        /// <summary>
        /// Initialise a new instance of Player, setting appropriate variables.
        /// </summary>
        /// <param name="Nick">The nickname used to represent the player in each game.</param>
        /// <param name="IP">The IP address the player is connecting from.</param>
        public Player(string Nick, IPAddress IP)
        {
            this.JoinTime = DateTime.UtcNow;
            this.Nickname = Nick;
            this.IP = IP;
        }

        private string _Nickname;
        /// <summary>
        /// The name used to identify the player.
        /// </summary>
        public string Nickname {
            get { return _Nickname; }
            set
            {
                foreach (char c in value)
                {
                    if (!validNickChars.Contains(c))
                        throw new ArgumentException
                        ("Nickname contained invalid characters.");
                    else continue;
                }

                _Nickname = value;
            }
        }
        /// <summary>
        /// The IP address the player is connecting from. Used to transmit messages.
        /// </summary>
        public IPAddress IP { get; set; }
        /// <summary>
        /// The UTC time at which the player connected. Automatically set by the constructor.
        /// </summary>
        public DateTime JoinTime { get; private set; }

        /// <summary>
        /// The number of Awesome Points the player currently has.
        /// </summary>
        public uint AwesomePoints { get; set; }

        /// <summary>
        /// The cards the player has currently drawn.
        /// </summary>
        public List<WhiteCard> DrawnCards { get; set; }

        /// <summary>
        /// A network library client representing the player. Used to communicate with the player.
        /// </summary>
        public NetLibClient Client { get; private set; }
    }
}
