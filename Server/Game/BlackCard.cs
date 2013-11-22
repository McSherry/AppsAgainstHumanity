using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanity.Server.Game
{
    /// <summary>
    /// A class representing a black card, which contains a question or sentence with blanks to be filled by white cards.
    /// </summary>
    public class BlackCard : Card
    {
        /// <summary>
        /// The type of card represented.
        /// </summary>
        public override CardType Type
        {
            get { return CardType.Black; }
        }
        /// <summary>
        /// The text of the black card.
        /// </summary>
        public override string Text { get; internal set; }

        /// <summary>
        /// Creates an instance of BlackCard with default Pick and Draw values.
        /// </summary>
        /// <param name="text">The text of the black card.</param>
        public BlackCard(string text)
        {
            this.Text = text;
            this.Pick = 1;
            this.Draw = 1;
        }
        /// <summary>
        /// Creates an instance of BlackCard with the default draw value.
        /// </summary>
        /// <param name="text">The text of the black card.</param>
        /// <param name="pick">The number of cards a player should pick in response to this card.</param>
        public BlackCard(string text, int pick)
        {
            this.Text = text;
            this.Pick = pick;
            this.Draw = 1;
        }
        /// <summary>
        /// Creates an instance of BlackCard.
        /// </summary>
        /// <param name="text">The text of the black card.</param>
        /// <param name="pick">The number of cards a player should pick in response to this card.</param>
        /// <param name="draw">The number of cards a player should draw in response to this card.</param>
        public BlackCard(string text, int pick, int draw)
        {
            this.Text = text;
            this.Pick = pick;
            this.Draw = draw;
        }

        /// <summary>
        /// The number of white cards a player should pick in response to this black card.
        /// </summary>
        public int Pick { get; private set; }
        /// <summary>
        /// The number of white cards a player should draw in response to this black card.
        /// </summary>
        public int Draw { get; private set; }
    }
}
