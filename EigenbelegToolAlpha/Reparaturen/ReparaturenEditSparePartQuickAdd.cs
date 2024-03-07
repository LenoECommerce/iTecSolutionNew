using ExcelLibrary.BinaryDrawingFormat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class ReparaturenEditSparePartQuickAdd : Form
    {
        public double costs = 0;
        public string taxation = "";
        public string idPart = "";
        public ReparaturenEditSparePartQuickAdd()
        {
            InitializeComponent();
        }
        private void OnKeyDownHandlerAndEnter(object sender, KeyEventArgs kea)
        {
            if (kea.KeyCode.Equals(Keys.Return))
            {
                SendKeys.Send("{TAB}");
                SendKeys.Send("{ENTER}");
            }
        }
        private void ReparaturenEditSparePartQuickAdd_Load(object sender, EventArgs e)
        {
            textBox_input.Focus();
        }

        private void btn_Execute_Click(object sender, EventArgs e)
        {
            idPart = textBox_input.Text;
            var dbManager = new DBManager();
            int id = dbManager.ExecuteQueryWithResult("SpareParts","Id","Id", idPart);
            if (id == 0)
            {
                MessageBox.Show($"Es wurde kein Ersatzteil mit der Id {idPart} gefunden");
                return;
            }
            costs = Convert.ToDouble(dbManager.ExecuteQueryWithResultString("SpareParts","PriceBr","Id", idPart));
            taxation = dbManager.ExecuteQueryWithResultString("SpareParts", "Taxation", "Id", idPart);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
