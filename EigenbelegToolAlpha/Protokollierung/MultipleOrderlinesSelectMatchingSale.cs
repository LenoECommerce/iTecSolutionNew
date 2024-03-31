using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Google.Apis.Requests.BatchRequest;

namespace EigenbelegToolAlpha
{
    public partial class MultipleOrderlinesSelectMatchingSale : Form
    {
        public MultipleOrderlinesSelectMatchingSale()
        {
            InitializeComponent();
        }

        private void btn_Execute_Click(object sender, EventArgs e)
        {
            string selectedItem = comboBox_possibleOrderlines.Text;
            Protokollierung.salesVolumeOrderline = AdaptOnlyRawSalesVolume(selectedItem);
            DeleteSelectionFromArray(selectedItem);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string AdaptOnlyRawSalesVolume(string input)
        {
            var posEuro = input.IndexOf("€");
            var length = input.Length;
            return input.Substring(0,posEuro);
        }

        private void MultipleOrderlinesSelectMatchingSale_Load(object sender, EventArgs e)
        {
            
            foreach (var item in Protokollierung.orderlines)
            {
                comboBox_possibleOrderlines.Items.Add(item);
            }
        }

        public void DeleteSelectionFromArray(string selectedItem)
        {
            // Iterate over the array
            for (int i = 0; i < Protokollierung.orderlines.Length; i++)
            {
                // Check if the current element matches the selected item
                if (Protokollierung.orderlines[i] == selectedItem)
                {
                    // Remove the matching element from the array
                    Protokollierung.orderlines = Protokollierung.orderlines.Where((val, idx) => idx != i).ToArray();

                    // Optional: Exit the loop if you want to delete only the first occurrence
                    break;
                }
            }
        }
    }
}
