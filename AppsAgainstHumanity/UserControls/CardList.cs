using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;

namespace AppsAgainstHumanityClient
{
	class CardList : Panel
	{
		private List<Card> Cards = new List<Card>();
		public List<Card> SelectedCards { get; private set; }
		private int CardsPerRow;
		private bool canSelectCards;
		public bool CanSelectCards
		{
			get
			{
				return canSelectCards;
			}
			set
			{
				canSelectCards = value;
			}
		}
		public int MaxSelectNum { get; set; }

		public CardList()
			: base()
		{
			SelectedCards = new List<Card>();
			base.AutoScroll = true;
			CanSelectCards = true;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			int width = Width - SystemInformation.VerticalScrollBarWidth;

			if (width < Card.CardFullWidth) {
				throw new InvalidOperationException("Size may not be less than card width.");
			}
			CardsPerRow = 1 + ((width - Card.CardWidth) / Card.CardFullWidth);
		}

		public void AddCard(Card card)
		{
			int x = Cards.Count % CardsPerRow;
			int y = Cards.Count / CardsPerRow;
			card.Location = new System.Drawing.Point(x * Card.CardFullWidth, y * Card.CardFullHeight);
			card.Click += card_Click;
			Cards.Add(card);
			base.SuspendLayout();
			base.Controls.Add(card);
			base.ResumeLayout(true);
		}

		private void RecalculateSelectionIndices()
		{
			for (int i = 0; i < Cards.Count; i++) {
				Card c = Cards[i];
				if (SelectedCards.Contains(c)) {
					c.SelectionIndex = SelectedCards.IndexOf(c) + 1;
					SelectedCards[c.SelectionIndex-1].BackColor = Color.FromArgb(225, 225, 255);
				} else {
					c.SelectionIndex = 0;
					c.BackColor = SystemColors.ControlLightLight;
				}
			}
		}

		void card_Click(object sender, EventArgs e)
		{
			if (CanSelectCards) {
				var card = (Card)sender;
				if (SelectedCards.Contains(card)) {
					SelectedCards.Remove(card);
				} else if (SelectedCards.Count == MaxSelectNum) {
					SelectedCards.RemoveAt(0);
				}
				SelectedCards.Add(card);
				card.SelectionIndex = SelectedCards.Count;
				RecalculateSelectionIndices();
			}
		}
	}
}
