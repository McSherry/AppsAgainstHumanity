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
		protected internal List<Card> Cards = new List<Card>();
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
			PutAt(card, Cards.Count);
			card.Click += card_Click;
			Cards.Add(card);
			base.SuspendLayout();
			base.Controls.Add(card);
			base.ResumeLayout(true);
		}

		protected virtual void RecalculateSelectionIndices()
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
		internal void RemoveCard(Card card)
		{
			if (card.InvokeRequired) {
				Invoke(new Action<Card>(RemoveCard), card);
			} else {
				Cards.Remove(card);
				if(SelectedCards.Contains(card)){
					SelectedCards.Remove(card);
				}
				base.SuspendLayout();
				base.Controls.Remove(card);
				base.ResumeLayout(true);
			}
			ReflowCards();
		}
		private void PutAt(Card card, int index)
		{
			int x = index % CardsPerRow;
			int y = index / CardsPerRow;
			card.Location = new System.Drawing.Point(x * Card.CardFullWidth, y * Card.CardFullHeight);
		}
		protected void ReflowCards()
		{
			base.SuspendLayout();
			for (int i = 0; i < Cards.Count; i++) {
				Card c = Cards[i];
				PutAt(c, i);
			}
			base.ResumeLayout(true);
		}
		protected virtual void card_Click(object sender, EventArgs e)
		{
			ReflowCards();
            if (CanSelectCards) SelectCard(sender as Card);
		}

        public void SelectCard(Card c, bool force = false)
        {
            if (SelectedCards.Contains(c)) SelectedCards.Remove(c);
            else
            {
                if (SelectedCards.Count == MaxSelectNum && !force) SelectedCards.RemoveAt(0);
                else
                {
                    SelectedCards.Add(c);
                    c.SelectionIndex = SelectedCards.Count;
                    c.RegenerateCardText(true);
                }
            }
            RecalculateSelectionIndices();
        }
	}
}
