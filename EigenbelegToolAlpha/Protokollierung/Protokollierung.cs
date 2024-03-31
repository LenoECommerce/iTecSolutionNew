using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class Protokollierung : Form
    {
        public static MySqlConnection conn;
        public string orderID = "";
        public string imei = "";
        public string internalNumber = "";
        public string marketplace = "";
        public string month = "";
        public string year = "";
        public string scanDate = "";
        public string newOrderIDValue = "";
        public static string salesVolumeOrderline = "";
        public int lastSelectedEntry;
        public string[] finishedOrdersToday = new string[0];
        public string[] finishedOrdersIMEI = new string[0];
        //multiple orderline part
        public static string[] orderlines = new string[0];
        public static int amountOfOrders = 0;
        public static int counter = 1;
        public static string orderIDMainMultipleOrders = "";
        public Protokollierung()
        {
            InitializeComponent();
            ShowProtokollierung();
        }

        public void ShowProtokollierung()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = DBManager.connString;
            conn.Open();

            string query = "SELECT * FROM `Protokollierung`";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            ////Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            //Daten anzeigen im Grid
            protokollierungDGV.DataSource = dataSet.Tables[0];

            protokollierungDGV.Columns[0].Visible = false;

            //Sortierte Ansicht
            protokollierungDGV.Sort(protokollierungDGV.Columns[0], ListSortDirection.Descending);
            conn.Close();
        }

        private void protokollierungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Eigenbelege window = new Eigenbelege();
            window.Show();
            this.Hide();

        }

        private void eigenbelegeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reparaturen window = new Reparaturen();
            window.Show();
            this.Hide();
        }
        public void ProtokollierungCreate()
        {
            if (textBox_IMEI.Text == "" ||
               textBox_OrderID.Text == "" ||
               comboBox_Marketplace.Text == "")
            {
                MessageBox.Show("Bitte füll alle Felder aus.");
                return;
            }
            orderID = textBox_OrderID.Text;
            imei = textBox_IMEI.Text;
            marketplace = comboBox_Marketplace.Text;
            scanDate = textBox_ScanDate.Text;


            string query = string.Format("INSERT INTO `Protokollierung`(`Bestellnummer`,`IMEI`,`Marktplatz`,`Scandatum`) VALUES ('{0}','{1}','{2}','{3}')"
           , orderID, imei, marketplace, scanDate);
            var dbManager = new DBManager();
            dbManager.ExecuteQuery(query);
            ShowProtokollierung();
        }
        public void btn_reparaturenCreate_Click(object sender, EventArgs e)
        {
            ProtokollierungCreate();
        }

        private void textBox_OrderID_TextChanged(object sender, EventArgs e)
        {
            comboBox_Marketplace.Text = CheckMarketplace(textBox_OrderID.Text);
        }

        private string CheckMarketplace(string checkValue)
        {
            if (checkValue.Contains("-"))
            {
                return "Ebay";
            }
            else
            {
                return "BackMarket";
            }
        }

       
        private void protokollierungDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox_OrderID.Text = protokollierungDGV.SelectedRows[0].Cells[1].Value.ToString();
            textBox_IMEI.Text = protokollierungDGV.SelectedRows[0].Cells[2].Value.ToString();
            comboBox_Marketplace.Text = protokollierungDGV.SelectedRows[0].Cells[3].Value.ToString();
            textBox_ScanDate.Text = protokollierungDGV.SelectedRows[0].Cells[4].Value.ToString();
            lastSelectedEntry = (int)protokollierungDGV.SelectedRows[0].Cells[0].Value;
        }

        private void btn_protokollierungDelete_Click(object sender, EventArgs e)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus");
                return;
            }
            DBManager window = new DBManager();
            window.deleteEntry(lastSelectedEntry, "Protokollierung");
            ShowProtokollierung();
        }

        private void Protokollierung_Load(object sender, EventArgs e)
        {
            textBox_ScanDate.Text = DateTime.Now.ToShortDateString();
        }

        private void btn_protokollierungEdit_Click(object sender, EventArgs e)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle zuerst einen Eintrag aus");
                return;
            }
            orderID = textBox_OrderID.Text;
            imei = textBox_IMEI.Text;
            marketplace = comboBox_Marketplace.Text;
            scanDate = textBox_ScanDate.Text;

            try
            {
                string query = string.Format("UPDATE `Protokollierung` SET `Bestellnummer` = '{0}',`IMEI` = '{1}', `Marktplatz` = '{2}', `Scandatum` = '{3}' WHERE `Id` = {4}"
                , orderID, imei, marketplace, scanDate, lastSelectedEntry);
                var dbManager = new DBManager();
                dbManager.ExecuteQuery(query);
                MessageBox.Show("Deine Änderungen wurden erfolgreich gespeichert.");
                ShowProtokollierung();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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
                    int cellCounter = protokollierungDGV.Rows[0].Cells.Count - 1;
                    int arrayIndexCounter = 0;
                    string[] matchingRows = new string[100];
                    foreach (DataGridViewRow row in protokollierungDGV.Rows)
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
                                protokollierungDGV.CurrentCell = null;
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
        public void ClearAllFields()
        {
            textBox_IMEI.Text = "";
            textBox_OrderID.Text = "";
            comboBox_Marketplace.Text = "";
        }
      

        private void btn_PushDataToAPIs_Click(object sender, EventArgs e)
        {
            // updating the backmarket data
            BackMarketAPIHandler backMarketAPIHandler = new BackMarketAPIHandler();
            backMarketAPIHandler.AfterSalesDataPush(finishedOrdersToday, finishedOrdersIMEI);
            // then adding the imei to the invoice and change the status
            BillBeeAPIHandler.UpdateAllOrders(finishedOrdersToday, finishedOrdersIMEI);
            MessageBox.Show("Erfolgreich ausgeführt");
        }

        private void textBox_IMEI_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
