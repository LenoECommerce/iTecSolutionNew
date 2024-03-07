﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class EvaluationSecondForm : Form
    {
        public static double B2BGrossSalesTotal;
        public static double B2BRevenue;
        public static double B2BGrossSalesOnlySpareParts;
        public static double rateConsumptionCables;
        public static double rateConsumptionNeutralPackages;
        public static double rateConsumptionCartons;
        public static double rateConsumptionTotal;
        public static double moreExternalCosts;
        public EvaluationSecondForm()
        {
            InitializeComponent();
        }

        private void btn_ContinueWithEvaluation3_Click(object sender, EventArgs e)
        {
            try
            {
                B2BGrossSalesTotal = Convert.ToDouble(textBox_B2BGrossSales.Text) + Convert.ToDouble(textBox_b2bSparePartsGrossSales.Text);
                B2BGrossSalesOnlySpareParts = Convert.ToDouble(textBox_b2bSparePartsGrossSales.Text);
                B2BRevenue = SumUpB2BRevenue();
                rateConsumptionCables = Convert.ToDouble(textBox_rateConsumptionCables.Text)*2;
                rateConsumptionNeutralPackages = Convert.ToDouble(textBox_rateConsumptionNeutralPackages.Text)*3.9;
                rateConsumptionCartons = Convert.ToDouble(textBox_rateConsumptionCartons.Text)*0.3;
                rateConsumptionTotal = rateConsumptionCables + rateConsumptionNeutralPackages + rateConsumptionCartons;
                moreExternalCosts = Convert.ToDouble(textBox_MoreExternalCosts.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            EvaluationThirdForm frm = new EvaluationThirdForm();
            frm.Show();
            this.Hide();
        }
        public double SumUpB2BRevenue()
        {
            return Convert.ToDouble(textBox_B2BGrossSalesRevenue.Text) + Convert.ToDouble(textBox_b2bSparePartsGrossSales.Text)/1.19;
        }
        private void textBox_rateConsumptionCables_TextChanged(object sender, EventArgs e)
        {

        }

        private void EvaluationSecondForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox_B2BGrossSales_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
