using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsNetLib2;

namespace AppsAgainstHumanity.Server.Game.Modes
{
    /// <summary>
    /// The interface from which all game modes for the server must
    /// derive.
    /// </summary>
    public interface IGameMode
    {
        /// <summary>
        /// The Game object making use of this game mode.
        /// </summary>
        private Game Parent;

        /// <summary>
        /// The name of the represented game mode.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A list of players which the implementation can modify. Will
        /// be copied back to the game's dictionary at the cessation of
        /// a round.
        /// </summary>
        Dictionary<long, Player> Players { get; internal set; }
        /// <summary>
        /// A dictionary containing all available white cards. Copied
        /// back to the game's pool at the end of a round.
        /// </summary>
        Dictionary<int, WhiteCard> WhiteCards { get; internal set; }
        /// <summary>
        /// The list of currently available black cards. Copied back
        /// into the game's pool when the round ends.
        /// </summary>
        List<BlackCard> BlackCards { get; internal set; }

        /// <summary>
        /// This game mode's handler for PICK commands.
        /// </summary>
        /// <param name="sender">The Client ID of the client sending the command.</param>
        /// <param name="arguments">Any arguments sent by the client with the command.</param>
        public void CommandPickHandler(long sender, string[] arguments);
        /// <summary>
        /// This game mode's handler for CZPK commands.
        /// </summary>
        /// <param name="sender">The Client ID of the client sending the command.</param>
        /// <param name="arguments">Any arguments sent by the client with the command.</param>
        public void CommandCzpkHandler(long sender, string[] arguments);
        /// <summary>
        /// Starts a round of the specified game mode.
        /// </summary>
        /// <returns>The player who was the winner of the round.</returns>
        public Player Start();
    }
}
