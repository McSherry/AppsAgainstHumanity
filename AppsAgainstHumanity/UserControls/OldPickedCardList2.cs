using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AppsAgainstHumanityClient.UserControls
{
	class OldPickedCardList : CardList
	{
		public int GroupNumber { get; set; }

		public OldPickedCardList()
			: base()
		{
		}

		protected override void RecalculateSelectionIndices()
		{
			for (int i = 0; i < Cards.Count; i++) {
				Card c = Cards[i];
				if (SelectedCards.Contains(c)) {
					c.BackColor = Color.FromArgb(225, 225, 255);
				} else {
					c.BackColor = SystemColors.ControlLightLight;
				}
			}
		}

		protected override void card_Click(object sender, EventArgs e)
		{
			if (CanSelectCards) {
				var card = (Card)sender;
				if (SelectedCards.Count != 0) {
					SelectedCards.Clear();
				}
				int index = Cards.IndexOf(card);
				// Integer division will return the index of the first location divided by GroupNumber.
				// Then we multiply by GroupNumber to get the index of the first location.
				int start = (index / GroupNumber) * GroupNumber;
				// After this, we can simply select the card for every next index.
				for (int i = start; i < start + GroupNumber; i++) {
					SelectedCards.Add(Cards[i]);
				}
				RecalculateSelectionIndices();
			}
		}

		internal void Clear()
		{
			if (Cards.Count != 0 && Cards[0].InvokeRequired) {
				Invoke(new Action(Clear));
			} else {
				base.SuspendLayout();
				while (Cards.Count != 0) {
					Card c = Cards[0];
					Cards.RemoveAt(0);
					base.Controls.Remove(c);
				}
				base.ResumeLayout(true);
			}
		}
	}
}
