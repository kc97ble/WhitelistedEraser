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
    public partial class PreviewForm : Form {
        private Data data;

        public PreviewForm(Data aData) {
            data = aData;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e) {

        }
    }
}
