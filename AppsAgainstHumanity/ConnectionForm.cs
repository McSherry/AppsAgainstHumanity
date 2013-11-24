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

		public ConnectionForm(NetworkInterface ni)
		{
			NetworkInterface = ni;
			InitializeComponent();
		}

		private void btn_Connect_Click(object sender, EventArgs e)
		{
			if (tbx_Nick.Text == string.Empty) {
				MessageBox.Show("Please enter a username", "No username specified", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			tbx_Nick.Enabled = false;
			tbx_Host.Enabled = false;
			try {
				NetworkInterface.Connect(tbx_Host.Text, tbx_Nick.Text);
			} catch (System.Net.Sockets.SocketException ex) {
				MessageBox.Show(ex.Message, "Unable to connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
				tbx_Nick.Enabled = true;
				tbx_Host.Enabled = true;
			}
		}
	}
}
