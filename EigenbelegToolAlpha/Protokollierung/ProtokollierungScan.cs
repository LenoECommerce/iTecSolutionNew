using EigenbelegToolAlpha;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class ProtokollierungScan : Form
    {
        public string scanInput = "";
        public string orderIDInput = "";
        public bool stillInOrderLineProcess = false;
        public ProtokollierungScan()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckLengthIMEICombo(textBox_scanField.Text) == false)
            {
                // exclude display sells
                if (!textBox_scanField.Text.Contains("DIS"))
                {
                    MessageBox.Show("Die IMEI Combo weist einen Fehler auf.");
                    this.Close();
                    return;
                }
            }
            if (textBox_orderID.Text.Contains("ß"))
            {
                orderIDInput = AdjustOrderNumberEbayStyle(textBox_orderID.Text);
            }
            else
            {
                orderIDInput = textBox_orderID.Text;
            }
            // case check: if the person forgets that the order has multiple devices
            if (CheckIfMultipleOrder(orderIDInput) == true && !orderIDInput.Contains("."))
            {
                MessageBox.Show("Die Order enthält mehrere Geräte, also bitte den Knopf drücken.");
                this.Close();
                return;
            }
            if (IsOrderIDValid(orderIDInput) == false)
            {
                MessageBox.Show("Die eingegebene Order ID ist laut BillBee API Anfrage nicht gültig. Bitte überprüfen.");
                this.Close();
                return;
            }
            // orderline part for adding sales volume
            if (stillInOrderLineProcess == true)
            {
                using (var form = new MultipleOrderlinesSelectMatchingSale())
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        // code continues normally
                    }
                }
            }
            scanInput = textBox_scanField.Text;
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
        public bool CheckIfMultipleOrder(string orderID)
        {
            string[] feedback = BillBeeAPIHandler.GetOrderlinesFromMainOrder(orderID);
            if (feedback.Length >1)
            {
                return true;
            }
            return false;
        }
        public string AdjustOrderNumberEbayStyle (string input)
        {
            string first = input.Substring(0, 2);
            string second = input.Substring(3, 5);
            string third = input.Substring(9, 5);
            return first + "-" + second + "-" + third;
        }
        public bool CheckLengthIMEICombo(string inputValue)
        {
            if (inputValue.Length != 21)
            {
                return false;
            }
            return true;
        }

        public bool IsOrderIDValid(string inputValue)
        {
            if (inputValue.Contains("."))
            {
                var length = inputValue.Length;
                inputValue = inputValue.Substring(0,length-2);
            }
            string billBeeInternalOrderId = BillBeeAPIHandler.GetInternalBillBeeID(inputValue);
            if (string.IsNullOrEmpty(billBeeInternalOrderId))
            {
                return false;
            }
            return true;
        }
        private void textBox_scanField_TextChanged(object sender, EventArgs e)
        {

        }
        private void OnKeyDownHandler(object sender, KeyEventArgs kea)
        {
            if (kea.KeyCode.Equals(Keys.Return))
            {
                if (stillInOrderLineProcess == true)
                {
                    SendKeys.Send("{TAB}");
                    SendKeys.Send("{ENTER}");
                }
                else
                {
                    SendKeys.Send("{TAB}");
                    SendKeys.Send("{TAB}");
                }
            }
        }
        private void OnKeyDownHandlerAndEnter(object sender, KeyEventArgs kea)
        {
            if (kea.KeyCode.Equals(Keys.Return))
            {
                SendKeys.Send("{TAB}");
                SendKeys.Send("{TAB}");
                SendKeys.Send("{TAB}");
                SendKeys.Send("{ENTER}");
            }
        }

        private void textBox_orderID_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_orderID_Enter(object sender, EventArgs e)
        {
            
        }

        private void ProtokollierungScan_Load(object sender, EventArgs e)
        {
            // get sure that there is no value from before!
            Protokollierung.salesVolumeOrderline = "";

            int amountOfOrderlines = Protokollierung.amountOfOrders;
            int i = Protokollierung.counter;
            if (amountOfOrderlines != 0 && amountOfOrderlines >= i)
            {
                stillInOrderLineProcess = true;
                textBox_orderID.Text = Protokollierung.orderIDMainMultipleOrders + "." + i.ToString();
                textBox_scanField.Focus();
                Protokollierung.counter++;
            }
            else
            {
                // otherwise for doubled use case multiple orderline buggs and continues counting on the old one
                Protokollierung.counter = 1;
            }
        }

        private void btn_multipleOrderlines_Click(object sender, EventArgs e)
        {
            int i = Protokollierung.counter;
            // Input of actual order number
            using (var form = new MultipleOrderlinesOrderIDInput())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // add real orderline data to Protokollierung array
                    string[] getOrderLinesFromAPI = BillBeeAPIHandler.GetOrderlinesFromMainOrder(form.orderID);
                    Protokollierung.orderlines = Protokollierung.orderlines.Concat(getOrderLinesFromAPI).ToArray();
                    // further stuff
                    Protokollierung.orderIDMainMultipleOrders = form.orderID;
                    Protokollierung.amountOfOrders = getOrderLinesFromAPI.Length;
                    textBox_orderID.Text = Protokollierung.orderIDMainMultipleOrders + "."+ i.ToString();
                    textBox_scanField.Focus();
                    Protokollierung.counter++;
                    stillInOrderLineProcess = true;
                }
            }
        }
    }
}
