using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfDocument = PdfSharp.Pdf.PdfDocument;
using PdfPage = PdfSharp.Pdf.PdfPage;
using ExcelLibrary.SpreadSheet;

namespace EigenbelegToolAlpha.Evaluations
{
    public class EvaluationsTestClassPDF
    {
        public static string fullPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TestPDFIntermediateValues.pdf";
        public static string fullPath2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TestPDFBreakDown.pdf";
        public static void PDFIntermediateValues()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //New document
            PdfDocument document = new PdfDocument();
            //New pages
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont heading = new XFont("Arial", 20);
            XFont main = new XFont("Arial", 14);
            XFont subFont = new XFont("Arial", 11);
            XTextFormatter tf = new XTextFormatter(gfx);

            gfx.DrawString("Ebay Vorsteuer", main, XBrushes.Black, new XPoint(200, 170));
            gfx.DrawString("B2B Ersatzteile", main, XBrushes.Black, new XPoint(200, 200));
            gfx.DrawString("Verbrauch Kosten", main, XBrushes.Black, new XPoint(200, 230));
            gfx.DrawString("Weitere Externe Kosten", main, XBrushes.Black, new XPoint(200, 260));
            gfx.DrawString(EvaluationsFirstPage.ebayTaxGetBack.ToString(), subFont, XBrushes.Black, new XPoint(400, 170));
            gfx.DrawString(EvaluationSecondForm.B2BGrossSalesOnlySpareParts.ToString(), subFont, XBrushes.Black, new XPoint(400, 200));
            gfx.DrawString(EvaluationSecondForm.rateConsumptionTotal.ToString(), subFont, XBrushes.Black, new XPoint(400, 230));
            gfx.DrawString(EvaluationSecondForm.moreExternalCosts.ToString(), subFont, XBrushes.Black, new XPoint(400, 260));

            document.Save(fullPath);
        }

        public static void PDFBreakDownValues(string[] orderIds, string[] internalNumbers, string[] amounts, string[] externalCostsArray,
                         string[] externalCostsDiffArray, string[] taxesTypes, string[] taxesArray,
                         string[] marketplaceFeesArray, string[] profits, string[] margins, string[] paymentFeesArray, string[] backShipCostsArray,
                         string[] sellerCommissionsArray, string ccbmFee)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //New document
            PdfDocument document = new PdfDocument();
            //New pages
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont heading = new XFont("Arial", 20);
            XFont main = new XFont("Arial", 14);
            XFont subFont = new XFont("Arial", 11);
            XTextFormatter tf = new XTextFormatter(gfx);
            page.Orientation = PageOrientation.Landscape;
            string[] headings = {"Order ID", "Internal", "Amount", "Co No", "Co. Ma.", "Tax Type", "Taxes", "Seller %",
                     "Payment", "BackShip", "CCBM", "Total", "Profit", "Margin" };

            double headlineX = 50;
            double headlineY = 50;
            double contentX = 50;
            double contentY = headlineY + 20;
            double orderIdColumnWidth = (page.Width - 2 * headlineX) * 2 / (3 * headings.Length - 1); // Double the size
            double otherColumnWidth = (page.Width - 2 * headlineX) / (3 * headings.Length - 1); // Equal size for other columns
            XFont headlineFont = new XFont("Arial", 10, XFontStyle.Bold);
            XFont contentFont = new XFont("Arial", 10, XFontStyle.Regular);
            bool isFirstPage = true; // Flag to track if it's the first page

            // Drawing the headlines on the first page
            for (int j = 0; j < headings.Length; j++)
            {
                double currentColumnWidth = j == 0 ? orderIdColumnWidth : otherColumnWidth;
                XRect headlineRect = new XRect(contentX, headlineY, currentColumnWidth, 20);
                gfx.DrawString(headings[j], headlineFont, XBrushes.Black, headlineRect, XStringFormats.TopLeft);
                contentX += currentColumnWidth + otherColumnWidth; // Increment by column width and additional space
            }


            contentX = 50; // Reset contentX to the initial value

            for (int i = 0; i < orderIds.Length; i++)
            {
                // Checking if a new page is needed
                if (!isFirstPage && contentY + 20 > page.Height - 50)
                {
                    // Adding a new page
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    page.Orientation = PageOrientation.Landscape;
                    contentY = headlineY + 40; // Start below the headings with a gap
                                               // Drawing the headlines on the new page
                    contentX = 50; // Reset contentX to the initial value
                    for (int j = 0; j < headings.Length; j++)
                    {
                        double currentColumnWidth = j == 0 ? orderIdColumnWidth : otherColumnWidth;
                        XRect headlineRect = new XRect(contentX, headlineY, currentColumnWidth, 20);
                        gfx.DrawString(headings[j], headlineFont, XBrushes.Black, headlineRect, XStringFormats.TopLeft);
                        contentX += currentColumnWidth + otherColumnWidth; // Increment by column width and additional space
                    }
                }

                XBrush color = XBrushes.Black;
                if (Convert.ToDouble(profits[i]) == 0)
                {
                    color = XBrushes.Red;
                }

                contentX = 50; // Reset contentX to the initial value

                for (int j = 0; j < headings.Length; j++)
                {
                    double currentColumnWidth = j == 0 ? orderIdColumnWidth : otherColumnWidth;
                    XRect contentRect = new XRect(contentX, contentY, currentColumnWidth, 20);
                    gfx.DrawString(GetContentForColumn(j, i, orderIds, internalNumbers, amounts, externalCostsArray, externalCostsDiffArray, taxesTypes, taxesArray, marketplaceFeesArray, profits, margins, paymentFeesArray, backShipCostsArray, sellerCommissionsArray, ccbmFee), contentFont, color, contentRect, XStringFormats.TopLeft);
                    contentX += currentColumnWidth + otherColumnWidth; // Increment by column width and additional space
                }

                contentY += 20;

                isFirstPage = false; // Update the flag after the first iteration
            }

            document.Save(fullPath2);

        }
        public static string GetContentForColumn(int columnIndex, int rowIndex, string[] orderIds, string[] internalNumbers, string[] amounts, string[] externalCostsArray,
                         string[] externalCostsDiffArray, string[] taxesTypes, string[] taxesArray,
                         string[] marketplaceFeesArray, string[] profits, string[] margins, string[] paymentFeesArray, string[] backShipCostsArray,
                         string[] sellerCommissionsArray, string ccbmFee)
        {
            switch (columnIndex)
            {
                case 0:  // Order ID
                    return orderIds[rowIndex];
                case 1:  // Internal
                    return internalNumbers[rowIndex];
                case 2:  // Amount
                    return amounts[rowIndex];
                case 3:  // External Costs Nor
                    return externalCostsArray[rowIndex];
                case 4:  // External Costs Mar
                    return externalCostsDiffArray[rowIndex];
                case 5:  // Tax Type
                    return taxesTypes[rowIndex];
                case 6:  // Taxes
                    return taxesArray[rowIndex];
                case 7:  // Seller Commission
                    return sellerCommissionsArray[rowIndex];
                case 8:  // Payment Fees
                    return paymentFeesArray[rowIndex];
                case 9:  // BackShip
                    return backShipCostsArray[rowIndex];
                case 10: // CCBM
                    return ccbmFee;
                case 11: // Total
                    return marketplaceFeesArray[rowIndex];
                case 12: // Profit
                    return profits[rowIndex];
                case 13: // Margin
                    return margins[rowIndex];
                default:
                    return string.Empty;
            }
        }
    }
}
