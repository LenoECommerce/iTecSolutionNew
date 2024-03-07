using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class EvaluationHistory : Form
    {
        public string currentYear = "2023";
        public string currentType = "Monthly";
        public int lastSelectedEntry;
        public static MySqlConnection conn;
        public string connString = DBManager.connString;
        public EvaluationHistory()
        {
            InitializeComponent();
            DataGridViewDistinction();
        }

        public void ShowMonthlyEvaluations()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connString;
            conn.Open();

            string query = "SELECT * FROM `Evaluations`";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            ////Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            //Daten anzeigen im Grid
            evaluationsDGV.DataSource = dataSet.Tables[0];

            int[] columnsToShow = new int[] { 1, 2, 3 };
            int maxColumn = 33;
            for (int i = 0; i <= maxColumn; i++)
            {
                if (!columnsToShow.Contains(i))
                {
                    evaluationsDGV.Columns[i].Visible = false;
                }
            }
            //Sortierte Ansicht
            evaluationsDGV.Sort(evaluationsDGV.Columns[1], ListSortDirection.Descending);

            foreach (DataGridViewRow row in evaluationsDGV.Rows)
            {
                var pos = row.Index;
                if (row.Cells[2].Value.ToString() != currentYear)
                {
                    if (pos == 0)
                    {
                        evaluationsDGV.CurrentCell = null;
                        row.Visible = false;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }

            conn.Close();
        }
        public void ShowQuarterlyEvaluations()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connString;
            conn.Open();

            string query = "SELECT * FROM `EvaluationsQuarterlyReports`";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            ////Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            //Daten anzeigen im Grid
            evaluationsDGV.DataSource = dataSet.Tables[0];

            //Sortierte Ansicht
            evaluationsDGV.Sort(evaluationsDGV.Columns[1], ListSortDirection.Descending);

            foreach (DataGridViewRow row in evaluationsDGV.Rows)
            {
                var pos = row.Index;
                if (row.Cells[2].Value.ToString() != currentYear)
                {
                    if (pos == 0)
                    {
                        evaluationsDGV.CurrentCell = null;
                        row.Visible = false;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }

            conn.Close();
        }
        public void DataGridViewDistinction()
        {
            if (currentType == "Monthly")
            {
                ShowMonthlyEvaluations();
            }
            else if (currentType == "Quarterly")
            {
                ShowQuarterlyEvaluations();
            }
        }
        private void EvaluationHistory_Load(object sender, EventArgs e)
        {

        }

        private void combobox_SelectedYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentYear = combobox_SelectedYear.SelectedItem.ToString();
            DataGridViewDistinction();
        }

        private void btn_DownloadReport_Click(object sender, EventArgs e)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle einen Eintrag aus.");
                return;
            }
            var dbManager = new DBManager();
            var result = dbManager.ExecuteQueryWithResultString("Evaluations", "Link", "Id", lastSelectedEntry.ToString());
            Clipboard.SetText(result);
            MessageBox.Show("Wurde erfolgreich kopiert.");
        }

        private void evaluationsDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lastSelectedEntry = (int)evaluationsDGV.SelectedRows[0].Cells[0].Value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentType = comboBox_type.Text;
            DataGridViewDistinction();
        }
    }
}
