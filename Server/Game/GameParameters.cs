using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanity.Server.Game
{
    /// <summary>
    /// The game ruleset the server should be configured to handle.
    /// </summary>
    public enum GameRuleset
    {
        /// <summary>
        /// The basic Cards Against Humanity ruleset without any modification.
        /// </summary>
        Basic = 0,

        // ONLY BASIC WILL BE SUPPORTED INITIALLY
        // FURTHER RULESETS MAY BE SUPPORTED IN FUTURE

        /// <summary>
        /// At any time, players may trade in an Awesome Point to return as many White Cards
        /// as they'd like to the deck and draw back up to ten.
        /// </summary>
        RebootingTheUniverse = 1,
        /// <summary>
        /// For all "Pick 2" cards, players draw an extra white card before playing their hands.
        /// </summary>
        PackingHeat = 2,
        /// <summary>
        /// In every round, an additional white card, beloning to an imaginary player, is thrown
        /// into the mix. If this imaginary player wins, all other players enter into a state of
        /// everlasting shame.
        /// </summary>
        RandoCardrissian = 3,
        /// <summary>
        /// Play without a Card Czar. Each player picks his or her favourite card each round, and
        /// the card with the most votes at the end of the round wins.
        /// </summary>
        GodIsDead = 4,
        /// <summary>
        /// After everyone has played a card, players take turns eliminating one card from the set.
        /// The card left at the end of these eliminations is the winner.
        /// </summary>
        SurvivalOfTheFittest = 5,
        /// <summary>
        /// Instead of picking a favourite card, the Card Czar selects three cards and ranks them
        /// from first to third. The first cards receives 3 Awesome Points, the second 2, and the
        /// third 1.
        /// </summary>
        SeriousBusiness = 6
    }

    /// <summary>
    /// How the server should select Card Czars each round.
    /// </summary>
    public enum CzarSelection
    {
        /// <summary>
        /// Cycle through players in the order that they joined.
        /// </summary>
        Sequential = 0,
        /// <summary>
        /// Pick a random player each round to be the Card Czar.
        /// </summary>
        Random = 1
    }

    /// <summary>
    /// A class containing all the parameters for a game of Cards Against Humanity.
    /// </summary>
    public class GameParameters
    {
        /// <summary>
        /// The ruleset the server should adhere to.
        /// </summary>
        public GameRuleset Ruleset { get; set; }
        /// <summary>
        /// How the server should select Card Czars for each round of the game.
        /// </summary>
        public CzarSelection CzarSelection { get; set; }
        /// <summary>
        /// The deck of cards to be used in this game.
        /// </summary>
        public Deck Cards { get; set; }
        /// <summary>
        /// The maximum number of players that can be present.
        /// </summary>
        public int Players { get; set; }
        /// <summary>
        /// The number of Awesome Points required to win the game.
        /// </summary>
        public int PointsLimit { get; set; }
        /// <summary>
        /// Allows players to wager an additional Awesome Point and play a
        /// second white card. If either of their white cards wins, they
        /// keep the wagered point. If they lose the round, their wagered
        /// point goes to the winner.
        /// </summary>
        public bool AllowGambling { get; set; }

        private int _timeoutLimit = 30;
        /// <summary>
        /// The length of time required for the card-picking period to time out.
        /// Defaults to 30 seconds.
        /// </summary>
        public int TimeoutLimit
        {
            get
            {
                return _timeoutLimit;
            }
            set
            {
                if (value > 15 && value < 60)
                    _timeoutLimit = value;
                else if (value < 15)
                    throw new ArgumentException
                    ("Timeout value cannot be less than fifteen.");
                else if (value > 60)
                    throw new ArgumentException
                    ("Timeout value cannot be longer than 60 seconds.");
            }
        }

        /// <summary>
        /// Whether the server should kick any players who fail to send
        /// in a choice in the allotted time. Includes Card Czar.
        /// </summary>
        public bool KickOnTimeout { get; set; }
        /// <summary>
        /// Whether players are allowed to chat on this server.
        /// </summary>
        public bool AllowPlayerChat { get; set; }
        /// <summary>
        /// Whether to only allow black cards where a single white card may be picked
        /// into the pool of black cards to be given to players.
        /// </summary>
        public bool BlackCardPickLimit { get; set; }
    }
}
