using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapScreens
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (sender, e) =>
                MessageBox.Show(e.Exception.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            new SnapMain();
            Application.Run();
        }
    }
}
