using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanity.Server.Game
{
    /// <summary>
    /// A class representing a white card, which contains a response to a black card.
    /// </summary>
    public class WhiteCard : Card
    {
        /// <summary>
        /// The type of card represented.
        /// </summary>
        public override CardType Type
        {
            get { return CardType.White; }
        }

        /// <summary>
        /// Create an instance of WhiteCard.
        /// </summary>
        /// <param name="text"></param>
        public WhiteCard(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// The text of the white card.
        /// </summary>
        public override string Text { get; internal set; }
    }
}
