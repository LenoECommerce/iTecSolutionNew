using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SautinSoft.Document;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using PdfSharp.Drawing.Layout;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using Org.BouncyCastle.Crypto;
using PayPal.Api;
using Billbee.Api.Client.Model;
using System.Drawing.Printing;

namespace EigenbelegToolAlpha
{
    public class OrderRelationPDF
    {
        EvaluationsBackMarketPDF evaluationsBackMarketPDF = new EvaluationsBackMarketPDF();
        EvaluationsEbayPDF evaluationsEbay = new EvaluationsEbayPDF();
        EvaluationsFirstPage EvaluationsFirstPage = new EvaluationsFirstPage();

        public static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
        public string backmarketOrdersPath = "";
        public static string fullPath = desktopPath + "test.pdf";
        public static double headingPosY = 30;
        public static int entriesAdded = 0;
        public static int beginLineEbay = 0;
        //new approach
        public static string[] orderIDs = new string[0];
        public static string[] internalNumbers = new string[0];
        public static string[] amounts = new string[0];
        public static string[] externalCostsArray = new string[0];
        public static string[] externalCostsDiffArray = new string[0];
        public static string[] taxesTypes = new string[0];
        public static string[] taxesArray = new string[0];
        public static string[] paymentFeesArray = new string[0];
        public static string[] backShipCostsArray = new string[0];
        public static string[] sellerCommissionArray = new string[0];
        public static string[] marketPlaceFeesArray = new string[0];
        public static string[] profits = new string[0];
        public static string[] margins = new string[0];
        //monthly report sum up values
        public static double grossSalesEbay = 0;
        public static double grossSalesBackmarketNormal = 0;
        public static double grossSalesBackmarketPayPal = 0;
        public static void Draw()
        {
            //Set up file with one entry
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont heading = new XFont("Arial", 20);
            XFont main = new XFont("Arial", 14);
            XFont subFont = new XFont("Arial", 11);
            //Zeilenüberschriften
            gfx.DrawString("Orderübersicht", main, XBrushes.Black, new XPoint(10, 10));
            gfx.DrawString("Bestellnummer", main, XBrushes.Black, new XPoint(10, headingPosY));
            gfx.DrawString("Intern", main, XBrushes.Black, new XPoint(110, headingPosY));
            gfx.DrawString("Kaufbetrag", main, XBrushes.Black, new XPoint(160, headingPosY));
            gfx.DrawString("Ko RE", main, XBrushes.Black, new XPoint(240, headingPosY));
            gfx.DrawString("Ko DI", main, XBrushes.Black, new XPoint(290, headingPosY));
            gfx.DrawString("Bst.", main, XBrushes.Black, new XPoint(340, headingPosY));
            gfx.DrawString("Steuern", main, XBrushes.Black, new XPoint(390, headingPosY));
            gfx.DrawString("MP", main, XBrushes.Black, new XPoint(450, headingPosY));
            gfx.DrawString("Rev", main, XBrushes.Black, new XPoint(490, headingPosY));
            gfx.DrawString("Mar", main, XBrushes.Black, new XPoint(530, headingPosY));

            //Vertikale Linien
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(105, 20), new XPoint(105, 1000));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(155, 20), new XPoint(155, 1000));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(235, 20), new XPoint(235, 1000));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(285, 20), new XPoint(285, 1000));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(335, 20), new XPoint(335, 1000));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(385, 20), new XPoint(385, 1000));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(105, 20), new XPoint(105, 1000));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(445, 20), new XPoint(445, 1000));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(485, 20), new XPoint(485, 1000));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(525, 20), new XPoint(525, 1000));
            //Horizontale Linien
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(0, 35), new XPoint(1000, 35));
            // value part
            double yPos = headingPosY + 20;
            for (int i = 0; i < orderIDs.Length; i++)
            {
                // color highlighting for errors
                XBrush color = XBrushes.Black;
                if (Convert.ToDouble(profits[i]) == 0)
                {
                    color = XBrushes.Red;
                }
                // add new pages
                if (entriesAdded >= 70)
                {
                    page = document.AddPage();
                    entriesAdded = 0;
                }
                gfx.DrawString(orderIDs[i], subFont, XBrushes.Black, new XPoint(10, yPos));
                gfx.DrawString(internalNumbers[i], subFont, XBrushes.Black, new XPoint(110, yPos));
                gfx.DrawString(amounts[i], subFont, XBrushes.Black, new XPoint(160, yPos));
                gfx.DrawString(externalCostsArray[i], subFont, XBrushes.Black, new XPoint(240, yPos));
                gfx.DrawString(externalCostsDiffArray[i], subFont, XBrushes.Black, new XPoint(290, yPos));
                gfx.DrawString(taxesTypes[i], subFont, XBrushes.Black, new XPoint(340, yPos));
                gfx.DrawString(taxesArray[i], subFont, XBrushes.Black, new XPoint(390, yPos));
                gfx.DrawString(marketPlaceFeesArray[i], subFont, XBrushes.Black, new XPoint(450, yPos));
                gfx.DrawString(profits[i], subFont, color, new XPoint(490, yPos));
                gfx.DrawString(margins[i], subFont, XBrushes.Black, new XPoint(530, yPos));
                entriesAdded++;
                yPos += 10;
            }
            document.Save(fullPath);
        }

    }
}
