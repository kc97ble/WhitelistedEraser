using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhitelistedEraser.UI {
    public partial class DeletingForm : Form {
        private const string LABEL1_FMT = "Status: {0}";

        private Logic.DeletingLogic DeletingLogic;

        private void DeletingLogicChanged(object sender, EventArgs e) {
            label1.Text = string.Format(LABEL1_FMT, DeletingLogic.Status.ToString());
            StartButton.Enabled = DeletingLogic.Status == Logic.DeletingStatus.Ready;
            StopButton.Enabled = DeletingLogic.Status == Logic.DeletingStatus.Doing;
            CloseButton.Enabled = DeletingLogic.Status == Logic.DeletingStatus.Done || DeletingLogic.Status == Logic.DeletingStatus.Cancelled;
            progressBar1.Value = Convert.ToInt32(100.0 * DeletingLogic.CompletedFileCount / DeletingLogic.Files.Count);
            Util.ListUtil.AssignList(listBox1.Items, DeletingLogic.Log);
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }

        public DeletingForm(Logic.DeletingLogic deletingLogic) {
            DeletingLogic = deletingLogic;
            InitializeComponent();
            DeletingLogic.OnChange += DeletingLogicChanged;
            DeletingLogicChanged(DeletingLogic, EventArgs.Empty);
            Util.ListUtil.AssignList(listBox2.Items, DeletingLogic.Files);
        }

        private void DeletingForm_Load(object sender, EventArgs e) {

        }

        private void button3_Click(object sender, EventArgs e) {
            DeletingLogic.Start();
        }

        private void button2_Click(object sender, EventArgs e) {
            Close();
        }

        private void StopButton_Click(object sender, EventArgs e) {
            DeletingLogic.Stop();
        }
    }
}
