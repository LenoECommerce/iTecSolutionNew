using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class BuyBackBidsOurCompany : Form
    {
        public static MySqlConnection conn;
        public static string connString = "SERVER=212.227.140.237;PORT=3306;Initial Catalog='sql11525524';username=somedude;password=!mRAqn.Pl83.X8H0CuB28;Pooling=True;";
        public string productID = "";
        public string grade = "";
        public string isInPortfolio = "";
        public int lastSelectedEntry;
        public BuyBackBidsOurCompany()
        {
            InitializeComponent();
            ShowBids();
        }

        private void BuyBackBidsOurCompany_Load(object sender, EventArgs e)
        {

        }
        public void ShowBids()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connString;
            conn.Open();
            string query = "SELECT * FROM `BuyBackBidsOurCompany`";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            ////Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            //Daten anzeigen im Grid
            buybackBidsOurCompanyDGV.DataSource = dataSet.Tables[0];
            //Column verstecken
            buybackBidsOurCompanyDGV.Columns[0].Visible = false;
            //Sortierte Ansicht
            buybackBidsOurCompanyDGV.Sort(buybackBidsOurCompanyDGV.Columns[1], ListSortDirection.Descending);

            conn.Close();
        }
        public void CreateBid()
        {
            if (comboBox_product.Text == "" ||
               comboBox_grades.Text == "" ||
               comboBox_IsInPortfolio.Text == "")
            {
                MessageBox.Show("Bitte füll alle Felder aus.");
                return;
            }
            productID = comboBox_product.Text;
            grade = comboBox_grades.Text;
            isInPortfolio = comboBox_IsInPortfolio.Text;

            string query = string.Format("INSERT INTO `BuyBackBidsOurCompany`(`ProductID`,`Grade`,`IsInCurrentPortfolio`) VALUES ('{0}','{1}','{2}')"
           , productID, grade, isInPortfolio);
            var dbManager = new DBManager();
            dbManager.ExecuteQuery(query);
            ShowBids();
        }
        private void btn_bidsCreate_Click(object sender, EventArgs e)
        {
            CreateBid();
        }

        private void buybackBidsOurCompanyDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            comboBox_product.Text = buybackBidsOurCompanyDGV.SelectedRows[0].Cells[1].Value.ToString();
            comboBox_grades.Text = buybackBidsOurCompanyDGV.SelectedRows[0].Cells[2].Value.ToString();
            comboBox_IsInPortfolio.Text = buybackBidsOurCompanyDGV.SelectedRows[0].Cells[3].Value.ToString();

            lastSelectedEntry = (int)buybackBidsOurCompanyDGV.SelectedRows[0].Cells[0].Value;
        }

        private void btn_bidsEdit_Click(object sender, EventArgs e)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus");
                return;
            }
            productID = comboBox_product.Text;
            grade = comboBox_grades.Text;
            isInPortfolio = comboBox_IsInPortfolio.Text;
            try
            {
                string query = string.Format("UPDATE `BuyBackBidsOurCompany` SET `ProductID` = '{0}',`Grade` = '{1}', `IsInCurrentPortfolio` = '{2}' WHERE `Id` = {3}"
                , productID, grade, isInPortfolio, lastSelectedEntry);
                MessageBox.Show("Deine Änderungen wurden erfolgreich gespeichert.");
                var dbManager = new DBManager();
                dbManager.ExecuteQuery(query);
                ShowBids();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_bidsRemove_Click(object sender, EventArgs e)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus");
                return;
            }
            DBManager window = new DBManager();
            window.deleteEntry(lastSelectedEntry, "BuyBackBidsOurCompany");
            ShowBids();
        }

        private void eigenbelegeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Eigenbelege eigenbelege = new Eigenbelege();
            eigenbelege.Show();
            this.Hide();
        }
    }
}
