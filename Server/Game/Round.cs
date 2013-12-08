using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AppsAgainstHumanity.Server.Game
{
    public class Round
    {
        // The round state. Values below.
        // -1   =   Not started
        //  0   =   Dealing white cards
        //  1   =   Accepting white card picks from players
        //  2   =   Accepting winner pick from czar
        private int _gameState = -1;
        private Random _cardSelectorRNG;
        /// <summary>
        /// Generates a random number within the bounds of the white card pool's length.
        /// </summary>
        private int _cardSelectorGenerate()
        {
            return _cardSelectorRNG.Next(this.WhiteCardPool.Count);
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
        public Round(
            BlackCard blackCard,
            Dictionary<int, WhiteCard> pool,
            Game parent
            ) 
        {
            this.BlackCard = blackCard;
            this.WhiteCardPool = pool;
            this.RoundSeed = new Crypto.SRand();
            this.Players = parent.Players;
            this._parent = parent;

            this._cardSelectorRNG = new Random((int)this.RoundSeed);
            this.HasPlayedList = new Dictionary<Player, bool>();

            foreach (Player p in Players.ToList())
            {
                this.HasPlayedList.Add(p, false);

                this._parent.SendCommand(
                    CsNetLib2.CommandType.BLCK,
                    new string[1] { this.BlackCard.Text },
                    p.ClientIdentifier
                );
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
        public List<Player> Players { get; internal set; }
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
        public Dictionary<Player, List<int>> PlayedCards { get; internal set; }

        /// <summary>
        /// Sends a card to a given player. Randomly selected from pool.
        /// </summary>
        /// <param name="p">The player to send the card to.</param>
        internal void SendRandomCards(Player p)
        {
            List<KeyValuePair<int, WhiteCard>> sendCards = new List<KeyValuePair<int, WhiteCard>>();
            for (int i = 0; i < BlackCard.Pick; i++)
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
        public Player Start()
        {
            // TODO: REMOVE THIS PLAYER
            Player tempPlayer = new Player("REMOVE_ME", 0);
            bool allPlayersSubmitted = false;

            // Players are limited in the time they have available to choose.
            // This timeout is in seconds in the parameters, so we have to multiply by 1000
            // to get it into milliseconds, Timer's accepted unit of time.
            // This timer will also be used to limit the Card Czar's choice time.
            Timer timeoutTimer = new Timer(this._parent.Parameters.TimeoutLimit * 1000);
            timeoutTimer.Elapsed += (s, e) =>
            {
                allPlayersSubmitted = true;
            };

            // Wait for all players to submit cards, or until
            // the timeout elapses.
            while (!allPlayersSubmitted) ;

            /* TODO:
             * 1. Bind handlers to events for receiving "PICK" commands from players.
             * 2. Bind handlers to events for timeout timer, ensuring no picks are accepted after timeout.
             * 3. Reset timer and move to Card Czar picking stage.
             * 4. Wait for czar pick.
             * 5. Return the player whose card was chosen by the card czar.
             * 6. Remove all played cards from Game.DrawnCards.
             */

            return tempPlayer;
        }

        /// <summary>
        /// Forcibly ends the round.
        /// </summary>
        public void End()
        {
            // TODO: implement
        }
    }
}
