using Billbee.Api.Client.Model;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace EigenbelegToolAlpha
{
    public partial class Eigenbelege : Form
    {


        public Eigenbelege()
        {
            InitializeComponent();
            ShowEigenbelege();
        }
        public static MySqlConnection conn;
        public static string currentUser = UserFileManagement.ReturnCurrentUser();
        public string path = "";
        public string destPath = "mainfile.xlsx";
        public static string pdfSaveDestPath = "";
        public static int lastSelectedProductKey;
        public static string imagesFolderPath = "";

        public static string eigenbelegNumber = "";
        public static string sellerName = "";
        public static string reference = "";
        public static string model = "";
        public static string dateBought = "";
        public static string transactionAmount = "";
        public static string mail = "";
        public static string platform = "";
        public static string paymentMethod = "";
        public static string address = "";
        public static string created = "";
        public static string arrived = "";
        public static string transactionText = "";
        public static string storage = "";
        public static string defect = "";
        public static string buybackGrade = "";

        public void Hauptmenü_Load(object sender, EventArgs e)
        {
            ShowEigenbelege();
            var dbManager = new DBManager();
        }

        public void ShowEigenbelege()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = DBManager.connString;
            conn.Open();

            string query = "SELECT * FROM `Eigenbelege`";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            ////Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            //Daten anzeigen im Grid
            eigenbelegeDGV.DataSource = dataSet.Tables[0];
            //Column verstecken
            eigenbelegeDGV.Columns[0].Visible = false;
            eigenbelegeDGV.Columns[13].Visible = false;
            eigenbelegeDGV.Columns[14].Visible = false;
            eigenbelegeDGV.Columns[15].Visible = false;
            eigenbelegeDGV.Columns[16].Visible = false;
            //Sortierte Ansicht
            eigenbelegeDGV.Sort(eigenbelegeDGV.Columns[1], ListSortDirection.Descending);
            foreach (DataGridViewRow row in eigenbelegeDGV.Rows)
            {
                string compareValue = row.Cells[12].Value.ToString();
                double compareValueAmount = AdaptAmount(row.Cells[6].Value.ToString());
                if (compareValue == "Nein")
                {
                    row.DefaultCellStyle.BackColor = Color.Orange;
                }
                if (compareValueAmount >= 500)
                {
                    row.DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                }
            }
            conn.Close();
            LogicCheckForNumbers();
        }
        public void LogicCheckForNumbers()
        {
            var dbManager = new DBManager();
            int id = dbManager.ExecuteQueryWithResult("Eigenbelege", "Id", "Eigenbelegnummer", "");
            if (id != 0)
            {
                MessageBox.Show("Es besteht ein Logikfehler. Es gibt einen Eigenbeleg mit der Nummer 0 oder einer komplett leeren Nummer. Bitte prüfen!");
            }
        }
        public double AdaptAmount(string adaptValue)
        {
            if (adaptValue.Contains("€"))
            {
                int length = adaptValue.Length;
                return Convert.ToDouble(adaptValue.Substring(0, length - 1));
            }
            return Convert.ToDouble(adaptValue);
        }
        public void ShowEigenbelegeFiltered(string[] filterValueModel, string[] filterValueCreated, string[] filterValuePlatform)
        {
            conn = new MySqlConnection();
            conn.ConnectionString = DBManager.connString;
            conn.Open();

            string query = "SELECT * FROM `Eigenbelege`";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            ////Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            //Daten anzeigen im Grid
            eigenbelegeDGV.DataSource = dataSet.Tables[0];
            eigenbelegeDGV.ClearSelection();
            //Column verstecken
            eigenbelegeDGV.Columns[0].Visible = false;
            //Sortierte Ansicht
            eigenbelegeDGV.Sort(eigenbelegeDGV.Columns[1], ListSortDirection.Descending);
            //Filtern
            eigenbelegeDGV.CurrentCell = null;
            for (int i = 0; i < eigenbelegeDGV.RowCount; i++)
            {
                if (filterValueModel.Contains(eigenbelegeDGV.Rows[i].Cells[4].Value.ToString()) == true)
                {
                    if (filterValueCreated.Contains(eigenbelegeDGV.Rows[i].Cells[11].Value.ToString()) == true)
                    {
                        if (filterValuePlatform.Contains(eigenbelegeDGV.Rows[i].Cells[8].Value.ToString()) == true)
                        {
                            eigenbelegeDGV.Rows[i].Visible = true;
                        }
                        else
                        {
                            eigenbelegeDGV.Rows[i].Visible = false;
                        }
                    }
                    else
                    {
                        eigenbelegeDGV.Rows[i].Visible = false;
                    }
                }
                else
                {
                    eigenbelegeDGV.Rows[i].Visible = false;
                }
            }

            conn.Close();
        }

        public static void ExecuteQuery(string query)
        {
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = DBManager.connString;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public static void dataSync(string destTable, string syncModel, string syncTransactionAmount, string syncStorage, string syncEigenbelegNumber)
        {
            //Abfrage welche Tabelle geupdatet wird
            if (destTable == "Reparaturen")
            {
                string query = string.Format("UPDATE `" + destTable + "` SET `Geraet` = '{0}', `Kaufbetrag` = '{1}', `Speicher` = '{2}' WHERE `EBReferenz` = {3}",
                           syncModel, syncTransactionAmount, syncStorage, syncEigenbelegNumber);
                ExecuteQuery(query);
            }
            else
            {
                string query = string.Format("UPDATE `" + destTable + "` SET `Modell` = '{0}', `Kaufbetrag` = '{1}', `Speicher` = '{2}' WHERE `Eigenbelegnummer` = {3}",
                           syncModel, syncTransactionAmount, syncStorage, syncEigenbelegNumber);
                ExecuteQuery(query);
            }

        }



        private void eigenbelegeDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            eigenbelegNumber = eigenbelegeDGV.SelectedRows[0].Cells[1].Value.ToString();
            sellerName = eigenbelegeDGV.SelectedRows[0].Cells[2].Value.ToString();
            reference = eigenbelegeDGV.SelectedRows[0].Cells[3].Value.ToString();
            model = eigenbelegeDGV.SelectedRows[0].Cells[4].Value.ToString();
            dateBought = eigenbelegeDGV.SelectedRows[0].Cells[5].Value.ToString();
            transactionAmount = eigenbelegeDGV.SelectedRows[0].Cells[6].Value.ToString();
            mail = eigenbelegeDGV.SelectedRows[0].Cells[7].Value.ToString();
            platform = eigenbelegeDGV.SelectedRows[0].Cells[8].Value.ToString();
            paymentMethod = eigenbelegeDGV.SelectedRows[0].Cells[9].Value.ToString();
            address = eigenbelegeDGV.SelectedRows[0].Cells[10].Value.ToString();
            created = eigenbelegeDGV.SelectedRows[0].Cells[11].Value.ToString();
            arrived = eigenbelegeDGV.SelectedRows[0].Cells[12].Value.ToString();
            transactionText = eigenbelegeDGV.SelectedRows[0].Cells[13].Value.ToString();
            storage = eigenbelegeDGV.SelectedRows[0].Cells[14].Value.ToString();

            lastSelectedProductKey = (int)eigenbelegeDGV.SelectedRows[0].Cells[0].Value;

        }

        private void btn_eigenbelegCreate_Click(object sender, EventArgs e)
        {
            using (var form = new EigenbelegCreate())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ShowEigenbelege();
                }
            }
        }

        private void btn_eigenbelegRemove_Click(object sender, EventArgs e)
        {
            if (lastSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle zuerst einen Eintrag aus");
                return;
            }
            DialogResult result = MessageBox.Show("Eintrag sicher löschen?", "Title", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string query = string.Format("DELETE FROM `Eigenbelege` WHERE `Id` = {0} ;", lastSelectedProductKey);
                ExecuteQuery(query);
                ShowEigenbelege();
                MessageBox.Show("Eintrag gelöscht.");
            }
            else
            {
                return;
            }

        }

        private void btn_eigenbelegEdit_Click(object sender, EventArgs e)
        {
            if (lastSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus");
                return;
            }
            using (var form = new EigenbelegEdit())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ShowEigenbelege();
                }
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (UserFileManagement.ReturnCurrentUser() != "LennartMainPC")
            {
                MessageBox.Show("Du bist für diese Methode nicht autorisiert.");
                return;
            }
            int counter = 0;

            foreach (DataGridViewRow row in eigenbelegeDGV.Rows)
            {
                string eigenbelegNumber = row.Cells[1].Value.ToString();
                string sellerName = row.Cells[2].Value.ToString();
                string dateBought = row.Cells[5].Value.ToString();
                string transactionAmount = row.Cells[6].Value.ToString();
                string article = row.Cells[13].Value.ToString();
                string platform = row.Cells[8].Value.ToString();
                string paymentMethod = row.Cells[9].Value.ToString();
                string sellerAddress = row.Cells[10].Value.ToString();
                string isPdFCreated = row.Cells[11].Value.ToString();
                string isPricePostProcessed = row.Cells[15].Value.ToString();
                if (isPdFCreated != "Ja" || isPricePostProcessed == "Yes")
                {
                    if (platform == "/")
                    {
                        var storno = new pdfDocumentStorno();
                        storno.CreateDocument(eigenbelegNumber, sellerName, dateBought, transactionAmount, article, platform, paymentMethod, sellerAddress);
                    }
                    else
                    {
                        var pdf = new pdfDocument();
                        pdf.CreateDocument(eigenbelegNumber, sellerName, dateBought, transactionAmount, article, platform, paymentMethod, sellerAddress);
                    }
                    string query = "UPDATE `Eigenbelege` SET `Erstellt?` = 'Ja' WHERE `Eigenbelegnummer` = '" + eigenbelegNumber.ToString() + "'";
                    var dbManager = new DBManager();
                    dbManager.ExecuteQuery(query);
                    counter++;
                }
            }
            MessageBox.Show($"Es wurden erfolgreich {counter.ToString()} PDF-Dokumente wurden erfolgreich erstellt.");
            ShowEigenbelege();
        }

        private void eigenbelegeDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        private void btn_SelectAllRows_Click(object sender, EventArgs e)
        {
            if (eigenbelegeDGV.AreAllCellsSelected(true) == true)
            {
                eigenbelegeDGV.ClearSelection();
                return;
            }
            eigenbelegeDGV.SelectAll();
        }

        private void btn_DeleteAll_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM `Eigenbelege`";
            ExecuteQuery(query);
            ShowEigenbelege();
        }

        public void btn_PushToRep_Click(object sender, EventArgs e)
        {
            int test = eigenbelegeDGV.SelectedRows.Count;
            if (eigenbelegeDGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Bitte wähle zuerst mind. einen Eintrag aus.");
                return;
            }
            PushToRep();
            MessageBox.Show("Die Reparatur wurde erfolgreich erfasst.");
        }

        public void PushToRep()
        {
            //Übernahme des Defekts in REP Eintrag
            try
            {
                var posTrenn3 = transactionText.IndexOf("trenn3");
                var posAnsonsten = transactionText.IndexOf("ansonsten");
                defect = transactionText.Substring(posTrenn3 + 7, posAnsonsten - posTrenn3 - 8);
            }
            catch (Exception)
            {

            }
            var dbManager = new DBManager();
            int intern = dbManager.ExecuteQueryWithResult("Config", "Nummer", "Typ", "InterneNummer") + 1; ;
            dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = '" + intern + "' WHERE `Typ` = 'InterneNummer'");
            string query = string.Format("INSERT INTO `Reparaturen`(`Intern`,`Kaufdatum`,`Geraet`,`Kaufbetrag`,`Speicher`,`Defekt`,`Reparaturstatus`,`Quelle`,`EBReferenz`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')"
            , intern.ToString(), dateBought, model, transactionAmount, storage, defect, "Entgegengenommen", platform, eigenbelegNumber);
            ExecuteQuery(query);
            Reparaturen reparaturen = new Reparaturen();
            reparaturen.Show();
            this.Hide();
            reparaturen.SelectReparaturFromEigenbelege(eigenbelegNumber);
        }

        public void PushToRepWorkflowMethod()
        {
            //Übernahme des Defekts in REP Eintrag
            try
            {
                var posTrenn3 = transactionText.IndexOf("trenn3");
                var posAnsonsten = transactionText.IndexOf("ansonsten");
                defect = transactionText.Substring(posTrenn3 + 7, posAnsonsten - posTrenn3 - 8);
            }
            catch (Exception)
            {

            }
            var dbManager = new DBManager();
            int intern = dbManager.ExecuteQueryWithResult("Config", "Nummer", "Typ", "InterneNummer") + 1; ;
            dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = '" + intern + "' WHERE `Typ` = 'InterneNummer'");
            string query = string.Format("INSERT INTO `Reparaturen`(`Intern`,`Kaufdatum`,`Geraet`,`Kaufbetrag`,`Speicher`,`Defekt`,`Reparaturstatus`,`Quelle`,`EBReferenz`, `BuyBackGrade`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')"
            , intern.ToString(), dateBought, model, transactionAmount, storage, defect, "Entgegengenommen", platform, eigenbelegNumber, buybackGrade);
            ExecuteQuery(query);
            Reparaturen reparaturen = new Reparaturen();
            reparaturen.Show();
            this.Hide();
            reparaturen.SelectReparaturFromEigenbelegeWorkflowMethod(eigenbelegNumber);
        }


        private void button5_Click_1(object sender, EventArgs e)
        {
            ShowEigenbelege();
        }

        public void btn_SwitchToRelatedReparatur_Click(object sender, EventArgs e)
        {
            if (lastSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle zuerst einen Eintrag aus");
                return;
            }
            Reparaturen reparaturen = new Reparaturen();
            reparaturen.Show();
            this.Hide();
            reparaturen.SelectReparaturFromEigenbelege(eigenbelegNumber);
        }

        public void SelectEigenbelegeFromReparatur(string relatedNumber)
        {
            int rowIndex = -1;
            foreach (DataGridViewRow row in eigenbelegeDGV.Rows)
            {
                if (row.Cells[1].Value.ToString().Equals(relatedNumber))
                {
                    rowIndex = row.Index;
                }
            }
            if (rowIndex != -1)
            {
                eigenbelegeDGV.ClearSelection();
                eigenbelegeDGV.Rows[rowIndex].Selected = true;
                //bad workaroung, weil selection nicht erkannt wird als Klick
                eigenbelegNumber = eigenbelegeDGV.Rows[rowIndex].Cells[1].Value.ToString();
                sellerName = eigenbelegeDGV.Rows[rowIndex].Cells[2].Value.ToString();
                reference = eigenbelegeDGV.Rows[rowIndex].Cells[3].Value.ToString();
                model = eigenbelegeDGV.Rows[rowIndex].Cells[4].Value.ToString();
                dateBought = eigenbelegeDGV.Rows[rowIndex].Cells[5].Value.ToString();
                transactionAmount = eigenbelegeDGV.Rows[rowIndex].Cells[6].Value.ToString();
                mail = eigenbelegeDGV.Rows[rowIndex].Cells[7].Value.ToString();
                platform = eigenbelegeDGV.Rows[rowIndex].Cells[8].Value.ToString();
                paymentMethod = eigenbelegeDGV.Rows[rowIndex].Cells[9].Value.ToString();
                address = eigenbelegeDGV.Rows[rowIndex].Cells[10].Value.ToString();
                created = eigenbelegeDGV.Rows[rowIndex].Cells[11].Value.ToString();
                arrived = eigenbelegeDGV.Rows[rowIndex].Cells[12].Value.ToString();
                transactionText = eigenbelegeDGV.Rows[rowIndex].Cells[13].Value.ToString();
                storage = eigenbelegeDGV.Rows[rowIndex].Cells[14].Value.ToString();
                lastSelectedProductKey = (int)eigenbelegeDGV.Rows[rowIndex].Cells[0].Value;

                using (var form = new EigenbelegEdit())
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        ShowEigenbelege();
                    }
                }
            }
            else
            {
                MessageBox.Show("Es konnte kein Eintrag gefunden werden.");
            }
        }


        private void eigenbelegeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reparaturen window = new Reparaturen();
            window.Show();
            this.Hide();
        }


        private void sucheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new SearchAlgorithm())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    //Collecting all rows with the matching term
                    string searchTerm = form.searchTerm.ToLower();
                    int cellCounter = eigenbelegeDGV.Rows[0].Cells.Count - 1;
                    int arrayIndexCounter = 0;
                    string[] matchingRows = new string[100];
                    foreach (DataGridViewRow row in eigenbelegeDGV.Rows)
                    {
                        var pos = row.Index;
                        for (int i = 1; i <= cellCounter; i++)
                        {
                            if (row.Cells[i].Value.ToString().ToLower().Contains(searchTerm))
                            {
                                if (SearchAlgorithm.CheckIfExists(pos, matchingRows) == false)
                                {
                                    matchingRows[arrayIndexCounter] = pos.ToString();
                                }
                            }
                        }
                        //Changing the visibility
                        if (matchingRows.Contains(pos.ToString()))
                        {
                            row.Visible = true;
                        }
                        else
                        {
                            if (pos == 0)
                            {
                                eigenbelegeDGV.CurrentCell = null;
                                row.Visible = false;
                            }
                            else
                            {
                                row.Visible = false;
                            }
                        }
                    }
                }
            }
        }

       
        private bool checkIfSelected()
        {
            if (lastSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle zuerst einen Eintrag aus");
                return false;
            }
            else
            {
                return true;
            }
        }
        private void btn_PrintLabelForSellOff_Click(object sender, EventArgs e)
        {
            string currentUser = UserFileManagement.ReturnCurrentUser();
            if (checkIfSelected() == false)
            {
                return;
            }
            using (var form = new EigenbelegeLabelSellOffInput())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    try
                    {
                        try
                        {
                            var dbManager = new DBManager();
                            path = dbManager.ExecuteQueryWithResultString("ConfigUser", "TemplateSellOff", "Nutzer", currentUser);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Bitte setze in den Einstellungen dein Template fest; Fehlermeldung:" + ex.Message);
                        }

                        bpac.Document doc = new bpac.Document();
                        doc.Open(path);
                        doc.SetPrinter("Brother QL-600", true);
                        string barcodeContent = form.imei;
                        var indexBarcode = doc.GetBarcodeIndex("barcode");
                        doc.SetBarcodeData(indexBarcode, barcodeContent);
                        doc.GetObject("number").Text = eigenbelegNumber;
                        doc.GetObject("model").Text = model;
                        doc.GetObject("storage").Text = storage;
                        doc.GetObject("condition").Text = "Zustand: " + form.condition;
                        doc.StartPrint("", bpac.PrintOptionConstants.bpoDefault);
                        doc.PrintOut(form.quantity, bpac.PrintOptionConstants.bpoDefault);
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

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

      

        public void btn_workflowArrivalOfGoods_Click(object sender, EventArgs e)
        {
            EigenbelegCreate window = new EigenbelegCreate();
            ReparaturenEdit repEdit = new ReparaturenEdit();
            window.Show();
            using (var form = new EigenbelegCreateInputBuyBack())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    BackMarketAPIHandler backMarketAPIHandler = new BackMarketAPIHandler();
                    string[] feedback = backMarketAPIHandler.PullBuyBackDataFromOrderID(form.shippingNumber);
                    window.textBox_eigenbelegSellerName.Text = feedback[2];
                    window.textBox_eigenbelegTransactionAmount.Text = window.CalculateBackMarketAmount(feedback[4]);
                    window.comboBox_eigenbelegeCreateDevice.Text = feedback[0];
                    window.comboBox_eigenbelegStorage.Text = feedback[1];
                    window.textBox_eigenbelegAdress.Text = feedback[3];
                    window.textBox_eigenbelegReference.Text = feedback[5];
                    window.comboBox_eigenbelegPlatform.Text = "BackMarket";
                    window.comboBox_eigenbelegPaymentMethod.Text = "BuyBack / Lastschrift";
                    DialogResult resultBox = MessageBox.Show("Zustand Allgemein: " + feedback[6] + "\r\n\r\n" + feedback[7]);
                    buybackGrade = feedback[6];
                    // only proceed if there is feedback
                    if (feedback[0] == "/")
                    {
                        return;
                    }
                    if (resultBox == DialogResult.OK)
                    {
                        window.SaveNewEntryToDatabase();
                        ShowEigenbelege();
                        eigenbelegeDGV.ClearSelection();
                        eigenbelegeDGV.Rows[0].Selected = true;
                        AssignValuesToVirtualClick();
                        PushToRepWorkflowMethod();
                    }
                }
            }
        }

       
        public void AssignValuesToVirtualClick()
        {
            foreach (DataGridViewRow row in eigenbelegeDGV.SelectedRows)
            {
                eigenbelegNumber = eigenbelegeDGV.Rows[row.Index].Cells[1].Value.ToString();
                sellerName = eigenbelegeDGV.Rows[row.Index].Cells[2].Value.ToString();
                reference = eigenbelegeDGV.Rows[row.Index].Cells[3].Value.ToString();
                model = eigenbelegeDGV.Rows[row.Index].Cells[4].Value.ToString();
                dateBought = eigenbelegeDGV.Rows[row.Index].Cells[5].Value.ToString();
                transactionAmount = eigenbelegeDGV.Rows[row.Index].Cells[6].Value.ToString();
                mail = eigenbelegeDGV.Rows[row.Index].Cells[7].Value.ToString();
                platform = eigenbelegeDGV.Rows[row.Index].Cells[8].Value.ToString();
                paymentMethod = eigenbelegeDGV.Rows[row.Index].Cells[9].Value.ToString();
                address = eigenbelegeDGV.Rows[row.Index].Cells[10].Value.ToString();
                created = eigenbelegeDGV.Rows[row.Index].Cells[11].Value.ToString();
                arrived = eigenbelegeDGV.Rows[row.Index].Cells[12].Value.ToString();
                transactionText = eigenbelegeDGV.Rows[row.Index].Cells[13].Value.ToString();
                storage = eigenbelegeDGV.Rows[row.Index].Cells[14].Value.ToString();
                lastSelectedProductKey = (int)eigenbelegeDGV.Rows[row.Index].Cells[0].Value;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings win = new Settings();
            win.Show();
        }
    }
}
