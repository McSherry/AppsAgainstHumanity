using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;

namespace AppsAgainstHumanity.Server
{
    /// <summary>
    /// A class to manage all of the settings required by the Apps Against Humanity
    /// server.
    /// </summary>
    public static class Settings
    {
        // We'll need this to keep watch on the file.
        private static FileSystemWatcher _settingFileChecker;
        private static FileStream _settingsFileStream;

        // If the settings file changes, we'll need to reload the settings. 
        private static void _fileChangeHandler(object sender, FileSystemEventArgs e)
        {
            try
            {
                // We need to temporarily disable the ability to raise events as,
                // otherwise, it'll raise two events.
                if (_settingFileChecker != null) _settingFileChecker.EnableRaisingEvents = false;

                try
                {
                    Load();
                }
                catch (FileNotFoundException) { Create(Constants.SettingsFile, true); Load(); }
                catch (XmlException) { Create(Constants.SettingsFile, true); Load(); }
                catch (InvalidDataException) { Create(); Load(); }
            }
            finally
            {
                if (_settingFileChecker != null) _settingFileChecker.EnableRaisingEvents = true;
            }
        }

        static Settings()
        {
            _fileChangeHandler(null, null);

            _settingsFileStream = File.Open(Constants.SettingsFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            //_settingFileChecker = new FileSystemWatcher(Environment.CurrentDirectory, Constants.SettingsFile);
            //_settingFileChecker.Changed += _fileChangeHandler;
            //_settingFileChecker.Deleted += _fileChangeHandler;
            //_settingFileChecker.EnableRaisingEvents = true;
        }

        /// <summary>
        /// The path to the directory the server should search for decks in.
        /// </summary>
        public static string DeckPath { get; private set; }
        /// <summary>
        /// The TCP port the server should listen for connections on.
        /// </summary>
        public static int Port { get; private set; }

        /// <summary>
        /// Attempts to load the settings file and interpret its content.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        /// <exception cref="System.IO.InvalidDataException"></exception>
        public static void Load()
        {
            if (!File.Exists(Constants.SettingsFile)) throw new FileNotFoundException("The server configuration file could not be found.");
            else
            {
                XmlDocument xmd = new XmlDocument();
                xmd.Load(Constants.SettingsFile);

                DeckPath = xmd.SelectSingleNode("/aah/deckpath").InnerText;
                try
                {
                    Port = int.Parse(xmd.SelectSingleNode("/aah/port").InnerText);
                }
                catch (FormatException)
                {
                    Port = Constants.DefaultPort;
                }
                finally
                {
                    // Ports under 1024 are reserved by IANA. If the user's
                    // trying to configure a port below that, we'll default
                    // to 11235 instead, since we shouldn't use reserved
                    // ports.
                    Port = Port < 1024 ? Constants.DefaultPort : Port;
                }
            }
        }

        public static void Create(string fileName = Constants.SettingsFile, bool overrideCurrentSettings = false)
        {
            if (overrideCurrentSettings)
            {
                XDocument setXd = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement(
                        "aah",
                        new XElement("port", Constants.DefaultPort),
                        new XComment("If the deck path is misconfigured, AAH will load a stored copy of the US Edition."),
                        new XElement("deckpath", Constants.DefaultDeckPath)
                    )
                );

                setXd.Save(Constants.SettingsFile);
            }
            else
            {
                XDocument setXd = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement(
                        "aah",
                        new XElement("port", Port < 1024 ? Constants.DefaultPort : Port),
                        new XComment("If the deck path is misconfigured, AAH will load a stored copy of the US Edition."),
                        new XElement("deckpath", DeckPath ?? Constants.DefaultDeckPath)
                    )
                );
            }
        }
    }
}
