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
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			var form = new ConnectionForm();
			var result = form.ShowDialog();
			if (result == DialogResult.OK) {
				// TODO: Start the game
			} else {
				base.Close(); // User pressed cancel
			}
		}
	}
}
