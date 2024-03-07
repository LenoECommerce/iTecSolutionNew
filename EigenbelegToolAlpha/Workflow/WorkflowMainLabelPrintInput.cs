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
    public partial class WorkflowMainLabelPrintInput : Form
    {
        public static int amountLabels = 0;
        public WorkflowMainLabelPrintInput()
        {
            InitializeComponent();
        }

        public void btn_1_Click(object sender, EventArgs e)
        {
            amountLabels = 1;
        }

        public void btn_2_Click(object sender, EventArgs e)
        {
            amountLabels = 2;
        }

        private void WorkflowMainLabelPrintInput_Load(object sender, EventArgs e)
        {

        }
    }
}
