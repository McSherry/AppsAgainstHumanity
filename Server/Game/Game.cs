using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanity.Server.Game
{
    public class Game
    {
        private Random _RNG;
        private ulong _gameSeed;
        private bool _gameWon = false;

        /// <summary>
        /// Selects a black card from the pool, and removes it from the pool.
        /// </summary>
        /// <returns>The selected black card.</returns>
        private BlackCard _selectBlack()
        {
            int index = _RNG.Next(BlackCardPool.Count);
            BlackCard choice = BlackCardPool[index];
            BlackCardPool.RemoveAt(index);

            return choice;
        }
        /// <summary>
        /// Selects the specified number of white cards from the pool.
        /// </summary>
        /// <param name="n">The number of white cards to select.</param>
        /// <returns>The white cards selected, with their IDs.</returns>
        private Dictionary<int, WhiteCard> _selectWhites(int n)
        {
            Dictionary<int, WhiteCard> choices = new Dictionary<int, WhiteCard>();

            for (int i = 0; i < n; i++)
            {
                KeyValuePair<int, WhiteCard> choice = WhiteCardPool.ElementAt(_RNG.Next(WhiteCardPool.Count);
                WhiteCardPool.Remove(choice.Key);
                choices.Add(choice.Key, choice.Value);
            }

            return choices;
        }

        public Game(GameParameters gameParams)
        {
            this.Parameters = gameParams;
            this._gameSeed = new Crypto.SRand();
            this._RNG = new Random((int)_gameSeed);

            foreach (WhiteCard wc in Parameters.Cards.WhiteCards)
            {
                this.WhiteCardPool.Add(_RNG.Next(), wc);
            }
            this.BlackCardPool = Parameters.Cards.BlackCards;
        }

        /// <summary>
        /// The parameters the game is currently configured to use.
        /// </summary>
        public GameParameters Parameters { get; private set; }
        /// <summary>
        /// A dictionary of white cards remaining in the pool, with IDs as their key.
        /// </summary>
        public Dictionary<int, WhiteCard> WhiteCardPool { get; private set; }
        /// <summary>
        /// A list of black cards remaining in the pool. Black cards do not have IDs.
        /// </summary>
        public List<BlackCard> BlackCardPool { get; private set; }
        /// <summary>
        /// The players currently in the game.
        /// </summary>
        public List<Player> Players { get; private set; }
        /// <summary>
        /// The white cards currently drawn by each player.
        /// </summary>
        public Dictionary<Player, Dictionary<int, WhiteCard>> DrawnCards { get; internal set; }

        public void Start()
        {
            while (!_gameWon)
            {
                BlackCard roundBlack = _selectBlack();
                Dictionary<int, WhiteCard> roundPool = _selectWhites(roundBlack.Pick * Players.Count);
                Round round = new Round(roundBlack, roundPool, Players, this);

                Player roundWinner = round.Start();
                ++roundWinner.AwesomePoints;
                // TODO: Verify the above works.
                // Dunno if returning a player maintains the pass by reference shit
                // which would allow modification of that player's class.
            }
        }
    }
}
