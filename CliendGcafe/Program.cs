using CCMS.lib;
using CCMS.view;
using System;
using System.Windows.Forms;

namespace CCMS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Slide2());
            Application.Run(new Slide2());
        }
    }
}
