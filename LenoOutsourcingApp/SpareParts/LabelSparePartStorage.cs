using System;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public class LabelSparePartStorage
    {
        public static void Print(string model, string part, string supplier, string id, int quantity)
        {
            try
            {
                string currentUser = UserFileManagement.ReturnCurrentUser();
                var dbManager = new DBManager();
                string path = dbManager.ExecuteQueryWithResultString("ConfigUser", "TemplateSparePartStorage", "Nutzer", currentUser);

                bpac.Document doc = new bpac.Document();
                doc.Open(path);
                doc.SetPrinter("Brother QL-600", true);

                string storageText = DateTime.Now.ToShortDateString() + " by " + currentUser;
                var indexBarcode = doc.GetBarcodeIndex("barcode");
                doc.SetBarcodeData(indexBarcode, id);
                doc.GetObject("part").Text = $"Part: {model} {part}";
                doc.GetObject("supplier_storage").Text = $"Supplier: {supplier}\r\nStorage: {storageText}";

                doc.StartPrint("", bpac.PrintOptionConstants.bpoDefault);
                doc.PrintOut(quantity, bpac.PrintOptionConstants.bpoDefault);
                doc.EndPrint();
                doc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Print Error: " + ex.Message);
            }
        }
    }
}
