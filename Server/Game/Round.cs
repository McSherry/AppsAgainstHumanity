using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanity.Server.Game
{
    public class Round
    {
        private Random _cardSelectorRNG;
        /// <summary>
        /// Generates a random number within the bounds of the white card pool's length.
        /// </summary>
        private int _cardSelectorGenerate()
        {
            return _cardSelectorRNG.Next(this.WhiteCardPool.Count);
        }

        /// <summary>
        /// Create a new Round.
        /// </summary>
        /// <param name="blackCard">The black card to play this round.</param>
        /// <param name="pool">The pool of white cards to use this round.</param>
        public Round(
            BlackCard blackCard,
            Dictionary<uint, WhiteCard> pool,
            List<Player> players
            ) 
        {
            this.BlackCard = blackCard;
            this.WhiteCardPool = pool;
            this.RoundSeed = new Crypto.SRand();
            this.Players = players;

            this._cardSelectorRNG = new Random((int)this.RoundSeed);
        }

        /// <summary>
        /// The black card that was played this round.
        /// </summary>
        public BlackCard BlackCard { get; private set; }
        /// <summary>
        /// The list of currently connected players.
        /// </summary>
        public List<Player> Players { get; internal set; }
        /// <summary>
        /// The white cards available to send to players this round.
        /// </summary>
        public Dictionary<uint, WhiteCard> WhiteCardPool { get; private set; }
        /// <summary>
        /// The randomly-generated seed for this round.
        /// </summary>
        public ulong RoundSeed { get; private set; }

        /// <summary>
        /// The white cards played by players.
        /// </summary>
        public Dictionary<Player, List<WhiteCard>> PlayedCards { get; internal set; }
    }
}
