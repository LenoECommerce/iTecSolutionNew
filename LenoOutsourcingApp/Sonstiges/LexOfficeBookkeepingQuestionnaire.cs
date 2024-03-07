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
    public partial class LexOfficeBookkeepingQuestionnaire : Form
    {
        public LexOfficeBookkeepingQuestionnaire()
        {
            InitializeComponent();
        }

        private void btn_Execute_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked &&
                checkBox2.Checked &&
                checkBox3.Checked &&
                textBox_amount.Text != "")
            {
                LexOfficeBookkeepingAutomization.Main(Convert.ToInt32(textBox_amount.Text));
                this.Hide();
            }
            else
            {
                MessageBox.Show("Bitte alle Felder ausfüllen.");
            }
        }
    }
}
