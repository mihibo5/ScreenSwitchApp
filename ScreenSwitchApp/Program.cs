using System;
using System.Windows.Forms;

namespace ScreenSwitchApp
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

            ScreenSwitch form = new ScreenSwitch
            {
                Visible = false,
                
            };
            Application.Run(form);
        }
    }
}
