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
    public partial class SparePartsPrintQuantityInput : Form
    {
        public int quantity = 0;
        public SparePartsPrintQuantityInput()
        {
            InitializeComponent();
        }

        private void btn_IndividualOK_Click(object sender, EventArgs e)
        {
            quantity = Convert.ToInt32(numericUpDown1.Value);
            SetDialogOKCloseWindow();
        }
        private void SetDialogOKCloseWindow()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btn_5OK_Click(object sender, EventArgs e)
        {
            quantity = 5;
            SetDialogOKCloseWindow();
        }

        private void btn_10OK_Click(object sender, EventArgs e)
        {
            quantity = 10;
            SetDialogOKCloseWindow();
        }

        private void btn_15OK_Click(object sender, EventArgs e)
        {
            quantity = 15;
            SetDialogOKCloseWindow();
        }

        private void btn_20OK_Click(object sender, EventArgs e)
        {
            quantity = 20;
            SetDialogOKCloseWindow();
        }
    }
}
