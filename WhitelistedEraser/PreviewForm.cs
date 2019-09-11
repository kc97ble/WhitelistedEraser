using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhitelistedEraser.Logic;
using WhitelistedEraser.Util;

namespace WhitelistedEraser {

    public partial class PreviewForm : Form {
        private const string LABEL1_FMT = "These files will be deleted ({0} file[s])";
        private const string LABEL2_FMT = "These files will be kept ({0} file[s])";

        private readonly PreviewLogic PreviewLogic;

        private void PreviewLogicChanged(object sender, EventArgs e) {
            label1.Text = string.Format(LABEL1_FMT, PreviewLogic.BlacklistedFilePaths.Count());
            ListUtil.AssignList(listBox1.Items, PreviewLogic.BlacklistedFilePaths);
            label2.Text = string.Format(LABEL2_FMT, PreviewLogic.WhitelistedFilePaths.Count());
            ListUtil.AssignList(listBox2.Items, PreviewLogic.WhitelistedFilePaths);
        }

        public PreviewForm(PreviewLogic previewLogic) {
            PreviewLogic = previewLogic;
            InitializeComponent();
            PreviewLogic.OnChange += PreviewLogicChanged;
            PreviewLogic.Reload();
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e) {

        }

        private void button3_Click(object sender, EventArgs e) {
            PreviewLogic.Reload();
        }

        private void button2_Click(object sender, EventArgs e) {
            Close();
        }

        private void button1_Click(object sender, EventArgs e) {

        }
    }
}

