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
    public partial class MultipleOrderlinesOrderIDInput : Form
    {
        public string orderID = "";
        public MultipleOrderlinesOrderIDInput()
        {
            InitializeComponent();
        }

        private void MultipleOrderlinesOrderIDInput_Load(object sender, EventArgs e)
        {

        }

        private void btn_Execute_Click(object sender, EventArgs e)
        {
            orderID = textBox1.Text;
            if (orderID.Contains("&"))
            {
                orderID = orderID.Replace("&","-");
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
