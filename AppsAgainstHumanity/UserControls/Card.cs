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
	public partial class Card : UserControl
	{
		public Card()
		{
			InitializeComponent();
		}
		public Card(string text = null, string id = null)
			:base()
		{
			InitializeComponent();
			Id = id;
			CardText = text;
		}

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
		protected internal void RegenerateCardText(bool noNumber = false)
		{
            if (lbl_CardText.InvokeRequired) lbl_CardText.Invoke(new Action<bool>(RegenerateCardText), noNumber);
            else
            {
                string cardAppend = SelectionIndex == 0 ? String.Empty : String.Format(" ({0})", SelectionIndex);
                cardAppend = noNumber ? String.Empty : cardAppend;
                lbl_CardText.Text = cardText + cardAppend;
            }
		}
		public string Id
		{
			get;
			private set;
		}
		public override string ToString()
		{
			return Id + ": " + CardText;
		}
	}
}
