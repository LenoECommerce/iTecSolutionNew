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
    public partial class BuyBackAcceptedReportInputcs : Form
    {
        public static string[] _acceptedOrders = new string[0];
        public BuyBackAcceptedReportInputcs()
        {
            InitializeComponent();
        }

        private void btn_Exectute_Click(object sender, EventArgs e)
        {
            string repNumber = ReadInternalNumberFromScan(textBox1_Input.Text);
            Main(repNumber);
            textBox1_Input.Text = "";
            textBox1_Input.Focus();
        }
        public static string ReadInternalNumberFromScan(string input)
        {
            var length = input.ToString().Length;
            var internalNumber = input.ToString().Substring(0, 5);
            for (int i = 0; i < 5; i++)
            {
                if (internalNumber.Substring(i, 1) != "0")
                {
                    internalNumber = internalNumber.Substring(i, 5 - i);
                    break;
                }
            }
            return internalNumber;
        }
        public static void Main(string input)
        {
            bool matched = false;
            foreach (var item in CounterOfferUpdating.acceptedOrders)
            {
                if (item == input)
                {
                    matched = true;
                }
            }
            if (matched == true)
            {
                MessageBox.Show("Freigeschaltet");
            }
            else
            {
                MessageBox.Show("Nicht freigeschaltet");
            }
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs kea)
        {
            if (kea.KeyCode.Equals(Keys.Return))
            {
                SendKeys.Send("{TAB}");
                SendKeys.Send("{ENTER}");
            }
        }
        private void BuyBackAcceptedReportInputcs_Load(object sender, EventArgs e)
        {
            textBox1_Input.Focus();
        }
    }
}
