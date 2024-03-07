using System;
using System.Linq;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class ReparturenEditSparePartsDetails : Form
    {
        public double costsWithTaxDeduction = 0;
        public double costsWithoutTaxDecution = 0;
        public int lastSelectedEntry = 0;
        public string sparePart = "";
        public ReparturenEditSparePartsDetails()
        {
            InitializeComponent();
            ShowUsedSpareParts();
        }

        private void ReparturenEditSparePartsDetails_Load(object sender, EventArgs e)
        {

        }
        private void ShowUsedSpareParts()
        {
            string usedSparePartsData = Reparaturen.usedSpareParts; // Replace with your actual static string value

            // Clear existing rows and columns in the DataGridView
            usedSparePartsDGV.Rows.Clear();
            usedSparePartsDGV.Columns.Clear();

            string[] usedSparePartsArray = usedSparePartsData.Split(';'); // Split the string into an array of values

            // Add column headings to the DataGridView
            usedSparePartsDGV.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Navy;
            usedSparePartsDGV.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            usedSparePartsDGV.Columns.Add("Id", "Id");
            usedSparePartsDGV.Columns.Add("Supplier", "Supplier");
            usedSparePartsDGV.Columns.Add("Part", "Part");
            usedSparePartsDGV.Columns.Add("PriceBr", "PriceBr");
            usedSparePartsDGV.Columns.Add("Taxation", "Taxation");
            usedSparePartsDGV.Columns.Add("Comment", "Comment");

            var dbManager = new DBManager();
            foreach (string value in usedSparePartsArray)
            {
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }
                if (int.TryParse(value, out _))
                {
                    string supplier = dbManager.ExecuteQueryWithResultString("SpareParts", "Supplier", "Id", value);
                    string part = dbManager.ExecuteQueryWithResultString("SpareParts", "Part", "Id", value);
                    string priceBr = dbManager.ExecuteQueryWithResultString("SpareParts", "PriceBr", "Id", value);
                    string taxation = dbManager.ExecuteQueryWithResultString("SpareParts", "Taxation", "Id", value);
                    string comment = dbManager.ExecuteQueryWithResultString("SpareParts", "Comment", "Id", value);
                    string id = value;
                    // Add a new row with the retrieved data to the DataGridView
                    usedSparePartsDGV.Rows.Add(value, supplier, part, priceBr, taxation, comment);
                }
                else
                {
                    // Add a new row with the value in the "Part" column and "/" in the other columns
                    usedSparePartsDGV.Rows.Add("/", "/", value, "/", "/", "/");
                }
            }
            usedSparePartsDGV.Columns[0].Visible = false;
        }

        private void btn_saveChanges_Click(object sender, EventArgs e)
        {
            CalculateCosts();
            SaveValuesToDB();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void CalculateCosts()
        {
            foreach (DataGridViewRow row in usedSparePartsDGV.Rows)
            {
                double newCosts = 0;
                string taxation = row.Cells[4].Value.ToString();
                string newCostsString = row.Cells[3].Value.ToString();
                string normalVatTerm = "Regelbesteuerung";
                string marginalVatTerm = "Differenzbesteuerung";
                // check if value is stable
                if (double.TryParse(newCostsString, out _))
                {
                    newCosts = Convert.ToDouble(newCostsString);
                }
                // actual calculation
                if (taxation == normalVatTerm)
                {
                    costsWithTaxDeduction += newCosts;
                }
                else if (taxation == marginalVatTerm)
                {
                    costsWithoutTaxDecution += newCosts;
                }
            }
        }
        private void SaveValuesToDB()
        {
            string newValue = "";
            string[] arrayValuesToSave = { };
            foreach (DataGridViewRow row in usedSparePartsDGV.Rows)
            {
                string id = row.Cells[0].Value.ToString();
                string part = row.Cells[2].Value.ToString();
                // if part is not from spare part db
                if (id == "/")
                {
                    newValue = part;
                }
                else
                {
                    newValue = id;
                }
                // add to array
                string[] newArray = { newValue };
                arrayValuesToSave = arrayValuesToSave.Concat(newArray).ToArray();
            }
            // form all values in the array to a normal string with delimiters
            for (int i = 0; i < arrayValuesToSave.Length; i++)
            {
                if (i == 0)
                {
                    newValue = arrayValuesToSave[i];
                }
                else
                {
                    newValue = newValue + ";" + arrayValuesToSave[i];
                }
            }
            Reparaturen.usedSpareParts = newValue;
        }

        private void usedSparePartsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void btn_usedSparePartsDelete_Click(object sender, EventArgs e)
        {
            if (usedSparePartsDGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Bitte zuerst einen Eintrag auswählen.");
                return;
            }
            // ausgewählte reihe löschen
            usedSparePartsDGV.Rows.RemoveAt(usedSparePartsDGV.SelectedRows[0].Index);
        }
    }
}
