using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class Reparaturen : Form
    {
        //SQL Verbindung zu Datenbank
        public static MySqlConnection conn;
        public static string connString = "SERVER=212.227.140.237;PORT=3306;Initial Catalog='sql11525524';username=somedude;password=!mRAqn.Pl83.X8H0CuB28;Pooling=True;";
        public static int lastSelectedProductKey;
        public static string internalNumber = "";
        public static string dateBought = "";
        public static string device = "";
        public static string make = "";
        public static string storage = "";
        public static string defect = "";
        public static string transactionAmount = "";
        public static string imei = "";
        public static string externalCosts = "";
        public static string comment = "";
        public static string source = "";
        public static string referenceToEB = "";
        public static string state = "";
        public static string usedSpareParts = "";
        public static string color = "";
        public static string taxes = "";
        public static string condition = "";
        public static string donorMonth = "";
        public static string fiveG = "";
        public static string externalCostsDiff = "";
        public static string donorYear = "";
        public static string selectedDefects = "";
        public static string commentRefurbisher = "";
        public static string batteryCapacity = "";
        public static string chargingCycles = "";
        public static string buybackGrade = "";

        public Reparaturen()
        {
            InitializeComponent();
        }

        public void ShowReparaturen()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connString;
            conn.Open();

            string query = "SELECT * FROM `Reparaturen`";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            ////Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            //Daten anzeigen im Grid
            reparaturenDGV.DataSource = dataSet.Tables[0];
            //Column verstecken
            reparaturenDGV.Columns[0].Visible = false;
            reparaturenDGV.Columns[12].Visible = false;
            reparaturenDGV.Columns[13].Visible = false;
            reparaturenDGV.Columns[14].Visible = false;
            reparaturenDGV.Columns[15].Visible = false;
            reparaturenDGV.Columns[16].Visible = false;
            reparaturenDGV.Columns[17].Visible = false;
            reparaturenDGV.Columns[18].Visible = false;
            reparaturenDGV.Columns[19].Visible = false;
            reparaturenDGV.Columns[20].Visible = false;
            reparaturenDGV.Columns[21].Visible = false;
            reparaturenDGV.Columns[23].Visible = false;
            reparaturenDGV.Columns[24].Visible = false;
            reparaturenDGV.Columns[25].Visible = false;
            reparaturenDGV.Columns[26].Visible = false;
            //width of columns
            reparaturenDGV.Columns[1].Width = 50;
            reparaturenDGV.Columns[2].Width = 100;
            reparaturenDGV.Columns[4].Width = 50;
            //Sortierte Ansicht
            reparaturenDGV.Sort(reparaturenDGV.Columns[1], ListSortDirection.Descending);
            conn.Close();
            LogicCheckForNumbers();
        }
        public void LogicCheckForNumbers()
        {
            var dbManager = new DBManager();
            int id = dbManager.ExecuteQueryWithResult("Reparaturen", "Id", "Intern", "");
            if (id != 0)
            {
                MessageBox.Show("Es besteht ein Logikfehler. Es gibt einen Reparatureintrag mit der Nummer 0. Bitte prüfen!");
            }
        }
        public void ShowReparaturenFiltered(string[] filterValueModel, string[] filterValueSource, string[] filterValueRepairState)
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connString;
            conn.Open();

            string query = "SELECT * FROM `Reparaturen`";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            ////Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            //Daten anzeigen im Grid
            reparaturenDGV.DataSource = dataSet.Tables[0];
            //Column verstecken
            reparaturenDGV.Columns[0].Visible = false;
            reparaturenDGV.Columns[23].Visible = false;
            reparaturenDGV.Columns[24].Visible = false;
            //Sortierte Ansicht
            reparaturenDGV.Sort(reparaturenDGV.Columns[1], ListSortDirection.Descending);
            //Filtern
            reparaturenDGV.CurrentCell = null;
            for (int i = 0; i < reparaturenDGV.RowCount; i++)
            {
                if (filterValueModel.Contains(reparaturenDGV.Rows[i].Cells[3].Value.ToString()) == true)
                {
                    if (filterValueSource.Contains(reparaturenDGV.Rows[i].Cells[19].Value.ToString()) == true)
                    {
                        if (filterValueRepairState.Contains(reparaturenDGV.Rows[i].Cells[18].Value.ToString()) == true)
                        {
                            reparaturenDGV.Rows[i].Visible = true;
                        }
                        else
                        {
                            reparaturenDGV.Rows[i].Visible = false;
                        }
                    }
                    else
                    {
                        reparaturenDGV.Rows[i].Visible = false;
                    }
                }
                else
                {
                    reparaturenDGV.Rows[i].Visible = false;
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

        public static double ExecuteQueryWithResult(string query)
        {
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = connString;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //zwischenspeichern und danach umformen um Fehlerquelle zu vermeiden
                var firstValueGetBack = cmd.ExecuteScalar();
                double result = Convert.ToDouble(firstValueGetBack);
                conn.Close();
                return result;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        public static string ExecuteQueryWithResultForManualDataImport(string query)
        {
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = connString;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //zwischenspeichern und danach umformen um Fehlerquelle zu vermeiden
                var firstValueGetBack = cmd.ExecuteScalar();
                var result = firstValueGetBack;
                conn.Close();
                if (result != null)
                {
                    return result.ToString();
                }
                return "";
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }


        public void SelectReparaturFromEigenbelege(string relatedNumber)
        {
            int rowIndex = -1;
            foreach (DataGridViewRow row in reparaturenDGV.Rows)
            {
                if (row.Cells[22].Value.Equals(relatedNumber))
                {
                    rowIndex = row.Index;
                }
            }
            if (rowIndex != -1)
            {
                reparaturenDGV.ClearSelection();
                reparaturenDGV.Rows[rowIndex].Selected = true;
                WorkroundMethodSelectEntry(rowIndex);
                using (var form = new ReparaturenEdit())
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        ShowReparaturen();
                    }
                }
            }

            else
            {
                MessageBox.Show("Es konnte kein Eintrag gefunden werden.");
            }

        }
        public void SelectReparaturFromEigenbelegeWorkflowMethod(string relatedNumber)
        {
            int rowIndex = -1;
            foreach (DataGridViewRow row in reparaturenDGV.Rows)
            {
                if (row.Cells[22].Value.Equals(relatedNumber))
                {
                    rowIndex = row.Index;
                }
            }
            if (rowIndex != -1)
            {
                HelpMethodSelectCurrentEntry(rowIndex);
            }

            else
            {
                MessageBox.Show("Es konnte kein Eintrag gefunden werden.");
            }
            // workflow part 2
            ReparaturenCreateImeiInput reparaturenCreateImeiInput = new ReparaturenCreateImeiInput();
            reparaturenCreateImeiInput.btn_Execute.Click += (sender, e) =>
            {
                reparaturenCreateImeiInput.Close();

                using (var form = new ReparaturenEdit())
                {
                    form.Load += (sender2, e2) =>
                    {
                        form.btn_nsysFillOut_Click(null, null);
                        form.btn_IMEICheck_Click(null, null);
                    };

                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        using (var labelPrintInput = new WorkflowMainLabelPrintInput())
                        {
                            labelPrintInput.btn_1.Click += (EventHandler)((sender3, e3) =>
                            {
                                labelPrintInput.DialogResult = DialogResult.OK;
                                labelPrintInput.Close();
                            });
                            labelPrintInput.btn_2.Click += (EventHandler)((sender3, e3) =>
                            {
                                labelPrintInput.DialogResult = DialogResult.OK;
                                labelPrintInput.Close();
                            });

                            labelPrintInput.ShowDialog();

                            if (labelPrintInput.DialogResult == DialogResult.OK)
                            {
                                HelpMethodSelectCurrentEntry(rowIndex);
                                BrotherPrintThisModell(WorkflowMainLabelPrintInput.amountLabels, 0);
                                this.Hide();
                                Eigenbelege eigenbelege = new Eigenbelege();
                                eigenbelege.Show();
                                eigenbelege.btn_workflowArrivalOfGoods_Click(null, null);
                            }
                        }
                    }
                }
            };
            reparaturenCreateImeiInput.Show();
        }
        private void WorkroundMethodSelectEntry(int rowIndex)
        {
            internalNumber = reparaturenDGV.Rows[rowIndex].Cells[1].Value.ToString();
            dateBought = reparaturenDGV.Rows[rowIndex].Cells[2].Value.ToString();
            device = reparaturenDGV.Rows[rowIndex].Cells[3].Value.ToString();
            transactionAmount = reparaturenDGV.Rows[rowIndex].Cells[4].Value.ToString();
            imei = reparaturenDGV.Rows[rowIndex].Cells[5].Value.ToString();
            make = reparaturenDGV.Rows[rowIndex].Cells[6].Value.ToString();
            color = reparaturenDGV.Rows[rowIndex].Cells[7].Value.ToString();
            storage = reparaturenDGV.Rows[rowIndex].Cells[8].Value.ToString();
            taxes = reparaturenDGV.Rows[rowIndex].Cells[9].Value.ToString();
            condition = reparaturenDGV.Rows[rowIndex].Cells[10].Value.ToString();
            defect = reparaturenDGV.Rows[rowIndex].Cells[11].Value.ToString();
            usedSpareParts = reparaturenDGV.Rows[rowIndex].Cells[12].Value.ToString();
            externalCosts = reparaturenDGV.Rows[rowIndex].Cells[13].Value.ToString();
            externalCostsDiff = reparaturenDGV.Rows[rowIndex].Cells[14].Value.ToString();
            comment = reparaturenDGV.Rows[rowIndex].Cells[15].Value.ToString();
            commentRefurbisher = reparaturenDGV.Rows[rowIndex].Cells[16].Value.ToString();
            batteryCapacity = reparaturenDGV.Rows[rowIndex].Cells[17].Value.ToString();
            state = reparaturenDGV.Rows[rowIndex].Cells[18].Value.ToString();
            source = reparaturenDGV.Rows[rowIndex].Cells[19].Value.ToString();
            buybackGrade = reparaturenDGV.Rows[rowIndex].Cells[20].Value.ToString();
            chargingCycles = reparaturenDGV.Rows[rowIndex].Cells[21].Value.ToString();
            referenceToEB = reparaturenDGV.Rows[rowIndex].Cells[22].Value.ToString();
            selectedDefects = reparaturenDGV.Rows[rowIndex].Cells[26].Value.ToString();
            fiveG = reparaturenDGV.Rows[rowIndex].Cells[24].Value.ToString();

            lastSelectedProductKey = (int)reparaturenDGV.Rows[rowIndex].Cells[0].Value;
        }
        public void HelpMethodSelectCurrentEntry(int rowIndex)
        {
            //lol
            reparaturenDGV.ClearSelection();
            ShowReparaturen();
            reparaturenDGV.Rows[rowIndex].Selected = true;
            WorkroundMethodSelectEntry(rowIndex);
        }

        private void reparaturenDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            internalNumber = reparaturenDGV.SelectedRows[0].Cells[1].Value.ToString();
            dateBought = reparaturenDGV.SelectedRows[0].Cells[2].Value.ToString();
            device = reparaturenDGV.SelectedRows[0].Cells[3].Value.ToString();
            transactionAmount = reparaturenDGV.SelectedRows[0].Cells[4].Value.ToString();
            imei = reparaturenDGV.SelectedRows[0].Cells[5].Value.ToString();
            make = reparaturenDGV.SelectedRows[0].Cells[6].Value.ToString();
            color = reparaturenDGV.SelectedRows[0].Cells[7].Value.ToString();
            storage = reparaturenDGV.SelectedRows[0].Cells[8].Value.ToString();
            taxes = reparaturenDGV.SelectedRows[0].Cells[9].Value.ToString();
            condition = reparaturenDGV.SelectedRows[0].Cells[10].Value.ToString();
            defect = reparaturenDGV.SelectedRows[0].Cells[11].Value.ToString();
            usedSpareParts = reparaturenDGV.SelectedRows[0].Cells[12].Value.ToString();
            externalCosts = reparaturenDGV.SelectedRows[0].Cells[13].Value.ToString();
            externalCostsDiff = reparaturenDGV.SelectedRows[0].Cells[14].Value.ToString();
            comment = reparaturenDGV.SelectedRows[0].Cells[15].Value.ToString();
            commentRefurbisher = reparaturenDGV.SelectedRows[0].Cells[16].Value.ToString();
            batteryCapacity = reparaturenDGV.SelectedRows[0].Cells[17].Value.ToString();
            state = reparaturenDGV.SelectedRows[0].Cells[18].Value.ToString();
            source = reparaturenDGV.SelectedRows[0].Cells[19].Value.ToString();
            buybackGrade = reparaturenDGV.SelectedRows[0].Cells[20].Value.ToString();
            chargingCycles = reparaturenDGV.SelectedRows[0].Cells[21].Value.ToString();
            referenceToEB = reparaturenDGV.SelectedRows[0].Cells[22].Value.ToString();
            donorMonth = reparaturenDGV.SelectedRows[0].Cells[23].Value.ToString();
            fiveG = reparaturenDGV.SelectedRows[0].Cells[24].Value.ToString();
            donorYear = reparaturenDGV.SelectedRows[0].Cells[25].Value.ToString();
            selectedDefects = reparaturenDGV.SelectedRows[0].Cells[26].Value.ToString();
            lastSelectedProductKey = (int)reparaturenDGV.SelectedRows[0].Cells[0].Value;

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



        private void btn_settings_Click(object sender, EventArgs e)
        {
            Eigenbelege eigenbelege = new Eigenbelege();
            eigenbelege.Show();
            this.Hide();
        }

        private void btn_reparaturenDelete_Click(object sender, EventArgs e)
        {
            if (checkIfSelected() == false)
            {
                return;
            }
            DialogResult result = MessageBox.Show("Eintrag sicher löschen?", "Title", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string query = string.Format("DELETE FROM `Reparaturen` WHERE `Id` = {0} ;", lastSelectedProductKey);
                ExecuteQuery(query);
                ShowReparaturen();
                MessageBox.Show("Eintrag gelöscht.");
            }
            else
            {
                return;
            }

        }



        private void btn_SelectAllRows_Click(object sender, EventArgs e)
        {
            if (reparaturenDGV.AreAllCellsSelected(true) == true)
            {
                reparaturenDGV.ClearSelection();
                return;
            }
            reparaturenDGV.SelectAll();
        }

        private void btn_DeleteAll_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM `Reparaturen`";
            ExecuteQuery(query);
            ShowReparaturen();
        }

        private void btn_reparaturenEdit_Click(object sender, EventArgs e)
        {
            if (checkIfSelected() == false)
            {
                return;
            }
            using (var form = new ReparaturenEdit())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ShowReparaturen();
                }
            }
        }

        private void Reparaturen_Load(object sender, EventArgs e)
        {
            ShowReparaturen();
            CounterOfferUpdating.MainOK();
        }

        private void reparaturenDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowReparaturen();
        }

        private void btn_reparaturenCreate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Momentan nicht verwendbar. Entwicklung ausstehend");
            return;
            using (var form = new ReparaturCreate())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ShowReparaturen();
                }
            }
        }


        private void btn_SwitchToRelatedEigenbeleg_Click(object sender, EventArgs e)
        {
            if (checkIfSelected() == false)
            {
                return;
            }
            Eigenbelege eigenbelege = new Eigenbelege();
            eigenbelege.Show();
            this.Hide();
            eigenbelege.SelectEigenbelegeFromReparatur(referenceToEB);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings window = new Settings();
            window.Show();
        }

        public void BrotherPrintThisModell(int quantityOfCopies, int valueBattery)
        {
            if (checkIfSelected() == false)
            {
                return;
            }

            try
            {
                string internPrefix = "";
                string barcodeSKU = "APL/10.1/B64C/DIFF";
                string path = "";
                try
                {
                    var dbManager = new DBManager();
                    path = dbManager.ExecuteQueryWithResultString("ConfigUser", "TemplateModell", "Nutzer", Settings.currentUser);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bitte setze in den Einstellungen dein Template fest; Fehlermeldung:" + ex.Message);
                }

                //Abfrage wie lang interne Nummer, dann prefix anpassen!
                int lengthIntern = internalNumber.Length;
                int freeDigits = 5 - lengthIntern;
                for (int i = 0; i < freeDigits; i++)
                {
                    internPrefix = internPrefix + "0";
                }

                //SKU Generator in andere Klasse auslagern!
                SKUGeneration newObject = new SKUGeneration();
                barcodeSKU = newObject.SKUGenerationMethod(make, device, color, condition, taxes, storage, fiveG, valueBattery);

                string barcodeIMEICombo = internPrefix + internalNumber + "/" + imei;


                bpac.Document doc = new bpac.Document();
                doc.Open(path);
                doc.SetPrinter("Brother QL-600", true);

                var temp = doc.GetBarcodeIndex("SKU");
                var temp2 = doc.GetBarcodeIndex("IMEICombo");
                doc.SetBarcodeData(temp, barcodeSKU);
                doc.SetBarcodeData(temp2, barcodeIMEICombo);

                doc.StartPrint("", bpac.PrintOptionConstants.bpoDefault);
                doc.PrintOut(quantityOfCopies, bpac.PrintOptionConstants.bpoDefault);
                doc.EndPrint();
                doc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Print Error: " + ex.Message);
            }
        }

        private void platinenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void hauptetikettToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrotherPrintThisModell(1, 0);
        }

        private void btn_WorkWithSpecificReparatur_Click(object sender, EventArgs e)
        {
            using (var form = new WorkWithSpecificRep())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    int rowIndex = -1;
                    foreach (DataGridViewRow row in reparaturenDGV.Rows)
                    {
                        if (row.Cells[5].Value.Equals(form.matchingValue))
                        {
                            rowIndex = row.Index;
                        }
                    }
                    if (rowIndex != -1)
                    {
                        reparaturenDGV.ClearSelection();
                        reparaturenDGV.Rows[rowIndex].Selected = true;
                        WorkroundMethodSelectEntry(rowIndex);
                        using (var form2 = new ReparaturenEdit())
                        {
                            var result2 = form2.ShowDialog();
                            if (result2 == DialogResult.OK)
                            {
                                ShowReparaturen();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Es konnte kein Eintrag gefunden werden.");
                    }
                }
            }
        }

        private void etikettenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void button4_Click(object sender, EventArgs e)
        {
            string query = "";
            ExecuteQuery(query);
        }

        private void eigenbelegeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Eigenbelege eigenbelege = new Eigenbelege();
            eigenbelege.Show();
            this.Hide();
        }

        private void protokollierungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Protokollierung window = new Protokollierung();
            window.Show();
            this.Hide();
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new ReparaturenFilter())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ShowReparaturenFiltered(form.selectedFilterModell, form.selectedFilterSource, form.selectedFilterRepairState);
                }
            }
        }

        private void xHauptetikettToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrotherPrintThisModell(2, 0);
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
                    int cellCounter = reparaturenDGV.Rows[0].Cells.Count - 1;
                    int arrayIndexCounter = 0;
                    string[] matchingRows = new string[100];
                    foreach (DataGridViewRow row in reparaturenDGV.Rows)
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
                                reparaturenDGV.CurrentCell = null;
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

        private void b2BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            B2B window = new B2B();
            window.Show();
            this.Close();
        }

        private void ebaySingleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EbaySingleListing window = new EbaySingleListing();
            window.Show();
        }

    }
}
