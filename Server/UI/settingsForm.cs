using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppsAgainstHumanity.Server.UI
{
    public partial class settingsForm : Form
    {
        public settingsForm()
        {
            InitializeComponent();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(deckLocTBox.Text) || String.IsNullOrWhiteSpace(deckLocTBox.Text))
            {
                deckLocTBox.Text = Constants.DefaultDeckPath;
            }

            int portTPa = 0;
            if (!int.TryParse(portNumTBox.Text, out portTPa))
            {
                portTPa = Constants.DefaultPort;
            }

            if (portTPa < 1024) portTPa = Constants.DefaultPort;

            Settings.Create(deckLocTBox.Text, portTPa);

            this.Close();
        }

        private void settingsForm_Load(object sender, EventArgs e)
        {
            this.deckLocTBox.Text = Settings.DeckPath;
            this.portNumTBox.Text = Settings.Port.ToString();
        }

        private void deckLocBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog()
            {
                Description = "Select the folder you want Apps Against Humanity to look for decks / expansions in.",
                ShowNewFolderButton = false,
                SelectedPath = Settings.DeckPath
            };

            DialogResult fbdR = fbd.ShowDialog(this);

            if (fbdR == System.Windows.Forms.DialogResult.OK)
            {
                this.deckLocTBox.Text = fbd.SelectedPath;
            }

            fbd.Dispose();
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            deckLocTBox.Text = Constants.DefaultDeckPath;
            portNumTBox.Text = Constants.DefaultPort.ToString();
        }
    }
}
