using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AppsAgainstHumanity.Server
{
    /// <summary>
    /// A class to manage all of the settings required by the Apps Against Humanity
    /// server.
    /// </summary>
    public static class Settings
    {
        // The name of the settings file.
        private const string _settingFileName = "Server.xml";
        // We'll need this to keep watch on the file.
        private static FileSystemWatcher _settingFileChecker;

        // If the settings file changes, we'll need to reload the settings. 
        private static void _fileChangeHandler(object sender, FileSystemEventArgs e)
        {
            
        }

        static Settings()
        {
            _settingFileChecker = new FileSystemWatcher(_settingFileName);
            _settingFileChecker.Changed += _fileChangeHandler;
        }
    }
}
