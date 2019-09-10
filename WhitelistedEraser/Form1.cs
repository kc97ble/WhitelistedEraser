using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhitelistedEraser {
    public partial class Form1 : Form {
        private Data data;

        public Form1(Data aData) {
            data = aData;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            data.OnChange = Form1_DataChanged;
            Form1_DataChanged(this, EventArgs.Empty);
        }

        private void Form1_DataChanged(object sender, EventArgs e) {
            textBox1.Text = data.WorkingDirectory;
            Util.AssignCheckedListBoxItems(checkedListBox1, data.SubfolderPaths.ToList(), data.CheckedSubfolderPaths.ToList());

        }

        private void button2_Click(object sender, EventArgs e) {
            folderBrowserDialog1.ShowNewFolderButton = true;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                data.WorkingDirectory = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            data.FetchSubfolderPaths();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                data.WorkingDirectory = textBox1.Text;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            data.OnChange = null;
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e) {
            var key = data.SubfolderPaths[e.Index];
            var found = data.CheckedSubfolderPaths.Contains(key);
            if (e.NewValue == CheckState.Checked && !found) {
                data.CheckedSubfolderPaths.Add(key);
            } else if (e.NewValue == CheckState.Unchecked && found) {
                data.CheckedSubfolderPaths.Remove(key);
            }
        }
    }
}
