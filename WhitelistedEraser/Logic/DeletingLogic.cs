using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhitelistedEraser.Logic {
    public enum DeletingStatus { Ready, Doing, Cancelling, Cancelled, Done };

    public class DeletingLogic {
        // Input
        public readonly List<string> Files;
        public event EventHandler OnChange;

        // Output
        public DeletingStatus Status { get; private set; } = DeletingStatus.Ready;
        public int CompletedFileCount { get; private set; } = 0;
        public List<string> Log { get; private set; } = new List<string>();

        // Other
        private BackgroundWorker _worker = new BackgroundWorker();

        public DeletingLogic(List<string> files) {
            Files = files;
            Log.Add(string.Format("{0} file(s) will be PERMANENTLY deleted.", Files.Count));
            Log.Add("This operation cannot be undone.");
            Log.Add("Press Start to proceed.");
        }

        private void _doWork(object sender, DoWorkEventArgs e) {
            BackgroundWorker worker = sender as BackgroundWorker;
            Action<string> report = msg => { Log.Add(msg); worker.ReportProgress(Log.Count); };
            int successCount = 0, failedCount = 0;

            Status = DeletingStatus.Doing;
            report(string.Format("Deleting {0} file(s)", Files.Count));

            foreach (string file in Files) {
                if (worker.CancellationPending == true) {
                    e.Cancel = true;
                    Status = DeletingStatus.Cancelled;
                    report(string.Format("Aborted by user. Success: {0}. Failed: {1}.", successCount, failedCount));
                    break;
                }
                try {
                    report(string.Format("Deleting {0}...", file));
                    System.IO.File.Delete(file);
                    //System.Threading.Thread.Sleep(2000);
                    //if (Environment.TickCount % 100 != 0) {
                    //    throw new Exception("yay");
                    //}
                    CompletedFileCount += 1;
                    successCount += 1;
                    report("Deleted");
                } catch (Exception ex) {
                    CompletedFileCount += 1;
                    failedCount += 1;
                    report(string.Format("Failed. Error: {0}", ex.Message));
                }
            }
            if (Status != DeletingStatus.Cancelled) {
                Status = DeletingStatus.Done;
                report(string.Format("Done. Success: {0}. Failed: {1}.", successCount, failedCount));
            }
        }

        private void _progressChanged(object sender, ProgressChangedEventArgs e) {
            Console.WriteLine(3876387543);
            OnChange(this, EventArgs.Empty);
        }

        public void Start() {
            if (Status != DeletingStatus.Ready)
                throw new Exception("Can only call if progress = Ready");

            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += _doWork;
            _worker.ProgressChanged += _progressChanged;
            _worker.RunWorkerAsync();
        }

        public void Stop() {
            if (_worker.WorkerSupportsCancellation) {
                Status = DeletingStatus.Cancelling;
                _worker.CancelAsync();
                OnChange(this, EventArgs.Empty);
            }
        }
    }
}
