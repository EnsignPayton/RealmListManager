using System;
using System.Diagnostics;
using System.IO;
using static RealmListManager.UI.Core.Utilities.FileUtilities;

namespace RealmListManager.UI.Core
{
    public class FileManager
    {
        private readonly IConfigurationManager _configurationManager;
        private Process _process;
        private string _path;

        public FileManager(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public event EventHandler ProcessExited;

        /// <summary>
        /// Starts an instance of the current location. If a realmlist is specified, uses it.
        /// </summary>
        /// <param name="path">Location Path</param>
        /// <param name="realmlistUrl">Realmlist URL</param>
        public bool StartLocation(string path, string realmlistUrl = null)
        {
            _path = path;

            if (realmlistUrl != null)
            {
                BackupRealmlist(path);
                ReplaceRealmlist(path, realmlistUrl);
            }

            try
            {
                _process = new Process();
                _process.StartInfo.FileName = Path.Combine(path, "Wow.exe");

                if (_configurationManager.RestoreRealmlist)
                {
                    _process.EnableRaisingEvents = true;
                    _process.Exited += Process_Exited;
                }

                _process.Start();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            if (_configurationManager.RestoreRealmlist)
            {
                RestoreRealmlist(_path);
            }

            ProcessExited?.Invoke(sender, e);
        }
    }
}
