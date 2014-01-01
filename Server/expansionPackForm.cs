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
            expansionPackListBox.BeginUpdate();
            expansionPackListBox.Items.Clear();
            _cardPacks = new Dictionary<int, Deck>();
            int deckCtr = 0;

            try
            {
                foreach (string s in Directory.GetFiles(Settings.DeckPath, "*.xml"))
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        _cardPacks.Add(
                            deckCtr,
                            new Deck(sr.ReadToEnd())
                        );
                    }
                    deckCtr++;
                }
            }
            catch (UnauthorizedAccessException) { } // Protected directory, can't load
            catch (ArgumentException) { } // Invalid directory, can't load
            catch (DirectoryNotFoundException) { } // Directory doesn't exist, can't load

            // Remove from the card packs dictionary all items which are not addons.
            foreach (KeyValuePair<int, Deck> kvp in _cardPacks.ToList().Where(cp => cp.Value.Type != PackType.Addon))
            {
                _cardPacks.Remove(kvp.Key);
                deckCtr--;
            }

            for (int i = 0; i < deckCtr; i++)
            {
                this.expansionPackListBox.Items.Add(_cardPacks.ElementAt(i).Value.Name);
            }
            expansionPackListBox.EndUpdate();
        }

        public expansionPackForm()
        {
            InitializeComponent();

            _fillExpansionSelectCheckList();

            // Check whether any of the decks present in the list of decks this form will show
            // are present in a list of selected decks stored by the server's form.
            List<int> objectsToCheck = new List<int>();
            foreach (var box in expansionPackListBox.Items)
            {
                if (Program.MainForm._expansionDecks.Any(dk => dk.Name == box.ToString()))
                {
                    objectsToCheck.Add(expansionPackListBox.Items.IndexOf(box));
                }
            }

            foreach (int i in objectsToCheck)
            {
                expansionPackListBox.SetItemChecked(i, true);
            }
        }

        private void reloadDecksBtn_Click(object sender, EventArgs e)
        {
            _fillExpansionSelectCheckList();
        }

        private void saveSelectionBtn_Click(object sender, EventArgs e)
        {
            Program.MainForm._expansionDecks.Clear();
            foreach (var item in expansionPackListBox.Items)
            {
                if (expansionPackListBox.GetItemChecked(expansionPackListBox.Items.IndexOf(item)))
                {
                    Program.MainForm._expansionDecks.Add(_cardPacks.ElementAt(expansionPackListBox.Items.IndexOf(item)).Value);
                }
            }

            this.Close();
        }
    }
}
