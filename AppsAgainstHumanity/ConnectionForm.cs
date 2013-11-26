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
	public partial class ConnectionForm : Form
	{
		internal delegate void SetConnectButtonStateCallback(bool state);

		private NetworkInterface NetworkInterface;

		public ConnectionForm()
		{
			NetworkInterface = new NetworkInterface();
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.ACKN, ProcessACKN);
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.REFU, ProcessREFU);
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.NDNY, ProcessNDNY);
			InitializeComponent();
		}
		private void ProcessACKN(long sender, string[] arguments)
		{

		}

		private void SetConnectButtonState(bool state)
		{
			if (btn_Connect.InvokeRequired) {
				Invoke(new SetConnectButtonStateCallback(SetConnectButtonState), new object[] { state });
			} else {
				btn_Connect.Enabled = state;
			}
		}

		private void ProcessREFU(long sender, string[] arguments)
		{
			MessageBox.Show(arguments[0], "Unable to connect", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			NetworkInterface.Disconnect();
			SetConnectButtonState(true);
		}
		private void ProcessNDNY(long sender, string[] arguments)
		{
			MessageBox.Show(arguments[0], "Server denies nickname", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			NetworkInterface.Disconnect();
			SetConnectButtonState(true);
		}

		private async void btn_Connect_Click(object sender, EventArgs e)
		{
			if (tbx_Nick.Text == string.Empty) {
				MessageBox.Show("Please enter a username", "No username specified", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			if (tbx_Nick.Text.Length > 20) {
				MessageBox.Show("Username may not be longer than 20 characters", "Invalid username", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			btn_Connect.Enabled = false;
			try {
				await NetworkInterface.Connect(tbx_Host.Text, tbx_Nick.Text);
			} catch (System.Net.Sockets.SocketException ex) {
				MessageBox.Show(ex.Message, "Unable to connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
				btn_Connect.Enabled = true;
			}
		}

		private void HandleNickDeny(string reason)
		{

		}
		private void HandleNickAccept()
		{

		}
		private void ShowMainUI()
		{
			base.Hide();
			new MainForm(NetworkInterface).Show();
		}
		private void btn_Exit_Click(object sender, EventArgs e)
		{
			NetworkInterface.Disconnect();
			base.Close();
		}
	}
}
