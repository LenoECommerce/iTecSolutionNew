using EigenbelegToolAlpha.Evaluations;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf.IO;
using System;
using System.IO;
using System.Windows.Forms;
using PdfDocument = PdfSharp.Pdf.PdfDocument;
using PdfPage = PdfSharp.Pdf.PdfPage;
using PdfReader = PdfSharp.Pdf.IO.PdfReader;

namespace EigenbelegToolAlpha
{
    public class MonthlyReportPDF
    {
        public static string month = EvaluationsFirstPage.month;
        public static string year = EvaluationsFirstPage.year;
        public static string filename = "test2";
        public static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
        public static string fullPath = desktopPath + filename + ".pdf";
        public static string monthlyReportFinishedPath = desktopPath + "Monthly Report" + month + ".pdf";
        //some value calcs
        public static double returnAmount = 0;
        public static double runningCostsTotalNet = 0;
        public static double grossSalesTotalB2C = 0;
        public static double grossSalesTotalSpareParts = 0;
        public static double costsAtAll = 0;
        public static double revenueAfterCosts = 0;
        public static double revenueWithB2BRevenue = 0;
        public static double revenuePerDevice = 0;
        public static double margin = 0;
        public static double costsPerOrder = 0;
        public static void CreatePDFFile()
        {
            //necessary because otherwise the code would not be reached
            SumUpCalculations();
            SaveValuesInDatabase();
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

            gfx.DrawString("Monthly Report " + month + " " + year, heading, XBrushes.Black, new XPoint(100, 100));
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
            gfx.DrawString(EvaluationsSingleOrderCalculations.grossSalesEbay.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 170));
            gfx.DrawString(EvaluationsSingleOrderCalculations.grossSalesBackmarketNormal.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 200));
            gfx.DrawString(EvaluationsSingleOrderCalculations.grossSalesBackmarketPayPal.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 230));
            gfx.DrawString(grossSalesTotalSpareParts.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 260));
            gfx.DrawString(returnAmount + "€", subFont, XBrushes.Black, new XPoint(400, 290));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(0, 310), new XPoint(1000, 310));
            //Sonstiges
            gfx.DrawString("Sonstiges", main, XBrushes.Black, new XPoint(25, 330));
            gfx.DrawString("Laufende Kosten", main, XBrushes.Black, new XPoint(200, 360));
            gfx.DrawString(runningCostsTotalNet.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 360));
            gfx.DrawString("B2B Umsatz", main, XBrushes.Black, new XPoint(200, 390));
            gfx.DrawString(EvaluationSecondForm.B2BGrossSalesTotal.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 390));
            gfx.DrawString("B2B Gewinn", main, XBrushes.Black, new XPoint(200, 420));
            gfx.DrawString(RoundOneDigit(EvaluationSecondForm.B2BRevenue).ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 420));
            gfx.DrawString("Externer Aufwand", main, XBrushes.Black, new XPoint(200, 450));
            gfx.DrawString(EvaluationSecondForm.moreExternalCosts.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 450));
            gfx.DrawString("Spender", main, XBrushes.Black, new XPoint(200, 480));
            gfx.DrawString(EvaluationCalculation.donorDevicesAmount.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 480));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(0, 500), new XPoint(1000, 500));
            //KPIs
            gfx.DrawString("KPIs (B2C)", main, XBrushes.Black, new XPoint(25, 520));
            gfx.DrawString("Umsatz", main, XBrushes.Black, new XPoint(200, 550));
            gfx.DrawString(grossSalesTotalB2C.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 550));
            gfx.DrawString("Gewinn", main, XBrushes.Black, new XPoint(200, 580));
            gfx.DrawString(EvaluationsSingleOrderCalculations.revenueTotal.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 580));
            gfx.DrawString("Gewinn n. K.", main, XBrushes.Black, new XPoint(200, 610));
            gfx.DrawString(revenueAfterCosts + "€", subFont, XBrushes.Black, new XPoint(400, 610));
            gfx.DrawString("Marge n. K.", main, XBrushes.Black, new XPoint(200, 640));
            gfx.DrawString(margin.ToString() + "%", subFont, XBrushes.Black, new XPoint(400, 640));
            gfx.DrawString("Quelle: Ebay", main, XBrushes.Black, new XPoint(200, 670));
            gfx.DrawString(EvaluationCalculation.kpiSourceCounterEbay.ToString() + " Geräte", subFont, XBrushes.Black, new XPoint(400, 670));
            gfx.DrawString("Quelle: Ebay Kleinanzeigen", main, XBrushes.Black, new XPoint(200, 700));
            gfx.DrawString(EvaluationCalculation.kpiSourceCounterEbayKleinanzeigen.ToString() + " Geräte", subFont, XBrushes.Black, new XPoint(400, 700));
            gfx.DrawString("Quelle: BackMarket", main, XBrushes.Black, new XPoint(200, 730));
            gfx.DrawString(EvaluationCalculation.kpiSourceCounterBackMarket.ToString() + " Geräte", subFont, XBrushes.Black, new XPoint(400, 730));
            gfx.DrawString("Durchfluss", main, XBrushes.Black, new XPoint(200, 760));
            gfx.DrawString(EvaluationCalculation.kpiDevicesPerMonth.ToString() + " Geräte", subFont, XBrushes.Black, new XPoint(400, 760));
            gfx.DrawString("Geräte verkauft", main, XBrushes.Black, new XPoint(200, 790));
            gfx.DrawString((EvaluationCalculation.kpiDevicesPerMonthSold - EvaluationsSingleOrderCalculations.counterDisplayOrders).ToString() + " Geräte", subFont, XBrushes.Black, new XPoint(400, 790));
            gfx.DrawString("Kosten / Auftrag (Misch)", main, XBrushes.Black, new XPoint(200, 820));
            gfx.DrawString(costsPerOrder.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 820));
            //2nd page
            gfx2.DrawString("Revenue / Device", main, XBrushes.Black, new XPoint(200, 100));
            gfx2.DrawString(revenuePerDevice.ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 100));
            gfx2.DrawString("Gewinn + B2B", main, XBrushes.Black, new XPoint(200, 130));
            gfx2.DrawString(RoundOneDigit(revenueWithB2BRevenue).ToString() + "€", subFont, XBrushes.Black, new XPoint(400, 130));
            //Leno Logo
            string imagePath = Path.GetTempPath();
            imagePath = imagePath + "lenologo.jpg";
            if (File.Exists(imagePath) == false)
            {
            }
            DrawImage(gfx, imagePath, 400, 10, 175, 100);
            //Verhältnis 1,75; passt Größe?
            document.Save(fullPath);
            MergeFiles();
            EvaluationsTestClassPDF.PDFIntermediateValues();
        }
        public static void SumUpCalculations()
        {
            grossSalesTotalSpareParts = EvaluationsSingleOrderCalculations.grossSalesDisplays + EvaluationSecondForm.B2BGrossSalesOnlySpareParts;
            runningCostsTotalNet = RoundOneDigit(EvaluationThirdForm.RunningCostsFinal + EvaluationSecondForm.rateConsumptionTotal);
            grossSalesTotalB2C = EvaluationsSingleOrderCalculations.grossSalesEbay + EvaluationsSingleOrderCalculations.grossSalesBackmarketPayPal + EvaluationsSingleOrderCalculations.grossSalesBackmarketNormal
                                 + EvaluationsSingleOrderCalculations.grossSalesExternalSells + EvaluationsSingleOrderCalculations.grossSalesDisplays;
            costsAtAll = runningCostsTotalNet + EvaluationSecondForm.moreExternalCosts + EvaluationCalculation.donorDevicesAmount;
            revenueAfterCosts = RoundOneDigit(EvaluationsSingleOrderCalculations.revenueTotal - costsAtAll);
            revenueWithB2BRevenue = revenueAfterCosts + EvaluationSecondForm.B2BRevenue;
            revenuePerDevice = RoundOneDigit(revenueAfterCosts / EvaluationCalculation.kpiDevicesPerMonthSold);
            // logic explanation: added B2B gross sales liquidation to the margin because it has a relevant impact on B2C margin
            margin = RoundOneDigit((revenueAfterCosts + EvaluationSecondForm.B2BGrossSalesOnlySpareParts) / (grossSalesTotalB2C + EvaluationSecondForm.B2BGrossSalesOnlySpareParts) * 100);
            costsPerOrder = RoundOneDigit(runningCostsTotalNet / EvaluationCalculation.kpiDevicesPerMonthSold);
            // collect return amounts
            EvaluationsBackMarketPDF evaluationsBackMarketPDF = new EvaluationsBackMarketPDF();
            EvaluationsEbayPDF evaluationsEbay = new EvaluationsEbayPDF();
            returnAmount = evaluationsBackMarketPDF.CollectReturnAmount();
            returnAmount += evaluationsEbay.CollectReturnAmount();
        }
        public static void SaveValuesInDatabase()
        {
            string[] types = new string[] { "GrossSalesEbay","GrossSalesBackMarketNormal","GrossSalesBackMarketPayPal","GrossSalesSpareParts", "TotalReturnAmount", "RunningCosts",
                                            "GrossSalesB2B","B2BRevenue","DonorDevices","GrossSalesTotal","Profit","ProfitAfterCosts", "PurchaseGoodsEbay","PurchaseGoodsEbayKleinanzeigen",
                                            "PurchaseGoodsBackMarket", "PurchaseGoodsTotal", "SoldDevices" };
            object[] values = new object[] { EvaluationsSingleOrderCalculations.grossSalesEbay.ToString(), EvaluationsSingleOrderCalculations.grossSalesBackmarketNormal.ToString(), EvaluationsSingleOrderCalculations.grossSalesBackmarketPayPal.ToString(),
                                             grossSalesTotalSpareParts.ToString(), returnAmount, runningCostsTotalNet.ToString(), EvaluationSecondForm.B2BGrossSalesTotal.ToString(),
                                             RoundOneDigit(EvaluationSecondForm.B2BRevenue).ToString(), EvaluationCalculation.donorDevicesAmount.ToString(), grossSalesTotalB2C.ToString(), revenueAfterCosts.ToString(),
                                             revenueAfterCosts, EvaluationCalculation.kpiSourceCounterEbay.ToString(), EvaluationCalculation.kpiSourceCounterEbayKleinanzeigen.ToString(), EvaluationCalculation.kpiSourceCounterBackMarket.ToString(),
                                             EvaluationCalculation.kpiDevicesPerMonth.ToString(), (EvaluationCalculation.kpiDevicesPerMonthSold - EvaluationsSingleOrderCalculations.counterDisplayOrders).ToString() };

            var dbManager = new DBManager();
            for (int i = 0; i < types.Length; i++)
            {
                dbManager.ExecuteQuery("UPDATE `Evaluations` SET `" + types[i] + "` = '" + values[i].ToString() + "' WHERE `Jahr` = '" + year + "' AND `Monat` = '" + month + "'");
            }
        }
        public static void MergeFiles()
        {
            PdfDocument document = new PdfDocument();
            string[] files = new string[2] { OrderRelationPDF.fullPath, fullPath };
            foreach (string pdfFile in files)
            {
                PdfDocument inputPDFDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);
                document.Version = inputPDFDocument.Version;
                //actual pdf merge
                foreach (PdfPage page in inputPDFDocument.Pages)
                {
                    document.AddPage(page);
                }
                // When document is add in pdf document remove file from folder  
                System.IO.File.Delete(pdfFile);
            }
            // Set font for paging  
            XFont font = new XFont("Verdana", 9);
            XBrush brush = XBrushes.Black;
            // Create variable that store page count  
            string noPages = document.Pages.Count.ToString();
            // Set for loop of document page count and set page number using DrawString function of PdfSharp  
            for (int i = 0; i < document.Pages.Count; ++i)
            {
                PdfPage page = document.Pages[i];
                // Make a layout rectangle.  
                XRect layoutRectangle = new XRect(240 /*X*/ , page.Height - font.Height - 10 /*Y*/ , page.Width /*Width*/ , font.Height /*Height*/ );
                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    gfx.DrawString("Page " + (i + 1).ToString() + " of " + noPages, font, brush, layoutRectangle, XStringFormats.Center);
                }
            }
            document.Options.CompressContentStreams = true;
            document.Options.NoCompression = false;
            // In the final stage, all documents are merged and save in your output file path.  
            document.Save(monthlyReportFinishedPath);
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
