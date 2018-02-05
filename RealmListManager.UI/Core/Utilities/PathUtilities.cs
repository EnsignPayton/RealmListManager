using System.IO;
using System.Linq;

namespace RealmListManager.UI.Core.Utilities
{
    public static class PathUtilities
    {
        public static bool IsWowFolder(string path)
        {
            if (!Path.IsPathRooted(path)) return false;
            if (!Directory.Exists(path)) return false;

            var info = new DirectoryInfo(path);
            return info.GetFiles("Wow.exe").Any();
        }
    }
}
