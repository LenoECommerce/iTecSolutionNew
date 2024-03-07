﻿using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System.IO;
using System.Windows.Forms;



namespace EigenbelegToolAlpha
{
    public class pdfDocument
    {
        public static string paymentMethodPath = "paypal.jpg";
        public static string filename = "";
        public string locationImages;
        public string savePath;

        public pdfDocument()
        {
            var dbManager = new DBManager();
            locationImages = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathImagesEB", "Nutzer", Settings.currentUser);
            savePath = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathSaveLocationEB", "Nutzer", Settings.currentUser);
        }

        public void CreateDocument(string pdfEigenbelegNumber, string pdfSellerName, string pdfDateBought,
             string pdfTransactionAmount, string pdfArticle, string pdfPlatform, string pdfPaymentmethod, string pdfAddress)
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


            //Text schreiben
            // XBrushed means color; xPoint sozusagen X und Y
            // Wichtig!! Position muss absolut sein und nicht dynamisch, mit Algorithmus
            //Drawline: XColor: Code Werte

            gfx.DrawString("Eigenbeleg", heading, XBrushes.Black, new XPoint(100, 100));
            //Vertikale Lines
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(180, 120), new XPoint(180, 1000));
            //gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(300, 675), new XPoint(300, 1000));
            //Nr + Betrag
            gfx.DrawString("Nr. " + pdfEigenbelegNumber, main, XBrushes.Black, new XPoint(100, 150));
            gfx.DrawString("Ausgaben Netto", main, XBrushes.Black, new XPoint(200, 150));
            gfx.DrawString(pdfTransactionAmount, main, XBrushes.Black, new XPoint(400, 150));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(0, 170), new XPoint(1000, 170));
            //Artikel
            gfx.DrawString("Artikel", main, XBrushes.Black, new XPoint(100, 250));

            //Textformatter!!
            XRect rect = new XRect(200, 200, 300, 300);
            gfx.DrawRectangle(XBrushes.AliceBlue, rect);
            tf.DrawString(pdfArticle, subFont, XBrushes.Black, rect, XStringFormats.TopLeft);


            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(0, 575), new XPoint(1000, 575));
            //Verkäufer
            gfx.DrawString("Verkäufer", main, XBrushes.Black, new XPoint(100, 600));
            gfx.DrawString(pdfAddress, subFont, XBrushes.Black, new XPoint(200, 600));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(0, 625), new XPoint(1000, 625));
            //Grund
            gfx.DrawString("Grund", main, XBrushes.Black, new XPoint(100, 650));
            gfx.DrawString("Kauf von Privatperson auf der Online-Plattform " + pdfPlatform, subFont, XBrushes.Black, new XPoint(200, 650));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(0, 675), new XPoint(1000, 675));
            //Kaufdatum und Unterschrift
            gfx.DrawString("Ankunftsdatum", main, XBrushes.Black, new XPoint(100, 700));
            gfx.DrawString(pdfDateBought, subFont, XBrushes.Black, new XPoint(200, 700));
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), new XPoint(0, 725), new XPoint(1000, 725));
            gfx.DrawString("Unterschrift", main, XBrushes.Black, new XPoint(100, 750));


            //Bilder!
            string imagePath = Path.GetTempPath();
            imagePath = imagePath + "unterschrift.png";
            if (File.Exists(imagePath) == false)
            {
            }
            DrawImage(gfx, imagePath, 200, 750, 280, 80);


            filename = "Eigenbeleg" + pdfEigenbelegNumber;
            document.Save(savePath + @"/" + filename + ".pdf");
        }

        //Methode für Bildererstellung
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
    }
}
