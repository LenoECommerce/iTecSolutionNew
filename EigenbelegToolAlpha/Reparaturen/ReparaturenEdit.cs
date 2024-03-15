using EigenbelegToolAlpha.APIs;
using EigenbelegToolAlpha.Sonstiges;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ListBox = System.Windows.Forms.ListBox;

namespace EigenbelegToolAlpha
{
    public partial class ReparaturenEdit : Form
    {
        public ReparaturenEdit()
        {
            InitializeComponent();
        }
        #region variables etc.
        Dictionary<string, string> monthOverview = new Dictionary<string, string>
        {
            { "01", "Januar" },
            { "02", "Februar" },
            { "03", "März" },
            { "04", "April" },
            { "05", "Mai" },
            { "06", "Juni" },
            { "07", "Juli" },
            { "08", "August" },
            { "09", "September" },
            { "10", "Oktober" },
            { "11", "November" },
            { "12", "Dezember" },
        };
        public string donorMonth = "";
        public string donorYear = "";
        public string internalNumber = "";
        #endregion

        #region main code

        private void button1_Click(object sender, EventArgs e)
        {
            SaveValuesToDB();
        }

        public void SaveValuesToDB()
        {
            string internalNumber = textBox_ReparaturenInternalNumber.Text;
            string dateBought = textBox_reparaturenDateBought.Text;
            string device = "";
            string storage = comboBox__reparaturenStorage.Text;
            string transactionAmount = textBox_reparaturenTransactionAmount.Text;
            string imei = textBox__reparaturenIMEI.Text;
            string comment = textBox_reparaturenComment.Text;
            string referenceToEB = textBox_reparaturenReferenceToEB.Text;
            string color = comboBox_ReparaturEditColor.Text;
            string condition = comboBox_ReparaturEditCondition.Text;
            string taxes = comboBox_ReparaturEditTaxes.Text;
            string commentRefurbisher = textbox_commentRefurbisher.Text;
            string batteryCapacity = comboBox_batteryCapacity.Text;
            string chargingCycles = textBox_chargingCycles.Text;
            string buybackGrade = comboBox_buybackGrade.Text;
            // save elements from listboxes to simple strings
            string selectedDefects = ConcatenateSelectedItems(listBox_defects, delimiter: ';');


            device = comboBox_reparaturenEditDevice.Text;


            // adaption that the values are saved to Reparaturen for workflow arrival of goods
            TempSaveForArrivalOfGoods(color, condition, taxes);
            // DB part
            string[] attributes = new string[] { "Intern", "Kaufdatum", "Geraet", "Kaufbetrag", "IMEI", "Speicher",
                                            "Kommentar", "CommentRefurbisher", "BatteryCapacity",
                                            "ChargingCycles", "BuyBackGrade", "EBReferenz", "Hauptteile", "Farbe", "Besteuerung", "Zustand",
                                            "Spendermonat", "Spenderjahr", "SelectedDefects"};
            string[] values = new string[] { internalNumber, dateBought, device, transactionAmount, imei, storage,
                                             comment, commentRefurbisher, batteryCapacity, chargingCycles,
                                             buybackGrade, referenceToEB, Reparaturen.usedSpareParts, color, taxes, condition, donorMonth, donorYear,selectedDefects };
            try
            {
                //Datasync
                Eigenbelege.dataSync("Eigenbelege", device, transactionAmount, storage, referenceToEB);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Fülle folgende Felder unbedingt aus: Modell, Betrag, Speicher und Referenz.");
            }
            try
            {
                var dbManager = new DBManager();
                dbManager.UpdateSpecificEntry("Reparaturen", attributes, values, Reparaturen.lastSelectedProductKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei Speicherung zur Datenbank: " + ex.Message);
            }
            MessageBox.Show("Deine Änderungen wurden erfolgreich gespeichert.");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void ReparaturenEdit_Load(object sender, EventArgs e)
        {
            LoadBatteryCapacityCombobox();
            LoadModelCombobox();
            CheckIfStorageIsHigh(Reparaturen.storage);
            SetFrontEndValues();
            LoadDefectListBox();
            textBox_ReparaturenInternalNumber.ReadOnly = !Reparaturen.IsEntryCreated;
            comboBox_ReparaturEditTaxes.Text = "Differenzbesteuerung";
        }
       

        private void SetFrontEndValues()
        {
            comboBox_reparaturenEditDevice.Text = Reparaturen.device;
            textBox_ReparaturenInternalNumber.Text = Reparaturen.internalNumber;
            textBox_reparaturenDateBought.Text = Reparaturen.dateBought;
            comboBox__reparaturenStorage.Text = Reparaturen.storage;
            textBox_reparaturenTransactionAmount.Text = Reparaturen.transactionAmount;
            textBox__reparaturenIMEI.Text = Reparaturen.imei;
            textBox_reparaturenComment.Text = Reparaturen.comment;
            comboBox_batteryCapacity.Text = Reparaturen.batteryCapacity;
            textbox_commentRefurbisher.Text = Reparaturen.commentRefurbisher;
            textBox_chargingCycles.Text = Reparaturen.chargingCycles;
            comboBox_buybackGrade.Text = Reparaturen.buybackGrade;
            textBox_reparaturenReferenceToEB.Text = Reparaturen.referenceToEB;
            comboBox_ReparaturEditColor.Text = Reparaturen.color;
            comboBox_ReparaturEditTaxes.Text = Reparaturen.taxes;
            comboBox_ReparaturEditCondition.Text = Reparaturen.condition;
        }


        public void btn_nsysFillOut_Click(object sender, EventArgs e)
        {
            WorkflowArrivalOfGoods.FillOutDataByIMEI(textBox__reparaturenIMEI.Text, comboBox_ReparaturEditColor.Text, comboBox_reparaturenEditDevice.Text, comboBox__reparaturenStorage.Text);
            comboBox_reparaturenEditDevice.Text = WorkflowArrivalOfGoods.model;
            comboBox_ReparaturEditColor.Text = WorkflowArrivalOfGoods.color;
            comboBox__reparaturenStorage.Text = WorkflowArrivalOfGoods.storage;
        }

        public void btn_IMEICheck_Click(object sender, EventArgs e)
        {
            string imeiField = textBox__reparaturenIMEI.Text;
            if (string.IsNullOrEmpty(imeiField))
            {
                MessageBox.Show("Bitte fülle zuerst das Feld IMEI aus.");
                return;
            }
            iFreeiCloudAPIHandler iFreeiCloudAPIHandler = new iFreeiCloudAPIHandler();
            iFreeiCloudAPIHandler.APIRequests(imeiField);
            string findMyiPhoneStatus = iFreeiCloudAPIHandler.findMyiPhoneStatus;
            string blacklistStatus = iFreeiCloudAPIHandler.blacklistStatus;
            string information = "Es sind keine weiteren BuyBack Aktionen notwendig.";

            if (findMyiPhoneStatus.ToLower() != "false" || blacklistStatus.ToLower() != "false")
            {
                information = "Bitte dringend BuyBack Aktionen umsetzen!";
            }

            MessageBox.Show($"Der IMEI Check hat folgendes Ergebnis:\r\n\r\n" +
                $"- Find My iPhone: {findMyiPhoneStatus}\r\n" +
                $"- Blacklist Status: {blacklistStatus}\r\n\r\n" +
                $"{information}");
        }

        #endregion

        #region Helper methods
        private void TempSaveForArrivalOfGoods(string color, string condition, string taxes)
        {
            Reparaturen.color = color;
            Reparaturen.condition = condition;
            Reparaturen.taxes = taxes;
        }
        private string ConcatenateSelectedItems(ListBox listBox, char delimiter)
        {
            // Get the selected items from the ListBox
            var selectedItems = listBox.SelectedItems.Cast<object>();

            // Join the selected items into a string with the delimiter
            string concatenatedString = string.Join(delimiter.ToString(), selectedItems);

            return concatenatedString;
        }
        private void ReorderSelectedItemsToTop(ListBox listBox)
        {
            // Create a list to store the selected items
            var selectedItems = new List<object>();

            // Retrieve and remove the selected items from the ListBox
            for (int i = listBox.SelectedIndices.Count - 1; i >= 0; i--)
            {
                int selectedIndex = listBox.SelectedIndices[i];
                selectedItems.Add(listBox.Items[selectedIndex]);
                listBox.Items.RemoveAt(selectedIndex);
            }

            // Insert the selected items at the beginning of the ListBox
            for (int i = selectedItems.Count - 1; i >= 0; i--)
            {
                listBox.Items.Insert(0, selectedItems[i]);
            }

            // Reselect the moved items
            foreach (object selectedItem in selectedItems)
            {
                listBox.SetSelected(listBox.Items.IndexOf(selectedItem), true);
            }
        }
        private void LoadBatteryCapacityCombobox()
        {
            for (int i = 100; i >= 50; i--)
            {
                comboBox_batteryCapacity.Items.Add(i.ToString() + "%");
            }
        }
        public void LoadModelCombobox()
        {
            foreach (var item in ProductModelCatalog.iPhoneModels)
            {
                comboBox_reparaturenEditDevice.Items.Add(item.ToString());
            }

        }
        public void LoadDefectListBox()
        {
            foreach (var item in ReparaturenConsts.defects)
            {
                listBox_defects.Items.Add(item.ToString());
            }
            string selectedDefects = Reparaturen.selectedDefects;
            char delimiter = ';';
            PopulateListBoxDefects(selectedDefects, delimiter, listBox_defects);
            ReorderSelectedItemsToTop(listBox_defects);

        }
        private void PopulateListBoxDefects(string selectedDefects, char delimiter, ListBox listBox_defects)
        {
            // Split the selectedDefects string into individual values
            string[] defectValues = selectedDefects.Split(delimiter);

            // Add the values to the ComboBox
            foreach (string value in defectValues)
            {
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }
                // Trim any leading/trailing whitespace from the value
                string trimmedValue = value.Trim();

                // Check if the trimmedValue already exists in the ComboBox
                if (!listBox_defects.Items.Contains(trimmedValue))
                {
                    // If the value doesn't exist, add it and select it
                    listBox_defects.Items.Add(trimmedValue);
                    listBox_defects.SelectedItem = trimmedValue;
                }
                else
                {
                    // If the value already exists, select it
                    listBox_defects.SelectedItem = trimmedValue;
                }
            }
        }
        public void CheckIfStorageIsHigh(string storage)
        {
            int result = GetDigitsFromString(storage);
            if (result > 64)
            {
                comboBox__reparaturenStorage.BackColor = Color.Red;
            }
        }
        
        public static int GetDigitsFromString(string input)
        {
            // Initialize a string to hold the digits we find
            string digitString = "";

            // Loop through each character in the input string
            foreach (char c in input)
            {
                // If the character is a digit, add it to our digit string
                if (Char.IsDigit(c))
                {
                    digitString += c;
                }
            }

            // Try to parse the digit string as an integer
            if (int.TryParse(digitString, out int result))
            {
                return result;
            }
            else
            {
                // Return -1 to indicate that we could not extract a valid integer
                return -1;
            }
        }
        #endregion

        #region empty methdods
        private void comboBox_reparaturenColor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox__reparaturenStorage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void textBox_reparaturenReferenceToEB_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_reparaturenWorthIt_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_reparaturenRiskLevel_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_reparaturenSource_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_reparaturenComment_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_reparaturenExternalCosts_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox__reparaturenIMEI_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_reparaturenTransactionAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox__reparaturenDefect_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_reparaturenDevice_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_reparaturenDateBought_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_ReparaturenInternalNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox_ReparaturenEditMainParts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion


    }
}

