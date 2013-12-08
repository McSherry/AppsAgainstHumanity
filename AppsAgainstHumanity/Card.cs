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
		public const int CardWidth = 120;
		public const int CardSpacing = 6;
		public const int CardFullWidth = CardWidth + CardSpacing;
		public const int CardFullHeight = CardFullWidth;
		public string Contents
		{
			get
			{
				return lbl_CardText.Text;
			}
			set
			{
				lbl_CardText.Text = value;
			}
		}
		public bool Selected
		{
			get;
			private set;
		}
		public bool Selectable { get; set; }

		public Card(bool selectable)
		{
			Selectable = selectable;
			InitializeComponent();
		}

		private void Card_Click(object sender, EventArgs e)
		{
			if (Selected) {
				BackColor = SystemColors.ControlLightLight;
			} else if(Selectable) {
				BackColor = Color.FromArgb(225, 225, 255);
			}
			Selected = !Selected;
		}
	}
}
