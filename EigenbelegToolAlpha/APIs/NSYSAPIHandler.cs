using Microsoft.Office.Interop.Access;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Google.Apis.Requests.BatchRequest;
using static iTextSharp.text.pdf.codec.TiffWriter;
using System.Text.Json;

namespace EigenbelegToolAlpha.APIs
{
    public class NSYSAPIHandler
    {
        private readonly static string apiKey = "aOAat45xkUWPFwkfJpBRrw";
        public static string imei = "";

        public static string[] ReturnValuesOfSpecificDevice(string imeiInput)
        {
            imei = imeiInput;
            string[] returnValues = new string[3];
            string response = GetResponseStringByIMEI();
            JObject json = JObject.Parse(response);
            returnValues[0] = (string)json["Model"];
            returnValues[1] = (string)json["Memory"];
            returnValues[2] = (string)json["Color"];
            return returnValues;
        }
        public static string GetDeviceIMEIFromLastMinute()
        {
            // 2h weniger weil alle Anfragen auf UTC 0 sind
            DateTime now = DateTime.Now.AddHours(-2);
            DateTime oneMinuteAgo = now.AddHours(-1);
            string dateBegin = oneMinuteAgo.ToString("yyyy-MM-dd HH:mm:ss");
            string dateEnd = now.ToString("yyyy-MM-dd HH:mm:ss");
            string endpointUrl = "https://api.nsystools.com/ClientApi/devices-connections?dateBegin=" + dateBegin + "&dateEnd=" + dateEnd;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpointUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";
            request.ContentLength = 0;
            request.Headers.Add("X-API-key:" + apiKey);


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Get the response stream and read the contents
                    Stream responseStream = response.GetResponseStream();
                    string responseString = new StreamReader(responseStream).ReadToEnd();
                    JArray results = JArray.Parse(responseString);
                    JObject lastResult = null;

                    foreach (JObject result in results)
                    {
                        lastResult = result;
                    }
                    return (string)lastResult["IMEI"];
                    // Do something with the response string
                }
                else
                {
                    // Handle error
                }
            }
            return "";
        }

        public static string GetCheckIMEIReponse(string type)
        {
            string endpointUrl = "https://api.nsystools.com/ClientApi/CheckImei?imei=" + imei + "&type=" + type;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpointUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";
            request.ContentLength = 0;
            request.Headers.Add("X-API-key:" + apiKey);


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Get the response stream and read the contents
                    Stream responseStream = response.GetResponseStream();
                    string responseString = new StreamReader(responseStream).ReadToEnd();
                    JObject json = JObject.Parse(responseString);
                    string returnValue = (string)json["Model"];
                    return responseString; ;
                    // Do something with the response string
                }
                else
                {
                    // Handle error
                }
            }
            return "";
        }

        public static string GetResponseStringByIMEI()
        {
            string endpointUrl = "https://api.nsystools.com/ClientApi/DeviceInfo?imei="+imei+"&getAgInfo=false";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpointUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";
            request.ContentLength = 0;
            request.Headers.Add("X-API-key:" + apiKey);


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Get the response stream and read the contents
                    Stream responseStream = response.GetResponseStream();
                    string responseString = new StreamReader(responseStream).ReadToEnd();
                    return responseString; ;
                    // Do something with the response string
                }
                else
                {
                    // Handle error
                }
            }
            return "";
        }
    }
}
