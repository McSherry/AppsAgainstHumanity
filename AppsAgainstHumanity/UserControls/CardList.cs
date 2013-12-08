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
		private List<Card> SelectedCards = new List<Card>();
		private int CardsPerRow;
		public bool CanSelectCards { get; set; }
		public int MaxSelectNum { get; set; }

		public CardList()
			: base()
		{
			base.AutoScroll = true;
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
			for (int i = 0; i < SelectedCards.Count; i++) {
				SelectedCards[i].SelectionIndex = i + 1;
			}
		}

		void card_Click(object sender, EventArgs e)
		{
			var card = (Card)sender;

			if (SelectedCards.Contains(card)) {
				SelectedCards.Remove(card);
			}
			SelectedCards.Add(card);
			card.SelectionIndex = SelectedCards.Count;
			card.BackColor = Color.FromArgb(225, 225, 255);
			RecalculateSelectionIndices();
		}
	}
}
