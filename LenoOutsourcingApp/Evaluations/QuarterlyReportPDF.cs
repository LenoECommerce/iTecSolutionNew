using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System;
using System.IO;
using System.Windows.Forms;
using PdfDocument = PdfSharp.Pdf.PdfDocument;
using PdfPage = PdfSharp.Pdf.PdfPage;

namespace EigenbelegToolAlpha.Evaluations
{
    public class QuarterlyReportPDF
    {
        public static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
        public static string fullPath = "";
        // calendarr logic
        public static string[] q1 = new string[] { "Januar", "Februar", "März" };
        public static string[] q2 = new string[] { "April", "Mai", "Juni" };
        public static string[] q3 = new string[] { "Juli", "August", "September" };
        public static string[] q4 = new string[] { "Oktober", "November", "Dezember" };
        // values which can be pulled from database
        public static double grossSalesEbay = 0;
        public static double grossSalesBackMarketNormal = 0;
        public static double grossSalesBackMarketPayPal = 0;
        public static double grossSalesSpareParts = 0;
        public static double totalReturnAmount = 0;
        public static double grossSalesB2B = 0;
        public static double grossSalesTotal = 0;
        public static double runningCosts = 0;
        public static double profitB2B = 0;
        public static double donorDevices = 0;
        public static double profit = 0;
        public static double profitAfterCosts = 0;
        public static int purchaseGoodsEbay = 0;
        public static int purchaseGoodsEbayKleinanzeigen = 0;
        public static int purchaseGoodsBackMarket = 0;
        public static int purchaseGoodsTotal = 0;
        public static int soldDevices = 0;
        // kpis getting calculated manually
        public static double margin = 0;
        public static double costsPerOrder = 0;
        public static double revenuePerDevice = 0;
        public static double profitInclusiveB2B = 0;

        public static void CreatePDFFile(string quarterNumber, string year)
        {
            try
            {
                fullPath = desktopPath + "Quarterly Report Q" + quarterNumber + " " + year + ".pdf";
                SetValuesFromDataBase(Convert.ToInt32(quarterNumber), year);
                CalculateRemainingKPIs();

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                //New document
                PdfDocument document = new PdfDocument();
                //New pages
                PdfPage page = document.AddPage();
                PdfPage page2 = document.AddPage();

                XGraphics gfx = XGraphics.FromPdfPage(page);
                XGraphics gfx2 = XGraphics.FromPdfPage(page2);
                XFont heading = new XFont("Arial", 20);
                XFont main = new XFont("Arial", 14);
                XFont subFont = new XFont("Arial", 11);
                XTextFormatter tf = new XTextFormatter(gfx);

                gfx.DrawString("Quarterly Report Q" + quarterNumber + " " + year, heading, XBrushes.Black, new XPoint(100, 100));
                //Vertikale Line
                gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(180, 120), new XPoint(180, 1000));
                //Anfang
                gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(0, 120), new XPoint(1000, 120));
                //Umsatzübersicht
                gfx.DrawString("Umsatzübersicht", main, XBrushes.Black, new XPoint(25, 140));
                gfx.DrawString("Ebay", main, XBrushes.Black, new XPoint(200, 170));
                gfx.DrawString("BM Normal", main, XBrushes.Black, new XPoint(200, 200));
                gfx.DrawString("BM PayPal", main, XBrushes.Black, new XPoint(200, 230));
                gfx.DrawString("Ersatzteile", main, XBrushes.Black, new XPoint(200, 260));
                gfx.DrawString("Erstattungen", main, XBrushes.Black, new XPoint(200, 290));
                gfx.DrawString(grossSalesEbay.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 170));
                gfx.DrawString(grossSalesBackMarketNormal.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 200));
                gfx.DrawString(grossSalesBackMarketPayPal.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 230));
                gfx.DrawString(grossSalesSpareParts.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 260));
                gfx.DrawString(totalReturnAmount.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 290));
                gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(0, 310), new XPoint(1000, 310));
                //Sonstiges
                gfx.DrawString("Sonstiges", main, XBrushes.Black, new XPoint(25, 330));
                gfx.DrawString("Laufende Kosten", main, XBrushes.Black, new XPoint(200, 360));
                gfx.DrawString(runningCosts.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 360));
                gfx.DrawString("B2B Umsatz", main, XBrushes.Black, new XPoint(200, 390));
                gfx.DrawString(grossSalesB2B.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 390));
                gfx.DrawString("B2B Gewinn", main, XBrushes.Black, new XPoint(200, 420));
                gfx.DrawString(profitB2B.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 420));
                gfx.DrawString("Spender", main, XBrushes.Black, new XPoint(200, 450));
                gfx.DrawString(donorDevices.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 450));
                gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(0, 500), new XPoint(1000, 500));
                //KPIs
                gfx.DrawString("KPIs (B2C)", main, XBrushes.Black, new XPoint(25, 520));
                gfx.DrawString("Umsatz", main, XBrushes.Black, new XPoint(200, 550));
                gfx.DrawString(grossSalesTotal.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 550));
                gfx.DrawString("Gewinn", main, XBrushes.Black, new XPoint(200, 580));
                gfx.DrawString(profit.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 580));
                gfx.DrawString("Gewinn n. K.", main, XBrushes.Black, new XPoint(200, 610));
                gfx.DrawString(profitAfterCosts.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 610));
                gfx.DrawString("Marge n. K.", main, XBrushes.Black, new XPoint(200, 640));
                gfx.DrawString(margin.ToString() + "%", subFont, XBrushes.Black, new XPoint(400, 640));
                gfx.DrawString("Quelle: Ebay", main, XBrushes.Black, new XPoint(200, 670));
                gfx.DrawString(purchaseGoodsEbay.ToString() + " Geräte", subFont, XBrushes.Black, new XPoint(400, 670));
                gfx.DrawString("Quelle: Ebay Kleinanzeigen", main, XBrushes.Black, new XPoint(200, 700));
                gfx.DrawString(purchaseGoodsEbayKleinanzeigen.ToString() + " Geräte", subFont, XBrushes.Black, new XPoint(400, 700));
                gfx.DrawString("Quelle: BackMarket", main, XBrushes.Black, new XPoint(200, 730));
                gfx.DrawString(purchaseGoodsBackMarket.ToString() + " Geräte", subFont, XBrushes.Black, new XPoint(400, 730));
                gfx.DrawString("Durchfluss", main, XBrushes.Black, new XPoint(200, 760));
                gfx.DrawString(purchaseGoodsTotal.ToString() + " Geräte", subFont, XBrushes.Black, new XPoint(400, 760));
                gfx.DrawString("Geräte verkauft", main, XBrushes.Black, new XPoint(200, 790));
                gfx.DrawString(soldDevices.ToString() + " Geräte", subFont, XBrushes.Black, new XPoint(400, 790));
                gfx.DrawString("Kosten / Auftrag (Misch)", main, XBrushes.Black, new XPoint(200, 820));
                gfx.DrawString(costsPerOrder.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 820));
                //2nd page
                gfx2.DrawString("Revenue / Device", main, XBrushes.Black, new XPoint(200, 100));
                gfx2.DrawString(revenuePerDevice.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 100));
                gfx2.DrawString("Gewinn + B2B", main, XBrushes.Black, new XPoint(200, 130));
                gfx2.DrawString(profitInclusiveB2B.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 130));
                //Leno Logo
                string imagePath = Path.GetTempPath();
                imagePath = imagePath + "lenologo.jpg";
                if (File.Exists(imagePath) == false)
                {
                }
                DrawImage(gfx, imagePath, 400, 10, 175, 100);
                //Verhältnis 1,75; passt Größe?
                document.Save(fullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Die PDF Erstellung hat einen Fehler: " + ex.Message);
            }

        }
        public static void UploadPDF(string quarterNumber, string year)
        {
            var dbManager = new DBManager();
            int returnValue = Convert.ToInt32(dbManager.ExecuteQueryWithResultStringTwoConditions("EvaluationsQuarterlyReports", "Id", "Quarter", "Year", quarterNumber, year));
            GoogleDrive drive = new GoogleDrive(fullPath, "pdf");
            if (returnValue == 0)
            {
                string query = string.Format("INSERT INTO `EvaluationsQuarterlyReports` (`Quarter`,`Year`,`Link`) VALUES ('{0}','{1}','{2}')"
                                , quarterNumber, year, GoogleDrive.currentLink);
                dbManager.ExecuteQuery(query);
                MessageBox.Show("Das Quartal Q" + quarterNumber + " " + year + " wurde erstmalig angelegt.");
            }
            else
            {
                string query = "UPDATE `EvaluationsQuarterlyReports` SET `Link` = '" + GoogleDrive.currentLink + "' WHERE `Id` = " + returnValue;
                dbManager.ExecuteQuery(query);
                MessageBox.Show("Das Quartal Q" + quarterNumber + " " + year + " wurde überschrieben.");
            }
        }
        public static void CalculateRemainingKPIs()
        {
            margin = RoundOneDigit(profitAfterCosts / grossSalesTotal * 100);
            costsPerOrder = RoundOneDigit(runningCosts / soldDevices);
            revenuePerDevice = RoundOneDigit(profitAfterCosts / soldDevices);
            profitInclusiveB2B = profitAfterCosts + profitB2B;
        }

        public static void SetValuesFromDataBase(int quarter, string year)
        {
            string[] selectedArray;
            switch (quarter)
            {
                case 1:
                    selectedArray = q1;
                    break;
                case 2:
                    selectedArray = q2;
                    break;
                case 3:
                    selectedArray = q3;
                    break;
                case 4:
                    selectedArray = q4;
                    break;
                default:
                    Console.WriteLine("Invalid quarter number.");
                    return;
            }

            var dbManager = new DBManager();
            foreach (var item in selectedArray)
            {
                string table = "Evaluations";
                grossSalesEbay += Convert.ToDouble(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "GrossSalesEbay", "Monat", "Jahr", item.ToString(), year));
                grossSalesBackMarketNormal += Convert.ToDouble(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "GrossSalesBackMarketNormal", "Monat", "Jahr", item.ToString(), year));
                grossSalesBackMarketPayPal += Convert.ToDouble(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "GrossSalesBackMarketPayPal", "Monat", "Jahr", item.ToString(), year));
                grossSalesSpareParts += Convert.ToDouble(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "GrossSalesSpareParts", "Monat", "Jahr", item.ToString(), year));
                grossSalesB2B += Convert.ToDouble(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "GrossSalesB2B", "Monat", "Jahr", item.ToString(), year));
                grossSalesTotal += Convert.ToDouble(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "GrossSalesTotal", "Monat", "Jahr", item.ToString(), year));
                totalReturnAmount += Convert.ToDouble(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "TotalReturnAmount", "Monat", "Jahr", item.ToString(), year));
                runningCosts += Convert.ToDouble(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "RunningCosts", "Monat", "Jahr", item.ToString(), year));
                profitB2B += Convert.ToDouble(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "B2BRevenue", "Monat", "Jahr", item.ToString(), year));
                profit += Convert.ToDouble(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "Profit", "Monat", "Jahr", item.ToString(), year));
                profitAfterCosts += Convert.ToDouble(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "ProfitAfterCosts", "Monat", "Jahr", item.ToString(), year));
                donorDevices += Convert.ToDouble(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "DonorDevices", "Monat", "Jahr", item.ToString(), year));
                purchaseGoodsEbay += Convert.ToInt32(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "PurchaseGoodsEbay", "Monat", "Jahr", item.ToString(), year));
                purchaseGoodsEbayKleinanzeigen += Convert.ToInt32(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "PurchaseGoodsEbayKleinanzeigen", "Monat", "Jahr", item.ToString(), year));
                purchaseGoodsBackMarket += Convert.ToInt32(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "PurchaseGoodsBackMarket", "Monat", "Jahr", item.ToString(), year));
                purchaseGoodsTotal += Convert.ToInt32(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "PurchaseGoodsTotal", "Monat", "Jahr", item.ToString(), year));
                soldDevices += Convert.ToInt32(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "SoldDevices", "Monat", "Jahr", item.ToString(), year));
            }
        }
        public static void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            if (File.Exists(jpegSamplePath) == false)
            {
                MessageBox.Show("Für den Pfad " + jpegSamplePath + " wurde keine Datei gefunden.");
                return;
            }
            XImage image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }

        public static double RoundOneDigit(double adaptValue)
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
    }
}
