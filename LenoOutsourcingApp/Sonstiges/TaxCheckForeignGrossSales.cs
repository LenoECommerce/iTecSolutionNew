using System;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class TaxCheckForeignGrossSales : Form
    {
        public TaxCheckForeignGrossSales()
        {
            InitializeComponent();
        }

        private void btn_Analyze_Click(object sender, EventArgs e)
        {
            openFD.ShowDialog();
            BillBeeTaxCheck.Analyse(openFD.FileName);
            string querySub1 = "UPDATE `Config` SET `Nummer` = '" + DateTime.Now.ToString() + "' WHERE `Typ` = 'LastTaxForeignGrossSales';";
            string querySub2 = "UPDATE `Config` SET `Nummer` = '" + BillBeeTaxCheck.tresholdAmount.ToString() + "' WHERE `Typ` = 'TaxForEignAmount';";
            string queryMain = querySub1 + "\r\n" + querySub2;
            var dbManager = new DBManager();
            dbManager.ExecuteQuery(queryMain);
            TaxCheckForeignGrossSales_Load(sender, e);
        }

        private void TaxCheckForeignGrossSales_Load(object sender, EventArgs e)
        {
            string table = "Config";
            string searchColum = "Nummer";
            var dbManager = new DBManager();
            lbl_lastDateUpdated.Text = dbManager.ExecuteQueryWithResultString(table, searchColum, "Typ", "LastTaxForeignGrossSales");
            lbl_amount.Text = dbManager.ExecuteQueryWithResultString(table, searchColum, "Typ", "TaxForEignAmount") + "€";
        }
    }
}
