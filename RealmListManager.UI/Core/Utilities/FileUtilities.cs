using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RealmListManager.UI.Core.Utilities
{
    public static class FileUtilities
    {
        private const string WowExecutable = "Wow.exe";
        private const string RealmlistWtf = "realmlist.wtf";

        /// <summary>
        /// Determines if a path is a World of Warcraft installation directory.
        /// </summary>
        /// <param name="path">Location Path</param>
        public static bool IsWowFolder(string path)
        {
            if (!Path.IsPathRooted(path)) return false;
            if (!Directory.Exists(path)) return false;

            var info = new DirectoryInfo(path);
            return info.GetFiles(WowExecutable).Any();
        }

        /// <summary>
        /// Determines if a path contains a running instance of World of Warcraft.
        /// </summary>
        /// <param name="path">Location Path</param>
        public static bool IsLocationRunning(string path)
        {
            var wowProcesses = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(WowExecutable));

            return wowProcesses.FirstOrDefault(x => x.MainModule.FileName.StartsWith(path,
                StringComparison.OrdinalIgnoreCase)) != null;
        }

        /// <summary>
        /// Backs up the current realmlist in a location.
        /// </summary>
        /// <param name="path">Location Path</param>
        /// <returns>Success Status</returns>
        public static bool BackupRealmlist(string path)
        {
            var realmlistWtf = Path.Combine(path, RealmlistWtf);
            var realmlistBak = Path.GetFileNameWithoutExtension(realmlistWtf) + ".bak";

            if (!File.Exists(realmlistWtf)) return false;

            File.Copy(realmlistWtf, realmlistBak);

            return true;
        }

        /// <summary>
        /// Restores a backup of the current realmlist in a location.
        /// </summary>
        /// <param name="path">Location Path</param>
        /// <returns>Success Status</returns>
        public static bool RestoreRealmlist(string path)
        {
            var realmlistWtf = Path.Combine(path, RealmlistWtf);
            var realmlistBak = Path.GetFileNameWithoutExtension(realmlistWtf) + ".bak";

            if (!File.Exists(realmlistBak)) return false;

            File.Copy(realmlistBak, realmlistWtf);

            return true;
        }

        /// <summary>
        /// Starts an instance of the current location with the current realmlist.
        /// </summary>
        /// <param name="path">Location Path</param>
        public static void StartLocation(string path)
        {
            Process.Start(Path.Combine(path, WowExecutable));
        }
    }
}
