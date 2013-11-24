using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace AppsAgainstHumanityClient
{
	class CardList : Panel
	{
		private List<Card> Cards = new List<Card>();

		private int CardsPerRow;

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
			Cards.Add(card);
			base.SuspendLayout();
			base.Controls.Add(card);
			base.ResumeLayout(true);
		}
	}
}
