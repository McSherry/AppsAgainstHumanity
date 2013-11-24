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

		public MainForm()
		{
			NetworkInterface = new NetworkInterface();
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			InitializeGame();
			/*var form = new ConnectionForm(NetworkInterface);
			var result = form.ShowDialog();
			if (result == DialogResult.OK) {
				InitializeGame();
			} else {
				base.Close(); // User pressed cancel or in any other way did not connect to the server. Close the application.
			}*/
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
