using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace EigenbelegToolAlpha
{
    public class EvaluationsBackMarketPDF
    {
        public void Main()
        {
            BuildTextFiles("Normal", "BackMarketNormal");
            BuildTextFiles("PayPal", "BackMarketPayPal");
            BuildTextFilesNewInvoicing();
        }

        public double CalculateSellerCommission(string salesVolume)
        {
            if (salesVolume == "N/A")
            {
                return 0;
            }
            var dbManager = new DBManager();
            double sellerCommissionPercentage = EvaluationHelperClass.ConvertPercentageStringToDouble(dbManager.ExecuteQueryWithResultString("ConfigFees", "Costs", "Typ", "BackMarketSellerCommission"));
            double sellerCommission = Convert.ToDouble(salesVolume) * sellerCommissionPercentage;
            return sellerCommission;
        }

        public double CollectReturnAmount()
        {
            double returnAmount = 0;
            string[] numbers = new string[3] { "1", "2", "3" };
            string pathPreset = "BackmarketNormal";
            string pathPreset2 = "BackmarketPayPal";
            string searchValue = "MONTANT DES COMMANDES REMBOURSÉES";
            string searchValueTotal = "TOTAL";
            int arrayIndexerPDF = 0;
            foreach (string number in numbers)
            {
                string buildPath = pathPreset + numbers[arrayIndexerPDF] + ".txt";
                string[] allLines = File.ReadAllLines(buildPath);
                arrayIndexerPDF++;
                int indexReturnList = findLine(allLines, searchValue);
                //Alle Orders in Array auflisten | sozusagen ein Filter von allLines[]
                for (int i = indexReturnList + 1; i < allLines.Count(); i++)
                {
                    if (!allLines[i].Contains(searchValueTotal))
                    {
                        double temp = GetReturnValueOfOneLine(i, allLines);
                        returnAmount += temp;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            arrayIndexerPDF = 0;
            foreach (string number in numbers)
            {
                string buildPath = pathPreset2 + numbers[arrayIndexerPDF] + ".txt";
                string[] allLines = File.ReadAllLines(buildPath);
                arrayIndexerPDF++;
                int indexReturnList = findLine(allLines, searchValue);
                //Alle Orders in Array auflisten | sozusagen ein Filter von allLines[]
                for (int i = indexReturnList + 1; i < allLines.Count(); i++)
                {
                    if (!allLines[i].Contains(searchValueTotal))
                    {
                        double temp = GetReturnValueOfOneLine(i, allLines);
                        returnAmount += temp;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return returnAmount;
        }

        public double RoundOneDigit(double adaptValue)
        {
            string tempValue = adaptValue.ToString();
            if (tempValue.Contains(","))
            {
                var pos = tempValue.IndexOf(",");
                tempValue = tempValue.Substring(0, pos + 2);
                adaptValue = Convert.ToDouble(tempValue);
            }
            return adaptValue;
        }
        public double RemoveMinus(double fixValue)
        {
            double newValue = -fixValue;
            return newValue;
        }
        public string GetSalesVolume(string orderID, string pdf)
        {
            double shippingPrice = GetSureDoubleConversionWorks(BackMarketAPIHandler.GetSpecificFieldOfOrder(orderID, "shipping_price"));
            double price = GetSureDoubleConversionWorks(BackMarketAPIHandler.GetSpecificFieldOfOrder(orderID, "price"));
            double total = shippingPrice + price;
            return total.ToString();
        }

        public double GetSureDoubleConversionWorks(string value)
        {
            double result;
            CultureInfo culture = CultureInfo.InvariantCulture;

            if (Double.TryParse(value, NumberStyles.Float, culture, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public bool CheckPDFTypeIfNormal(string pdfPath)
        {
            if (pdfPath.ToLower().Contains("paypal"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public string FindPDFViaOrderNumber(string orderID)
        {
            if (orderID.Contains("€"))
            {
                return "";
            }
            string[] numbers = new string[3] { "1", "2", "3" };
            string pathPreset = "BackmarketNormal";
            string searchValueForGrossSalesList = " 1 ";
            string searchValueForGrossSalesList2 = " 2 ";
            string searchValueForGrossSalesList3 = "TOTAL";
            string searchValueHeadingGrossSalesList = "MONTANT DES COMMANDES EXPEDIÉES DU";
            int arrayIndexerPDF = 0;
            int arrayIndexerSales = 0;
            foreach (string number in numbers)
            {
                string buildPath = pathPreset + numbers[arrayIndexerPDF] + ".txt";
                arrayIndexerPDF++;
                string[] salesList = new string[1000];
                string[] allLines = File.ReadAllLines(buildPath);
                int indexGrossSalesList = findLine(allLines, searchValueHeadingGrossSalesList);
                //Alle Orders in Array auflisten | sozusagen ein Filter von allLines[]
                for (int i = indexGrossSalesList + 1; i < allLines.Count(); i++)
                {
                    if (allLines[i].Contains(searchValueForGrossSalesList) || allLines[i].Contains(searchValueForGrossSalesList2) || !allLines[i].Contains(searchValueForGrossSalesList3))
                    {
                        salesList[arrayIndexerSales] = allLines[i];
                        arrayIndexerSales++;
                    }
                    else
                    {
                        break;
                    }
                }
                // hier abfragen ob die hauptorder vorhanden ist
                foreach (string line in salesList)
                {
                    if (line != null)
                    {
                        if (line.Contains(orderID))
                        {
                            return buildPath;
                        }
                    }
                }
            }
            //PayPal
            string[] numbers2 = new string[3] { "1", "2", "3" };
            string pathPreset2 = "BackmarketPayPal";
            int arrayIndexerPDF2 = 0;
            foreach (string number in numbers)
            {
                string buildPath = pathPreset2 + numbers2[arrayIndexerPDF2] + ".txt";
                arrayIndexerPDF2++;
                string[] salesList = new string[1000];
                string[] allLines = File.ReadAllLines(buildPath);
                int indexGrossSalesList = findLine(allLines, searchValueHeadingGrossSalesList);
                //Alle Orders in Array auflisten | sozusagen ein Filter von allLines[]
                for (int i = indexGrossSalesList + 1; i < allLines.Count(); i++)
                {
                    if (allLines[i].Contains(searchValueForGrossSalesList) || allLines[i].Contains(searchValueForGrossSalesList2) || !allLines[i].Contains(searchValueForGrossSalesList3))
                    {
                        salesList[arrayIndexerSales] = allLines[i];
                        arrayIndexerSales++;
                    }
                    else
                    {
                        break;
                    }
                }
                // hier abfragen ob die hauptorder vorhanden ist
                foreach (string line in salesList)
                {
                    if (line != null)
                    {
                        if (line.Contains(orderID))
                        {
                            return buildPath;
                        }
                    }
                }
            }
            //PayPal International
            string[] numbers3 = new string[3] { "1", "2", "3" };
            string pathPreset3 = "BackmarketPayPalInternational";
            int arrayIndexerPDF3 = 0;
            foreach (string number in numbers)
            {
                string buildPath = pathPreset3 + numbers3[arrayIndexerPDF3] + ".txt";
                arrayIndexerPDF3++;
                string[] salesList = new string[1000];
                string[] allLines = File.ReadAllLines(buildPath);
                int indexGrossSalesList = findLine(allLines, searchValueHeadingGrossSalesList);
                //Alle Orders in Array auflisten | sozusagen ein Filter von allLines[]
                for (int i = indexGrossSalesList + 1; i < allLines.Count(); i++)
                {
                    if (allLines[i].Contains(searchValueForGrossSalesList) || allLines[i].Contains(searchValueForGrossSalesList2) || !allLines[i].Contains(searchValueForGrossSalesList3))
                    {
                        salesList[arrayIndexerSales] = allLines[i];
                        arrayIndexerSales++;
                    }
                    else
                    {
                        break;
                    }
                }
                // hier abfragen ob die hauptorder vorhanden ist
                foreach (string line in salesList)
                {
                    if (line != null)
                    {
                        if (line.Contains(orderID))
                        {
                            return buildPath;
                        }
                    }
                }
            }
            return "N/A";

        }

        public double GetReturnValueOfOneLine(int index, string[] array)
        {
            double value = 0;
            string temp = array[index].ToString();
            string space = " ";
            string euroSign = "€";
            int posEuroSign = temp.IndexOf(euroSign);
            int fullLength = temp.Length;
            string newTemp = temp.Substring(posEuroSign - 10, fullLength - posEuroSign + 10);
            int lengthNewTemp = newTemp.Length;
            int posSpace = newTemp.IndexOf(space);
            newTemp = newTemp.Substring(posSpace, lengthNewTemp - posSpace - 1);
            value = Convert.ToDouble(newTemp);
            return value;
        }
        public double getValueOfOneLine(int index, string[] array, int lengthOfTheFirstPos, string firstPos, string secondPos)
        {
            string newValue = "";
            string tempSave = array[index].ToString();
            var fullLength = tempSave.Length;
            var posFirst = tempSave.IndexOf(firstPos);
            var posSecond = tempSave.IndexOf(secondPos);
            string tempValue = tempSave.Substring(posFirst + lengthOfTheFirstPos, posSecond - posFirst - lengthOfTheFirstPos - 1);
            //Erweiterung für Tausenderbeträge mit Leerzeichen
            if (checkSpaceSign(tempValue) == true)
            {
                if (checkMinusSign(tempValue) == true)
                {
                    var posSpace = tempValue.IndexOf(" ");
                    var length = tempValue.Length;
                    string temp1 = tempValue.Substring(1, 1);
                    string temp2 = tempValue.Substring(posSpace + 1, length - posSpace - 1);
                    tempValue = temp1 + temp2;
                }
                else
                {
                    var posSpace = tempValue.IndexOf(" ");
                    var length = tempValue.Length;
                    string temp1 = tempValue.Substring(0, 1);
                    string temp2 = tempValue.Substring(posSpace + 1, length - posSpace - 1);
                    tempValue = temp1 + temp2;
                }
            }
            if (checkMinusSign(tempValue) == true)
            {
                var length2 = tempValue.Length;
                tempValue = tempValue.Substring(0, length2 - 1);
            }
            if (tempValue == "0,00")
            {
                tempValue = "0";
            }

            newValue = tempValue;
            double value = Convert.ToDouble(newValue);
            return value;
        }
        public bool checkMinusSign(string tempValue)
        {
            if (tempValue.Contains("-"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkSpaceSign(string tempValue)
        {
            if (tempValue.Contains(" "))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int findLine(string[] array, string searchValue)
        {
            int backValue = 0;
            for (int i = 1; i < array.Count(); i++)
            {
                if (array[i].Contains(searchValue))
                {
                    backValue = i + 1;
                    break;
                }
            }
            return backValue;
        }
        public int FindLineWithSpecificBegin(string[] array, string searchValue, int beginIndex)
        {
            int backValue = 0;
            for (int i = beginIndex; i < array.Count(); i++)
            {
                if (array[i].Contains(searchValue))
                {
                    backValue = i + 1;
                    break;
                }
            }
            return backValue;
        }
        private void BuildTextFiles(string type, string preSet)
        {
            EvaluationsFirstPage eval = new EvaluationsFirstPage();
            string[] numbers = new string[3] { "1", "2", "3" };
            string attribute = "";
            //build 3 .txt files
            int arrayIndexer = 0;
            var dbManager = new DBManager();
            foreach (string number in numbers)
            {
                attribute = preSet + numbers[arrayIndexer];
                arrayIndexer++;
                string path = dbManager.ExecuteQueryWithResultStringTwoConditions("Evaluations", attribute, "Monat", "Jahr", EvaluationsFirstPage.month, EvaluationsFirstPage.year);
                string tempPath = "Backmarket" + type + arrayIndexer + ".txt";
                FileStream stream = File.Create(tempPath);
                stream.Close();
                File.WriteAllText(tempPath, ExtractTextFromPdf(path));
                if (File.Exists(path) != true)
                {
                    continue;
                }

            }
        }
        public void BuildTextFilesNewInvoicing()
        {
            EvaluationsFirstPage eval = new EvaluationsFirstPage();
            string[] numbers = new string[3] { "1", "2", "3" };
            string attribute = "";
            //build 3 .txt files
            int arrayIndexer = 0;
            var dbManager = new DBManager();
            foreach (string number in numbers)
            {
                attribute = "BackMarketPayPal" + numbers[arrayIndexer] + "International";
                arrayIndexer++;
                string path = dbManager.ExecuteQueryWithResultStringTwoConditions("Evaluations", attribute, "Monat", "Jahr", EvaluationsFirstPage.month, EvaluationsFirstPage.year);
                string tempPath = "BackmarketPayPalInternational" + arrayIndexer + ".txt";
                FileStream stream = File.Create(tempPath);
                stream.Close();
                File.WriteAllText(tempPath, ExtractTextFromPdf(path));
                if (File.Exists(path) != true)
                {
                    continue;
                }

            }
        }

        public static string ExtractTextFromPdf(string path)
        {
            try
            {
                using (PdfReader reader = new PdfReader(path))
                {
                    StringBuilder text = new StringBuilder();

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                    }

                    return text.ToString();
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }

}
