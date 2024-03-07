using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace EigenbelegToolAlpha
{
    public class BuyBackPostPriceAdaption
    {
        public static string Main(string strFileName)
        {
            int xlrow;
            int countValueChanges = 0;
            string newLastDataImport = "";

            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
            Microsoft.Office.Interop.Excel.Range xlRange;

            var dbManager = new DBManager();
            try
            {
                if (strFileName != "")
                {
                    xlApp = new Microsoft.Office.Interop.Excel.Application();
                    xlWorkbook = xlApp.Workbooks.Open(strFileName);
                    xlWorksheet = xlWorkbook.Worksheets["Worksheet"];
                    xlRange = xlWorksheet.UsedRange;

                    for (xlrow = 1; xlrow <= xlRange.Rows.Count; xlrow++)
                    {
                        if (xlRange.Cells[xlrow, 7].Text == "ADYEN NV")
                        {
                            string orderID = GetOrderIDFromKuselWusel(xlRange.Cells[xlrow, 11].Text);
                            // sync price part
                            string transactionAmount = RemoveMinus(xlRange.Cells[xlrow, 13].Text);
                            var posMatchingEntry = dbManager.ExecuteQueryWithResult("Eigenbelege", "Id", "Referenz", orderID);
                            
                            //Compare amount values
                            string tempNumber = dbManager.ExecuteQueryWithResultString("Eigenbelege", "Eigenbelegnummer", "Id", posMatchingEntry.ToString());
                            string tempCalcOldAmount = CheckEuroSign(dbManager.ExecuteQueryWithResultString("Eigenbelege", "Kaufbetrag", "Id", posMatchingEntry.ToString()));
                            double calcOldAmount = Convert.ToDouble(tempCalcOldAmount);
                            double calcNewAmount = Convert.ToDouble(transactionAmount);
                            double difference = calcOldAmount - calcNewAmount;
                            if (difference != 0)
                            {
                                dbManager.ExecuteQuery("UPDATE `Eigenbelege` SET `Kaufbetrag` = '" + transactionAmount + "' WHERE `Referenz` = '" + orderID + "'");
                                dbManager.ExecuteQuery("UPDATE `Reparaturen` SET `Kaufbetrag` = '" + transactionAmount + "' WHERE `EBReferenz` = '" + tempNumber + "'");
                                AdaptDBIndicatorField(orderID);
                                countValueChanges++;
                            }
                            // adding bank payment date
                            string bankpaymentDate = dbManager.ExecuteQueryWithResultString("Eigenbelege", "BankPaymentDate", "Referenz", orderID);
                            if (string.IsNullOrEmpty(bankpaymentDate))
                            {
                                string date = xlRange.Cells[xlrow, 3].Text;
                                string dateAdapted = date.Substring(0, 10).Replace("-",".");
                                dbManager.ExecuteQuery("UPDATE `Eigenbelege` SET `BankPaymentDate` = '" + dateAdapted + "' WHERE `Referenz` = '" + orderID + "'");
                            }
                        }
                        
                    }
                    xlWorkbook.Close();
                    xlApp.Quit();
                    newLastDataImport = DateTime.Now.ToString();
                    dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = '" + newLastDataImport + "' WHERE `Typ` = 'LastBuyBackSync'");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message);
            }
            MessageBox.Show("Es wurden " + countValueChanges + " Werte angepasst.");
            return newLastDataImport;
        }

        public static void AdaptDBIndicatorField(string id)
        {
            string query = "UPDATE `Eigenbelege` SET `IsPricePostProcessed` = 'Yes' WHERE `Referenz` = '" + id + "'";
            var dbManager = new DBManager();
            dbManager.ExecuteQuery(query);
        }

        public static string CheckEuroSign(string checkValue)
        {
            if (checkValue.Contains("€"))
            {
                var length = checkValue.Length;
                checkValue = checkValue.Substring(0, length - 1);
            }
            return checkValue;
        }

        public static string RemoveMinus(string adaptValue)
        {
            return adaptValue.Substring(1, adaptValue.Length - 1);
        }

        public static string GetOrderIDFromKuselWusel(string fullTransactionstext)
        {
            var fullLength = fullTransactionstext.Length;
            var posMarket = fullTransactionstext.IndexOf("Market");
            string temp = fullTransactionstext.Substring(posMarket + 7, fullLength - posMarket - 7);
            var posLastSpace = temp.IndexOf(" ");
            return temp.Substring(0, posLastSpace);
        }
    }
}
