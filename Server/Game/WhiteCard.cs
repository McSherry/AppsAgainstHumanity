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
        public WhiteCard(string text, byte[] hash)
        {
            this.Text = text;
            base.Hash = hash;
        }

        /// <summary>
        /// The text of the white card.
        /// </summary>
        public override string Text { get; internal set; }

        /// <summary>
        /// A hash used to verify the authenticity of the card.
        /// </summary>
        public override byte[] Hash
        {
            get
            {
                return base.Hash;
            }
        }
    }
}
