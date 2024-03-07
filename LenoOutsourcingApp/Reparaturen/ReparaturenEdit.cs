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
            string make = comboBox_reparaturenMake.Text;
            string storage = comboBox__reparaturenStorage.Text;
            string defect = textBox__reparaturenDefect.Text;
            string transactionAmount = textBox_reparaturenTransactionAmount.Text;
            string imei = textBox__reparaturenIMEI.Text;
            string externalCosts = textBox_reparaturenExternalCosts.Text;
            string comment = textBox_reparaturenComment.Text;
            string source = comboBox_source.Text;
            string referenceToEB = textBox_reparaturenReferenceToEB.Text;
            string state = comboBox_ReparaturenReparaturStatus.Text;
            string color = comboBox_ReparaturEditColor.Text;
            string condition = comboBox_ReparaturEditCondition.Text;
            string taxes = comboBox_ReparaturEditTaxes.Text;
            string fiveG = comboBox_FiveG.Text;
            string externalCostsDiff = textBox_ExternalCostsDiff.Text;
            string commentRefurbisher = textbox_commentRefurbisher.Text;
            string batteryCapacity = comboBox_batteryCapacity.Text;
            string chargingCycles = textBox_chargingCycles.Text;
            string buybackGrade = comboBox_buybackGrade.Text;
            // save elements from listboxes to simple strings
            string selectedDefects = ConcatenateSelectedItems(listBox_defects, delimiter: ';');
            // old code for model
            if (comboBox_SamsungDevices.Text != "")
            {
                device = comboBox_SamsungDevices.Text;
            }
            else if (comboBox_reparaturenEditDevice.Text != "")
            {
                device = comboBox_reparaturenEditDevice.Text;
            }

            // adaption that the values are saved to Reparaturen for workflow arrival of goods
            TempSaveForArrivalOfGoods(make, color, condition, taxes, fiveG);
            // DB part
            string[] attributes = new string[] { "Intern", "Kaufdatum", "Geraet", "Kaufbetrag", "IMEI", "Marke", "Speicher", "Defekt",
                                            "ExterneKosten", "Kommentar", "CommentRefurbisher", "BatteryCapacity", "Reparaturstatus", "Quelle",
                                            "ChargingCycles", "BuyBackGrade", "EBReferenz", "Hauptteile", "Farbe", "Besteuerung", "Zustand",
                                            "Spendermonat", "Spenderjahr", "5G","ExterneKostenDIFF", "SelectedDefects"};
            string[] values = new string[] { internalNumber, dateBought, device, transactionAmount, imei, make, storage, defect,
                                             externalCosts, comment, commentRefurbisher, batteryCapacity, state, source, chargingCycles,
                                             buybackGrade, referenceToEB, Reparaturen.usedSpareParts, color, taxes, condition, donorMonth, donorYear, fiveG,
                                             externalCostsDiff, selectedDefects };
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
            LoadModelCombobox();
            LoadBatteryCapacityCombobox();
            CheckIfStorageIsHigh(Reparaturen.storage);
            CheckIfModelIsRelevantFor100Battery(Reparaturen.device);
            SetFrontEndValues();
            AdaptionsBusinessLogic();
            string tempCheckDevice = Reparaturen.device;
            if (comboBox_reparaturenEditDevice.Items.Contains(tempCheckDevice))
            {
                comboBox_reparaturenEditDevice.Text = tempCheckDevice;
            }
            else if (comboBox_SamsungDevices.Items.Contains(tempCheckDevice))
            {
                comboBox_SamsungDevices.Text = tempCheckDevice;
            }
            // preselect make with help of the model value
            if (comboBox_reparaturenMake.Text == "")
            {
                string returnValue = "";
                try
                {
                    returnValue = SKUGeneration.modelleDictionaryApple[Reparaturen.device];
                }
                catch (Exception ex)
                {

                }
                if (returnValue != "")
                {
                    comboBox_reparaturenMake.Text = "Apple";
                }
                else
                {
                    comboBox_reparaturenMake.Text = "Samsung";
                }
            }
            //check if device value is given and modify the visibility
            if (comboBox_SamsungDevices.Text != "")
            {
                comboBox_SamsungDevices.Visible = true;
            }
            else if (comboBox_reparaturenEditDevice.Text != "")
            {
                comboBox_reparaturenEditDevice.Visible = true;
            }
            AdjustCostsTextBoxesLogic();
            LoadUsedSparePartsListBox();
            LoadDefectListBox();
        }
        private void AdjustCostsTextBoxesLogic()
        {
            if (string.IsNullOrEmpty(textBox_reparaturenExternalCosts.Text))
            {
                textBox_reparaturenExternalCosts.Text = "0€";
            }
            if (string.IsNullOrEmpty(textBox_ExternalCostsDiff.Text))
            {
                textBox_ExternalCostsDiff.Text = "0€";
            }
        }
        public void LoadUsedSparePartsListBox()
        {
            listBox_UsedSpareParts.Items.Clear();
            string partIds = Reparaturen.usedSpareParts;
            string[] valueArray = partIds.Split(';'); // Split the string into an array of values

            foreach (string value in valueArray)
            {
                int index = 0;
                string insertValue = "";
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }
                // get information from supplier table (DB)
                if (int.TryParse(value, out _))
                {
                    var dbManager = new DBManager();
                    string part = dbManager.ExecuteQueryWithResultString("SpareParts", "Part", "Id", value);
                    string supplier = dbManager.ExecuteQueryWithResultString("SpareParts", "Supplier", "Id", value);
                    insertValue = part + $" ({supplier})";
                }
                else
                {
                    insertValue = value + " (old system)";
                }
                listBox_UsedSpareParts.Items.Add(insertValue);
                index = listBox_UsedSpareParts.Items.IndexOf(insertValue);
                listBox_UsedSpareParts.SetSelected(index, true);
            }
        }
        private void AdaptionsBusinessLogic()
        {
            if (comboBox_source.Text == "BackMarket")
            {
                comboBox_ReparaturEditTaxes.Text = "Differenzbesteuerung";
            }
        }
        private void comboBox_reparaturenMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_reparaturenMake.Text == "Apple")
            {
                comboBox_reparaturenEditDevice.Visible = true;
                comboBox_SamsungDevices.Visible = false;
            }
            else if (comboBox_reparaturenMake.Text == "Samsung")
            {
                comboBox_reparaturenEditDevice.Visible = false;
                comboBox_SamsungDevices.Visible = true;
                label25.Visible = true;
                comboBox_FiveG.Visible = true;
            }
        }
        private void SetFrontEndValues()
        {
            internalNumber = Reparaturen.internalNumber;
            textBox_ReparaturenInternalNumber.Text = Reparaturen.internalNumber;
            textBox_reparaturenDateBought.Text = Reparaturen.dateBought;
            comboBox_reparaturenMake.Text = Reparaturen.make;
            comboBox__reparaturenStorage.Text = Reparaturen.storage;
            textBox__reparaturenDefect.Text = Reparaturen.defect;
            textBox_reparaturenTransactionAmount.Text = Reparaturen.transactionAmount;
            textBox__reparaturenIMEI.Text = Reparaturen.imei;
            textBox_reparaturenExternalCosts.Text = Reparaturen.externalCosts;
            textBox_reparaturenComment.Text = Reparaturen.comment;
            comboBox_batteryCapacity.Text = Reparaturen.batteryCapacity;
            textbox_commentRefurbisher.Text = Reparaturen.commentRefurbisher;
            textBox_chargingCycles.Text = Reparaturen.chargingCycles;
            comboBox_buybackGrade.Text = Reparaturen.buybackGrade;
            textBox_reparaturenReferenceToEB.Text = Reparaturen.referenceToEB;
            comboBox_ReparaturenReparaturStatus.Text = Reparaturen.state;
            comboBox_ReparaturEditColor.Text = Reparaturen.color;
            comboBox_ReparaturEditTaxes.Text = Reparaturen.taxes;
            comboBox_ReparaturEditCondition.Text = Reparaturen.condition;
            lbl_donorMonthAndYear.Text = Reparaturen.donorMonth + Reparaturen.donorYear;
            comboBox_FiveG.Text = Reparaturen.fiveG;
            textBox_ExternalCostsDiff.Text = Reparaturen.externalCostsDiff;
            comboBox_source.Text = Reparaturen.source;
        }

        private void comboBox_ReparaturenReparaturStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_ReparaturenReparaturStatus.Text == "Spender")
            {
                string currentMonth = DateTime.Now.ToString().Substring(3, 2);
                donorYear = DateTime.Now.ToString().Substring(6, 4);
                donorMonth = monthOverview[currentMonth];
                lbl_donorMonthAndYear.Text = donorMonth + donorYear;
            }
        }

        private void btn_100batteryprint_Click(object sender, EventArgs e)
        {
            Reparaturen window = new Reparaturen();
            window.BrotherPrintThisModell(1, 100);
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
        private void button2_Click(object sender, EventArgs e)
        {
            using (var form = new ReparturenEditSparePartsDetails())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    textBox_reparaturenExternalCosts.Text = form.costsWithTaxDeduction.ToString();
                    textBox_ExternalCostsDiff.Text = form.costsWithoutTaxDecution.ToString();
                    LoadUsedSparePartsListBox();
                }
            }
        }

        private void btn_reparaturenAddNewExternalCosts_Click(object sender, EventArgs e)
        {
            //Unterscheidung, ob im Textfeld ein € Zeichen ist
            double currentValue;
            double sumValue;
            double newCosts = Convert.ToDouble(textBox_ReparaturEditAddNewExternalCosts.Text);
            if (textBox_reparaturenExternalCosts.Text.Contains("€"))
            {
                var actualLength = textBox_reparaturenExternalCosts.TextLength;
                var fixedField = textBox_reparaturenExternalCosts.Text.Substring(0, actualLength - 1);
                currentValue = Convert.ToDouble(fixedField);
                sumValue = currentValue + newCosts;
                textBox_reparaturenExternalCosts.Text = sumValue.ToString() + "€";
            }
            else
            {
                //Momentanen Wert festhalten in Textbox
                currentValue = Convert.ToDouble(textBox_reparaturenExternalCosts.Text);
                sumValue = currentValue + newCosts;
                textBox_reparaturenExternalCosts.Text = sumValue.ToString() + "€";
            }
            textBox_ReparaturEditAddNewExternalCosts.Text = "";
            MessageBox.Show("Es wurden " + newCosts + "€ hinzugefügt");
        }
        #endregion

        #region Helper methods
        private void TempSaveForArrivalOfGoods(string make, string color, string condition, string taxes, string fiveG)
        {
            Reparaturen.make = make;
            Reparaturen.color = color;
            Reparaturen.condition = condition;
            Reparaturen.taxes = taxes;
            Reparaturen.fiveG = fiveG;
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
            foreach (var item in ProductModelCatalog.samsungModels)
            {
                comboBox_SamsungDevices.Items.Add(item.ToString());
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
        public void CheckIfModelIsRelevantFor100Battery(string model)
        {
            var dbManager = new DBManager();
            string input = dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "RelevantModelsFor100Battery");
            string[] parts = input.Split(';');
            foreach (string part in parts)
            {
                if (part == model)
                {
                    btn_100batteryprint.Visible = true;
                }
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            using (var form = new ReparaturenEditSparePartQuickAdd())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // adding costs 
                    if (form.taxation == "Regelbesteuerung")
                    {
                        AdjustTextBoxNewCosts(textBox_reparaturenExternalCosts, form.costs);
                    }
                    else
                    {
                        AdjustTextBoxNewCosts(textBox_ExternalCostsDiff, form.costs);
                    }
                    // add id
                    string newValue = HelperClass.GetValueWithDelimiterAdd(form.idPart, Reparaturen.usedSpareParts);
                    Reparaturen.usedSpareParts = newValue;
                    LoadUsedSparePartsListBox();
                }
            }
        }
        private void AdjustTextBoxNewCosts(System.Windows.Forms.TextBox textBox, double costsFromAdding)
        {
            string temp = HelperClass.RemoveEuroSign(textBox.Text);
            double newCosts = costsFromAdding + Convert.ToDouble(temp);
            textBox.Text = newCosts.ToString();
        }

        private void btn_quickActions_Click(object sender, EventArgs e)
        {
            using (var form = new RepEditQuickActionsSelection())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string content = form.contentForCommentBox;
                    if (!string.IsNullOrEmpty(content))
                    {
                        string existingContent = textBox_reparaturenComment.Text;
                        // absatz nur hinzufügen wenn existingContent nicht leer ist
                        if (!string.IsNullOrEmpty(existingContent))
                        {
                            existingContent = existingContent + "\r\n";
                        }
                        textBox_reparaturenComment.Text = existingContent + content;
                    }
                }
            }
        }
    }
}

