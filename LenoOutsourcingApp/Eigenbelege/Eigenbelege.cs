using EigenbelegToolAlpha.BackMarket;
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
        public static string connString = "SERVER=212.227.140.237;PORT=3306;Initial Catalog='sql11525524';username=somedude;password=!mRAqn.Pl83.X8H0CuB28;Pooling=True;";

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
            SetValuesOfMenuStrip();
            CheckIfThereIsAnEntryLonger10Days();
            var dbManager = new DBManager();
            string lastPayPalImport = dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "LastPayPalImport").ToString();
            lbl_LastPayPalImport.Text = "Letzter PayPal-Import: " + lastPayPalImport;
            string lastBuyBackSync = dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "LastBuyBackSync").ToString();
            lbl_LastBuyBackSync.Text = "Letzter BuyBackSync: " + lastBuyBackSync;
        }
        public void SetValuesOfMenuStrip()
        {
            var dbManager = new DBManager();
            string input = dbManager.ExecuteQueryWithResultString("ConfigUser", "ArrangementPreferences", "Nutzer", currentUser) + "Fensterwechsel;Suche;Filter;Auswertungen";
            string[] parts = input.Split(';');
            foreach (ToolStripItem item in prefferedWindowsStrip.DropDownItems)
            {
                if (parts.Contains(item.Text))
                {
                    item.Visible = true;
                }
                else
                {
                    item.Visible = false;
                }
            }
        }

        private void CheckIfThereIsAnEntryLonger10Days()
        {
            var dbManager = new DBManager();
            string[] entriesMatched = new string[10];
            DateTime today = DateTime.Now;
            int arraycounter = 0;
            string numbersAsString = "";
            DateTime lastCheck = Convert.ToDateTime(dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "LastNotArrivedCheck"));
            if (today.Subtract(lastCheck).TotalDays < 1)
            {
                return;
            }
            foreach (DataGridViewRow row in eigenbelegeDGV.Rows)
            {
                DateTime checkDate = Convert.ToDateTime(row.Cells[5].Value);
                string arrived = row.Cells[12].Value.ToString();
                if (today.Subtract(checkDate).TotalDays >= 10 && arrived != "Ja")
                {
                    entriesMatched[arraycounter] = row.Cells[1].Value.ToString();
                    arraycounter++;
                }
            }
            foreach (var item in entriesMatched)
            {
                if (item != "")
                {
                    numbersAsString = string.Join(",", entriesMatched.Select(n => n));
                }
            }
            if (numbersAsString == ",,,,,,,,,")
            {
                return;
            }
            else
            {
                string message = "Folgende Aufträge sind betroffen: \r\n\r\n" + numbersAsString;
                MessageBox.Show(message);
            }
            dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = ' " + today + "' WHERE `Typ` = 'LastNotArrivedCheck'");
        }

        private void dataImport()
        {
            openFD.ShowDialog();
            string strFileName = openFD.FileName;
            EigenbelegePayPalImport algorithm = new EigenbelegePayPalImport();
            algorithm.MainAlgorithm(strFileName);
            ShowEigenbelege();
            string newLastDataImport = DateTime.Now.ToString();
            var dbManager = new DBManager();
            dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = '" + newLastDataImport + "' WHERE `Typ` = 'LastPayPalImport'");
            lbl_LastPayPalImport.Text = "Letzer Datenimport: " + newLastDataImport;
        }


        public void ShowEigenbelege()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connString;
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
            conn.ConnectionString = connString;
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
                conn.ConnectionString = connString;
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

        private void PayPalDataImport()
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            dataImport();
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
            var dbManager = new DBManager();
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (eigenbelegeDGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Bitte wähle zuerst mind. einen Eintrag aus.");
                return;
            }

            if (File.ReadAllText("config.txt") == "")
            {
                MessageBox.Show("Du hast noch keinen Speicherpfad angegeben.");
                return;
            }



            for (int i = 0; i < eigenbelegeDGV.SelectedRows.Count; i++)
            {
                string eigenbelegNumber = eigenbelegeDGV.SelectedRows[i].Cells[1].Value.ToString();
                string sellerName = eigenbelegeDGV.SelectedRows[i].Cells[2].Value.ToString();
                string dateBought = eigenbelegeDGV.SelectedRows[i].Cells[3].Value.ToString();
                string transactionAmount = eigenbelegeDGV.SelectedRows[i].Cells[4].Value.ToString();
                string article = eigenbelegeDGV.SelectedRows[i].Cells[5].Value.ToString();
                string platform = eigenbelegeDGV.SelectedRows[i].Cells[6].Value.ToString();
                string paymentMethod = eigenbelegeDGV.SelectedRows[i].Cells[7].Value.ToString();
                string sellerAddress = eigenbelegeDGV.SelectedRows[i].Cells[8].Value.ToString();
                var storno = new pdfDocumentStorno();
                storno.CreateDocument(eigenbelegNumber, sellerName, dateBought, transactionAmount, article, platform, paymentMethod, sellerAddress);
            }

            MessageBox.Show("Dein Storno-Eigenbeleg wurde erfolgreich erstellt.");
        }



        private void button2_Click_3(object sender, EventArgs e)
        {
            Reparaturen reparaturen = new Reparaturen();
            reparaturen.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PayPalDataImport();
        }

        private void button5_Click(object sender, EventArgs e)
        {

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

        private void btn_settings2_Click(object sender, EventArgs e)
        {
            using (var form = new Settings())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    SetValuesOfMenuStrip();
                }
            }
        }


        private void eigenbelegeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reparaturen window = new Reparaturen();
            window.Show();
            this.Hide();
        }

        private void protokollierungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Protokollierung window = new Protokollierung();
            window.Show();
            this.Hide();
        }

        private void btn_folderInstaCreate_Click(object sender, EventArgs e)
        {
            FolderInstaCreate window = new FolderInstaCreate();
            window.Show();
        }

        private void btn_buybackPriceAdaptions_Click(object sender, EventArgs e)
        {
            openFD.ShowDialog();
            string strFileName = openFD.FileName;
            string newLastDataImport = BuyBackPostPriceAdaption.Main(strFileName);
            lbl_LastBuyBackSync.Text = "Letzer Datenimport: " + newLastDataImport;
            ShowEigenbelege();
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new EigenbelegFilter())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ShowEigenbelegeFiltered(form.selectedFilterModell, form.selectedFilterCreated, form.selectedFilterPlatform);
                }
            }
        }

        private void proofingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Proofing window = new Proofing();
            window.Show();
            this.Hide();
        }

        private void auswertungenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EvaluationChoice window = new EvaluationChoice();
            window.Show();
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

        private void serviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Service window = new Service();
            window.Show();
            this.Hide();
        }

        private void b2CAnkaufToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServiceB2CAnkauf_ProActive window = new ServiceB2CAnkauf_ProActive();
            window.Show();
            this.Hide();
        }

        private void b2BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            B2B window = new B2B();
            window.Show();
            this.Hide();
        }

        private void lieferscheineMergeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void etikettenMergeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btn_RegularSalesAlgo_Click(object sender, EventArgs e)
        {

        }

        private void regularSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegularSalesAlgoOverview window = new RegularSalesAlgoOverview();
            window.Show();
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

        private void rEGCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxCheckForeignGrossSales window = new TaxCheckForeignGrossSales();
            window.Show();
        }

        private void iMEIErkennungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IMEIRecognizer iMEIRecognizer = new IMEIRecognizer();
            iMEIRecognizer.Main();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }



        private void buyBackBidsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuyBackBidsOurCompany window = new BuyBackBidsOurCompany();
            window.Show();
            this.Hide();
        }



        private void displaySellOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplaySellOfInputs window = new DisplaySellOfInputs();
            window.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void btn_weeklyVolumeWindow_Click(object sender, EventArgs e)
        {
            BuyBackWeeklyVolumes window = new BuyBackWeeklyVolumes();
            window.Show();
            this.Hide();
        }



        private void regularSalesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RegularSalesAlgoOverview window = new RegularSalesAlgoOverview();
            window.Show();
        }

        private void rEGCheckToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TaxCheckForeignGrossSales window = new TaxCheckForeignGrossSales();
            window.Show();
        }

        private void displaySellOffToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DisplaySellOfInputs window = new DisplaySellOfInputs();
            window.Show();
        }

        private void buyBackBidsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BuyBackBidsOurCompany window = new BuyBackBidsOurCompany();
            window.Show();
            this.Hide();
        }

        private void lexofficeSkriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //HelpFormShowCurrentMousePosition window = new HelpFormShowCurrentMousePosition();
            //window.Show();
            LexOfficeBookkeepingQuestionnaire window = new LexOfficeBookkeepingQuestionnaire();
            window.Show();
        }

        private void buyBackWeeklyVolumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuyBackWeeklyVolumes window = new BuyBackWeeklyVolumes();
            window.Show();
            this.Hide();
        }

        private void b2BToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            B2B window = new B2B();
            window.Show();
            this.Hide();
        }

        private void b2CAnkaufToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ServiceB2CAnkauf_ProActive window = new ServiceB2CAnkauf_ProActive();
            window.Show();
            this.Hide();
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

        private void ersatzteilpreiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SparePartsFrontend window = new SparePartsFrontend();
            window.Show();
            this.Hide();
        }

        private void listingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListingFrontend listingFrontend = new ListingFrontend();
            listingFrontend.Show();
            this.Hide();
        }

        private void btn_ExeCuteWorkflowShippingProcess_Click(object sender, EventArgs e)
        {
            UIWorkflowShippingProcess window = new UIWorkflowShippingProcess();
            window.Show();
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            BackMarketAPIHandler.SetFreeLatestOrders();
        }


    }
}
