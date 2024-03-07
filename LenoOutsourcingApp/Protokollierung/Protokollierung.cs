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
        public static string connString = "SERVER=212.227.140.237;PORT=3306;Initial Catalog='sql11525524';username=somedude;password=!mRAqn.Pl83.X8H0CuB28;Pooling=True;";
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
            conn.ConnectionString = connString;
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
               textBox_InternalNumber.Text == "" ||
               textBox_OrderID.Text == "" ||
               comboBox_Marketplace.Text == "")
            {
                MessageBox.Show("Bitte füll alle Felder aus.");
                return;
            }
            orderID = textBox_OrderID.Text;
            imei = textBox_IMEI.Text;
            internalNumber = textBox_InternalNumber.Text;
            marketplace = comboBox_Marketplace.Text;
            month = comboBox_Months.Text;
            scanDate = textBox_ScanDate.Text;
            year = comboBox_year.Text;


            string query = string.Format("INSERT INTO `Protokollierung`(`Bestellnummer`,`IMEI`,`Intern`,`Marktplatz`,`Monat`,`Jahr`,`Scandatum`,`SalesVolumeOrderLine`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')"
           , orderID, imei, internalNumber, marketplace, month, year, scanDate, salesVolumeOrderline);
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

        public void button1_Click(object sender, EventArgs e)
        {
            OpenScanWindowProcess();
        }
        public void OpenScanWindowProcess()
        {
            try
            {
                string orderIDValue = "";
                using (var form = new ProtokollierungScan())
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        orderIDValue = form.orderIDInput;
                        // case distinction display or device
                        if (form.scanInput.Contains("DIS"))
                        {
                            textBox_IMEI.Text = form.scanInput;
                            textBox_InternalNumber.Text = form.scanInput;
                        }
                        else
                        {
                            ReadAndSetInternalNumber(form.scanInput);
                        }
                        var imeiTemp = textBox_IMEI.Text;
                        // marketplace distinction
                        if (orderIDValue != "")
                        {
                            if (orderIDValue.Contains("ß"))
                            {
                                string first = orderIDValue.Substring(0, 2);
                                string second = orderIDValue.Substring(3, 5);
                                string third = orderIDValue.Substring(9, 5);
                                textBox_OrderID.Text = first + "-" + second + "-" + third;

                            }
                            else
                            {
                                textBox_OrderID.Text = orderIDValue;
                            }
                        }
                        string[] newArray = new string[] { orderIDValue };
                        finishedOrdersToday = finishedOrdersToday.Concat(newArray).ToArray();
                        string[] newArray2 = new string[] { imeiTemp };
                        finishedOrdersIMEI = finishedOrdersIMEI.Concat(newArray2).ToArray();
                    }
                    comboBox_Marketplace.Text = CheckMarketplace(textBox_OrderID.Text);
                }
                CreateDataBaseEntryOpenScanAgain(orderIDValue);
                ClearAllFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CreateDataBaseEntryOpenScanAgain(string orderId)
        {
            if (orderId != "")
            {
                ProtokollierungCreate();
                Thread.Sleep(1000);
                OpenScanWindowProcess();
            }
        }
        public void ReadAndSetInternalNumber(string giveBackValue)
        {
            var length = giveBackValue.ToString().Length;
            string imeiTemp = giveBackValue.ToString().Substring(6, length - 6);
            textBox_IMEI.Text = imeiTemp;
            var internalNumber = giveBackValue.ToString().Substring(0, 5);
            for (int i = 0; i < 5; i++)
            {
                if (internalNumber.Substring(i, 1) != "0")
                {
                    internalNumber = internalNumber.Substring(i, 5 - i);
                    break;
                }
            }
            textBox_InternalNumber.Text = internalNumber;
        }
        private void protokollierungDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox_OrderID.Text = protokollierungDGV.SelectedRows[0].Cells[1].Value.ToString();
            textBox_IMEI.Text = protokollierungDGV.SelectedRows[0].Cells[2].Value.ToString();
            textBox_InternalNumber.Text = protokollierungDGV.SelectedRows[0].Cells[3].Value.ToString();
            comboBox_Marketplace.Text = protokollierungDGV.SelectedRows[0].Cells[4].Value.ToString();
            comboBox_Months.Text = protokollierungDGV.SelectedRows[0].Cells[5].Value.ToString();
            textBox_ScanDate.Text = protokollierungDGV.SelectedRows[0].Cells[7].Value.ToString();
            comboBox_year.Text = protokollierungDGV.SelectedRows[0].Cells[6].Value.ToString();
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
            textBox_ScanDate.Text = DateTime.Now.ToString().Substring(0, 10);
        }

        private void btn_protokollierungEdit_Click(object sender, EventArgs e)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus");
                return;
            }
            orderID = textBox_OrderID.Text;
            imei = textBox_IMEI.Text;
            internalNumber = textBox_InternalNumber.Text;
            marketplace = comboBox_Marketplace.Text;
            month = comboBox_Months.Text;
            scanDate = textBox_ScanDate.Text;
            year = comboBox_year.Text;

            try
            {
                string query = string.Format("UPDATE `Protokollierung` SET `Bestellnummer` = '{0}',`IMEI` = '{1}', `Intern` = '{2}', `Marktplatz` = '{3}', `Monat` = '{4}', `Scandatum` = '{5}', `Jahr` = '{6}' WHERE `Id` = {7}"
                , orderID, imei, internalNumber, marketplace, month, scanDate, year, lastSelectedEntry);
                MessageBox.Show("Deine Änderungen wurden erfolgreich gespeichert.");
                var dbManager = new DBManager();
                dbManager.ExecuteQuery(query);
                ShowProtokollierung();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
            Microsoft.Office.Interop.Excel.Range xlRange;

            int xlrow = 2;
            string strFileName;
            //Excel Datei aussuchen
            openFD.ShowDialog();
            strFileName = openFD.FileName;

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkbook = xlApp.Workbooks.Open(strFileName);
            xlWorksheet = xlWorkbook.Worksheets["Tabelle1"];
            xlRange = xlWorksheet.UsedRange;

            //row 2 weil erst da die daten anfange
            var dbManager = new DBManager();
            for (xlrow = 2; xlrow <= xlRange.Rows.Count; xlrow++)
            {

                string orderID = xlRange.Cells[xlrow, 1].Text;
                string imei = xlRange.Cells[xlrow, 2].Text;
                string internalNumber = xlRange.Cells[xlrow, 3].Text;
                string marketplace = xlRange.Cells[xlrow, 4].Text;
                string month = xlRange.Cells[xlrow, 5].Text;

                //in Datenbank einfügen

                string query = string.Format("INSERT INTO `Protokollierung` (`Bestellnummer`, `IMEI`,`Intern`,`Marktplatz`,`Monat`) VALUES ('{0}','{1}','{2}','{3}','{4}')"
                            , orderID, imei, internalNumber, marketplace, month);
                dbManager.ExecuteQuery(query);

            }

            xlWorkbook.Close();
            xlApp.Quit();

            ShowProtokollierung();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM `Protokollierung`";
            var dbManager = new DBManager();
            dbManager.ExecuteQuery(query);
            ShowProtokollierung();
        }


        private void btn_BulkEditor_Click(object sender, EventArgs e)
        {
            using (var form = new ProtokollierungBulkEditor())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var newYear = form.selectedYear;
                    var newMonth = form.selectedMonth;
                    var dbManager = new DBManager();
                    foreach (DataGridViewRow row in protokollierungDGV.SelectedRows)
                    {
                        string tempValue = row.Cells[0].Value.ToString();
                        int getId = Convert.ToInt32(tempValue);
                        string query = $"UPDATE `Protokollierung` SET `Jahr` = '{newYear}', `Monat` = '{newMonth}' WHERE `Id` = " + getId;
                        dbManager.ExecuteQuery(query);
                    }
                }
            }
            ShowProtokollierung();
        }

        private void proofingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Proofing window = new Proofing();
            window.Show();
            this.Hide();
        }

        private void auswertungToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void serviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Service window = new Service();
            window.Show();
            this.Hide();
        }
        public void ClearAllFields()
        {
            textBox_IMEI.Text = "";
            textBox_InternalNumber.Text = "";
            textBox_OrderID.Text = "";
            comboBox_Marketplace.Text = "";
            comboBox_Months.Text = "";
        }
        private void externenVerkaufHinzufügenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new ProtokollierungAddExternalSell())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    textBox_IMEI.Text = form.imei;
                    textBox_InternalNumber.Text = form.internalNumber;
                    textBox_OrderID.Text = form.orderNumberReplacement;
                    comboBox_Months.Text = form.month;
                    comboBox_Marketplace.Text = form.reference;
                    comboBox_year.Text = form.year;
                    textBox_ScanDate.Text = "/";
                    ProtokollierungCreate();
                    ClearAllFields();
                }
            }
        }
        public async Task AssignMonthToDataSetAsync(string year, string month)
        {
            string rowOrderID = "";
            try
            {
                List<string> relevantRowIds = new List<string>();
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("@Year", year));
                parameters.Add(new MySqlParameter("@Month", month));
                foreach (DataGridViewRow row in protokollierungDGV.Rows)
                {
                    string rowId = row.Cells[0].Value.ToString();
                    string rowYear = row.Cells[6].Value.ToString();
                    string rowMarketPlace = row.Cells[4].Value.ToString();
                    rowOrderID = row.Cells[1].Value.ToString();


                    // condition: if year is already assigned skip
                    if (string.IsNullOrEmpty(rowYear))
                    {
                        bool isRelevant = await ScriptAssignMonthToDataSet.CheckIfOrderIsRelevantAsync(rowOrderID, rowMarketPlace);
                        if (isRelevant)
                        {
                            relevantRowIds.Add(rowId);
                        }
                    }
                }

                if (relevantRowIds.Count > 0)
                {
                    string updateQuery = "UPDATE `Protokollierung` SET `Jahr` = @Year, `Monat` = @Month WHERE `Id` IN (" + string.Join(",", relevantRowIds) + ")";
                    var dbManager = new DBManager();
                    await dbManager.ExecuteQueryAsync(updateQuery, parameters.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Die Monatszuordnung hat ein Problem mit folgendem Datensatz: {rowOrderID} {ex.Message}");
            }
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

    }
}
