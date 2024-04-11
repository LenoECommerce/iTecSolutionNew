using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using EigenbelegToolAlpha.APIs;
using EigenbelegToolAlpha.ConstsAndEnums;
using System.Web;

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

            SyncAllOrders.Main();

            Application.Run(new Reparaturen());

        }
    }
}
