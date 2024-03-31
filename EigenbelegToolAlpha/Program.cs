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

            //string orderId = "39226586";
            //string orderId2 = "39169190";
            //string paymentMethod = BackMarketAPIHandler.GetSpecificFieldOfOrder(orderId, "payment_method");
            //string test = BackMarketAPIHandler.GetFieldOrder1LayerArray(orderId, "orderlines","price");

            Application.Run(new Reparaturen());

        }
    }
}
