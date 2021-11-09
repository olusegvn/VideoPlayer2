using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoPlayer
{

    
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        
        public static void Main()
        {
            if (Environment.OSVersion.Version.Major > 0)
                SetProcessDPIAware();
            Application.EnableVisualStyles();
            /*Application.SetCompatibleTextRenderingDefault(false);*/
            Application.Run(new baseForm());
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}

