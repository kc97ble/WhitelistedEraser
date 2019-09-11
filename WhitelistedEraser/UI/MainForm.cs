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
        }

        private void Form1_Load(object sender, EventArgs e) {
            MainLogic.OnChange += Form1_MainLogicChanged;
            Form1_MainLogicChanged(this, EventArgs.Empty);
        }

        private void Form1_MainLogicChanged(object sender, EventArgs e) {
            textBox1.Text = MainLogic.WorkingDirectory;
            ListUtil.AssignCheckedListBoxItems(checkedListBox1, MainLogic.SubfolderPaths.ToList(), MainLogic.WhitelistedSubfolderPaths.ToList());
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

        private void button4_Click(object sender, EventArgs e) {
            foreach (string folder in MainLogic.SubfolderPaths) {
                _setCheckState(folder, true);
            }
        }

        private void button5_Click(object sender, EventArgs e) {
            foreach (string folder in MainLogic.SubfolderPaths) {
                _setCheckState(folder, false);
            }
        }
    }
}
