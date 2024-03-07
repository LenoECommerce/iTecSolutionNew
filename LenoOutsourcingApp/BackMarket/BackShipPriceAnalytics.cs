using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace EigenbelegToolAlpha
{
    public class BackShipPriceAnalytics
    {
        public static string[] shippingCountries = new string[] { };
        public static string[] backShipCosts = new string[] { };
        public static string[] shippingDates = new string[] { };
        public static int ordersTotal = 0;
        public static int ordersBackShip = 0;

        public static void MainOK()
        {
            CollectValuesToArrays();
            GenerateBackShipPriceReport();
        }

        public static void AnalyseExcel()
        {
            string filePath = "C:\\Users\\lenna\\Desktop\\BackShipPriceReport.xlsx";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet worksheet = workbook.ActiveSheet;

            // Find the last row in column A
            Excel.Range lastRowRange = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            int lastRow = lastRowRange.Row;

            // Filter and calculate average costs for each month and country
            Dictionary<string, Dictionary<string, List<double>>> monthlyAverages = new Dictionary<string, Dictionary<string, List<double>>>();

            for (int row = 3; row <= lastRow; row++) // Start from row 2, assuming row 1 contains headers
            {
                Excel.Range countryCodeCell = worksheet.Cells[row, 1];
                Excel.Range dateCell = worksheet.Cells[row, 2];
                Excel.Range costCell = worksheet.Cells[row, 3];

                string countryCode = countryCodeCell.Value?.ToString();
                string dateString = dateCell.Value?.ToString();
                string costString = costCell.Value?.ToString();

                if (string.IsNullOrEmpty(countryCode) || string.IsNullOrEmpty(dateString) || string.IsNullOrEmpty(costString))
                    break; // Stop iteration if empty row is encountered

                DateTime date = DateTime.ParseExact(dateString, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                string monthYear = date.ToString("MM/yyyy");

                if (!monthlyAverages.ContainsKey(monthYear))
                    monthlyAverages.Add(monthYear, new Dictionary<string, List<double>>());

                if (!monthlyAverages[monthYear].ContainsKey(countryCode))
                    monthlyAverages[monthYear].Add(countryCode, new List<double>());

                double cost = Convert.ToDouble(costString);
                monthlyAverages[monthYear][countryCode].Add(cost);
            }

            // Create a new worksheet for the results
            Excel.Worksheet resultsWorksheet = workbook.Worksheets.Add();
            resultsWorksheet.Name = "Monthly Averages";

            // Write the header row
            resultsWorksheet.Cells[1, 1] = "Month/Year";

            List<string> countryCodes = monthlyAverages.Values.SelectMany(x => x.Keys).Distinct().OrderBy(x => x).ToList();

            for (int i = 0; i < countryCodes.Count; i++)
            {
                resultsWorksheet.Cells[1, i + 2] = countryCodes[i];
            }

            // Write the data rows
            int resultsRow = 2;

            foreach (var entry in monthlyAverages.OrderBy(x => x.Key))
            {
                string monthYear = entry.Key;
                var countryAverages = entry.Value;

                resultsWorksheet.Cells[resultsRow, 1] = monthYear;

                foreach (var countryCode in countryCodes)
                {
                    double averageCost = countryAverages.ContainsKey(countryCode) ? countryAverages[countryCode].Average() : 0.0;
                    resultsWorksheet.Cells[resultsRow, countryCodes.IndexOf(countryCode) + 2] = averageCost.ToString("0.00", CultureInfo.GetCultureInfo("en-US"));
                }

                resultsRow++;
            }

            // Save and close the workbook
            workbook.Save();
            workbook.Close();
            // Cleanup
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(resultsWorksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            worksheet = null;
            resultsWorksheet = null;
            workbook = null;
            excelApp = null;
            GC.Collect();
        }

        public static void GenerateBackShipPriceReport()
        {
            // Create a new Excel application
            Excel.Application excelApp = new Excel.Application();

            // Create a new workbook
            Excel.Workbook workbook = excelApp.Workbooks.Add();

            // Get the first worksheet
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets.Add();
            worksheet.Name = "BackShipPriceReport";

            // Set the values in A1 and B1 cells
            worksheet.Cells[1, 1] = "Total Orders";
            worksheet.Cells[1, 2] = "Orders BackShip";
            worksheet.Cells[1, 3] = ordersTotal;
            worksheet.Cells[1, 4] = ordersBackShip;

            // Set the column headings in A2, B2, and C2 cells
            worksheet.Cells[2, 1] = "Country";
            worksheet.Cells[2, 2] = "Shipping Date";
            worksheet.Cells[2, 3] = "Costs";

            // Set the values for shipping countries, shipping dates, and back ship costs
            for (int i = 0; i < shippingCountries.Length; i++)
            {
                worksheet.Cells[i + 3, 1] = shippingCountries[i];
                worksheet.Cells[i + 3, 2] = shippingDates[i];
                worksheet.Cells[i + 3, 3] = backShipCosts[i];
            }

            // Auto-fit the columns for better readability
            worksheet.Columns.AutoFit();

            // Save the Excel file
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "BackShipPriceReport.xlsx");
            workbook.SaveAs(filePath);

            // Close the workbook and Excel application
            workbook.Close();
            excelApp.Quit();

            // Clean up
            ReleaseObject(worksheet);
            ReleaseObject(workbook);
            ReleaseObject(excelApp);
        }

        // Helper method to release COM objects
        private static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                Console.WriteLine("Exception occurred while releasing object: " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        public static void CollectValuesToArrays()
        {
            string url = "https://www.backmarket.fr/ws/orders?page=1&pageSize=100";
            string responseAPI = BackMarketAPIHandler.GetRequest(url);
            JObject json = JObject.Parse(responseAPI);
            int count = (int)json["count"];
            int pageSize = 50;
            int totalPages = CalculatePageCount(count, pageSize);

            for (int page = 1; page <= totalPages; page++)
            {
                if (page > 1)
                {
                    string nextPageUrl = $"https://www.backmarket.fr/ws/orders?page={page}&page-size={pageSize}";
                    responseAPI = BackMarketAPIHandler.GetRequest(nextPageUrl);
                }

                CollectValuesFromResponse(responseAPI, ref ordersTotal, ref ordersBackShip);
            }
        }
        public static void RunTroughPDFs()
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                string folderName = string.Empty;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    folderName = dialog.SelectedPath;
                    string[] filesInDir = Directory.GetFiles(folderName);
                    foreach (string file in filesInDir)
                    {
                        ExtractBackShipCostsFromPDF(file);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void ExtractBackShipCostsFromPDF(string path)
        {
            string destPath = @"C:\\Users\\lenna\\Desktop\\test.txt";
            File.WriteAllText(destPath, TextOperations.ExtractTextFromPdf(path));
            string searchValue = "DETAILS DES BACKSHIP";
            string lineCondition = "Express";
            string[] allLines = File.ReadAllLines(destPath);
            int begin = TextOperations.FindLine(allLines, searchValue) + 1;
            // found no costs
            if (begin == 1)
            {
                return;
            }
            var dbManager = new DBManager();
            for (int i = begin; i < allLines.Length; i++)
            {
                if (allLines[i].Contains(lineCondition))
                {
                    var length = allLines[i].Length;
                    int beginCosts = 63;
                    string costs = "";
                    // no order id in pdf?
                    try
                    {
                        costs = allLines[i].Substring(beginCosts, length - beginCosts - 2);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    costs = allLines[i].Substring(beginCosts, length - beginCosts - 2);
                    string orderId = allLines[i].Substring(11, 8);
                    // exclude the line with two entrie
                    if (costs.Length > 10)
                    {
                        continue;
                    }
                    string[] attributes = new string[] { "OrderId", "Costs" };
                    string[] values = new string[] { orderId, costs };
                    dbManager.CreateEntry("FoundOrderBackShipCosts", attributes, values);
                }
                else
                {
                    break;
                }
            }
        }

        public static void CollectValuesFromResponse(string responseAPI, ref int totalOrders, ref int backShipOrders)
        {
            JObject json = JObject.Parse(responseAPI);
            JArray results = (JArray)json["results"];
            var dbManager = new DBManager();
            foreach (JObject result in results)
            {
                totalOrders++;
                string orderId = (string)result["order_id"];
                string backShipValue = (string)result["is_backship"];
                if (backShipValue == "True")
                {
                    backShipOrders++;
                    int id = dbManager.ExecuteQueryWithResult("FoundOrderBackShipCosts", "Id", "OrderId", orderId);
                    // if there is no entry for this order number skip
                    if (id == 0)
                    {
                        continue;
                    }
                    string shippingCountry = (string)result["shipping_address"]["country"];
                    string orderDate = (string)result["date_creation"];
                    string costs = dbManager.ExecuteQueryWithResultString("FoundOrderBackShipCosts", "Costs", "Id", id.ToString());
                    //add to public arrays
                    string[] newArray = new string[] { shippingCountry };
                    string[] newArray2 = new string[] { orderDate };
                    string[] newArray3 = new string[] { costs };
                    shippingCountries = shippingCountries.Concat(newArray).ToArray();
                    shippingDates = shippingDates.Concat(newArray2).ToArray();
                    backShipCosts = backShipCosts.Concat(newArray3).ToArray();
                }
            }
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
    }
}
