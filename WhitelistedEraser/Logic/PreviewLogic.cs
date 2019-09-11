using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhitelistedEraser.Logic {

    public class PreviewLogic {
        private readonly string WorkingDirectory;
        private readonly List<string> SubfolderPaths;
        private readonly List<string> WhitelistedSubfolderPaths;

        public PreviewLogic(string workingDirectory, IList<string> subfolderPaths, IList<string> whilelistedSubfolderPaths) {
            WorkingDirectory = workingDirectory;
            SubfolderPaths = new List<string>(subfolderPaths);
            WhitelistedSubfolderPaths = new List<string>(whilelistedSubfolderPaths);
        }

        public event EventHandler OnChange;
        public List<string> BlacklistedFilePaths { get; } = new List<string>();
        public List<string> WhitelistedFilePaths { get; } = new List<string>();

        private readonly char _delim = System.IO.Path.DirectorySeparatorChar;

        private bool _isFileInFolder(string filePath, string folderPath) {
            if (filePath.Length > folderPath.Length && filePath.StartsWith(folderPath)) {
                return filePath[folderPath.Length] == _delim || folderPath.Last() == _delim;
            }
            return false;
        }

        private string _basename(string file) => System.IO.Path.GetFileNameWithoutExtension(file);

        private List<string> _getWhitelistedFilePaths(List<string> filePaths) {
            List<string> whiteFolders = WhitelistedSubfolderPaths
                .Where(path => _isFileInFolder(path, WorkingDirectory))
                .ToList();
            List<string> whiteBases = filePaths
                .Where(file => whiteFolders.Any(folder => _isFileInFolder(file, folder)))
                .Select(file => _basename(file))
                .ToList();
            List<string> result = filePaths
                .Where(file => whiteBases.Contains(_basename(file)))
                .ToList();
            return result;
        }

        public void Reload() {
            var filePaths = Util.FileUtil.FetchAllFiles(WorkingDirectory);
            var whitePaths = _getWhitelistedFilePaths(filePaths);
            var blackPaths = filePaths.Where(path => !whitePaths.Contains(path)).ToList();
            Util.ListUtil.AssignList(WhitelistedFilePaths, whitePaths);
            Util.ListUtil.AssignList(BlacklistedFilePaths, blackPaths);
            OnChange(this, EventArgs.Empty);
        }
    }
}
