using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.CompilerServices;
using ExcelLibrary.SpreadSheet;
using System.Windows.Forms;
using System.Data;

namespace EigenbelegToolAlpha
{
    public class ExcelManager
    {
        private  Microsoft.Office.Interop.Excel.Application xlApp;
        private  Microsoft.Office.Interop.Excel.Workbook xlWorkbook;
        private  Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
        private  Microsoft.Office.Interop.Excel.Range xlRange;
        private  Microsoft.Office.Interop.Excel.Range xlRangeColumns;
        
        public ExcelManager()
        {
            // Initialize Excel application instance
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.Visible = false;
            xlApp.DisplayAlerts = false;
        }

        #region commission refund
        public void CreateNewExcelFileCommissionRefund(
            string fileName,
            string[] columns,
            int rowsQuantity,
            string[] orderIds,
            string[] comments,
            string[] proofs)
        {
            try
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                MessageBox.Show("test");
                xlApp.Visible = false;
                xlApp.DisplayAlerts = false;
                xlWorkbook = xlApp.Workbooks.Add(Type.Missing);
                xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.ActiveSheet; xlWorksheet.Name = "Main";
                xlWorksheet.Range[xlWorksheet.Cells[1, 1], xlWorksheet.Cells[100, 100]]();
                xlWorksheet.Cells[1, 1] = fileName;
                xlWorksheet.Cells.Font.Size = 15;

                int counter = 1;
                foreach (var item in columns)
                {
                    xlWorksheet.Cells[2, counter] = item.ToString();
                    counter++;
                }

                int rowInsertCounter = 3;
                int arrayCounter = 0;
                foreach (var item in orderIds)
                {
                    if (item != null)
                    {
                        xlWorksheet.Cells[rowInsertCounter, 1] = orderIds[arrayCounter];
                        xlWorksheet.Cells[rowInsertCounter, 2] = comments[arrayCounter];
                        xlWorksheet.Cells[rowInsertCounter, 3] = proofs[arrayCounter];
                        arrayCounter++;
                        rowInsertCounter++;
                    }
                }
                xlRange = xlWorksheet.Range[xlWorksheet.Cells[1, 1], xlWorksheet.Cells[100, 100]];
                xlRangeColumns = xlWorksheet.Range[xlWorksheet.Cells[2, 1], xlWorksheet.Cells[2, columns.Count()]];
                xlRangeColumns.Font.Bold = true;
                xlRange.EntireColumn.AutoFit();
                xlRange.HorizontalAlignment = XlHAlign.xlHAlignRight;
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
                string fullPath = desktopPath + fileName;
                xlWorkbook.SaveAs(fullPath);
                xlWorkbook.Close();
                xlApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        public void CreateExcelGoogleSheetPrezi(
            string[] headings,
            string[] dataAttributes,
            int[] dataIds,
            string fileName,
            string table)
        {

            CreateNewExcelFile(
                fileName
                , headings, 
                out xlWorkbook,
                false);
            DBManager db = new DBManager();
            System.Data.DataTable dataFromDatabase = db.GetDataFromDatabaseByIdsFilteredByAttributes(
                dataIds,
                table,
                dataAttributes);

            // Convert DataTable to a 2D array
            object[,] data = new object[dataFromDatabase.Rows.Count, dataFromDatabase.Columns.Count];
            for (int i = 0; i < dataFromDatabase.Rows.Count; i++)
            {
                for (int j = 0; j < dataFromDatabase.Columns.Count; j++)
                {
                    data[i, j] = dataFromDatabase.Rows[i][j];
                }
            }

            // Insert the data into the existing Excel file
            InsertDataIntoExcel(xlWorkbook, data);

            // Close and quit Excel
            xlWorkbook.Close();
            xlApp.Quit();
        }

        public void DefectiveMotherboardExcelFile(int[] dataIds)
        {
            // otis ist cool
            try
            {
                // Basic Excel file setup using CreateNewExcelFile method
                string[] headings = new string[] {
                "Id",
                "Internal",
                "FMI",
                "Defect",
                "Model",
                "Storage"
            };

                CreateNewExcelFile($"Motherboard Offer {DateTime.Now.ToShortDateString()}", headings, out xlWorkbook);
                DBManager db = new DBManager();
                System.Data.DataTable dataFromDatabase = db.GetDataFromDatabaseByIds(dataIds, "DefectiveMotherboards");

                // Convert DataTable to a 2D array
                object[,] data = new object[dataFromDatabase.Rows.Count, dataFromDatabase.Columns.Count];
                for (int i = 0; i < dataFromDatabase.Rows.Count; i++)
                {
                    for (int j = 0; j < dataFromDatabase.Columns.Count; j++)
                    {
                        data[i, j] = dataFromDatabase.Rows[i][j];
                    }
                }

                // Insert the data into the existing Excel file
                InsertDataIntoExcel(xlWorkbook, data);

                // Close and quit Excel
                xlWorkbook.Close();
                xlApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Ensure that Excel objects are properly released
                ReleaseObject(xlWorkbook);
                ReleaseObject(xlApp);
                MessageBox.Show("Datei erfolgreich erzeugt.");
            }
        }

        
        
        private void InsertDataIntoExcel(Microsoft.Office.Interop.Excel.Workbook xlWorkbook, object[,] data)
        {
            try
            {
                // Get the active Excel worksheet
                Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.ActiveSheet;

                // Find the last used row in the worksheet
                int lastUsedRow = xlWorksheet.Cells.Find("*", System.Reflection.Missing.Value,
                                    System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                    Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows,
                                    Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                                    false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

                // Determine the starting row for data insertion
                int startRow = lastUsedRow + 1;

                // Insert data into the worksheet
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    for (int j = 0; j < data.GetLength(1); j++)
                    {
                        xlWorksheet.Cells[startRow + i, j + 1] = data[i, j];
                    }
                }

                // Auto-fit columns and save the workbook
                xlWorksheet.UsedRange.Columns.AutoFit();
                xlWorkbook.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        public void CreateNewExcelFile(
            string fileName, 
            string[] columns, 
            out Microsoft.Office.Interop.Excel.Workbook xlWorkbook,
            bool insertTitle = true)
        {
            xlWorkbook = null;

            try
            {
                xlWorkbook = xlApp.Workbooks.Add(Type.Missing);
                xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Sheets[1];
                xlWorksheet.Name = "Main";

                if (insertTitle)
                {
                    xlWorksheet.Range[xlWorksheet.Cells[1, 1], xlWorksheet.Cells[1, columns.Length]].Merge();
                    xlWorksheet.Cells[1, 1] = fileName;
                    xlWorksheet.Cells.Font.Size = 15;
                }

                int counter = 1;
                foreach (var item in columns)
                {
                    xlWorksheet.Cells[insertTitle ? 2 : 1, counter] = item;
                    counter++;
                }

                xlWorksheet.Range[xlWorksheet.Cells[insertTitle ? 2 : 1, 1], xlWorksheet.Cells[insertTitle ? 2 : 1, columns.Length]].Font.Bold = true;

                xlWorksheet.UsedRange.Columns.AutoFit();
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
                string fullPath = desktopPath + fileName + ".xlsx";
                xlWorkbook.SaveAs(fullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ReleaseObject(xlWorkbook);
                ReleaseObject(xlApp);
            }
        }
    }
}
