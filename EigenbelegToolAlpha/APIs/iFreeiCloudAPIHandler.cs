using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha.APIs
{
    public class iFreeiCloudAPIHandler
    {
        private static readonly string apiKey = "89M-AHK-D68-FQE-1HG-8NH-LID-QWG";
        private static readonly string baseUrl = "https://api.ifreeicloud.co.uk";
        public static string findMyiPhoneStatus = "";
        public static string blacklistStatus = "";
        public static string model = "";
        public static string storage = "";
        public static string color = "";

        public static void SetProductAttributes(string imei)
        {
            string serviceId = "81";
            string endpointUrl = baseUrl + "?imei=" + imei + "&key=" + apiKey + "&service=" + serviceId;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpointUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Get the response stream and read the contents
                    Stream responseStream = response.GetResponseStream();
                    string responseString = new StreamReader(responseStream).ReadToEnd();
                    JObject json = JObject.Parse(responseString);
                    color = (string)json["object"]["color"];
                    storage = (string)json["object"]["capacity"];
                    model = (string)json["object"]["model"];
                    // Do something with the response string
                }
                else
                {
                    MessageBox.Show("Es gab einen Fehler bei der FMI Check Anfrage.");
                }
            }

        }
        public void APIRequests(string imei)
        {
            //FMI part
            string serviceId = "4";
            string endpointUrl = baseUrl + "?imei=" + imei + "&key=" + apiKey + "&service=" + serviceId;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpointUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Get the response stream and read the contents
                    Stream responseStream = response.GetResponseStream();
                    string responseString = new StreamReader(responseStream).ReadToEnd();
                    JObject json = JObject.Parse(responseString);
                    findMyiPhoneStatus = (string)json["object"]["fmiOn"];
                    // Do something with the response string
                }
                else
                {
                    MessageBox.Show("Es gab einen Fehler bei der FMI Check Anfrage.");
                }
            }

            // blacklist status
            blacklistStatus = "false";
            //serviceId = "55";
            //endpointUrl = baseUrl + "?imei=" + imei + "&key=" + apiKey + "&service=" + serviceId;
            //HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(endpointUrl);
            //request2.Method = "POST";
            //request2.ContentType = "application/x-www-form-urlencoded";
            //request2.Accept = "application/json";

            //using (HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse())
            //{
            //    if (response2.StatusCode == HttpStatusCode.OK)
            //    {
            //        // Get the response stream and read the contents
            //        Stream responseStream = response2.GetResponseStream();
            //        string responseString = new StreamReader(responseStream).ReadToEnd();
            //        JObject json = JObject.Parse(responseString);
            //        blacklistStatus = (string)json["object"]["blacklistStatus"];
            //        MessageBox.Show("p)9LWtzwWTpe4o$x");
            //        // Do something with the response string
            //    }
            //    else
            //    {
            //        MessageBox.Show("Es gab einen Fehler bei der Blacklist Status Check Anfrage.");
            //    }
            //}
        }

    }
}
