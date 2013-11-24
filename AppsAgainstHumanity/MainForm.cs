using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppsAgainstHumanityClient
{
	public partial class MainForm : Form
	{
		private NetworkInterface NetworkInterface;

		public MainForm(NetworkInterface networkInterface)
		{
			NetworkInterface = networkInterface;
			InitializeComponent();
		}
		private void InitializeGame()
		{
			for (int i = 0; i < 13; i++) {
				var card = new Card();
				card.Contents = SystemInformation.VerticalScrollBarWidth.ToString();
				crl_PickedCards.AddCard(card);
			}
			sts_GameStatus.Text = "My life is hollow inside";
		}
	}
}
