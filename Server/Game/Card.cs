﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanity.Server.Game
{
    public enum CardType
    {
        Black,
        White
    }

    /// <summary>
    /// A base card class from which other, more specialised cards, can inherit.
    /// </summary>
    [Serializable]
    public abstract class Card
    {
        /// <summary>
        /// The text of the card.
        /// </summary>
        public abstract string Text { get; internal set; }
        /// <summary>
        /// The type of card stored.
        /// </summary>
        public abstract CardType Type { get; }

        /// <summary>
        /// Creates an instance of a white card, cast as a Card abstract type.
        /// </summary>
        /// <param name="text">The text of the white card.</param>
        /// <returns>A white card cast as a Card.</returns>
        public static Card CreateWhite(string text, byte[] hash)
        {
            return new WhiteCard(text, hash);
        }

        /// <summary>
        /// A hash used to verify the authenticity of white cards.
        /// </summary>
        public virtual byte[] Hash { get; internal set; }
    }
}
