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
    public partial class UIWorkflowShippingProcess : Form
    {
        public UIWorkflowShippingProcess()
        {
            InitializeComponent();
        }

        private void UIWorkflowShippingProcess_Load(object sender, EventArgs e)
        {
            
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            var wsp = new WorkflowShippingProcess();
            wsp.Main(this);
        }
        public void UpdateLabel(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                label.Invoke((MethodInvoker)delegate {
                    label.Text = text;
                    Application.DoEvents();
                });
            }
            else
            {
                label.Text = text;
            }
        }
        public void UpdateLabelColor(Label label, Color color)
        {
            if (label.InvokeRequired)
            {
                label.Invoke((MethodInvoker)delegate {
                    label.ForeColor = color;
                    Application.DoEvents();
                });
            }
            else
            {
                label.ForeColor = color;
            }
        }
        public void UpdateProgressbar(ProgressBar progress, int value)
        {
            if (progress.InvokeRequired)
            {
                progress.Invoke((MethodInvoker)delegate {
                    progress.Value = value;
                    Application.DoEvents();
                });
            }
            else
            {
                progress.Value = value;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
