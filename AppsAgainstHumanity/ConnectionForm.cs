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
			NetworkInterface.Connect(tbx_Host.Text, tbx_Nick.Text);
		}
	}
}
