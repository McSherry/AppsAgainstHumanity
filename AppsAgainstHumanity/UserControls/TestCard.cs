using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppsAgainstHumanityClient
{
	public partial class TestCard : UserControl
	{
		public const int CardWidth = 120;
		public const int CardSpacing = 6;
		public const int CardFullWidth = CardWidth + CardSpacing;
		public const int CardFullHeight = CardFullWidth;
		private string cardText;
		public string CardText
		{
			get { return cardText; }
			set
			{
				cardText = value;
				RegenerateCardText();
			}
		}
		private int selectionIndex;
		public int SelectionIndex
		{
			get { return selectionIndex; }
			set
			{
				selectionIndex = value;
				RegenerateCardText();
			}
		}
		private void RegenerateCardText()
		{
			lbl_CardText.Text = cardText + (SelectionIndex == 0 ? "" : string.Format(" ({0})", SelectionIndex));
		}
		public string Id
		{
			get;
			private set;
		}

		public TestCard()
		{
			InitializeComponent();
		}
		public TestCard(string text = null, string id = null)
			:base()
		{
			InitializeComponent();
			Id = id;
			CardText = text;
		}

		/*private void Card_Click(object sender, EventArgs e)
		{
			if (Selected) {
				BackColor = SystemColors.ControlLightLight;
			} else if(Selectable) {
				BackColor = Color.FromArgb(225, 225, 255);
			}
			Selected = !Selected;
		}*/
	}
}
