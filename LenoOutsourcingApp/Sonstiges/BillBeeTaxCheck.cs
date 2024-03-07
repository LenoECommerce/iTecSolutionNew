using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public class BillBeeTaxCheck
    {
        public static Microsoft.Office.Interop.Excel.Application xlApp;
        public static Microsoft.Office.Interop.Excel.Workbook xlWorkbook;
        public static Microsoft.Office.Interop.Excel.Workbook xlWorkbook2;
        public static Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
        public static Microsoft.Office.Interop.Excel.Range xlRange;
        public static Microsoft.Office.Interop.Excel.Range xlRangeSumUp;
        public static Microsoft.Office.Interop.Excel.Range xlRangeHeader;
        public static Microsoft.Office.Interop.Excel.Range xlRangeHeaderSumUp;
        public static double tresholdAmount = 0;
        public static int[] rowsOrdersAbove10k = new int[100];
        public static string[] rowsInvoiceID = new string[100];
        public static string[] rowsCountries = new string[100];
        public static string[] rowsAmount = new string[100];
        public static string fileName = "Tax Foreign Countries Report";
        public static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
        public static string fullPath = desktopPath + fileName;

        public static Dictionary<string, string> countryBIC = new Dictionary<string, string>
        {
            { "IS", "Island" },
            { "IT", "Italien" },
            { "AF", "Afghanistan" },
            { "AT", "Österreich" },
            { "BE", "Belgien" },
            { "DK", "Dänemark" },
            { "FI", "Finnland" },
            { "FR", "Frankreich" },
            { "GB", "Großbritannien" },
            { "IE", "Irland" },
            { "ES", "Spanien" },
            { "EE", "Estland" },
            { "LT", "Litauen" },
            { "LV", "Lettland" },
            { "PT", "Portugal" },
            { "MT", "Malta" },
            { "NL", "Niederlande" },
            { "SE", "Schweden" },
            { "SK", "Slowakische Republik" },
            { "SI", "Slowenien" },
        };
        public static Dictionary<string, double> taxRates = new Dictionary<string, double>
        {
            { "Portugal", 0.23 },
            { "Island", 0.24 },
            { "Afghanistan", 0 },
            { "Spanien", 0.21 },
            { "Italien", 0.22 },
            { "Österreich", 0.20 },
            { "Belgien", 0.21 },
            { "Dänemark", 0.25 },
            { "Finnland", 0.24 },
            { "Frankreich", 0.20 },
            { "Großbritannien", 0.20 },
            { "Irland", 0.23 },
            { "Estland", 0.20 },
            { "Litauen",0.21 },
            { "Lettland", 0.2 },
            { "Malta", 0.18 },
            { "Niederlande", 0.21 },
            { "Schweden", 0.25 },
            { "Slowakische Republik", 0.20 },
            { "Slowenien", 0.22 },
        };

        public static string[] countriesArray = new string[] { "Portugal", "Island", "Spanien", "Italien", "Österreich", "Belgien", "Dänemark", "Finnland", "Frankreich", "Großbritannien",
        "Irland", "Estland", "Litauen", "Lettland", "Malta", "Niederlande", "Schweden", "Slowakische Republik", "Slowenien"};
        public static void Analyse(string path)
        {
            int xlrow = 0;
            int arrayCounter = 0;
            try
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(path);
                xlWorksheet = xlWorkbook.Worksheets["Worksheet"];
                xlRange = xlWorksheet.UsedRange;
                for (xlrow = 2; xlrow <= xlRange.Rows.Count; xlrow++)
                {
                    double taxAmount = Convert.ToDouble(xlRange.Cells[xlrow, 17].Text);
                    string country = xlRange.Cells[xlrow, 11].Text;
                    string invoice = xlRange.Cells[xlrow, 12].Text;
                    if (taxAmount != 0 && country != "DE" && invoice != "")
                    {
                        tresholdAmount += Convert.ToDouble(xlRange.Cells[xlrow, 5].Text);
                        if (tresholdAmount >= 10000)
                        {
                            rowsOrdersAbove10k[arrayCounter] = xlrow;
                            rowsInvoiceID[arrayCounter] = xlRange.Cells[xlrow, 12].Text.Substring(0, 3) + xlRange.Cells[xlrow, 13].Text;
                            rowsCountries[arrayCounter] = xlRange.Cells[xlrow, 11].Text;
                            rowsAmount[arrayCounter] = xlRange.Cells[xlrow, 5].Text;
                            arrayCounter++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Es gab einen Fehler in Reihe " + xlrow + e.Message);
            }
            CreateExcelReport();
            AddWorkSheetSumUp();
            MessageBox.Show("Fertig erstellt.");
        }

        public static void CreateExcelReport()
        {
            try
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlApp.Visible = false;
                xlApp.DisplayAlerts = false;
                xlWorkbook = xlApp.Workbooks.Add(Type.Missing);
                xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.ActiveSheet; xlWorksheet.Name = "Einzelübersicht";
                xlWorksheet.Cells.Font.Size = 15;
                int counter = 2;
                int arraycounter = 0;
                foreach (var item in rowsOrdersAbove10k)
                {
                    if (item != 0)
                    {
                        try
                        {
                            xlWorksheet.Cells[counter, 1] = item.ToString();
                            xlWorksheet.Cells[counter, 2] = rowsInvoiceID[arraycounter];
                            xlWorksheet.Cells[counter, 3] = Convert.ToDouble(rowsAmount[arraycounter]);
                            string country = countryBIC[rowsCountries[arraycounter]];
                            xlWorksheet.Cells[counter, 4] = country;
                            double temp = taxRates[country] * 100;
                            xlWorksheet.Cells[counter, 5] = temp.ToString() + "%";
                            counter++;
                            arraycounter++;
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message + "Reihe " + item.ToString());
                        }

                    }
                }
                //header werte setzen
                xlWorksheet.Cells[1, 1] = "Excel Row";
                xlWorksheet.Cells[1, 2] = "Rechnungsnummer";
                xlWorksheet.Cells[1, 3] = "Kaufbetrag";
                xlWorksheet.Cells[1, 4] = "Lieferland";
                xlWorksheet.Cells[1, 5] = "Steuersatz";
                xlRangeHeader = xlWorksheet.Range[xlWorksheet.Cells[1, 1], xlWorksheet.Cells[1, 5]];
                xlRangeHeader.Font.Bold = true;
                //rest
                xlRange = xlWorksheet.Range[xlWorksheet.Cells[1, 1], xlWorksheet.Cells[counter, 4]];
                xlRange.EntireColumn.AutoFit();
                xlWorkbook.SaveAs(fullPath);
                xlWorkbook.Close();
                xlApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void AddWorkSheetSumUp()
        {
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.Visible = false;
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(fullPath);
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = xlWorkbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheetSumUp = xlWorkbook.Worksheets.Add();
            xlWorksheetSumUp.Name = "Sum Up";
            xlWorksheetSumUp.Cells.Font.Size = 15;
            xlWorksheetSumUp.Cells[1, 1] = "Land";
            xlWorksheetSumUp.Cells[1, 2] = "Gesamtbetrag";
            xlWorksheetSumUp.Cells[1, 3] = "Steuerbetrag";

            xlRangeHeaderSumUp = xlWorksheetSumUp.Range[xlWorksheetSumUp.Cells[1, 1], xlWorksheetSumUp.Cells[1, 3]];
            xlRangeHeaderSumUp.Font.Bold = true;
            int counter = 2;
            foreach (var item in countriesArray)
            {
                double sum = (double)xlApp.WorksheetFunction.SumIf(xlWorksheet.Range["D:D"], item.ToString(), xlWorksheet.Range["C:C"]);
                double taxMultiple = taxRates[item.ToString()];
                double taxRateSum = sum * taxMultiple;


                xlWorksheetSumUp.Cells[counter, 1] = item.ToString();
                xlWorksheetSumUp.Cells[counter, 2] = sum.ToString();
                xlWorksheetSumUp.Cells[counter, 3] = taxRateSum.ToString();
                counter++;
            }

            xlRangeSumUp = xlWorksheetSumUp.Range[xlWorksheetSumUp.Cells[1, 1], xlWorksheetSumUp.Cells[counter, 4]];
            xlRangeSumUp.EntireColumn.AutoFit();
            xlRangeSumUp.HorizontalAlignment = XlHAlign.xlHAlignRight;
            xlWorkbook.SaveAs(fullPath);
            xlWorkbook.Close();
            xlApp.Quit();
        }
    }
}
