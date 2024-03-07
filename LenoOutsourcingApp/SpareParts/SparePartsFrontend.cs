using EigenbelegToolAlpha.ConstsAndEnums;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class SparePartsFrontend : Form
    {
        public static MySqlConnection conn;
        public static string connString = "SERVER=212.227.140.237;PORT=3306;Initial Catalog='sql11525524';username=somedude;password=!mRAqn.Pl83.X8H0CuB28;Pooling=True;";
        public int lastSelectedEntry;
        public string model = "";
        public string supplier = "";
        public string sparePart = "";
        public string price = "";
        public string taxation = "";
        public string comment = "";
        public SparePartsFrontend()
        {
            InitializeComponent();
        }

        private void SparePartsFrontend_Load(object sender, EventArgs e)
        {
            LoadSupplierCombobox();
            LoadSparePartsCombobox();
            LoadModelCombobox();
            ShowDGV();
        }
        public void ShowDGV()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connString;
            conn.Open();

            string query = "SELECT * FROM `SpareParts` WHERE 1=1";

            if (!string.IsNullOrEmpty(comboBox_filterSupplier.Text))
            {
                query += " AND Supplier = '" + comboBox_filterSupplier.Text + "'";
            }

            if (!string.IsNullOrEmpty(comboBox_filterDevice.Text))
            {
                query += " AND Model = '" + comboBox_filterDevice.Text + "'";
            }

            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            // Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);

            // Daten anzeigen im Grid
            sparepartsDGV.DataSource = dataSet.Tables[0];

            // Sortierte Ansicht
            sparepartsDGV.Sort(sparepartsDGV.Columns[0], ListSortDirection.Descending);
            conn.Close();
        }


        private void LoadModelCombobox()
        {
            foreach (var item in ProductModelCatalog.iPhoneModels)
            {
                comboBox_device.Items.Add(item.ToString());
                comboBox_filterDevice.Items.Add(item.ToString());
            }
        }
        public void LoadSupplierCombobox()
        {
            foreach (var item in SupplierConsts.supplierNames)
            {
                comboBox_filterSupplier.Items.Add(item.ToString());
                comboBox_supplier.Items.Add(item.ToString());
            }
        }
        public void LoadSparePartsCombobox()
        {
            foreach (var item in SparePartsTypesConsts.sparePartTypes)
            {
                comboBox_part.Items.Add(item.ToString());
            }
        }

        private void zurückZuFensterEigenbelegeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Eigenbelege window = new Eigenbelege();
            window.Show();
            this.Hide();
        }
        public void SparePartCreate()
        {
            if (textBox_price.Text == "" ||
               comboBox_taxation.Text == "" ||
               comboBox_part.Text == "" ||
               comboBox_supplier.Text == "" ||
               comboBox_device.Text == "")
            {
                MessageBox.Show("Bitte füll alle Felder aus.");
                return;
            }
            supplier = comboBox_supplier.Text;
            sparePart = comboBox_part.Text;
            price = textBox_price.Text;
            taxation = comboBox_taxation.Text;
            comment = textBox_comment.Text;
            model = comboBox_device.Text;
            string[] attributes = { "Supplier", "Part", "PriceBr", "Taxation", "Comment", "Model" };
            string[] values = { supplier, sparePart, price, taxation, comment, model };
            var dbManager = new DBManager();
            dbManager.CreateEntry("SpareParts", attributes, values);
            ShowDGV();
        }
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle zuerst einen Eintrag aus");
                return;
            }
            DBManager window = new DBManager();
            window.deleteEntry(lastSelectedEntry, "SpareParts");
            ShowDGV();
        }

        private void sparepartsDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lastSelectedEntry = (int)sparepartsDGV.SelectedRows[0].Cells[0].Value;
            supplier = sparepartsDGV.SelectedRows[0].Cells[1].Value.ToString();
            sparePart = sparepartsDGV.SelectedRows[0].Cells[2].Value.ToString();
            price = sparepartsDGV.SelectedRows[0].Cells[3].Value.ToString();
            taxation = sparepartsDGV.SelectedRows[0].Cells[4].Value.ToString();
            comment = sparepartsDGV.SelectedRows[0].Cells[5].Value.ToString();
            model = sparepartsDGV.SelectedRows[0].Cells[6].Value.ToString();
            comboBox_supplier.Text = supplier;
            comboBox_part.Text = sparePart;
            textBox_price.Text = price;
            comboBox_taxation.Text = taxation;
            textBox_comment.Text = comment;
            comboBox_device.Text = model;
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            SparePartCreate();
        }

        private void sparepartsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus");
                return;
            }
            supplier = comboBox_supplier.Text;
            sparePart = comboBox_part.Text;
            price = textBox_price.Text;
            taxation = comboBox_taxation.Text;
            comment = textBox_comment.Text;
            model = comboBox_device.Text;

            try
            {
                string[] attributes = { "Supplier", "Part", "PriceBr", "Taxation", "Comment", "Model" };
                string[] values = { supplier, sparePart, price, taxation, comment, model };
                var dbManager = new DBManager();
                dbManager.UpdateSpecificEntry("SpareParts", attributes, values, lastSelectedEntry);
                ShowDGV();
                MessageBox.Show("Eintrag erfolgreich bearbeitet");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            int selectedRows = sparepartsDGV.SelectedRows.Count;
            if (selectedRows == 0)
            {
                MessageBox.Show("Bitte erst einen Eintrag auswählen.");
                return;
            }
            using (var form = new SparePartsPrintQuantityInput())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    int quantity = form.quantity;
                    LabelSparePartStorage.Print(model, sparePart, supplier, lastSelectedEntry.ToString(), quantity);
                }
            }
        }

        private void comboBox_filterSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowDGV();
        }

        private void comboBox_filterDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowDGV();
        }

        private void btn_resetFilters_Click(object sender, EventArgs e)
        {
            comboBox_filterDevice.SelectedItem = null;
            comboBox_filterSupplier.SelectedItem = null;
            ShowDGV();
        }
    }
}
