using ExcelLibrary.BinaryFileFormat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace EigenbelegToolAlpha
{
    public partial class EbaySingleListing : Form
    {
        private string lack = "";
        private string batteryCapacity = "";
        private string price = "";
        private string notification = "";
        private string sku = "";
        private string picturesRequired = "";

        public EbaySingleListing()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void EbaySingleListing_Load(object sender, EventArgs e)
        {
            var dbManager = new DBManager();
            textBox_SKUNumber.Text = dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "DisplaySellOffCounter");
        }
        private void LoadBatteryPercentage()
        {
            for (int i = 100; i >= 50; i--)
            {
                comboBox_Battery.Items.Add(i.ToString() + "%");
            }
        }

        private void comboBox_Battery_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_Execute_Click(object sender, EventArgs e)
        {
            lack = textBox_lack.Text;
            price = textBox_price.Text;
            picturesRequired = comboBox_pictureRequired.Text;
            notification = comboBox_notification.Text;
            batteryCapacity = comboBox_Battery.Text;
            sku = textBox_SKUNumber.Text;
            int newNumber = Convert.ToInt32(sku) + 1;
            var dbManager = new DBManager();
            dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = '" + newNumber.ToString() + "' WHERE `Typ` = 'DisplaySellOffCounter'");
            PrintLabel();
        }

        private void PrintLabel()
        {
            string currentUser = UserFileManagement.ReturnCurrentUser();
            var dbManager = new DBManager();
            string path = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathTemplateEbaySingleListing", "Nutzer", currentUser);
            bpac.Document doc = new bpac.Document();
            doc.Open(path);
            doc.SetPrinter("Brother QL-600", true);
            doc.GetObject("input").Text = 
                $"- Mangel: {lack}\r\n" +
                $"- Akku: {batteryCapacity}\r\n" +
                $"- Meldung: {notification}\r\n" +
                $"- Bilder relevant?: {picturesRequired}";
            doc.GetObject("price").Text = $"{price}€";
            //barcode here
            var barcodeIndex = doc.GetBarcodeIndex("barcode");
            var barcodeData = sku;
            doc.SetBarcodeData(barcodeIndex, sku);

            doc.StartPrint("", bpac.PrintOptionConstants.bpoDefault);
            doc.PrintOut(1, bpac.PrintOptionConstants.bpoDefault);
            doc.EndPrint();
            doc.Close();
        }
    }
}
