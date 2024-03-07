using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using OfficeOpenXml;

namespace EigenbelegToolAlpha
{
    public class GoogleAPIHandler
    {
        private SheetsService sheetsService;
        private string ApplicationName = "DriveAccessLeno";
        private string[] Scopes = { DriveService.Scope.Drive };

        public GoogleAPIHandler()
        {
            this.sheetsService = InitializeSheetsService();
        }

        private SheetsService InitializeSheetsService()
        {
            try
            {
                // Get the user credentials
                UserCredential credential = GetCredentials();

                // Create the SheetsService using the user credentials
                return new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing SheetsService: {ex.Message}");
                return null;
            }
        }

        private UserCredential GetCredentials()
        {
            UserCredential credential;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\credentials.json";
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,  // Make sure you define Scopes in your class
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            return credential;
        }

        public void ReplaceGoogleSheetWithExcel(string spreadsheetName, string sheetName, string excelFilePath)
        {
            try
            {
                // Clear existing data in the sheet
                GoogleSheetsConfig config = new GoogleSheetsConfig();
                string spreadsheetId = config.GetSpreadsheetId(spreadsheetName);
                ClearSheet(spreadsheetId, sheetName);

                // Set the license context for EPPlus
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // Use NonCommercial license in this example

                // Read the Excel file using EPPlus
                using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Assuming your data is in the first sheet

                    // Get the data range from the Excel file
                    var startCell = worksheet.Cells["A1"];
                    var endCell = worksheet.Cells[worksheet.Dimension.End.Address];

                    // Read the data into a list of lists
                    var values = new List<IList<object>>();
                    for (int row = startCell.Start.Row; row <= endCell.Start.Row; row++)
                    {
                        var rowData = new List<object>();
                        for (int col = startCell.Start.Column; col <= endCell.Start.Column; col++)
                        {
                            rowData.Add(worksheet.Cells[row, col].Text);
                        }
                        values.Add(rowData);
                    }

                    // Create the Google Sheets API request for appending the new data with a limited range
                    SpreadsheetsResource.ValuesResource.AppendRequest request =
                        sheetsService.Spreadsheets.Values.Append(new ValueRange { Values = values }, spreadsheetId, sheetName + "!A1");

                    request.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.OVERWRITE;
                    request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;

                    // Execute the request
                    AppendValuesResponse response = request.Execute();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error replacing content in Google Sheets: {ex.Message}");
            }
        }

        private void ClearSheet(string spreadsheetId, string sheetName)
        {
            try
            {
                ClearValuesRequest clearRequest = new ClearValuesRequest();
                SpreadsheetsResource.ValuesResource.ClearRequest request =
                    sheetsService.Spreadsheets.Values.Clear(clearRequest, spreadsheetId, sheetName);

                request.Execute();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing sheet: {ex.Message}");
            }
        }
    }
}
