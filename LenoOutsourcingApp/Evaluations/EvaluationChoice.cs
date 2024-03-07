using EigenbelegToolAlpha.Evaluations;
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
    public partial class EvaluationChoice : Form
    {
        public EvaluationChoice()
        {
            InitializeComponent();
        }

        private void btn_StartNewEvaluation_Click(object sender, EventArgs e)
        {
            EvaluationsFirstPage window = new EvaluationsFirstPage();
            window.Show();
        }

        private void btn_EvaluationsOverview_Click(object sender, EventArgs e)
        {
            EvaluationHistory window = new EvaluationHistory();
            window.Show();
        }

        private void btn_startQuarterlyReport_Click(object sender, EventArgs e)
        {
            using (var form = new QuartleryReportCreationInput())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    try
                    {
                        QuarterlyReportPDF.CreatePDFFile(form.quarter, form.year);
                        QuarterlyReportPDF.UploadPDF(form.quarter, form.year);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler: " + ex.Message);
                    }
                }
            }
        }
    }
}
