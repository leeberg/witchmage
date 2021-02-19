using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WitchMage
{
    public static class SetupFiles
    {


        public static string SettingsRelativePath = GetCurrentDomain();
        public static string ConfigFile = SettingsRelativePath + "/setup.json";

        public static string SettingsFolder =>
        GetFolderSafe(SettingsRelativePath);

        public static string CurrentFolder =>
        GetCurrentDomain();

        private static string GetFolderSafe(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        private static string GetCurrentDomain()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
