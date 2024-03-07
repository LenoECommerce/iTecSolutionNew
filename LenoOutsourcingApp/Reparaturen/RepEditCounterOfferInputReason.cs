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
    public partial class RepEditCounterOfferInputReason : Form
    {
        public string counterOfferReason = "";  
        public RepEditCounterOfferInputReason()
        {
            InitializeComponent();
        }

        private void btn_Execute_Click(object sender, EventArgs e)
        {
            counterOfferReason = textBox_inputReason.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
