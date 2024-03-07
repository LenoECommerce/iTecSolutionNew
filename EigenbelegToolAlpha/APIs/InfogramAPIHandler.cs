using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace EigenbelegToolAlpha.APIs
{
    public class InfogramAPIHandler
    {
        public static string accessKey = "RfvUnSaIV8MD5dEIa8iMMk1lneGvLqwf";
        public static string secretKey = "ckYD3O0LyVt7otrUWEjoJfuoDBJZfiwM";

        private static readonly string[] UriRfc3986CharsToEscape = new[] { "!", "*", "'", "(", ")" };
        internal static string EscapeUriDataStringRfc3986(string value)
        {
            StringBuilder escaped = new StringBuilder(Uri.EscapeDataString(value));
            for (int i = 0; i < UriRfc3986CharsToEscape.Length; i++)
            {
                escaped.Replace(UriRfc3986CharsToEscape[i], Uri.HexEscape(UriRfc3986CharsToEscape[i][0]));
            }

            return escaped.ToString();
        }
        public static void Main()
        {
            var id = "c0c6fe92-96b2-4315-8b03-7b3ab9de75d2";
            var url = "https://infogr.am/service/v1/infographics/" + id;
            var key = accessKey;
            var secret = secretKey;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetSignedUrl("get", url, key, secret));

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();
                string responseString = new StreamReader(responseStream).ReadToEnd();
                Console.WriteLine(responseString);
            }
        }

        //public static void Main()
        //{
        //    var id = "c0c6fe92-96b2-4315-8b03-7b3ab9de75d2";
        //    var url = "https://infogr.am/service/v1/infographics/" + id;
        //    var key = accessKey;
        //    var secret = secretKey;

        //    string updatedContent = "{ \"type\": \"h1\", \"text\": \"Testing API client\" }";

        //    string json = "{\"data\":{\"content\":[" + updatedContent + "]}}";

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GetSignedUrl("put", url, key, secret));
        //    request.Method = "PUT";
        //    request.ContentType = "application/json";

        //    using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        //    {
        //        writer.Write(json);
        //        writer.Flush();
        //    }

        //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //    {
        //        Stream responseStream = response.GetResponseStream();
        //        string responseString = new StreamReader(responseStream).ReadToEnd();
        //        Console.WriteLine(responseString);
        //    }
        //}

        private static string GetSignedUrl(string method, string url, string apiKey, string apiSecret)
        {
            UriBuilder uri = new UriBuilder(url);
            List<string> signatureComponents = new List<string>();
            signatureComponents.Add(method.ToUpper());
            signatureComponents.Add(EscapeUriDataStringRfc3986(uri.Scheme + "://" + uri.Host + uri.Path));
            NameValueCollection qs = HttpUtility.ParseQueryString(uri.Query);
            qs.Add("api_key", apiKey);
            string[] orderedKeys = qs.AllKeys;
            Array.Sort(orderedKeys);
            List<string> ql = new List<string>();
            foreach (var key in orderedKeys)
            {
                ql.Add(EscapeUriDataStringRfc3986(key) + "=" + EscapeUriDataStringRfc3986(qs[key]));
            }

            if (qs.Count > 0)
            {
                signatureComponents.Add(EscapeUriDataStringRfc3986(String.Join("&", ql)));
            }

            var signatureBase = String.Join("&", signatureComponents);

            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(apiSecret));
            var bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(signatureBase));
            var apiSig = Convert.ToBase64String(bytes);

            qs.Add("api_sig", apiSig);
            uri.Query = qs.ToString();
            return uri.ToString();
        }


    }
}

