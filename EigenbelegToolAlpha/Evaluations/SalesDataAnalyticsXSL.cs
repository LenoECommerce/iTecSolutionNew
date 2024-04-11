using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public class SalesDataAnalyticsXSL
    {
        private Microsoft.Office.Interop.Excel.Application xlApp;
        private Microsoft.Office.Interop.Excel.Workbook xlWorkbook;
        private Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
        private Microsoft.Office.Interop.Excel.Range xlRange;
        private Microsoft.Office.Interop.Excel.Range xlRangeColumns;


        public void CreateXSLFile(
            string[] orderId,
            string[] model,
            string[] purchaseGrade,
            string[] profit,
            string[] margin)
        {
            try
            {
                // Öffnen des OpenFileDialogs und Dateipfad speichern
                //OpenFileDialog openFileDialog = new OpenFileDialog();
                //openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                //openFileDialog.ShowDialog();
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
                string filePath = desktopPath + "Sales Data Analytics.xlsx";

                // Überprüfen, ob eine Datei ausgewählt wurde
                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("Keine Datei ausgewählt.");
                    return;
                }

                // Initialisierung von Excel
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(filePath);
                xlWorksheet = xlWorkbook.Sheets["raw data"];

                // Start ab der zweiten Zeile (Index 2) für das Einfügen
                int startRow = 2;

                // Daten einfügen
                InsertData(startRow, orderId, model, purchaseGrade, profit, margin);

                // Speichern und schließen der Arbeitsmappe
                xlWorkbook.Save();
                xlWorkbook.Close();

                // Aufräumen
                Marshal.ReleaseComObject(xlWorksheet);
                Marshal.ReleaseComObject(xlWorkbook);
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SalesAnalyticsXSL Fehler beim Bearbeiten der Excel-Datei: " + ex.Message);
            }
        }

        // Hilfsmethode zum Einfügen der Daten in die Excel-Datei
        private void InsertData(int startRow, string[] orderId, string[] model, string[] purchaseGrade, string[] profit, string[] margin)
        {
            int rowCount = orderId.Length;

            // Daten ab Spalte 1 (Index 1) einfügen
            for (int i = 0; i < rowCount; i++)
            {
                xlWorksheet.Cells[startRow + i, 1].Value = orderId[i]; // Bestellnummer
                xlWorksheet.Cells[startRow + i, 2].Value = model[i]; // Modell
                xlWorksheet.Cells[startRow + i, 3].Value = purchaseGrade[i]; // Ankauf Grade
                xlWorksheet.Cells[startRow + i, 4].Value = double.Parse(profit[i]); // Gewinn
                xlWorksheet.Cells[startRow + i, 5].Value = double.Parse(margin[i]); // Marge
            }
        }

    }
}
