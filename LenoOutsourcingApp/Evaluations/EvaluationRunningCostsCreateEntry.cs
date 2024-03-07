using System;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class EvaluationRunningCostsCreateEntry : Form
    {
        public EvaluationRunningCostsCreateEntry()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string invoiceProvider = textBox_invoiceProvider.Text;
            string amount = textBox_Amount.Text;
            string taxdeduction = comboBox_taxdeduction.Text;

            string query = string.Format("INSERT INTO `EvaluationsCurrentCosts`(`Rechnungssteller`,`Betrag`,`Vorsteuerabzug`) VALUES ('{0}','{1}','{2}')",
                            invoiceProvider, amount, taxdeduction);
            var dbManager = new DBManager();
            dbManager.ExecuteQuery(query);
            MessageBox.Show("Dein Eintrag wurde erfolgreich erstellt.");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
