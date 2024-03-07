using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using EigenbelegToolAlpha.APIs;
using EigenbelegToolAlpha.ConstsAndEnums;

namespace EigenbelegToolAlpha
{
    internal static class Program
    {
        public static string currentUser = "";
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Reparaturen());

        }
    }
}
