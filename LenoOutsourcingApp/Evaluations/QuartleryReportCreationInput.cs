using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha.Evaluations
{
    public partial class QuartleryReportCreationInput : Form
    {
        public string year = "";
        public string quarter = "";
        public QuartleryReportCreationInput()
        {
            InitializeComponent();
            LoadComboBoxYearSelection();
        }
        public void LoadComboBoxYearSelection()
        {
            // Aktuelles Jahr bestimmen
            int currentYear = DateTime.Now.Year;

            // Combobox mit Werten füllen
            for (int i = currentYear - 3; i <= currentYear + 3; i++)
            {
                comboBox_yearSelection.Items.Add(i.ToString());
            }
        }
        private void btn_Execute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox_quarterSelection.Text) || string.IsNullOrEmpty(comboBox_yearSelection.Text))
            {
                MessageBox.Show("Bitte fülle alle Felder aus.");
            }
            year = comboBox_yearSelection.Text;
            quarter = comboBox_quarterSelection.Text;
            this.DialogResult = DialogResult.OK;

        }
    }
}
