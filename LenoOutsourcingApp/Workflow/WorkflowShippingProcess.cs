using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public class WorkflowShippingProcess
    {
        public string pathDeliveryNotes;
        public string pathShippingLabels;
        public static string today = DateTime.Today.ToShortDateString();
        public static string pathOutputDeliveryNotes = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Output Delivery Notes " + today + ".pdf";
        public static string pathOutputShippingLabels = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Output Shipping Labels " + today + ".pdf";

        public WorkflowShippingProcess()
        {
            var dbManager = new DBManager();
            pathDeliveryNotes = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathDirDeliveryNotes", "Nutzer", UserFileManagement.ReturnCurrentUser());
            pathShippingLabels = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathDirShippingLabels", "Nutzer", UserFileManagement.ReturnCurrentUser());

        }

        public void Main(UIWorkflowShippingProcess UI)
        {
            string[] ordersNeedToBeShipped = new string[0];
            int counter = 0;
            try
            {
                ordersNeedToBeShipped = GetAllOrdersNeedToBeShipped();
                DialogResult result = MessageBox.Show("Ist die Anzahl der in BillBee angezeigten 'versandfertigen Bestellungen' = " + ordersNeedToBeShipped.Length, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
                UI.UpdateLabel(UI.lbl_foundOrders, ordersNeedToBeShipped.Length.ToString());
                UI.UpdateLabelColor(UI.lbl_foundOrders, Color.Green);
                UI.UpdateProgressbar(UI.progressBar1, 20);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei Ordersuche: " + ex.Message);
                UI.UpdateLabel(UI.lbl_foundOrders, "Fehler");
                UI.UpdateLabelColor(UI.lbl_foundOrders, Color.Red);

            }

            foreach (var item in ordersNeedToBeShipped)
            {
                try
                {
                    DownloadDeliveryNotes(item, counter);
                    UI.UpdateLabel(UI.lbl_DeliveryNotes, (Convert.ToInt32(UI.lbl_DeliveryNotes.Text) + 1).ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Der Download des Lieferscheins für Bestellnummer " + item + " hat einen Fehler: " + ex.Message);
                    UI.UpdateLabelColor(UI.lbl_DeliveryNotes, Color.Red);
                }
                //CheckForAddressBugs(item);
                try
                {
                    DownloadShippingLabels(item, counter);
                    UI.UpdateLabel(UI.lbl_shippingLabels, (Convert.ToInt32(UI.lbl_shippingLabels.Text) + 1).ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Der Download des Etiketts für Bestellnummer " + item + " hat einen Fehler: " + ex.Message);
                    UI.UpdateLabelColor(UI.lbl_shippingLabels, Color.Red);
                }
                counter++;
            }
            UI.UpdateProgressbar(UI.progressBar1, 40);
            UI.UpdateProgressbar(UI.progressBar1, 60);
            UI.UpdateLabelColor(UI.lbl_DeliveryNotes, Color.Green);
            UI.UpdateLabelColor(UI.lbl_shippingLabels, Color.Green);
            try
            {
                MergeFilesAndSaveToDesktop(
                    directoryPath: pathDeliveryNotes,
                    outputPath: pathOutputDeliveryNotes);
                MergeFilesAndSaveToDesktop(
                    directoryPath: pathShippingLabels,
                    outputPath: pathOutputShippingLabels);
                UI.UpdateLabel(UI.lbl_merge, "Erfolgreich");
                UI.UpdateLabelColor(UI.lbl_merge, Color.Green);
                UI.UpdateProgressbar(UI.progressBar1, 80);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Der Datei Merge hat einen Fehler: " + ex.Message);
                UI.UpdateLabel(UI.lbl_merge, "Fehler");
                UI.UpdateLabelColor(UI.lbl_merge, Color.Red);
            }
            try
            {
                UploadFileToDrive(pathOutputShippingLabels);
                UploadFileToDrive(pathOutputDeliveryNotes);
                UI.UpdateLabel(UI.lbl_upload, "Erfolgreich");
                UI.UpdateLabelColor(UI.lbl_upload, Color.Green);
                UI.UpdateProgressbar(UI.progressBar1, 100);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Der Datei Upload hat einen Fehler: " + ex.Message);
                UI.UpdateLabel(UI.lbl_upload, "Fehler");
                UI.UpdateLabelColor(UI.lbl_upload, Color.Red);
            }
        }

        public static void CheckForAddressBugs(string orderID)
        {
            string addressStreetField = BillBeeAPIHandler.GetSingleLayerField(orderID, "ShippingAddress", "Street");
            if (IsNumberAtBegin(addressStreetField) == true)
            {
                // separate number and street
                addressStreetField = "Hanswurst 1";
                var posSpace = addressStreetField.IndexOf(" ");
                var fullLength = addressStreetField.Length;
                var streetNumber = addressStreetField.Substring(0, posSpace);
                var streetName = addressStreetField.Substring(posSpace + 1, fullLength - posSpace - 1);
                BillBeeAPIHandler.UpdateAddressOfSpecificOrder(orderID, streetNumber, streetName);
                MessageBox.Show("");
            }
        }

        public static bool IsNumberAtBegin(string inputValue)
        {
            var firstDigit = inputValue.Substring(0, 1);
            if (int.TryParse(firstDigit, out int intValue))
            {
                return true;
            }
            return false;
        }

        public static void MergeFilesAndSaveToDesktop(string directoryPath, string outputPath)
        {
            // Get the PDF files in the directory
            string[] filepaths = Directory.GetFiles(directoryPath, "*.pdf");
            // Create a document object and a PdfSmartCopy object to merge the PDF files
            Document document = new Document();
            PdfSmartCopy copy = new PdfSmartCopy(document, new FileStream(outputPath, FileMode.Create));
            // Open the document and loop through the PDF files
            document.Open();
            int pageNumber = 0;
            foreach (string filepath in filepaths)
            {
                // Add the PDF file to the merged document
                PdfReader reader = new PdfReader(filepath);
                int pages = reader.NumberOfPages;
                for (int i = 1; i <= pages; i++)
                {
                    pageNumber++;
                    PdfImportedPage page = copy.GetImportedPage(reader, i);
                    copy.AddPage(page);
                }
                reader.Close();

                // Delete the PDF file if desired
                File.Delete(filepath);
            }
            // Close the document
            document.Close();
        }

        public static void UploadFileToDrive(string path)
        {
            GoogleDrive drive = new GoogleDrive(path, "pdf");
            File.Delete(path);
        }
        public void DownloadDeliveryNotes(string orderId, int counter)
        {
            string path = BuildPath(pathDeliveryNotes, counter);
            if (GetMarketPlace(orderId) == "Ebay")
            {
                BillBeeAPIHandler.DownloadDeliveryNoteToPath(path, orderId);
            }
            else
            {
                BackMarketAPIHandler.DownloadDeliveryNoteToPath(path, orderId);
                AddBarcodeToPdf(orderId, path, counter);
            }
        }
        public void AddBarcodeToPdf(string orderID, string path, int counter)
        {
            string newPath = pathDeliveryNotes + @"\newPdF" + counter.ToString() + ".pdf";
            // Load the existing PDF file
            PdfReader reader = new PdfReader(path);
            PdfStamper stamper = new PdfStamper(reader, new FileStream(newPath, FileMode.Create));
            // Generate the barcode
            Barcode128 barcode = new Barcode128();
            barcode.Code = orderID;
            iTextSharp.text.Image img = barcode.CreateImageWithBarcode(stamper.GetOverContent(1), BaseColor.BLACK, BaseColor.BLACK);
            // Double the size of the barcode
            float originalWidth = img.ScaledWidth;
            float originalHeight = img.ScaledHeight;
            float newWidth = originalWidth * 2;
            float newHeight = originalHeight * 2;
            img.ScaleAbsolute(newWidth, newHeight);
            // Get the width and height of the page
            float pageWidth = reader.GetPageSize(1).Width;
            float pageHeight = reader.GetPageSize(1).Height;
            // Set the position of the barcode in the top-right corner of the page
            float barcodeWidth = img.ScaledWidth;
            float barcodeHeight = img.ScaledHeight;
            float x = pageWidth - barcodeWidth - 20; // 20 is the margin from the right edge of the page
            float y = pageHeight - barcodeHeight - 20; // 20 is the margin from the top edge of the page
            img.SetAbsolutePosition(x, y);
            // Add the barcode to the PDF document
            stamper.GetOverContent(1).AddImage(img);
            // Close the stamper and reader
            stamper.Close();
            reader.Close();
            File.Delete(path);
        }



        public void DownloadShippingLabels(string orderId, int counter)
        {
            string path = BuildPath(pathShippingLabels, counter);
            string shippingCountry = BillBeeAPIHandler.GetSingleLayerField(orderId, "ShippingAddress", "Country");
            string marketPlace = GetMarketPlace(orderId);
            // logic: if the order is not from Ebay and not a BackShip order the label is normally created in BillBee
            if (marketPlace == "BackMarket" && BackMarketAPIHandler.GetSpecificFieldOfOrder(orderId, "is_backship") == "True")
            {
                BackMarketAPIHandler.DownloadShippingLabel(path, orderId);
            }
            else
            {
                BillBeeAPIHandler.DownloadShippingLabelToPath(path, orderId, shippingCountry);
            }
        }
        public static string BuildPath(string path, int number)
        {
            string fileName = "test" + number.ToString() + ".pdf";
            return Path.Combine(path, fileName);
        }
        public static string[] GetAllOrdersNeedToBeShipped()
        {
            // logic from BillBee as "versandbereit" | state: paid, no shipping number, no storno invoice, no B2B channel
            string[] returnValuesOrderIds = new string[0];
            string responseString = BillBeeAPIHandler.GetAllOrdersByState("3");
            JObject json = JObject.Parse(responseString);
            JArray dataArray = (JArray)json["Data"];
            foreach (JObject result in dataArray)
            {
                if ((double)result["TotalCost"] < 0)
                {
                    continue;
                }
                if ((string)result["ApiAccountName"] == "B2B")
                {
                    continue;
                }
                if (string.IsNullOrEmpty((string)result["ShippedAt"]))
                {
                    string[] newArray = new string[] { (string)result["OrderNumber"] };
                    returnValuesOrderIds = returnValuesOrderIds.Concat(newArray).ToArray();
                }
            }
            return returnValuesOrderIds;
        }
        public static string GetMarketPlace(string orderId)
        {
            if (orderId.Contains("-"))
            {
                return "Ebay";
            }
            return "BackMarket";
        }
    }
}
