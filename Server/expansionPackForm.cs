using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppsAgainstHumanity.Server.Game;
using System.IO;

namespace AppsAgainstHumanity.Server
{
    public partial class expansionPackForm : Form
    {
        private Dictionary<int, Deck> _cardPacks;
        private void _fillExpansionSelectCheckList()
        {
            expansionPackListBox.Items.Clear();
            _cardPacks = new Dictionary<int, Deck>();
            string deckPath = "decks";
            int deckCtr = 0;

            foreach (string s in Directory.GetFiles(deckPath, "*.xml"))
            {
                try
                {
                    //using (StreamReader
                }
                catch (UnauthorizedAccessException) { }
            }
        }

        public expansionPackForm()
        {
            InitializeComponent();
        }
    }
}
