using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public class BillBeeAPIHandler

    {
        public static string apiUsername = "developeraccess";
        public static string apiPassword = "CjAMgF-XMy>42ZV-WE27h6g";
        public static string apiKey = "4414BB64-33FD-49C8-AE83-584C5F889115";
        public static void UpdateAllOrders(string[] orders, string[] imeis)
        {
            foreach (var item in orders)
            {
                // multiple orderlines part
                if (item.Contains("."))
                {
                    var index = item.IndexOf(".");
                    string adaptedOrderID = item.Substring(0, index);
                    SetOrderToStatusSent(adaptedOrderID);
                }
                // normal handling
                else
                {
                    int pos = Array.IndexOf(orders, item);
                    string imei = imeis[pos];
                    PatchAfterSalesData(item, imei);
                }

            }
        }
        public static void SetOrderToStatusSent(string externalOrderId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Billbee-Api-Key", apiKey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{apiUsername}:{apiPassword}")));

            // Sende eine GET-Anfrage an die BillBee API mit dem "externalId" - Filter
            HttpResponseMessage getResponse = client.GetAsync($"https://api.billbee.io/api/v1/orders/findbyextref/{externalOrderId}").Result;
            if (getResponse.IsSuccessStatusCode)
            {
                // anhand der externen order Id erstmal die normale BillBee Order Id finden
                string result = getResponse.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(result);
                var pos = result.IndexOf("BillBeeOrderId");
                string tempId = result.Substring(pos + 16, 17);
                long orderId = Convert.ToInt64(tempId);
                //Status auf "Versendet"
                HttpRequestMessage request3 = new HttpRequestMessage(new HttpMethod("PUT"), $"https://api.billbee.io/api/v1/orders/{orderId}/orderstate");
                string patchContent3 = "{ \"NewStateId\": \"4\" }";
                request3.Content = new StringContent(patchContent3, Encoding.UTF8, "application/json");
                HttpResponseMessage putResponse3 = client.SendAsync(request3).Result;
            }
        }

        public static string GetAllOrdersByState(string orderState)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Billbee-Api-Key", apiKey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{apiUsername}:{apiPassword}")));

            // Sende eine GET-Anfrage an die BillBee API mit dem "externalId" - Filter
            HttpResponseMessage getResponse = client.GetAsync($"https://api.billbee.io/api/v1/orders?orderStateId={orderState}&pageSize=1000").Result;
            if (getResponse.IsSuccessStatusCode)
            {
                // anhand der externen order Id erstmal die normale BillBee Order Id finden
                string result = getResponse.Content.ReadAsStringAsync().Result;
                return result;
            }
            return "";
        }

        public static void DownloadDeliveryNoteToPath(string path, string externalOrderId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Billbee-Api-Key", apiKey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{apiUsername}:{apiPassword}")));

            HttpResponseMessage getResponse = client.GetAsync($"https://api.billbee.io/api/v1/orders/findbyextref/{externalOrderId}").Result;
            if (getResponse.IsSuccessStatusCode)
            {
                // anhand der externen order Id erstmal die normale BillBee Order Id finden
                string result = getResponse.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(result);
                var pos = result.IndexOf("BillBeeOrderId");
                string tempId = result.Substring(pos + 16, 17);
                long orderId = Convert.ToInt64(tempId);

                var requestUri = $"https://api.billbee.io/api/v1/orders/CreateDeliveryNote/{orderId}?includePdf=true";
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var response = client.PostAsync(requestUri, content).Result;
                string responseString = response.Content.ReadAsStringAsync().Result;

                // pdf file download
                JObject json2 = JObject.Parse(responseString);
                string pdfData = (string)json2["Data"]["PDFData"];
                byte[] pdfBytes = Convert.FromBase64String(pdfData);
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    fs.Write(pdfBytes, 0, pdfBytes.Length);
                }
            }
        }

        public static void DownloadShippingLabelToPath(string path, string externalOrderId, string country)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Billbee-Api-Key", apiKey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{apiUsername}:{apiPassword}")));

            HttpResponseMessage getResponse = client.GetAsync($"https://api.billbee.io/api/v1/orders/findbyextref/{externalOrderId}").Result;
            if (getResponse.IsSuccessStatusCode)
            {
                // anhand der externen order Id erstmal die normale BillBee Order Id finden
                string result = getResponse.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(result);
                var pos = result.IndexOf("BillBeeOrderId");
                string tempId = result.Substring(pos + 16, 17);
                long orderId = Convert.ToInt64(tempId);
                long productId = 0;
                if (country == "DE")
                {
                    productId = 20000000000106553;
                }
                else
                {
                    productId = 20000000000109520;
                }

                var shipmentInformation = new
                {
                    OrderId = orderId,
                    ProviderId = 20000000000014121,
                    ProductId = productId,
                    ChangeStateToSend = true,
                    //PrinterName = "",
                    WeightInGram = 1000,
                    ShipDate = DateTime.UtcNow,
                    ClientReference = "ABC123",
                    Dimension = new
                    {
                        length = 10,
                        width = 5,
                        height = 2
                    }
                };
                var requestUri = $"https://api.billbee.io/api/v1/shipment/shipwithlabel";
                var content = new StringContent(JsonConvert.SerializeObject(shipmentInformation), Encoding.UTF8, "application/json");
                var response = client.PostAsync(requestUri, content).Result;
                string responseString = response.Content.ReadAsStringAsync().Result;
                //pdf download
                JObject json2 = JObject.Parse(responseString);
                string pdfData = (string)json2["Data"]["LabelDataPdf"];
                byte[] pdfBytes = Convert.FromBase64String(pdfData);
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    fs.Write(pdfBytes, 0, pdfBytes.Length);
                }
            }
        }
        public static string GetInternalBillBeeID(string externalOrderId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Billbee-Api-Key", apiKey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{apiUsername}:{apiPassword}")));

            // Sende eine GET-Anfrage an die BillBee API mit dem "externalId" - Filter
            HttpResponseMessage getResponse = client.GetAsync($"https://api.billbee.io/api/v1/orders/findbyextref/{externalOrderId}").Result;
            if (getResponse.IsSuccessStatusCode)
            {
                // anhand der externen order Id erstmal die normale BillBee Order Id finden
                string result = getResponse.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(result);
                var pos = result.IndexOf("BillBeeOrderId");
                string tempId = result.Substring(pos + 16, 17);
                long orderId = Convert.ToInt64(tempId);
                return orderId.ToString();
            }
            return "";
        }

        public static string GetSingleLayerField(string externalOrderId, string layer, string attribute)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Billbee-Api-Key", apiKey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{apiUsername}:{apiPassword}")));

            // Sende eine GET-Anfrage an die BillBee API mit dem "externalId" - Filter
            HttpResponseMessage getResponse = client.GetAsync($"https://api.billbee.io/api/v1/orders/findbyextref/{externalOrderId}").Result;
            if (getResponse.IsSuccessStatusCode)
            {
                // anhand der externen order Id erstmal die normale BillBee Order Id finden
                string result = getResponse.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(result);
                var pos = result.IndexOf("BillBeeOrderId");
                string tempId = result.Substring(pos + 16, 17);
                long orderId = Convert.ToInt64(tempId);
                // Sendungsnummer erhalten
                HttpResponseMessage response = client.GetAsync("https://api.billbee.io/api/v1/orders/" + orderId).Result;
                string responseString = response.Content.ReadAsStringAsync().Result;
                JObject json2 = JObject.Parse(responseString);
                string streetName = (string)json2["Data"][layer][attribute];
                return streetName;
            }
            return "";
        }

        public static string[] GetOrderlinesFromMainOrder(string externalOrderId)
        {
            string[] returnArray = new string[0];

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Billbee-Api-Key", apiKey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{apiUsername}:{apiPassword}")));

            // Sende eine GET-Anfrage an die BillBee API mit dem "externalId" - Filter
            HttpResponseMessage getResponse = client.GetAsync($"https://api.billbee.io/api/v1/orders/findbyextref/{externalOrderId}").Result;
            if (getResponse.IsSuccessStatusCode)
            {
                // anhand der externen order Id erstmal die normale BillBee Order Id finden
                string result = getResponse.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(result);
                var pos = result.IndexOf("BillBeeOrderId");
                string tempId = result.Substring(pos + 16, 17);
                long orderId = Convert.ToInt64(tempId);
                // Sendungsnummer erhalten
                HttpResponseMessage response = client.GetAsync("https://api.billbee.io/api/v1/orders/" + orderId).Result;
                string responseString = response.Content.ReadAsStringAsync().Result;
                JObject json2 = JObject.Parse(responseString);
                JArray orderlines = (JArray)json2["Data"]["OrderItems"];
                foreach (var item in orderlines)
                {
                    int quantity = (int)item["Quantity"];
                    double price = (double)item["TotalPrice"];
                    string product = (string)item["Product"]["Title"];
                    if (quantity > 1)
                    {
                        string adaptedPrice = (price / quantity).ToString();
                        for (int i = 0; i < quantity; i++)
                        {
                            string[] newArray = new string[] { adaptedPrice + "€ " + product };
                            returnArray = returnArray.Concat(newArray).ToArray();
                        }
                    }
                    else
                    {
                        string[] newArray = new string[] { price.ToString() + "€ " + product };
                        returnArray = returnArray.Concat(newArray).ToArray();
                    }
                }
                return returnArray;
            }
            return returnArray;
        }

        public static string GetTrackingNumber(string externalOrderId)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Billbee-Api-Key", apiKey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{apiUsername}:{apiPassword}")));

            // Sende eine GET-Anfrage an die BillBee API mit dem "externalId" - Filter
            HttpResponseMessage getResponse = client.GetAsync($"https://api.billbee.io/api/v1/orders/findbyextref/{externalOrderId}").Result;
            if (getResponse.IsSuccessStatusCode)
            {
                // anhand der externen order Id erstmal die normale BillBee Order Id finden
                string result = getResponse.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(result);
                var pos = result.IndexOf("BillBeeOrderId");
                string tempId = result.Substring(pos + 16, 17);
                long orderId = Convert.ToInt64(tempId);
                // Sendungsnummer erhalten
                HttpResponseMessage response = client.GetAsync("https://api.billbee.io/api/v1/orders/" + orderId).Result;
                string responseString = response.Content.ReadAsStringAsync().Result;
                JObject json2 = JObject.Parse(responseString);
                JArray shippingIds = (JArray)json2["Data"]["ShippingIds"];
                string shippingId = (string)shippingIds[0]["ShippingId"];
                return shippingId;
            }
            return "";
        }
        //public static void UpdateAddressOfSpecificOrder(string externalOrderId, string streetNumber, string streetName)
        //{
        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Add("X-Billbee-Api-Key", apiKey);
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
        //        Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{apiUsername}:{apiPassword}")));

        //    // Sende eine GET-Anfrage an die BillBee API mit dem "externalId" - Filter
        //    HttpResponseMessage getResponse = client.GetAsync($"https://api.billbee.io/api/v1/orders/findbyextref/{externalOrderId}").Result;
        //    if (getResponse.IsSuccessStatusCode)
        //    {
        //        // Lese den Inhalt der Antwort als String
        //        string result = getResponse.Content.ReadAsStringAsync().Result;
        //        // Parsen des JSON-Ergebnisses
        //        JObject json = JObject.Parse(result);
        //        var pos = result.IndexOf("BillBeeOrderId");
        //        string tempId = result.Substring(pos + 16, 17);
        //        long orderId = Convert.ToInt64(tempId);

        //        JToken shippingAddress = GetSingleLayer(externalOrderId, "ShippingAdress");
        //        var shippingAddressDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(shippingAddress.ToString());
        //        shippingAddressDict["HouseNumber"] = streetNumber;
        //        shippingAddressDict["Street"] = streetName;

        //        var requestUri = $"https://api.billbee.io/api/v1/orders/{orderId}";
        //        var content = new StringContent(JsonConvert.SerializeObject(new { ShippingAddress = shippingAddressDict }), Encoding.UTF8, "application/json");
        //        var method = new HttpMethod("PATCH");
        //        var request = new HttpRequestMessage(method, requestUri) { Content = content };
        //        var response = client.SendAsync(request).Result;

        //        MessageBox.Show("");
        //    }
        //}
        public static void UpdateAddressOfSpecificOrder(string externalOrderId, string streetNumber, string streetName)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Billbee-Api-Key", apiKey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{apiUsername}:{apiPassword}")));

            // Sende eine GET-Anfrage an die BillBee API mit dem "externalId" - Filter
            HttpResponseMessage getResponse = client.GetAsync($"https://api.billbee.io/api/v1/orders/findbyextref/{externalOrderId}").Result;
            if (getResponse.IsSuccessStatusCode)
            {
                // Lese den Inhalt der Antwort als String
                string result = getResponse.Content.ReadAsStringAsync().Result;
                // Parsen des JSON-Ergebnisses
                JObject json = JObject.Parse(result);
                var pos = result.IndexOf("BillBeeOrderId");
                string tempId = result.Substring(pos + 16, 17);
                long orderId = Convert.ToInt64(tempId);

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://api.billbee.io/api/v1/orders/{orderId}");
                // Erstelle ein HttpContent-Objekt mit dem PATCH-Inhalt
                string patchContent = "{ \"ShippingAddress\": { \"Street\": \"" + streetName + "\", \"HouseNumber\": \"" + streetNumber + "\" } }";
                request.Content = new StringContent(patchContent, Encoding.UTF8, "application/json");
                HttpResponseMessage putResponse = client.SendAsync(request).Result;
                MessageBox.Show("");
            }
        }

        public static void PatchAfterSalesData(string externalOrderId, string imei)
        {
            try
            {
                // Erstelle einen HttpClient und setze den X-Billbee-Api-Key-Header
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-Billbee-Api-Key", apiKey);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{apiUsername}:{apiPassword}")));

                // Sende eine GET-Anfrage an die BillBee API mit dem "externalId" - Filter
                HttpResponseMessage getResponse = client.GetAsync($"https://api.billbee.io/api/v1/orders/findbyextref/{externalOrderId}").Result;
                if (getResponse.IsSuccessStatusCode)
                {
                    // Lese den Inhalt der Antwort als String
                    string result = getResponse.Content.ReadAsStringAsync().Result;
                    // Parsen des JSON-Ergebnisses
                    JObject json = JObject.Parse(result);
                    var pos = result.IndexOf("BillBeeOrderId");
                    string tempId = result.Substring(pos + 16, 17);
                    long orderId = Convert.ToInt64(tempId);
                    HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://api.billbee.io/api/v1/orders/{orderId}");
                    // Erstelle ein HttpContent-Objekt mit dem PATCH-Inhalt
                    string patchContent = "{ \"OrderNumber\": \"" + externalOrderId + " IMEI: " + imei + "\" }";
                    request.Content = new StringContent(patchContent, Encoding.UTF8, "application/json");
                    // Sende die PATCH-Anfrage
                    HttpResponseMessage putResponse = client.SendAsync(request).Result;
                    //Status auf "Gepackt"
                    HttpRequestMessage request2 = new HttpRequestMessage(new HttpMethod("PUT"), $"https://api.billbee.io/api/v1/orders/{orderId}/orderstate");
                    string patchContent2 = "{ \"NewStateId\": \"13\" }";
                    request2.Content = new StringContent(patchContent2, Encoding.UTF8, "application/json");

                    HttpResponseMessage putResponse2 = client.SendAsync(request2).Result;

                    //Status auf "Versendet"
                    HttpRequestMessage request3 = new HttpRequestMessage(new HttpMethod("PUT"), $"https://api.billbee.io/api/v1/orders/{orderId}/orderstate");
                    string patchContent3 = "{ \"NewStateId\": \"4\" }";
                    request3.Content = new StringContent(patchContent3, Encoding.UTF8, "application/json");

                    HttpResponseMessage putResponse3 = client.SendAsync(request3).Result;
                    //if (CheckMarketPlace(externalOrderId) == "Ebay")
                    //{
                    //    // remove the imei because of complications with PayJoe's API stuff
                    //    HttpRequestMessage request4 = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://api.billbee.io/api/v1/orders/{orderId}");
                    //    // Erstelle ein HttpContent-Objekt mit dem PATCH-Inhalt
                    //    string patchContent4 = "{ \"OrderNumber\": \"" + externalOrderId + "\" }";
                    //    request4.Content = new StringContent(patchContent, Encoding.UTF8, "application/json");
                    //    // Sende die PATCH-Anfrage
                    //    HttpResponseMessage putResponse4 = client.SendAsync(request4).Result;
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Es gab folgenden Fehler bei der Order" + externalOrderId + ex.Message);
            }

        }
        public static string CheckMarketPlace(string orderId)
        {
            if (orderId.Contains("-"))
            {
                return "Ebay";
            }
            return "BackMarket";
        }
    }
}
