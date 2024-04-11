using ExcelLibrary.BinaryFileFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public class SyncAllOrders
    {

        public static void Main()
        {
            DBManager dBManager = new DBManager();

            DateTime today = DateTime.Now;
            DateTime lastUpdate = Convert.ToDateTime(dBManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "LastOrderSync"));

            TimeSpan timeDifference = today.Subtract(lastUpdate);
            TimeSpan timeBuffer = new TimeSpan(24, 0, 0);

            if (timeDifference.CompareTo(timeBuffer) >= 0)
            {
                BackMarketAPIHandler handler = new BackMarketAPIHandler();
                string[] backMarketOrders = handler.GetAllOrderIdsFromSells();
                string[] orderIdsWarenausgang = dBManager.GetValuesFromOneAttribute("Bestellnummer", "Protokollierung");

                foreach (var item in backMarketOrders)
                {
                    // does this order id exist in Warenausgang
                    bool isContained = Array.Exists(orderIdsWarenausgang, element => element == item);

                    if (!isContained)
                    {
                        InsertDataToWarenausgang(item);
                    }
                }

                // take new date as value
                AdaptDBValues("LastOrderSync", today.ToString());
            }

        }

        private static void InsertDataToWarenausgang(string orderId)
        {
            DBManager db = new DBManager();
            string imei = BackMarketAPIHandler.GetFieldOrder1LayerArray(orderId, "orderlines", "imei");
            string scanDate = BackMarketAPIHandler.GetSpecificFieldOfOrder(orderId, "date_shipping");
            string marketPlace = "BackMarket";


            string[] attributes = new string[] {
                "Bestellnummer", 
                "IMEI",
                "Marktplatz",
                "Scandatum"};

            string[] values = new string[] {
                orderId,
                imei,
                marketPlace,
                scanDate};

            db.CreateEntry(
                "Protokollierung",
                attributes,
                values);
        }

        private static string[] GetAllBackMarketSellsAsOrderIds()
        {
            return new string[] { };
        }

        private static void AdaptDBValues(string attribute, string value)
        {
            string query = $"UPDATE `Config` SET `Nummer` = '{value}' WHERE `Typ` = '{attribute}'";
            var dbManager = new DBManager();
            dbManager.ExecuteQuery(query);
        }

    }
}
