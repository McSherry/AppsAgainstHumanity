using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace AppsAgainstHumanity.Server.Game
{
    /// <summary>
    /// An enum representing the type of card pack detected.
    /// </summary>
    public enum PackType
    {
        /// <summary>
        /// A full pack of cards. Usually contains >500 cards.
        /// </summary>
        Pack = 0,
        /// <summary>
        /// An addon/expansion pack of cards. Usually contains 100 cards.
        /// </summary>
        Addon = 1
    }

    /// <summary>
    /// Represents a deck of cards.
    /// </summary>
    public class Deck
    {
        // Class doesn't support loading addons yet. Will do in future, just not now.

        /// <summary>
        /// Determines the pack type of the given XML document.
        /// </summary>
        /// <param name="xml">The XML to retrieve the pack type from.</param>
        /// <returns>The pack type of the given document.</returns>
        private PackType _determinePackType()
        {
            return (PackType)Enum
                .Parse(
                    typeof(PackType),
                    _xmd.SelectNodes("/deck").Item(0).Attributes["type"].Value
                );
        }
        private XmlDocument _xmd;

        /// <summary>
        /// Creates a new deck.
        /// </summary>
        public Deck() { }
        /// <summary>
        /// Creates a new deck from XML.
        /// </summary>
        /// <param name="deckXml">The XML of a main deck to create this class from.</param>
        public Deck(string deckXml)
        {
            _xmd = new XmlDocument();
            _xmd.LoadXml(deckXml);

            this.WhiteCards = new List<WhiteCard>(int.Parse(_xmd.SelectNodes("/deck/count").Item(0).Attributes["white"].Value));
            this.BlackCards = new List<BlackCard>(int.Parse(_xmd.SelectNodes("/deck/count").Item(0).Attributes["black"].Value));

            try
            {
                if (_determinePackType() == PackType.Addon)
                    throw new FormatException
                    ("The provided XML document represents an addon pack and not a main card pack.");
                else
                {
                    this.Name = _xmd.SelectNodes("/deck/name").Item(0).InnerText;
                    foreach (XmlNode xmn in _xmd.SelectNodes("/deck/cards/white/card"))
                    {
                        // Select white cards from file and load into list.
                        // White cards all reside as a <card> element which is
                        // the child of a <white> element.
                        this.WhiteCards.Add(new WhiteCard(xmn.InnerText));
                    }
                    foreach (XmlNode xmn in _xmd.SelectNodes("/deck/cards/black/card"))
                    {
                        int pick = 0;
                        try
                        {
                            pick = int.Parse(xmn.Attributes["pick"].Value);
                        }
                        catch (FormatException fex)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                String.Format(
                                    "Pick value for card \"{0}\" is invalid: \n\n{1}",
                                    xmn.InnerText, fex.Message
                                    )
                                );
                        }

                        // Extra draw is considered true if attribute is present when its value is not false.
                        // If not present, or if its attribute value is false, it is considered false.
                        bool extraDraw = false;
                        if (xmn.Attributes["xd"] != null)
                            if (xmn.Attributes["xd"].Value.ToLower() == "false") extraDraw = false;
                            else extraDraw = true;
                        else if (xmn.Attributes["xd"] == null)
                            extraDraw = false;
                        // Select all black cards from file and load into list.
                        // Black cards are under a <black> element as a <card>
                        // element with the "pick" attribute and optional
                        // "xd" attribute.
                        this.BlackCards.Add(
                            new BlackCard(
                                xmn.InnerText,
                                pick,
                                // By default, a black card allows a player to draw a single (1) card
                                // However, some require a player draws two cards. If a black card has
                                // the "xd" (eXtra Draw) attribute, two cards should be drawn.
                                extraDraw ? 2 : 1
                                )
                            );
                    }
                }
            }
            catch (XmlException xex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "An XmlException Occured:\n\n" +
                    xex.Message
                    );
            }
        }

        /// <summary>
        /// Extends the number of cards in a deck using addon packs.
        /// </summary>
        /// <param name="addonXml">The XML of the addon pack file.</param>
        public void ExtendDeck(string addonXml)
        {
            throw new NotImplementedException
            ("This functionality is not currently implemented.");
        }

        /// <summary>
        /// The white cards present in this deck.
        /// </summary>
        public List<WhiteCard> WhiteCards { get; private set; }
        /// <summary>
        /// The black cards present in this deck.
        /// </summary>
        public List<BlackCard> BlackCards { get; private set; }

        /// <summary>
        /// The name of the main card pack of the deck.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The names of any configured addon packs in this deck.
        /// </summary>
        public List<string> AddonPacks { get; private set; }
    }
}
