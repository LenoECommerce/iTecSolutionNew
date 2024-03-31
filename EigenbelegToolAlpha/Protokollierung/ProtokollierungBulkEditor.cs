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
    public partial class ProtokollierungBulkEditor : Form
    {
        public string selectedYear = "";
        public string selectedMonth = "";
        public ProtokollierungBulkEditor()
        {
            InitializeComponent();
        }

        private void btn_Execute_Click(object sender, EventArgs e)
        {
            selectedYear = comboBox_year.SelectedItem.ToString();
            selectedMonth = comboBox_monhts.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Hide();

        }
    }
}
