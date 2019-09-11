using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhitelistedEraser {
    public class Data {

        public EventHandler OnChange { get; set; }
        private void Changed() { OnChange?.Invoke(this, EventArgs.Empty); }
        private void Changed(object sender, EventArgs e) { OnChange?.Invoke(this, e); }

        private string workingDirectory;
        public string WorkingDirectory {
            get { return workingDirectory; }
            set {
                if (workingDirectory == value) return;
                workingDirectory = value;
                FetchSubfolderPaths();
                Changed();
            }
        }

        public ObservableCollection<string> SubfolderPaths { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> CheckedSubfolderPaths { get; } = new ObservableCollection<string>();

        public void FetchSubfolderPaths() {
            try {
                List<string> result = CustomSearcher.GetDirectories(workingDirectory, searchOption: System.IO.SearchOption.AllDirectories);
                Util.AssignList(SubfolderPaths, result);
            } catch (Exception e) {
                Console.Error.WriteLine(e.Message);
                SubfolderPaths.Clear();
            }   
        }

        private ObservableCollection<string> FilePaths { get; } = new ObservableCollection<string>();

        public Data() {
            SubfolderPaths.CollectionChanged += new NotifyCollectionChangedEventHandler(Changed);
            CheckedSubfolderPaths.CollectionChanged += new NotifyCollectionChangedEventHandler(Changed);
        }
    }
}
