using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class Matching : Form
    {
        public Matching()
        {
            InitializeComponent();
            ShowMatching();
        }
        public static MySqlConnection conn;
        public static string connString = "SERVER=212.227.140.237;PORT=3306;Initial Catalog='sql11525524';username=somedude;password=!mRAqn.Pl83.X8H0CuB28;Pooling=True;";
        public void ShowMatching()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connString;
            conn.Open();

            string query = "SELECT * FROM `Matching`";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            ////Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            //Daten anzeigen im Grid
            matchingDGV.DataSource = dataSet.Tables[0];
            //Column verstecken
            matchingDGV.Columns[0].Visible = false;
            conn.Close();
        }


        private void Matching_Load(object sender, EventArgs e)
        {

        }
    }
}
