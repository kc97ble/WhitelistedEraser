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

namespace WhitelistedEraser.UI {
    public partial class MainForm : Form {
        private MainLogic MainLogic;

        public MainForm(MainLogic mainLogic) {
            MainLogic = mainLogic;
            InitializeComponent();
            MainLogic.OnChange += Form1_MainLogicChanged;
            Form1_MainLogicChanged(this, EventArgs.Empty);
            tableLayoutPanel5.Visible = false;
            CheckListBoxPlaceholder.Dock = DockStyle.Fill;
        }

        private void Form1_Load(object sender, EventArgs e) {
            
        }

        private void Form1_MainLogicChanged(object sender, EventArgs e) {
            textBox1.Text = MainLogic.WorkingDirectory;
            ListUtil.AssignCheckedListBoxItems(CheckedListBox, MainLogic.SubfolderPaths.ToList(), MainLogic.WhitelistedSubfolderPaths.ToList());

            var canGo = System.IO.Directory.Exists(MainLogic.WorkingDirectory);
            NextButton.Enabled = canGo;
            CheckAllButton.Visible = canGo;
            UncheckAllButton.Visible = canGo;
            ReloadButton.Visible = canGo;
            CheckedListBox.Visible = canGo;
            CheckedListBoxLabel.Visible = canGo;
            AboutButton.Visible = !canGo;
            tableLayoutPanel5.Visible = tableLayoutPanel5.Visible && !canGo;
            CheckListBoxPlaceholder.Visible = !canGo;
        }

        private void button2_Click(object sender, EventArgs e) {
            folderBrowserDialog1.ShowNewFolderButton = true;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                MainLogic.WorkingDirectory = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            MainLogic.FetchSubfolderPaths();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                MainLogic.WorkingDirectory = textBox1.Text;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
        }

        private void _setCheckState(string key, bool value) {
            var found = MainLogic.WhitelistedSubfolderPaths.Contains(key);
            if (value && !found) {
                MainLogic.WhitelistedSubfolderPaths.Add(key);
            } else if (!value && found) {
                MainLogic.WhitelistedSubfolderPaths.Remove(key);
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e) {
            _setCheckState(MainLogic.SubfolderPaths[e.Index], e.NewValue == CheckState.Checked);
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e) {

        }

        private void button3_Click(object sender, EventArgs e) {
            var form = new PreviewForm(
                new Logic.PreviewLogic(MainLogic.WorkingDirectory, MainLogic.SubfolderPaths, MainLogic.WhitelistedSubfolderPaths)
            );
            form.ShowDialog();
            MainLogic.FetchSubfolderPaths();
        }

        private void CheckAllButton_Click(object sender, EventArgs e) {
            foreach (string folder in MainLogic.SubfolderPaths) {
                _setCheckState(folder, true);
            }
        }

        private void button5_Click(object sender, EventArgs e) {
            tableLayoutPanel5.Visible = !tableLayoutPanel5.Visible;
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1) {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e) {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 1) {
                if (System.IO.Directory.Exists(files[0])) {
                    MainLogic.WorkingDirectory = files[0];
                } else {
                    MainLogic.WorkingDirectory = System.IO.Path.GetDirectoryName(files[0]);
                }
            }
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e) {

        }

        private void UncheckAll_Click_1(object sender, EventArgs e) {
            foreach (string folder in MainLogic.SubfolderPaths) {
                _setCheckState(folder, false);
            }
        }
    }
}
