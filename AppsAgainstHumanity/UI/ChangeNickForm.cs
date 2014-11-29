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
	/// <summary>
	/// Displayed when the player wants to change their nickname.
	/// </summary>
	public partial class ChangeNickForm : Form
	{
		public ChangeNickForm()
		{
			InitializeComponent();
		}

		public string Nick { get; private set; }

		private void btn_Ok_Click(object sender, EventArgs e)
		{
			if (tbx_NewNick.Text == string.Empty) {
				MessageBox.Show(this, "Nickname field may not be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
			} else {
				Nick = tbx_NewNick.Text;
				DialogResult = DialogResult.OK;
				// TODO: Send NICK command and check for replies
			}
		}
	}
}
