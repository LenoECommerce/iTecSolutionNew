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
    public partial class ProtokollierungInputForAssigningMonth : Form
    {
        public ProtokollierungInputForAssigningMonth()
        {
            InitializeComponent();
        }
        public string month = "";
        public string year = "";

        private void btn_Execute_Click(object sender, EventArgs e)
        {
            month = comboBox_Months.Text;
            year = comboBox_year.Text;
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
    }
}
