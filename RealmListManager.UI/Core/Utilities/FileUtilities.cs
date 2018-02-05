using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RealmListManager.UI.Core.Utilities
{
    public static class FileUtilities
    {
        /// <summary>
        /// Determines if a path is a World of Warcraft installation directory.
        /// </summary>
        /// <param name="path">Location Path</param>
        public static bool IsWowFolder(string path)
        {
            if (!Path.IsPathRooted(path)) return false;
            if (!Directory.Exists(path)) return false;

            var info = new DirectoryInfo(path);
            return info.GetFiles("Wow.exe").Any();
        }

        /// <summary>
        /// Determines if a path contains a running instance of World of Warcraft.
        /// </summary>
        /// <param name="path">Location Path</param>
        public static bool IsLocationRunning(string path)
        {
            var wowProcesses = Process.GetProcessesByName("Wow");

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
            var realmlistWtf = GetRealmlistFile(path);
            var realmlistBak = Path.Combine(Path.GetDirectoryName(realmlistWtf), Path.GetFileNameWithoutExtension(realmlistWtf) + ".bak");

            if (!File.Exists(realmlistWtf)) return false;

            File.Copy(realmlistWtf, realmlistBak, true);

            return true;
        }

        /// <summary>
        /// Restores a backup of the current realmlist in a location.
        /// </summary>
        /// <param name="path">Location Path</param>
        /// <returns>Success Status</returns>
        public static bool RestoreRealmlist(string path)
        {
            var realmlistWtf = GetRealmlistFile(path);
            var realmlistBak = Path.GetFileNameWithoutExtension(realmlistWtf) + ".bak";

            if (!File.Exists(realmlistBak)) return false;

            File.Copy(realmlistBak, realmlistWtf);

            return true;
        }

        /// <summary>
        /// Replaces the current realmlist.
        /// </summary>
        /// <param name="path">Location Path</param>
        /// <param name="realmlistUrl">Realmlist URL</param>
        public static void ReplaceRealmlist(string path, string realmlistUrl)
        {
            var realmlistWtf = GetRealmlistFile(path);
            var realmlistData = File.ReadAllLines(realmlistWtf);

            for (int i = 0; i < realmlistData.Length; i++)
            {
                if (!realmlistData[i].StartsWith("set realmlist")) continue;
                realmlistData[i] = $"set realmlist {realmlistUrl}";
            }

            File.WriteAllLines(realmlistWtf, realmlistData);
        }

        /// <summary>
        /// Starts an instance of the current location. If a realmlist is specified, uses it.
        /// </summary>
        /// <param name="path">Location Path</param>
        /// <param name="realmlistUrl">Realmlist URL</param>
        public static void StartLocation(string path, string realmlistUrl = null)
        {
            if (realmlistUrl != null)
            {
                BackupRealmlist(path);
                ReplaceRealmlist(path, realmlistUrl);
            }

            Process.Start(Path.Combine(path, "Wow.exe"));
        }

        private static string GetRealmlistFile(string path)
        {
            // Prior to 3.0.2, realmlist.wtf should be located in the root directory
            var oldLocation = Path.Combine(path, "realmlist.wtf");
            if (File.Exists(oldLocation)) return oldLocation;

            // Afterward, it should be located in a subdirectory
            var subdirs = Directory.GetDirectories(Path.Combine(path, "Data"));

            foreach (var subdir in subdirs)
            {
                var newLocation = Path.Combine(subdir, "realmlist.wtf");
                if (File.Exists(newLocation)) return newLocation;
            }

            // If it's in neither place, then we're out of luck
            return null;
        }
    }
}
