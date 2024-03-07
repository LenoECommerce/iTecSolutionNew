using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class EvaluationsFirstPage : Form
    {
        public EvaluationsFirstPage()
        {
            InitializeComponent();
        }
        public static string month = "";
        public static string year = "";
        public static double ebayTaxGetBack = 0;

        private void EvaluationsFirstPage_Load(object sender, EventArgs e)
        {
            LoadValuesToFrontend();
        }

        public void LoadValuesToFrontend()
        {
            string table = "Evaluations";
            var dbManager = new DBManager();
            int id = Convert.ToInt32(dbManager.ExecuteQueryWithResultStringTwoConditions(table, "Id", "Monat", "Jahr", month, year));
            if (id != 0)
            {
                lbl_BackMarketNormal1.Text = dbManager.ExecuteQueryWithResultStringTwoConditions(table, "BackMarketNormal1", "Monat", "Jahr", month, year);
                lbl_BackMarketNormal2.Text = dbManager.ExecuteQueryWithResultStringTwoConditions(table, "BackMarketNormal2", "Monat", "Jahr", month, year);
                lbl_BackMarketNormal3.Text = dbManager.ExecuteQueryWithResultStringTwoConditions(table, "BackMarketNormal3", "Monat", "Jahr", month, year);
                lbl_BackMarketPayPal1.Text = dbManager.ExecuteQueryWithResultStringTwoConditions(table, "BackMarketPayPal1", "Monat", "Jahr", month, year);
                lbl_paypal2.Text = dbManager.ExecuteQueryWithResultStringTwoConditions(table, "BackMarketPayPal2", "Monat", "Jahr", month, year);
                lbl_BackMarketPayPal3.Text = dbManager.ExecuteQueryWithResultStringTwoConditions(table, "BackMarketPayPal3", "Monat", "Jahr", month, year);
                lbl_backMarketPayPalInt1.Text = dbManager.ExecuteQueryWithResultStringTwoConditions(table, "BackMarketPayPal1International", "Monat", "Jahr", month, year);
                lbl_backMarketPayPalInt2.Text = dbManager.ExecuteQueryWithResultStringTwoConditions(table, "BackMarketPayPal2International", "Monat", "Jahr", month, year);
                lbl_backMarketPayPalInt3.Text = dbManager.ExecuteQueryWithResultStringTwoConditions(table, "BackMarketPayPal3International", "Monat", "Jahr", month, year);
                lbl_ebayInvoice.Text = dbManager.ExecuteQueryWithResultStringTwoConditions(table, "EbayInvoice", "Monat", "Jahr", month, year);
                lbl_ebayReport.Text = dbManager.ExecuteQueryWithResultStringTwoConditions(table, "EbayReport", "Monat", "Jahr", month, year);
            }
        }


        public void EbayPDFInvoiceAlgorithm()
        {
            EvaluationsFirstPage eval = new EvaluationsFirstPage();
            TextOperations textOperations = new TextOperations();

            string searchValueOrderValue = "USt zu 19 %";
            double tempTaxGetBack = 0;

            var dbManager = new DBManager();
            string path = dbManager.ExecuteQueryWithResultStringTwoConditions("Evaluations", "EbayInvoice", "Monat", "Jahr", month, year);
            string tempPath = "ebaydata2.txt";
            FileStream stream = File.Create(tempPath);
            stream.Close();
            File.WriteAllText(tempPath, ExtractTextFromPdf(path));
            string[] allLines = File.ReadAllLines(tempPath);

            int indexTaxGetBack = TextOperations.FindLine(allLines, searchValueOrderValue) - 1;
            tempTaxGetBack = TextOperations.getValueOfOneLineEbayFormat(indexTaxGetBack, allLines, 7, "19 % ");

            ebayTaxGetBack = tempTaxGetBack;
        }

        public static string ExtractTextFromPdf(string path)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }

                return text.ToString();
            }
        }
        public string getOpenFileDialog()
        {
            openFD.ShowDialog();
            return openFD.FileName;
        }

        private void lbl_BackMarketNormal1_Click(object sender, EventArgs e)
        {
            lbl_BackMarketNormal1.Text = getOpenFileDialog();
        }
        private void lbl_BackMarketNormal2_Click(object sender, EventArgs e)
        {
            lbl_BackMarketNormal2.Text = getOpenFileDialog();
        }

        private void lbl_BackMarketNormal3_Click(object sender, EventArgs e)
        {
            lbl_BackMarketNormal3.Text = getOpenFileDialog(); ;
        }

        private void lbl_BackMarketPayPal1_Click(object sender, EventArgs e)
        {
            lbl_BackMarketPayPal1.Text = getOpenFileDialog();
        }

        private void lbl_BackMarketPayPal2_Click(object sender, EventArgs e)
        {
            lbl_paypal2.Text = getOpenFileDialog();
        }

        private void lbl_BackMarketPayPal3_Click(object sender, EventArgs e)
        {
            lbl_BackMarketPayPal3.Text = getOpenFileDialog();
        }

        private void lbl_ebayReport_Click(object sender, EventArgs e)
        {
            lbl_ebayReport.Text = getOpenFileDialog();
        }


        private async void btn_ContinueWithEvaluation2_Click(object sender, EventArgs e)
        {
            string query = "";
            string backMarketNormal1 = lbl_BackMarketNormal1.Text.Replace(@"\", @"\\");
            string backMarketNormal2 = lbl_BackMarketNormal2.Text.Replace(@"\", @"\\");
            string backMarketNormal3 = lbl_BackMarketNormal3.Text.Replace(@"\", @"\\");
            string backMarketPayPal1 = lbl_BackMarketPayPal1.Text.Replace(@"\", @"\\");
            string backMarketPayPal2 = lbl_paypal2.Text.Replace(@"\", @"\\");
            string backMarketPayPal3 = lbl_BackMarketPayPal3.Text.Replace(@"\", @"\\");
            string backMarketPayPal1Int = lbl_backMarketPayPalInt1.Text.Replace(@"\", @"\\");
            string backMarketPayPal2Int = lbl_backMarketPayPalInt2.Text.Replace(@"\", @"\\");
            string backMarketPayPal3Int = lbl_backMarketPayPalInt3.Text.Replace(@"\", @"\\");
            string ebayInvoice = lbl_ebayInvoice.Text.Replace(@"\", @"\\");
            string ebayReport = lbl_ebayReport.Text.Replace(@"\", @"\\");
            //check if the month report already exists
            var dbManager = new DBManager();
            int id = Convert.ToInt32(dbManager.ExecuteQueryWithResultStringTwoConditions("Evaluations", "Id", "Monat", "Jahr", month, year));
            if (id != 0)
            {
                query = string.Format("UPDATE `Evaluations` SET `BackMarketNormal1` = '{0}', `BackMarketNormal2` = '{1}', `BackMarketNormal3` = '{2}', `BackMarketPayPal1` = '{3}', `BackMarketPayPal2` = '{4}', `BackMarketPayPal3` = '{5}', `EbayReport` = '{6}', `EbayInvoice` = '{7}', `BackMarketPayPal1International` = '{8}', `BackMarketPayPal2International` = '{9}', `BackMarketPayPal3International` = '{10}' WHERE `Id` = '" + id.ToString() + "'"
                                        , backMarketNormal1, backMarketNormal2, backMarketNormal3, backMarketPayPal1, backMarketPayPal2, backMarketPayPal3, ebayReport, ebayInvoice, backMarketPayPal1Int, backMarketPayPal2Int, backMarketPayPal3Int);
            }
            else
            {
                query = string.Format("INSERT INTO `Evaluations` (`Monat`,`Jahr`,`BackMarketNormal1`, `BackMarketNormal2`, `BackMarketNormal3`, `BackMarketPayPal1`, `BackMarketPayPal2`, `BackMarketPayPal3`, `EbayReport`, `EbayInvoice`, `BackMarketPayPal1International`, `BackMarketPayPal2International`, `BackMarketPayPal3International`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')"
                                        , month, year, backMarketNormal1, backMarketNormal2, backMarketNormal3, backMarketPayPal1, backMarketPayPal2, backMarketPayPal3, ebayReport, ebayInvoice, backMarketPayPal1Int, backMarketPayPal2Int, backMarketPayPal3Int);
            }
            dbManager.ExecuteQuery(query);
            EbayPDFInvoiceAlgorithm();
            // entry point for assigning the sell of month + year for the current report
            await AssigningMonthAndYear();

            EvaluationSecondForm frm = new EvaluationSecondForm();
            frm.Show();
            this.Hide();
        }
        private async Task AssigningMonthAndYear()
        {
            Thread staThread = new Thread(() =>
            {
                Protokollierung prot = new Protokollierung();
                prot.AssignMonthToDataSetAsync(year, month).GetAwaiter().GetResult();
            });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
        }

        private void comboBox_MonthOfEvaluation_SelectedIndexChanged(object sender, EventArgs e)
        {
            month = comboBox_MonthOfEvaluation.Text;
            LoadValuesToFrontend();
        }


        private void comboBox_Years_SelectedIndexChanged(object sender, EventArgs e)
        {
            year = comboBox_Years.Text;
            LoadValuesToFrontend();
        }


        private void lbl_ebayInvoice_Click(object sender, EventArgs e)
        {
            lbl_ebayInvoice.Text = getOpenFileDialog();
        }

        private void lbl_backMarketPayPalInt1_Click(object sender, EventArgs e)
        {
            lbl_backMarketPayPalInt1.Text = getOpenFileDialog();
        }

        private void lbl_backMarketPayPalInt2_Click(object sender, EventArgs e)
        {
            lbl_backMarketPayPalInt2.Text = getOpenFileDialog();
        }

        private void lbl_backMarketPayPalInt3_Click(object sender, EventArgs e)
        {
            lbl_backMarketPayPalInt3.Text = getOpenFileDialog();
        }

        private void lbl_BackMarketNormal1_Click_1(object sender, EventArgs e)
        {
            lbl_BackMarketNormal1.Text = getOpenFileDialog();
        }
    }
}
