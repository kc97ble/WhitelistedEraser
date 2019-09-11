using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace WhitelistedEraser.Util {
    class FileUtil {
        public static List<string> FetchAllDirectories(string path) {
            return Directory.GetDirectories(path, "*", SearchOption.AllDirectories).ToList();
        }

        public static List<string> FetchAllFiles(string path) {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();
        }

        [Obsolete]
        public static List<string> GetDirectories(string path, string searchPattern = "*",
            SearchOption searchOption = SearchOption.TopDirectoryOnly) {
            if (searchOption == SearchOption.TopDirectoryOnly)
                return Directory.GetDirectories(path, searchPattern).ToList();

            var directories = new List<string>(GetDirectories(path, searchPattern));

            for (var i = 0; i < directories.Count; i++)
                directories.AddRange(GetDirectories(directories[i], searchPattern));

            return directories;
        }

        [Obsolete]
        private static List<string> GetDirectories(string path, string searchPattern) {
            try {
                return Directory.GetDirectories(path, searchPattern).ToList();
            } catch (UnauthorizedAccessException) {
                return new List<string>();
            }
        }
    }
}
