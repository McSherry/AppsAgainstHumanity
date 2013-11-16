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
		public Card(string contents)
		{
			InitializeComponent();
			lbl_CardText.Text = contents;
		}
	}
}
