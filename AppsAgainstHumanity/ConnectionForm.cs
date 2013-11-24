//#define bypass
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
	public partial class ConnectionForm : Form
	{
		private NetworkInterface NetworkInterface;
		private MainForm MainForm;

		public ConnectionForm()
		{
			NetworkInterface = new NetworkInterface();
			InitializeComponent();
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
#if bypass
			ShowMainUI();
			return;
#else
			tbx_Nick.Enabled = false;
			tbx_Host.Enabled = false;
			btn_Connect.Enabled = false;
			try {
				await NetworkInterface.Connect(tbx_Host.Text, tbx_Nick.Text);
				ShowMainUI();
			} catch (System.Net.Sockets.SocketException ex) {
				MessageBox.Show(ex.Message, "Unable to connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
				tbx_Nick.Enabled = true;
				tbx_Host.Enabled = true;
				btn_Connect.Enabled = true;
			}
#endif
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
