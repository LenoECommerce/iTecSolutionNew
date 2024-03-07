using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class ListingFrontend : Form
    {
        public static MySqlConnection conn;
        public static string connString = "SERVER=212.227.140.237;PORT=3306;Initial Catalog='sql11525524';username=somedude;password=!mRAqn.Pl83.X8H0CuB28;Pooling=True;";
        public int lastSelectedEntry;
        public string information = "";
        public string amount = "";
        public string sku = "";
        public string marketPlace = "";
        public string price = "";
        public string finished = "";
        public string finishedTime = "";
        public string[] modelsForEbay = new string[] {"APL-8GO64A", "APL-8B64A", "APL-8SI64A","APL-8GO64B","APL-8B64B","APL-8SI64B","APL-8RE64B",
                                                      "APL-2B","APL-2W", "APL2RE" };
        public ListingFrontend()
        {
            InitializeComponent();
        }

        private void zurückZuFensterEigenbelegeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Eigenbelege eigenbelege = new Eigenbelege();
            eigenbelege.Show();
            this.Hide();
        }

        private void ListingFrontend_Load(object sender, EventArgs e)
        {
            ShowDGV();
        }

        public void ShowDGV()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connString;
            conn.Open();

            string query = "SELECT * FROM `Listing`";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            ////Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            //Daten anzeigen im Grid
            listingsDGV.DataSource = dataSet.Tables[0];
            listingsDGV.Columns[0].Visible = false;
            listingsDGV.Columns[7].Visible = false;
            //Sortierte Ansicht
            listingsDGV.Sort(listingsDGV.Columns[4], ListSortDirection.Descending);
            //Filter
            listingsDGV.CurrentCell = null;
            for (int i = 0; i < listingsDGV.RowCount; i++)
            {
                string filter = comboBox_filterFinished.Text;
                if (listingsDGV.Rows[i].Cells[6].Value.ToString() != filter)
                {
                    listingsDGV.Rows[i].Visible = false;
                }
            }
            if (comboBox_filterFinished.Text == "Ja")
            {
                listingsDGV.Sort(listingsDGV.Columns[7], ListSortDirection.Descending);
                listingsDGV.Columns[7].Visible = true;
            }
            listingsDGV.Columns[1].Width = 300;

            conn.Close();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle zuerst einen Eintrag aus");
                return;
            }
            DBManager window = new DBManager();
            window.deleteEntry(lastSelectedEntry, "Listing");
            ShowDGV();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle zuerst einen Eintrag aus");
                return;
            }
            SetVariableValues();
            try
            {
                string[] attributes = new string[] { "Infos", "Anzahl", "SKU", "Plattform", "Preis", "Erledigt?" };
                string[] values = new string[] { information, amount, sku, marketPlace, price, finished };
                var dbManager = new DBManager();
                dbManager.UpdateSpecificEntry("Listing", attributes, values, lastSelectedEntry);
                MessageBox.Show("Deine Änderungen wurden erfolgreich gespeichert.");
                ShowDGV();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listingsDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lastSelectedEntry = (int)listingsDGV.SelectedRows[0].Cells[0].Value;
            textBox_information.Text = listingsDGV.SelectedRows[0].Cells[1].Value.ToString();
            comboBox_amount.Text = listingsDGV.SelectedRows[0].Cells[2].Value.ToString();
            textBox_SKU.Text = listingsDGV.SelectedRows[0].Cells[3].Value.ToString();
            comboBox_marketPlace.Text = listingsDGV.SelectedRows[0].Cells[4].Value.ToString();
            textBox_Price.Text = listingsDGV.SelectedRows[0].Cells[5].Value.ToString();
            comboBox_finished.Text = listingsDGV.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            CreateListing();
            ResetAllFields();

        }
        public void ResetAllFields()
        {
            comboBox_amount.Text = "1";
            comboBox_finished.Text = "Nein";
            comboBox_marketPlace.Text = "/";
            textBox_information.Text = "/";
            textBox_Price.Text = "/";
            textBox_SKU.Text = "/";
        }
        public void SetVariableValues()
        {
            information = textBox_information.Text;
            amount = comboBox_amount.Text;
            sku = textBox_SKU.Text;
            marketPlace = comboBox_marketPlace.Text;
            price = textBox_Price.Text;
            finished = comboBox_finished.Text;
        }
        public void CreateListing()
        {
            if (comboBox_amount.Text == "" ||
                textBox_SKU.Text == "" ||
               comboBox_marketPlace.Text == "" ||
               textBox_Price.Text == "" ||
               comboBox_finished.Text == "")
            {
                MessageBox.Show("Bitte füll alle Felder aus.");
                return;
            }
            SetVariableValues();

            string[] attributes = new string[] { "Infos", "Anzahl", "SKU", "Plattform", "Preis", "Erledigt?" };
            string[] values = new string[] { information, amount, sku, marketPlace, price, finished };
            var dbManager = new DBManager();
            dbManager.CreateEntry("Listing", attributes, values);
            ShowDGV();
        }

        private void comboBox_filterFinished_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowDGV();
        }

        private void btn_Scan_Click(object sender, EventArgs e)
        {
            ResetAllInputFields();
            OpenScanWindowProcess();
        }
        public void ResetAllInputFields()
        {
            textBox_information.Text = "/";
            textBox_Price.Text = "/";
            comboBox_amount.Text = "1";
            comboBox_marketPlace.Text = "/";
        }

        public void OpenScanWindowProcess()
        {
            using (var form = new ListingScanInput())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string scan = ListingScanInput.scan;
                    textBox_SKU.Text = scan;
                    comboBox_marketPlace.Text = MarketPlaceDecision(scan);
                    CheckIfListingExistsAndInteract(scan);
                    OpenScanWindowProcess();
                }
            }
        }
        public void CheckIfListingExistsAndInteract(string scan)
        {
            var dbManager = new DBManager();
            int id = Convert.ToInt32(dbManager.ExecuteQueryWithResultStringTwoConditions("Listing", "Id", "SKU", "Erledigt?", scan, "Nein"));
            string infos = dbManager.ExecuteQueryWithResultString("Listing", "Infos", "Id", id.ToString());
            if (id != 0 && infos == "/")
            {
                int currentAmount = dbManager.ExecuteQueryWithResult("Listing", "Anzahl", "Id", id.ToString());
                string[] attributes = new string[] { "Anzahl" };
                string[] values = new string[] { (currentAmount + 1).ToString() };
                dbManager.UpdateSpecificEntry("Listing", attributes, values, id);
                ShowDGV();
            }
            else
            {
                CreateListing();
            }
        }
        public string MarketPlaceDecision(string scan)
        {
            foreach (var item in modelsForEbay)
            {
                if (scan.Contains(item))
                {
                    return "Ebay";
                }
            }
            return "BackMarket";
        }

        private void btn_DoneSpecificListing_Click(object sender, EventArgs e)
        {
            string[] attributes = new string[] { "Erledigt?", "Zeitpunkt" };
            string[] values = new string[] { "Ja", DateTime.Now.ToString() };
            var dbManager = new DBManager();
            dbManager.UpdateSpecificEntry("Listing", attributes, values, lastSelectedEntry);
            ShowDGV();
        }
    }
}
