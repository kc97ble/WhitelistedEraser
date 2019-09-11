using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhitelistedEraser.Logic;

namespace WhitelistedEraser {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        private static void MyCommonExceptionHandlingMethod(object sender, ThreadExceptionEventArgs t) {
            MessageBox.Show(t.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
        }

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(MyCommonExceptionHandlingMethod);
            MainLogic data = new MainLogic();
            Application.Run(new UI.MainForm(data));
        }
    }
}
