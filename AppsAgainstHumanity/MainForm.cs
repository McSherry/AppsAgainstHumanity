using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CsNetLib2;

namespace AppsAgainstHumanityClient
{
	public partial class MainForm : Form
	{
		private NetworkInterface NetworkInterface;
		private CommandProcessor CommandProcessor;

		public MainForm(NetworkInterface networkInterface)
		{
			NetworkInterface = networkInterface;
			CommandProcessor = new CommandProcessor();
			networkInterface.RegisterCommandHandler(CommandType.REFU, CommandProcessor.ProcessREFU);
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

		private void AddChatLine(string line)
		{
			tbx_ChatLog.AppendText( "\r\n" + line);
		}

		private void SendChatMessage(string message)
		{
			AddChatLine(message);
		}

		private void tbx_Chat_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && tbx_Chat.Text != string.Empty) {
				SendChatMessage(tbx_Chat.Text);
				tbx_Chat.Text = string.Empty;
			}
		}
	}
}
