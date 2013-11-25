using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppsAgainstHumanity.Server.Crypto;
using CsNetLib2;
using AppsAgainstHumanity.Server.Game;
using System.IO;

namespace AppsAgainstHumanity.Server
{
	public partial class Form1 : Form
	{
        public const byte ETX = 3;

		//private NetLibServer Server;
		//private AAHProtocolWrapper ServerWrapper;
        Game.Game game;

		public Form1()
		{
			InitializeComponent();
            string xml = String.Empty;

            using (StreamReader sr = new StreamReader("UK.xml"))
            {
                xml = sr.ReadToEnd();
            }
            GameParameters gp = new GameParameters() {
                Players = 10,
                Cards = new Deck(xml)
            };
            game = new Game.Game(gp);

            /*Server = new NetLibServer(11235, TransferProtocol.Delimited);
			Server.Delimiter = ETX;
            ServerWrapper = new AAHProtocolWrapper(Server);
			ServerWrapper.RegisterCommandHandler(CsNetLib2.CommandType.JOIN, (sender, arguments) =>
			{
				MessageBox.Show(String.Format("Client #{0} attempts to connect with nickname {1}", sender, arguments[0]));
			});

			Server.Start();

            Server.OnDataAvailable += (s2, e2) =>
            {
                MessageBox.Show("Data received!");
            };
            Server.OnBytesAvailable += (s2, e2) =>
            {
                MessageBox.Show("Bytes received!");
            };*/
		}

        private void button1_Click(object sender, EventArgs e)
        {
            throw new Exception();
            Random rnd = new Random((int)(new SRand() & 0xFFFF));
            int[] randoms = new int[600];

            for (int i = 0; i < randoms.Length; i++)
                randoms[i] = rnd.Next();

            int ctr = -randoms.Length;
            foreach (int n in randoms)
                foreach (int m in randoms)
                    if (m == n) ++ctr;
        }
	}
}
