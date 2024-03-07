using System;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class EvaluationRunningCostsEditEntry : Form
    {
        public EvaluationRunningCostsEditEntry()
        {
            InitializeComponent();
        }

        private void EvaluationRunningCostsEditEntry_Load(object sender, EventArgs e)
        {
            string table = "EvaluationsCurrentCosts";
            string equalColum = "Id";
            var dbManager = new DBManager();
            string equalValue = EvaluationRunningCosts.lastSelectedEntry.ToString();
            textBox_Amount.Text = dbManager.ExecuteQueryWithResultString(table, "Betrag", equalColum, equalValue);
            textBox_invoiceProvider.Text = dbManager.ExecuteQueryWithResultString(table, "Rechnungssteller", equalColum, equalValue);
            comboBox_taxdeduction.Text = dbManager.ExecuteQueryWithResultString(table, "Vorsteuerabzug", equalColum, equalValue);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string amount = textBox_Amount.Text;
            string invoiceProvider = textBox_invoiceProvider.Text;
            string taxDeduction = comboBox_taxdeduction.Text;
            string query = string.Format("UPDATE `EvaluationsCurrentCosts` SET `Rechnungssteller` = '{0}', `Betrag` = '{1}', `Vorsteuerabzug` = '{2}' WHERE `Id` = '{3}'",
                            invoiceProvider, amount, taxDeduction, EvaluationRunningCosts.lastSelectedEntry.ToString());
            var dbManager = new DBManager();
            dbManager.ExecuteQuery(query);
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
