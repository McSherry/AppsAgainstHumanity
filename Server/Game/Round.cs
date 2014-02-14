using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CsNetLib2;
using AppsAgainstHumanity.Server.Game.Modes;

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
            return _cardSelectorRNG.Next(this.WhiteCardPool.Count - 1);
        }
        /// <summary>
        /// The game this round is a part of.
        /// </summary>
        private Game _parent;

        /// <summary>
        /// Create a new Round.
        /// </summary>
        /// <param name="blackCard">The black card to play this round.</param>
        /// <param name="pool">The pool of white cards to use this round.</param>
        /// <param name="cardCzar">The player chosen as Card Czar for this round.</param>
        /// <param name="parent">The instance of game which instantiated this round.</param>
        /// <param name="drawNewWhites">Whether to deal new white cards to players. True by default.</param>
        public Round(
            BlackCard blackCard,
            Dictionary<int, WhiteCard> pool,
            Game parent,
            Player cardCzar,
            bool drawNewWhites = true
            ) 
        {
            this.BlackCard = blackCard;
            this.WhiteCardPool = pool;
            this.RoundSeed = new Crypto.SRand();
            this._parent = parent;
            this.CardCzar = cardCzar;

            this._cardSelectorRNG = new Random((int)this.RoundSeed);
            this.HasPlayedList = new Dictionary<Player, bool>();
            this.PlayedCards = new Dictionary<Player, Dictionary<int, WhiteCard>>();
            this.DrawNewWhites = drawNewWhites;

            foreach (Player p in Players.ToList())
            {
                // If the player is this iteration is the Czar, we won't need
                // to set up a place for them in dictionaries, as they won't
                // be playing any cards.
                if (p == cardCzar) continue;
                this.HasPlayedList.Add(p, false);
                this.PlayedCards.Add(p, new Dictionary<int, WhiteCard>());
                /* TODO:
                 * 1. Send the black card for this round to each player
                 * 2. Use NetLib wrapper's "BLCK" command.
                 */
            }
        }

        /// <summary>
        /// The black card that was played this round.
        /// </summary>
        public BlackCard BlackCard { get; private set; }
        /// <summary>
        /// The list of currently connected players.
        /// </summary>
        public List<Player> Players
        {
            get { return _parent.Players; }
        }
        /// <summary>
        /// The Card Czar for this round.
        /// </summary>
        public Player CardCzar { get; private set; }
        /// <summary>
        /// The white cards available to send to players this round.
        /// </summary>
        public Dictionary<int, WhiteCard> WhiteCardPool { get; private set; }

        /// <summary>
        /// The randomly-generated seed for this round.
        /// </summary>
        public ulong RoundSeed { get; private set; }

        /// <summary>
        /// A list of players and whether they've played a card this round.
        /// </summary>
        public Dictionary<Player, bool> HasPlayedList { get; internal set; }
        /// <summary>
        /// The white cards played by players.
        /// </summary>
        public Dictionary<Player, Dictionary<int, WhiteCard>> PlayedCards { get; internal set; }
        /// <summary>
        /// Whether a new set of white cards should be drawn and dealt to players at
        /// the beginning of the round.
        /// </summary>
        public bool DrawNewWhites { get; internal set; }

        /// <summary>
        /// Sends a card to a given player. Randomly selected from pool.
        /// </summary>
        /// <param name="p">The player to send the card to.</param>
        internal void SendRandomCards(Player p)
        {
            List<KeyValuePair<int, WhiteCard>> sendCards = new List<KeyValuePair<int, WhiteCard>>();
            for (int i = 0; i < BlackCard.Draw; i++)
            {
                // Select a single ID/Card pair using the generator
                KeyValuePair<int, WhiteCard> wc = WhiteCardPool.ElementAt(_cardSelectorGenerate());
                // Remove it from this round's pool so it isn't selected again.
                WhiteCardPool.Remove(wc.Key);
                // Add it to the Game class's list of drawn cards for this player. It was already
                // removed from the Game's master pool when the card was sent to this Round class,
                // so we don't need to remove it from there.
                this._parent.DrawnCards[p].Add(wc.Key, wc.Value);
                // Add to this function's list of cards to send.
                sendCards.Add(wc);
            }

            foreach (KeyValuePair<int, WhiteCard> kvp in sendCards)
            {
                _parent.SendCommand(
                    CommandType.WHTE,
                    new string[2] { kvp.Key.ToString(), kvp.Value.Text },
                    p.ClientIdentifier
                );
            }
            /* TODO:
             * 1. Replace Player.Client with a wrapper over NetLib
             * 2. Send "WHTE" command to player, with the cards selected
             * 
             * NOTE: 1 WHTE command per card. Up to 3 WHTE commands will be issued,
             *       as the maximum "Pick" number is 3.
             */
        }

        /// <summary>
        /// Starts this round.
        /// </summary>
        /// <returns>The player who has won this round.</returns>
        public void Start()
        {
            foreach (Player p in this._parent.Players.ToList())
            {
                // Inform clients that the game has started.
                this._parent.SendCommand(
                    CommandType.RSTR,
                    (string[])null,
                    p.ClientIdentifier
                );
            }
            // Start the current game mode.
            this._parent.CurrentGameMode.Start();

            return;
        }

        /// <summary>
        /// Forcibly ends the round.
        /// </summary>
        public void Stop()
        {
            // TODO: implement
        }
    }
}
