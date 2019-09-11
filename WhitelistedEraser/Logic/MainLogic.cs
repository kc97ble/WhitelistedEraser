using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhitelistedEraser.Util;

namespace WhitelistedEraser.Logic {
    public class MainLogic {

        public event EventHandler OnChange;
        private void Changed(object sender, EventArgs e) => OnChange(this, e);

        private string _workingDirectory;
        public string WorkingDirectory {
            get { return _workingDirectory; }
            set {
                if (_workingDirectory == value) return;
                _workingDirectory = value;
                FetchSubfolderPaths();
                OnChange(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<string> SubfolderPaths { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> WhitelistedSubfolderPaths { get; } = new ObservableCollection<string>();

        public void FetchSubfolderPaths() {
            if (_workingDirectory == "") {
                SubfolderPaths.Clear();
                return;
            }
            try {
                List<string> result = FileUtil.FetchAllDirectories(_workingDirectory);
                ListUtil.AssignList(SubfolderPaths, result);
                OnChange(this, EventArgs.Empty);
            } catch (Exception e) {
                SubfolderPaths.Clear();
                throw;
            }
        }

        private ObservableCollection<string> FilePaths { get; } = new ObservableCollection<string>();

        public MainLogic() {
            SubfolderPaths.CollectionChanged += Changed;
            WhitelistedSubfolderPaths.CollectionChanged += Changed;
        }
    }
}
