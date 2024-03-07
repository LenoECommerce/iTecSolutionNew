using iTextSharp.text.pdf.codec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public class ScriptAssignMonthToDataSet
    {

        public static async Task<bool> CheckIfOrderIsRelevantAsync(string orderId, string marketPlace)
        {
            // check for multiple orderline adaption
            if (orderId.Contains("."))
            {
                var length = orderId.Length;
                orderId = orderId.Substring(0, length-2);
            }
            bool isRelevant = false;
            if (marketPlace == "Ebay")
            {
                isRelevant = await CheckEbayPDF(orderId);
            }
            else if (marketPlace == "BackMarket")
            {
                isRelevant = await CheckBackMarketPDF(orderId);
            }
            else if (marketPlace.Contains("Extern"))
            {
                MessageBox.Show("Folgender Datensatz der Klasse Extern hat noch kein zugewiesenes Jahr: " + orderId);
            }
            return isRelevant;
        }
        public static async Task<bool> CheckBackMarketPDF (string orderId)
        {
            bool isRelevant = false;
            // building the text files
            EvaluationsBackMarketPDF pdfManager = new EvaluationsBackMarketPDF();
            pdfManager.Main();
            string pdfMatching = pdfManager.FindPDFViaOrderNumber(orderId);
            if (pdfMatching != "N/A")
            {
                isRelevant = true;
            }
            return isRelevant;
        }
        public static async Task<bool> CheckEbayPDF(string orderId)
        {
            bool isRelevant = false;
            // building the text file
            EvaluationsEbayPDF evaluationsEbayPDF = new EvaluationsEbayPDF();
            evaluationsEbayPDF.Main();
            string returnValue = evaluationsEbayPDF.GetSalesVolume(orderId);
            if (returnValue != "N/A")
            {
                isRelevant = true;
            }
            return isRelevant;
        }




    }
}
