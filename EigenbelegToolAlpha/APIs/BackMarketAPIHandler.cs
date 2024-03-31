using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using MySqlX.XDevAPI;
using System.IO;
using System.Net;
using System.Drawing;
using PayPal.Api;
using Org.BouncyCastle.Asn1.Mozilla;
using Newtonsoft.Json;
using MySqlX.XDevAPI.Common;
using Google.Protobuf.WellKnownTypes;
using MigraDoc.DocumentObjectModel;
using System.Diagnostics;
using Billbee.Api.Client.Model;
using Newtonsoft.Json.Linq;
using ceTe.DynamicPDF.PageElements.Charting;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using EigenbelegToolAlpha.ConstsAndEnums;

namespace EigenbelegToolAlpha
{
    public class BackMarketAPIHandler
    {
        public static string apiKey = "ZDE1NmE3ZDE5YjMwYzQzYjJkMDA5ZDpCTVQtNmJkOTZhODQwMTJmZjliY2Y4MTg2Y2IzZWRlMTk4YmRhMjkzZjMyNA==";
        public static string userAgent = "I-Tec-Shop";
        // this is a test 
        public static Dictionary<string, string> conditionsEqualization = new Dictionary<string, string>
        {
            { "DIAMOND", "A" },
            { "PLATINUM", "B" },
            { "GOLD", "C" },
            { "SILVER", "D" },
            { "BRONZE", "E" },
            { "STALLONE", "F" },
        };

        public static Dictionary<string, string> conditionsExplanations = new Dictionary<string, string>
        {
            { "A", "Technisch: Funktionsfähig\r\nOptisch: Hervorragend" },
            { "B", "Technisch: Funktionsfähig\r\nOptisch: Sehr Gut" },
            { "C", "Technisch: Funktionsfähig\r\nOptisch: Gut" },
            { "D", "Technisch: Funktionsfähig \r\nOptisch: Beschädigt" },
            { "E", "Technisch: Nicht funktionsfähig\r\nOptisch: Gut" },
            { "F", "Technisch: Nicht funktionsfähig\r\nOptisch: Beschädigt" },
        };

        public static string GetRequest(string endpointUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpointUrl);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Headers.Add("Authorization:Basic " + apiKey);
            request.Headers.Add("Accept-Language:de-de");
            request.UserAgent = userAgent;


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Check the status code of the response
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Get the response stream and read the contents
                    Stream responseStream = response.GetResponseStream();
                    string responseString = new StreamReader(responseStream).ReadToEnd();
                    return responseString;
                    // Do something with the response string
                }
                else
                {
                    // Handle error
                }
            }
            return "";
        }

        public static bool IsBuyBackOrderFMIExpired(string orderId)
        {
            string url = $"https://www.backmarket.fr/ws/buyback/v1/orders/{orderId}";
            string response = GetRequest(url);
            JObject json = JObject.Parse(response);
            JArray suspendReasons = (JArray)json["suspendReasons"];
            string suspendReasonIdentifier = (string)suspendReasons[0]["identifier"];
            var status = (string)json["status"];
            if (status == "CANCELED"  &&
                suspendReasonIdentifier == "customer_account_present")
            {
                return true;
            }
            return false;
        }

        public static void SendMessageToBuyBackCustomer(string message, string[]files, string orderId)
        {
            try
            {
                string url = $"https://www.backmarket.fr/ws/buyback/v1/orders/{orderId}/messages";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Headers.Add("Authorization", "Basic " + apiKey);
                request.Headers.Add("Accept-Language", "de-de");
                request.UserAgent = userAgent;
                var data = new
                {
                    message = message,
                    files = files
                };
                var jsonData = JsonConvert.SerializeObject(data);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = response.GetResponseStream())
                        {
                            var reader = new StreamReader(responseStream);
                            var responseString = reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        // Handle error
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void SuspendBuyBackOrder(string[] suspendReasons, string orderId)
        {
            try
            {
                string url = $"https://www.backmarket.fr/ws/buyback/v1/orders/{orderId}/suspend";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "PUT";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Headers.Add("Authorization", "Basic " + apiKey);
                request.Headers.Add("Accept-Language", "de-de");
                request.UserAgent = userAgent;
                var data = new
                {
                    reasons = suspendReasons
                };
                var jsonData = JsonConvert.SerializeObject(data);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = response.GetResponseStream())
                        {
                            var reader = new StreamReader(responseStream);
                            var responseString = reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        // Handle error
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void SetFreeLatestOrders()
        {
            string currentOrderId = "";
            int counterSetFree = 0;
            try
            {
                string url = "https://www.backmarket.fr/ws/buyback/v1/orders?page=1&pageSize=100&status=RECEIVED";
                string response = GetRequest(url);
                JObject json = JObject.Parse(response);
                JArray results = (JArray)json["results"];
                foreach (JObject result in results)
                {
                    currentOrderId = (string)result["orderId"];
                    DateTime receivalDate = (DateTime)result["receivalDate"];
                    DateTime expirationDate = GetExpirationDateBusinessLogic(receivalDate);

                    TimeSpan timeLeftForSetFree = expirationDate.Subtract(DateTime.Now);
                    TimeSpan timeBuffer = new TimeSpan(23, 45, 0);

                    if (timeLeftForSetFree.CompareTo(timeBuffer) <= 0)
                    {
                        var dbManager = new DBManager();
                        int id = dbManager.ExecuteQueryWithResult("Eigenbelege", "Id", "Referenz", currentOrderId);
                        if (id == 0)
                        {
                            MessageBox.Show($"Die Order {currentOrderId} wurde in der Datenbank nicht erfasst und wurde daher übersprungen. \r\nBitte überprüfen.");
                            continue;
                        }
                        PutOrderRequest(currentOrderId);
                        counterSetFree++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei " + currentOrderId + ex.Message);
            }
            MessageBox.Show("Es wurden " + counterSetFree + " Bestellungen freigesetzt.");
        }

        public static DateTime GetExpirationDateBusinessLogic(DateTime receivalDate)
        {
            DateTime potentialExpirationDate = receivalDate.AddDays(2);
            DateTime returnDate = new DateTime();
            DayOfWeek dayPotentialExpiration = potentialExpirationDate.DayOfWeek;
            if (dayPotentialExpiration == DayOfWeek.Saturday || dayPotentialExpiration == DayOfWeek.Sunday)
            {
                // add 2 weekend days because they are not concerned by BuyBack business logic
                returnDate = potentialExpirationDate.AddDays(2);
            }
            else
            {
                returnDate = potentialExpirationDate;
            }
            // add holiday logic
            string returnDateSubString = returnDate.ToString().Substring(0, 5);
            if (BackMarketHolidayLogic.holidayDays.Contains(returnDateSubString))
            {
                returnDate = returnDate.AddDays(1);
            }
            return returnDate;
        }

        public static void DownloadShippingLabel(string path, string orderId)
        {
            string url = $"https://www.backmarket.fr/ws/shipping/v1/deliveries?order_id={orderId}";
            string response = GetRequest(url);
            JObject json = JObject.Parse(response);
            JArray results = (JArray)json["results"];
            string pdfDownloadUrl = (string)results[0]["labelUrl"];
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(pdfDownloadUrl, path);
            }
        }

        public static void DownloadDeliveryNoteToPath(string path, string orderId)
        {
            string pdfDownloadUrl = GetSpecificFieldOfOrder(orderId, "delivery_note");
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(pdfDownloadUrl, path);
            }
        }
        public static string GetSpecificFieldOfOrder(string orderId, string fieldName)
        {
            string url = "https://www.backmarket.fr/ws/orders/" + orderId;
            string response = GetRequest(url);
            JObject json = JObject.Parse(response);
            string value = (string)json[fieldName];
            return value;
        }

        public static string GetSpecificFieldOfOrder1Layer(string orderId, string layer, string fieldName)
        {
            string url = "https://www.backmarket.fr/ws/orders/" + orderId;
            string response = GetRequest(url);
            JObject json = JObject.Parse(response);
            string value = (string)json[layer][fieldName];
            return value;
        }

        public static string GetFieldOrder1LayerArray(string orderId, string layerName, string fieldName)
        {
            string url = "https://www.backmarket.fr/ws/orders/" + orderId;
            string response = GetRequest(url);
            JObject json = JObject.Parse(response);
            JArray results = (JArray)json[layerName];
            string value = (string)results[0][fieldName];
            return value;
        }

        public void AfterSalesDataPush(string[] finishedOrderIds, string[] imeis)
        {
            string trackingNumber = "";
            foreach (string orderId in finishedOrderIds)
            {
                try
                {
                    int pos = Array.IndexOf(finishedOrderIds, orderId);
                    string imei = imeis[pos];
                    // only if backmarket order
                    if (!orderId.Contains("-"))
                    {
                        // check if order needs tracking number
                        if (GetSpecificFieldOfOrder(orderId, "is_backship") == "True")
                        {
                            trackingNumber = GetSpecificFieldOfOrder(orderId, "tracking_number");
                            BillBeeAPIHandler.SetOrderToStatusSent(orderId);
                        }
                        else
                        {
                            trackingNumber = BillBeeAPIHandler.GetTrackingNumber(orderId);
                        }
                        // multiple orderline part
                        if (orderId.Contains("."))
                        {
                            MessageBox.Show($"Bitte hinterlege die Sendungsnummer und IMEIs für die Bestellnummer {orderId} manuell");
                        }
                        else
                        {
                            UpdateOrderRequest(orderId, trackingNumber, imei);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Es gab einen Fehler im After Sales Push zu BackMarket für folgende Order: "+orderId+ "\r\n\r\nFehlercode: " + ex.Message);
                }
            }
        }

        public static void PutOrderRequest(string orderId)
        {
            string url = $"https://www.backmarket.fr/ws/buyback/v1/orders/{orderId}/validate";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Headers.Add("Authorization", "Basic " + apiKey);
            request.Headers.Add("Accept-Language", "de-de");
            request.UserAgent = userAgent;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Check the status code of the response
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    // handle error?
                }
            }
        }
    

        public void UpdateOrderRequest(string orderId, string trackingNumber, string imei)
        {
            string url = "https://www.backmarket.fr/ws/orders/" + orderId;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Headers.Add("Authorization", "Basic " + apiKey);
            request.Headers.Add("Accept-Language", "de-de");
            request.UserAgent = userAgent;
            var data = new
            {
                order_id = orderId,
                new_state = 3,
                tracking_number = trackingNumber,
                imei = imei
            };
            var jsonData = JsonConvert.SerializeObject(data);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        var reader = new StreamReader(responseStream);
                        var responseString = reader.ReadToEnd();
                    }
                }
                else
                {
                    // Handle error
                }
            }
        }


       
        public string[] PullBuyBackDataFromOrderID(string inputShippingNumber)
        {
            string[] feedback = new string[8];
            try
            {
                DateTime now = DateTime.Now;
                string twoMonthsAgo = "&creationDate=" + now.AddMonths(-2).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); ;
                feedback = IterateThroughPagesBuyBackArrival(twoMonthsAgo, inputShippingNumber);
                // case distinction if no feedback
                if (feedback.Length == 0)
                {
                    MessageBox.Show($"Keine Order mit der Tracking Nummer {inputShippingNumber} gefunden.");
                }
                return feedback;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return feedback;
            }
        }
        
        public void BuyBackControllingOurCompany()
        {
            DateTime lastMonday = StartOfWeek(DateTime.Now, DayOfWeek.Monday);
            string creationDate = "&creationDate=" + lastMonday.ToString("yyyy-MM-ddTHH:mm:ssZ");
            double amount = IterateThroughPages(creationDate);
            if (UserFileManagement.ReturnCurrentUser() != "LennartMainPC")
            {
                return;
            }
            CheckForIndicatorReset();
            if (IsAmountReached(amount) == true && IsNotificationIndicatorTriggered() == false)
            {
                MessageBox.Show("Das BuyBack Budget ist ausgeschöpft!");
                AdaptIndicator();
            }
        }
        public double[] GetVolumeOfSpecificWeek(DateTime startDate, DateTime endDate)
        {
            double[] returnValues = new double[2];
            string filterOrders = "&creationDate=" + startDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string response = GetRequest("https://www.backmarket.fr/ws/buyback/v1/orders?page=1&pageSize=100" + filterOrders);
            JObject json = JObject.Parse(response);
            string next = (string)json["next"];
            int count = (int)json["count"];
            // calculate how many pages there are
            int pagesTest = CalculatePageCount(count, 100);
            for (int i = 0; i < pagesTest; i++)
            {
                if (i != 0)
                {
                    response = GetRequest(next);
                    dynamic json2 = JsonConvert.DeserializeObject(response);
                    next = json2.next;
                }
                double[] newAmounts = CollectValuesForWeek(response, endDate);
                returnValues = SumUpTheNewValues(newAmounts, returnValues);
            }
            return returnValues;
        }
        public double[] CollectValuesForWeek(string response, DateTime endDate)
        {
            double[] returnValues = new double[2];
            JObject json = JObject.Parse(response);
            JArray results = (JArray)json["results"];
            foreach (JObject result in results)
            {
                if (DateTime.Compare((DateTime)result["creationDate"], endDate) < 0)
                {
                    returnValues[0]++;
                    returnValues[1] += (double)result["originalPrice"]["value"];
                }
            }
            return returnValues;
        }
        public int CalculatePageCount(int itemCount, int maxItemsPerPage)
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
        public string[] IterateThroughPagesBuyBackArrival(string creationDate, string inputShippingNumber)
        {
            string[] feedback = new string[8];
            string response = GetRequest("https://www.backmarket.fr/ws/buyback/v1/orders?page=1&pageSize=100&" + creationDate);
            double amount = 0;
            dynamic json = JsonConvert.DeserializeObject(response);
            int count = json.count;
            string next = json.next;

            int pagesTest = CalculatePageCount(count, 100);
            for (int i = 0; i < pagesTest; i++)
            {
                if (i != 0)
                {
                    response = GetRequest(next + creationDate);
                    dynamic json2 = JsonConvert.DeserializeObject(response);
                    next = json2.next;
                }
                JObject jsonTest = JObject.Parse(response);
                JArray results = (JArray)jsonTest["results"];
                foreach (JObject result in results)
                {
                    if ((string)result["trackingNumber"] == inputShippingNumber)
                    {
                        string[] temp = SeperateModelAndStorage((string)result["listing"]["title"]);
                        feedback[0] = temp[0];
                        feedback[1] = temp[1];
                        feedback[2] = (string)result["customer"]["firstName"] + " " + (string)result["customer"]["lastName"];
                        feedback[3] = feedback[2] + ", " + (string)result["returnAddress"]["address1"] + ", " + (string)result["returnAddress"]["zipcode"] + " " + (string)result["returnAddress"]["city"];
                        feedback[4] = (string)result["originalPrice"]["value"];
                        feedback[5] = (string)result["orderId"];
                        feedback[6] = conditionsEqualization[(string)result["listing"]["grade"]];
                        feedback[7] = conditionsExplanations[feedback[6]];
                        return feedback;
                    }
                }
            }
            return feedback;
        }
        public double IterateThroughPages(string creationDate)
        {
            string response = GetRequest("https://www.backmarket.fr/ws/buyback/v1/orders?page=1&pageSize=100" + creationDate);
            double amount = 0;
            dynamic json = JsonConvert.DeserializeObject(response);
            int count = json.count;
            string next = json.next;

            int pagesTest = CalculatePageCount(count, 100);
            for (int i = 0; i < pagesTest; i++)
            {
                if (i != 0)
                {
                    response = GetRequest(next + creationDate);
                    dynamic json2 = JsonConvert.DeserializeObject(response);
                    next = json2.next;
                }
                amount += CalculateReachedAmount(response);
            }
            return amount;
        }
        public void CheckForIndicatorReset()
        {
            DateTime today = DateTime.Now;
            var dbManager = new DBManager();
            DateTime lastReset = Convert.ToDateTime(dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "LastResetBuyBackIndicators"));
            if (DateTime.Compare(GetMostRecentMonday(), lastReset) >= 0)
            {
                ResetIndicators();
                //set lastReset date
                dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = '" + today + "' WHERE `Typ` = 'LastResetBuyBackIndicators'");
            }
        }
        public void ResetIndicators()
        {
            var dbManager = new DBManager();
            dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = '0' WHERE `Typ` = 'IndicatorNotficiationBuyBackPurchaseLimit'");
        }
        public static DateTime GetMostRecentMonday()
        {
            DayOfWeek today = DateTime.Today.DayOfWeek;
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today + 7) % 7;
            DateTime mostRecentMonday = DateTime.Today.AddDays(-daysUntilMonday);
            return mostRecentMonday;
        }
        public void AdaptIndicator()
        {
            string query = "UPDATE `Config` SET `Nummer` = '1' WHERE `Typ` = 'IndicatorNotficiationBuyBackPurchaseLimit'";
            var dbManager = new DBManager();
            dbManager.ExecuteQuery(query);
        }
        public double[] SumUpTheNewValues(double[] newAmounts, double[] oldAmounts)
        {
            for (int i = 0; i < oldAmounts.Length; i++)
            {
                oldAmounts[i] += newAmounts[i];
            }
            return oldAmounts;
        }
        public bool IsNotificationIndicatorTriggered()
        {
            var dbManager = new DBManager();
            int value = dbManager.ExecuteQueryWithResult("Config", "Nummer", "Typ", "IndicatorNotficiationBuyBackPurchaseLimit");
            if (value == 1)
            {
                return true;
            }
            return false;
        }

        public bool IsAmountReached(double checkAmount)
        {
            var dbManager = new DBManager();
            double budget = Convert.ToDouble(dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "BuyBackPurchaseLimit"));
            double arrivalRate = Convert.ToDouble(dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "BuyBackArrivalRate"));
            double paymentRate = Convert.ToDouble(dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "BuyBackPaymentRate"));
            checkAmount = checkAmount * arrivalRate * paymentRate;
            if (checkAmount >= budget)
            {
                return true;
            }
            return false;
        }

        public double CalculateReachedAmount(string result)
        {
            //first slot: normal devices; second slot: risk devices
            double reachedAmountValue = 0;
            //deserialize JSON part
            dynamic json = JsonConvert.DeserializeObject(result);
            var results = json.results;
            //iteration
            var dbManager = new DBManager();
            foreach (var item in results)
            {
                //mit item.blabla kann ich auf einzelne Properties im JSON Format zugreifen
                string title = item.listing.title;
                string gradeBackMarketAlias = item.listing.grade;
                string grade = conditionsEqualization[gradeBackMarketAlias];
                //DB Abfrage

                int id = dbManager.ExecuteQueryWithResultThreeConditions("BuyBackBidsOurCompany", "Id", "ProductID", title, "Grade", grade, "IsInCurrentPortfolio", "Ja");
                if (id == 0)
                {
                    reachedAmountValue += AdaptAmountWithBuyBackFees(item.originalPrice.value);
                }
            }
            return reachedAmountValue;
        }
        public double AdaptAmountWithBuyBackFees(dynamic purchaseAmount)
        {
            return purchaseAmount + 10 + purchaseAmount * 0.1;
        }

        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public string[] SeperateModelAndStorage(string mainValue)
        {
            string[] returnValues = new string[2];
            var fullLength = mainValue.Length;
            //storage part
            var posLastSpaceSign = mainValue.LastIndexOf(" ");
            var posSpaceSignBeforeStorage = mainValue.Substring(0, posLastSpaceSign).LastIndexOf(" ");
            returnValues[1] = mainValue.Substring(posSpaceSignBeforeStorage + 1, fullLength - posSpaceSignBeforeStorage - 1);
            //model part
            returnValues[0] = mainValue.Substring(6, posSpaceSignBeforeStorage - 6);
            return returnValues;
        }
    }
}
