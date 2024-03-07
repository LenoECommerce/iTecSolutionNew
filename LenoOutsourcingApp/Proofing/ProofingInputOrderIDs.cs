using System;
using System.Linq;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class ProofingInputOrderIDs : Form
    {
        public ProofingInputOrderIDs()
        {
            InitializeComponent();
        }
        public string[] columns = new string[6] { "Order ID", "Intern", "IMEI", "Aesthetic condition before delivery", "Technical Certificate", "Additional Information" };
        public string[] collectedIMEI = new string[] { };
        public string[] collectedVideoLink = new string[] { };
        public string[] collectedTechnicalCertificate = new string[] { };
        string[] matchingInternalNumbers = new string[] { };
        string[] orderIds = new string[] { };
        public int elementCounter = 0;
        private void btn_Execute_Click(object sender, EventArgs e)
        {
            string[] newArray = new string[] { textBox_Input.Text };
            orderIds = orderIds.Concat(newArray).ToArray();
            textBox_Input.Text = "";
        }

        private void btn_createExcelFile_Click(object sender, EventArgs e)
        {
            var dbManager = new DBManager();
            foreach (var item in orderIds)
            {
                elementCounter++;
                string[] newArray = new string[] { dbManager.ExecuteQueryWithResultString("Protokollierung", "Intern", "Bestellnummer", item.ToString()) };
                matchingInternalNumbers = matchingInternalNumbers.Concat(newArray).ToArray();
                string[] newArray2 = new string[] { dbManager.ExecuteQueryWithResultString("Proofing", "IMEI", "Intern", item.ToString()) };
                collectedIMEI = collectedIMEI.Concat(newArray2).ToArray();
                string[] newArray3 = new string[] { dbManager.ExecuteQueryWithResultString("Proofing", "NSYS-Zertifikat", "Intern", item.ToString()) };
                collectedTechnicalCertificate = collectedTechnicalCertificate.Concat(newArray3).ToArray();
                string[] newArray4 = new string[] { dbManager.ExecuteQueryWithResultString("Proofing", "Video", "Intern", item.ToString()) };
                collectedVideoLink = collectedIMEI.Concat(newArray4).ToArray();
            }

            ExcelManager.CreateNewExcelFileCommissionRefund("BM Commission Refund Request", columns, elementCounter, orderIds, matchingInternalNumbers, collectedIMEI, collectedVideoLink, collectedTechnicalCertificate);
            MessageBox.Show("Deine Excel Datei wurde erfolgreich erstellt.");
        }
    }
}
