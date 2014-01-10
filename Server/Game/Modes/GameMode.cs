using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanity.Server.Game.Modes
{
    /// <summary>
    /// An abstract class from which game mode implementations should inherit.
    /// </summary>
    public abstract class GameMode
    {
        protected internal virtual Game Parent { get; internal set; }

        /// <summary>
        /// Initialise an instance of StandardGameMode with a parent Game instance.
        /// </summary>
        /// <param name="parent">The Game instance to treat as a parent.</param>
        public GameMode(Game parent)
        {
            this.Parent = parent;
            this.Players = parent.Players;
            this.WhiteCards = parent.CurrentRound.WhiteCardPool;
            this.BlackCard = parent.CurrentRound.BlackCard;

            foreach (Player p in this.Players.ToList())
                this.PickedList.Add(p, false);
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
        /// This game mode's handler for PICK commands.
        /// </summary>
        /// <param name="sender">The Client ID of the client sending the command.</param>
        /// <param name="arguments">Any arguments sent by the client with the command.</param>
        public abstract void CommandPickHandler(long sender, string[] arguments);
        /// <summary>
        /// This game mode's handler for CZPK commands.
        /// </summary>
        /// <param name="sender">The Client ID of the client sending the command.</param>
        /// <param name="arguments">Any arguments sent by the client with the command.</param>
        public abstract void CommandCzpkHandler(long sender, string[] arguments);
        /// <summary>
        /// Starts a round of the specified game mode.
        /// </summary>
        /// <returns>The player who was the winner of the round.</returns>
        public abstract Player Start();
    }
}
