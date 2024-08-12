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
    public partial class EigenbelegCreateInputBuyBack : Form
    {
        public string shippingNumber = "";
        public EigenbelegCreateInputBuyBack()
        {
            InitializeComponent();
            this.ActiveControl = textBox_order;
        }

        private void btn_Execute_Click(object sender, EventArgs e)
        {
            shippingNumber = AdaptShippingNumber(textBox_order.Text);
            DialogResult = DialogResult.OK;
            this.Close();
        }
        private string AdaptShippingNumber(string input)
        {
            // case: dhl express
            if (input.Substring(0,2) == "JJ")
            {
                var length = input.Length;
                return input.Substring(1,length-1);
            }
            else if (input.Length > 20)
            {
                // dpd for DE
                return input.Substring(8, 14);
            }
            else
            {
                // france
                return input.Substring(8, 14);
            }
        }
        private void OnKeyDownHandlerAndEnter(object sender, KeyEventArgs kea)
        {
            if (kea.KeyCode.Equals(Keys.Return))
            {
                SendKeys.Send("{TAB}");
                SendKeys.Send("{ENTER}");
            }
        }

        private void EigenbelegCreateInputBuyBack_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            shippingNumber = textBox_order.Text;
            if (shippingNumber.Contains("%"))
            {
                shippingNumber = AdaptShippingNumber(shippingNumber);
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
