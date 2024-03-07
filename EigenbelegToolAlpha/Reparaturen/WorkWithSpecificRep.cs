using System;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class WorkWithSpecificRep : Form
    {
        public WorkWithSpecificRep()
        {
            InitializeComponent();
        }

        private string inputValue = "";
        public string imeiFiltered = "";
        public int internFiltered;

        private void btn_Executre_Click(object sender, EventArgs e)
        {
            internFiltered = HelperClassTemp.ReturnInternalNumberFromBarcode(inputValue);
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            inputValue = textBox1.Text;
        }

        private void WorkWithSpecificRep_Load(object sender, EventArgs e)
        {

        }

        private void OnKeyDownHandlerAndEnter(object sender, KeyEventArgs kea)
        {
            if (kea.KeyCode.Equals(Keys.Return))
            {
                SendKeys.Send("{TAB}");
                SendKeys.Send("{ENTER}");
            }
        }
    }
}
