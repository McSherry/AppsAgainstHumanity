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
		private Game Game = new Game();

		public delegate void UpdatePlayerListCallback();

		public MainForm(NetworkInterface networkInterface)
		{
			NetworkInterface = networkInterface;
			// We no longer want the login form to handle commands
			NetworkInterface.ClientWrapper.UnregisterAllHandlers();
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.CLNF, (sender, arguments) =>
			{
				Game.AddPlayers(arguments);
				UpdatePlayerList();
			});
			// Check for any commands that got queued up while the connection form was being displayed
			while (NetworkInterface.ClientWrapper.HasQueuedCommand) {
				var cmd = NetworkInterface.ClientWrapper.GetNextQueuedCommand();
				if (cmd.Type == CommandType.CLNF) {
					Game.AddPlayers(cmd.Arguments);
					UpdatePlayerList();
				} else {
					throw new InvalidOperationException("Command type " + cmd.Type.ToString() + " was unexpected at this time.");
				}
			}
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

		protected override void OnClosing(CancelEventArgs e)
		{
			//NetworkInterface.ClientWrapper.SendCommand(CommandTyp
			base.OnClosing(e);
		}

		private void UpdatePlayerList()
		{
			if (lb_Players.InvokeRequired) {
				Invoke(new UpdatePlayerListCallback(UpdatePlayerList));
			} else {
				lb_Players.Items.Clear();
				foreach (Player p in Game.Players) {
					lb_Players.Items.Add(p);
				}
			}
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
