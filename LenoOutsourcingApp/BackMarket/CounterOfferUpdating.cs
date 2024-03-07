using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public class CounterOfferUpdating
    {
        public static string[] acceptedOrders = new string[0];
        public static void MainOK()
        {
            if (UserFileManagement.ReturnCurrentUser() != "LennartMainPC")
            {
                return;
            }
            UpdateForCanceledOrders();
            UpdateForAcceptedOrders();
        }

        public static void UpdateForAcceptedOrders()
        {
            DateTime today = DateTime.Now;
            var dbManager = new DBManager();
            DateTime lastUpdate = Convert.ToDateTime(dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "LastUpdateAcceptedCounterOffers"));
            TimeSpan timeLastUpdate = today.Subtract(lastUpdate);
            // 7 days
            TimeSpan timeBuffer = new TimeSpan(168, 0, 0);
            if (timeLastUpdate.CompareTo(timeBuffer) >= 0)
            {
                string lastUpdateISO = lastUpdate.AddDays(-50).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                string response = BackMarketAPIHandler.GetRequest($"https://www.backmarket.fr/ws/buyback/v1/orders?page=1&pageSize=100&receivedDate={lastUpdateISO}");
                JObject json = JObject.Parse(response);
                string next = (string)json["next"];
                int count = (int)json["count"];
                // calculate how many pages there are
                int pagesTest = CalculatePageCount(count, 100);
                for (int i = 0; i < pagesTest; i++)
                {
                    if (i != 0)
                    {
                        response = BackMarketAPIHandler.GetRequest(next);
                        dynamic json2 = JsonConvert.DeserializeObject(response);
                        next = json2.next;
                    }
                    // sum up values here
                    string[] newArray = CollectValues(response);
                    acceptedOrders = acceptedOrders.Concat(newArray).ToArray();
                }
                BuyBackAcceptedReportInputcs window = new BuyBackAcceptedReportInputcs();
                window.Show();
                MessageBox.Show("BuyBack Accepted Report hat sich im Hintergrund geöffnet.");
                AdaptDBValues("LastUpdateAcceptedCounterOffers", today.ToString());
            }
        }

        public static string[] CollectValues(string response)
        {
            string[] returnValues = new string[0];
            JObject json = JObject.Parse(response);
            JArray results = (JArray)json["results"];
            foreach (var item in results)
            {
                string counterProposalDate = (string)item["counterProposalDate"];
                // logic if there was a counter proposal and if the state is one of the paid ones
                if (!string.IsNullOrEmpty(counterProposalDate))
                {
                    string status = (string)item["status"];
                    if (status == "PAID" || status == "VALIDATED" || status == "MONEY_TRANSFERED")
                    {
                        string orderID = (string)item["orderId"];
                        //save to array
                        string[] newArray = new string[] { GetREPNumberViaOrderNumber(orderID) };
                        returnValues = returnValues.Concat(newArray).ToArray();
                    }
                }
            }
            return returnValues;
        }

        public static void UpdateForCanceledOrders()
        {
            string[] returnValues = new string[0];
            int count = 0;
            DateTime today = DateTime.Now;
            var dbManager = new DBManager();
            DateTime lastUpdate = Convert.ToDateTime(dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "LastUpdateCanceledCounterOffers"));
            TimeSpan timeLeftForSetFree = today.Subtract(lastUpdate);
            TimeSpan timeBuffer = new TimeSpan(24, 0, 0);

            if (timeLeftForSetFree.CompareTo(timeBuffer) >= 0)
            {
                string lastUpdateISO = lastUpdate.AddDays(-50).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                string response = BackMarketAPIHandler.GetRequest($"https://www.backmarket.fr/ws/buyback/v1/orders?page=1&pageSize=100&status=CANCELED&receivedDate={lastUpdateISO}");
                JObject json = JObject.Parse(response);
                JArray results = (JArray)json["results"];
                foreach (var item in results)
                {
                    string counterOfferCategory = "";
                    string orderID = (string)item["orderId"];
                    int id = dbManager.ExecuteQueryWithResult("BuyBackCanceledOrders", "Id", "OrderID", orderID);
                    try
                    {
                        counterOfferCategory = BackMarketAPIHandler.GetFieldBuyBackOrder1LayerArray(orderID, "suspendReasons", "category");
                    }
                    catch (Exception)
                    {
                    }
                    if (id == 0 && counterOfferCategory != "Aktion vom Kunden erforderlich")
                    {
                        //save to array
                        string[] newArray = new string[] { GetREPNumberViaOrderNumber(orderID) };
                        returnValues = returnValues.Concat(newArray).ToArray();
                        string query = $"INSERT INTO `BuyBackCanceledOrders` (`OrderID`) VALUES('{orderID}')";
                        dbManager.ExecuteQuery(query);
                    }
                }
                ShowMessageBox(returnValues, "Canceled Order Report\n");
                AdaptDBValues("LastUpdateCanceledCounterOffers", today.ToString());
                AdaptDBValues("CanceledOrdersLastCount", count.ToString());
            }
        }
        public static string GetREPNumberViaOrderNumber(string orderID)
        {
            var dbManager = new DBManager();
            string ebNumber = dbManager.ExecuteQueryWithResultString("Eigenbelege", "Eigenbelegnummer", "Referenz", orderID);
            string repNumber = dbManager.ExecuteQueryWithResultString("Reparaturen", "Intern", "EBReferenz", ebNumber);
            return repNumber;
        }
        public static int CalculatePageCount(int itemCount, int maxItemsPerPage)
        {
            if (itemCount <= 0 || maxItemsPerPage <= 0)
            {
                return 0;
            }

            int pageCount = itemCount / maxItemsPerPage;
            if (itemCount % maxItemsPerPage != 0)
            {
                pageCount++;
            }

            return pageCount;
        }
        public static void AdaptDBValues(string attribute, string value)
        {
            string query = $"UPDATE `Config` SET `Nummer` = '{value}' WHERE `Typ` = '{attribute}'";
            var dbManager = new DBManager();
            dbManager.ExecuteQuery(query);
        }
        public static void ShowMessageBox(string[] values, string message)
        {
            if (values.Length == 0)
            {
                return;
            }
            else
            {

                foreach (string value in values)
                {
                    message += "- " + value + "\n";
                }

                MessageBox.Show(message);
            }
        }
    }
}
