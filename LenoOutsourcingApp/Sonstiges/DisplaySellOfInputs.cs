using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class DisplaySellOfInputs : Form
    {
        public static string currentUser = UserFileManagement.ReturnCurrentUser();
        public DisplaySellOfInputs()
        {
            InitializeComponent();
        }
        public Dictionary<string, string> taxes = new Dictionary<string, string>
        {
            {"Differenzbesteuert", "DIFF"},
            {"Regelbesteuert", "REG"},
        };
        public Dictionary<string, string> models = new Dictionary<string, string>
        {
            {"iPhone 6S", "6S"},
            {"iPhone 6S Plus", "6SP"},
            {"iPhone 7", "7"},
            {"iPhone 7 Plus", "7P"},
            {"iPhone 8", "8"},
            {"iPhone 8 Plus", "8P"},
            {"iPhone X", "X"},
            {"iPhone XS", "XS"},
            {"iPhone XR", "XR"},
            {"iPhone XS Max", "XSM"},
            {"iPhone 11", "11"},
            {"iPhone 11 Pro", "11P"},
            {"iPhone 11 Pro Max", "11PM"},
            {"iPhone 12 / 12 Pro", "12/12P"},
            {"iPhone 12 Pro Max", "12PM"},
            {"iPhone 12 Mini", "12M"},
            {"iPhone 13", "13"},
            {"iPhone 13 Mini", "13M"},
            {"iPhone 13 Pro", "13P"},
            {"iPhone 13 Pro Max", "13 Pro Max"},
        };

        private void button1_Click(object sender, EventArgs e)
        {
            string model = comboBox_model.Text;
            string flaw = comboBox_flaws.Text;
            string touch = comboBox_touchFunction.Text;
            string scratches = comboBox_scratches.Text;
            string tax = comboBox1.Text;
            string number = textBox_number.Text;
            string price = textBox_price.Text;
            string barcodeContent = GenerateBarcode(number, tax);
            AdaptNumberInDB(number);
            PrintLabel(barcodeContent, model, flaw, touch, scratches, price);
        }
        public string GenerateBarcode(string number, string tax)
        {
            return "DIS/" + number.PadLeft(5, '0') + "/" + taxes[tax];
        }
        public void AdaptNumberInDB(string number)
        {
            int newNumber = Convert.ToInt32(number) + 1;
            var dbManager = new DBManager();
            dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = '" + newNumber.ToString() + "' WHERE `Typ` = 'DisplaySellOffCounter'");
        }
        public void PrintLabel(string barcodeContent, string model, string flaw, string touchFunction, string scratches, string price)
        {
            try
            {
                var dbManager = new DBManager();
                string path = dbManager.ExecuteQueryWithResultString("ConfigUser", "TemplateDisplay", "Nutzer", currentUser);
                bpac.Document doc = new bpac.Document();
                doc.Open(path);
                doc.SetPrinter("Brother QL-600", true);

                //set values
                var indexBarcode = doc.GetBarcodeIndex("Barcode7");
                doc.SetBarcodeData(indexBarcode, barcodeContent);
                doc.GetObject("Modell").Text = models[model];
                doc.GetObject("Fehler").Text = "- Mangel: " + flaw;
                doc.GetObject("Touch").Text = "- Touch: " + touchFunction;
                doc.GetObject("Kratzer").Text = "- Kratzer: " + scratches;
                doc.GetObject("preis").Text = price;

                //doc.Save();
                doc.StartPrint("", bpac.PrintOptionConstants.bpoDefault);
                doc.PrintOut(1, bpac.PrintOptionConstants.bpoDefault);
                doc.EndPrint();
                doc.Close();
                MessageBox.Show("Erfolgreich ausgeführt.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        private void DisplaySellOfInputs_Load(object sender, EventArgs e)
        {
            var dbManager = new DBManager();
            textBox_number.Text = dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "DisplaySellOffCounter");
        }
    }
}
