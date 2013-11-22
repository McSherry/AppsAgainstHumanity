using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppsAgainstHumanity.Server
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

        private void button1_Click(object sender, EventArgs e)
        {
            string first = "McSherry";
            var rsa = new Crypto.Rsa();
            var keys = Crypto.Rsa.GenerateKeyPair();
            byte[] second = rsa.Sign(Encoding.UTF8.GetBytes(first), keys.Key);
            bool third = rsa.Verify(second, keys.Value, Encoding.UTF8.GetBytes(first));

            this.label1.Text = third.ToString();
        }
	}
}
