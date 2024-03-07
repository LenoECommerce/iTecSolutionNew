using System;
using System.Windows.Forms;


namespace EigenbelegToolAlpha
{
    public partial class Settings : Form
    {
        public static string currentUser = UserFileManagement.ReturnCurrentUser();
        public string userPreferences = "";
        public string devices100Battery = "";
        public string valueIntern;
        public string valueEigenbelegNumber;
        public string defaultStartingWindow;
        public string folderLocation;

        public Settings()
        {
            InitializeComponent();

            var dbManager = new DBManager();
            valueIntern = dbManager.ExecuteQueryWithResult("Config", "Nummer", "Typ", "InterneNummer").ToString();
            valueEigenbelegNumber = dbManager.ExecuteQueryWithResult("Config", "Nummer", "Typ", "Eigenbelegnummer").ToString();
            defaultStartingWindow = dbManager.ExecuteQueryWithResultString("ConfigUser", "Standardfenster", "Nutzer", currentUser).ToString();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            textBox_SettingsEigenbelegNummer.Text = valueEigenbelegNumber;
            textBox_SettingsInternalNumber.Text = valueIntern;
            comboBox_PreferdStartWindow.Text = defaultStartingWindow;
            lbl_userName.Text = "Hallo " + currentUser;
            LoadMenuPreferences();
            LoadDevicesFor100Battery();
            string table = "ConfigUser";
            string termUser = "Nutzer";
            var dbManager = new DBManager();
            lbl_pathDisplayTemplate.Text = dbManager.ExecuteQueryWithResultString(table, "TemplateDisplay", termUser, currentUser);
            lbl_templatePathIMEINotIdentifiable.Text = dbManager.ExecuteQueryWithResultString(table, "PathTemplateIdentifiable", termUser, currentUser);
            textBox_arrivalRate.Text = dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "BuyBackArrivalRate");
            textBox_paymentRate.Text = dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "BuyBackPaymentRate");
            textBox_buybackBudgetNormalDevices.Text = dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "BuyBackPurchaseLimit");
            lbl_currentPathModellTemplate.Text = dbManager.ExecuteQueryWithResultString(table, "TemplateModell", termUser, currentUser);
            lbl_pathSparePartStorage.Text = dbManager.ExecuteQueryWithResultString(table, "TemplateSparePartStorage", termUser, currentUser);
            lbl_directoryPathDeliveryNotes.Text = dbManager.ExecuteQueryWithResultString(table, "PathDirDeliveryNotes", termUser, currentUser);
            lbl_directoryPathShippingLabels.Text = dbManager.ExecuteQueryWithResultString(table, "PathDirShippingLabels", termUser, currentUser);
            lbl_SaveLocationPDF.Text = dbManager.ExecuteQueryWithResultString(table, "PathSaveLocationEB", termUser, currentUser);
            lbl_PathVideosUpload.Text = dbManager.ExecuteQueryWithResultString(table, "PathVideosForUpload", termUser, currentUser);
            lbl_pathTemplateDFUReset.Text = dbManager.ExecuteQueryWithResultString(table, "PathTemplateDFUReset", termUser, currentUser);
            lbl_pathTemplateCounterOffer.Text = dbManager.ExecuteQueryWithResultString(table, "PathTemplateCounterOffer", termUser, currentUser);
            lbl_pathTemplateLockedDevice.Text = dbManager.ExecuteQueryWithResultString(table, "PathTemplateLockedDevice", termUser, currentUser);
            lbl_pathTemplateSimCard.Text = dbManager.ExecuteQueryWithResultString(table, "PathTemplateReceivedSimCard", termUser, currentUser);
            lbl_pathEbaySingleListing.Text = dbManager.ExecuteQueryWithResultString(table, "PathTemplateEbaySingleListing", termUser, currentUser);
        }
        public void LoadDevicesFor100Battery()
        {
            var dbManager = new DBManager();
            string input = dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "RelevantModelsFor100Battery");
            string[] parts = input.Split(';');
            foreach (string part in parts)
            {
                listBoxDevices100Battery.SelectedItems.Add(part);
            }
        }
        private void PathBums(Label label, string dbAttribute)
        {
            openFD.ShowDialog();
            string selectedFileName = openFD.FileName;
            selectedFileName = selectedFileName.Replace(@"\", @"\\");
            var dbManager = new DBManager();
            dbManager.ExecuteQuery($"UPDATE `ConfigUser` SET `{dbAttribute}` = '{selectedFileName}' WHERE `Nutzer` = '{currentUser}'");
            label.Text = selectedFileName;
        }
        public void LoadMenuPreferences()
        {
            var dbManager = new DBManager();
            string input = dbManager.ExecuteQueryWithResultString("ConfigUser", "ArrangementPreferences", "Nutzer", currentUser);
            string[] parts = input.Split(';');
            foreach (string part in parts)
            {
                listBox1.SelectedItems.Add(part);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //simplification for queries
            var dbManager = new DBManager();
            string[] types = new string[] { "BuyBackPurchaseLimit", "BuyBackArrivalRate", "BuyBackPaymentRate", "Eigenbelegnummer", "InterneNummer" };
            object[] values = new object[] { textBox_buybackBudgetNormalDevices.Text, textBox_arrivalRate.Text, textBox_paymentRate.Text, textBox_SettingsEigenbelegNummer.Text, textBox_SettingsInternalNumber.Text };
            for (int i = 0; i < types.Length; i++)
            {
                dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = '" + values[i].ToString() + "' WHERE `Typ` = '" + types[i] + "'");
            }
            dbManager.ExecuteQuery("UPDATE `ConfigUser` SET `Standardfenster` = '" + comboBox_PreferdStartWindow.Text + "' WHERE `Nutzer` = '" + currentUser + "'");
            SaveUserPreferences();
            SaveDevices100Battery();
            MessageBox.Show("Deine Einstellungen wurden gespeichert.");
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
        public void SaveDevices100Battery()
        {
            foreach (object items in listBoxDevices100Battery.SelectedItems)
            {
                devices100Battery += items + ";";
            }
            var dbManager = new DBManager();
            dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = '" + devices100Battery + "' WHERE `Typ` = 'RelevantModelsFor100Battery'");
        }
        public void SaveUserPreferences()
        {
            foreach (object items in listBox1.SelectedItems)
            {
                userPreferences += items + ";";
            }
            var dbManager = new DBManager();
            dbManager.ExecuteQuery("UPDATE `ConfigUser` SET `ArrangementPreferences` = '" + userPreferences + "' WHERE `Nutzer` = '" + currentUser + "'");
        }

        private void btn_SetTemplateModell_Click(object sender, EventArgs e)
        {

        }

        private void lbl_currentPathModellTemplate_Click(object sender, EventArgs e)
        {
            PathBums(lbl_currentPathModellTemplate, "TemplateModell");
        }

        private void lbl_currentPathDisplayTemplate_Click(object sender, EventArgs e)
        {
            PathBums(lbl_pathSparePartStorage, "TemplateSparePartStorage");
        }



        private void btn_LocationTemplates_Click(object sender, EventArgs e)
        {
            PathBums(lbl_SaveLocationPDF, "PathSaveLocationEB");
        }



        private void textBox_SettingsInternalNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbl_PathVideosUpload_Click(object sender, EventArgs e)
        {
            PathBums(lbl_PathVideosUpload, "PathVideosForUpload");
        }


        private void btn_runningCostsEdit_Click(object sender, EventArgs e)
        {
            EvaluationRunningCosts window = new EvaluationRunningCosts();
            window.Show();
        }

        private void lbl_pathDisplayTemplate_Click(object sender, EventArgs e)
        {
            PathBums(lbl_pathDisplayTemplate, "TemplateDisplay");
        }

        private void label19_Click(object sender, EventArgs e)
        {
            PathBums(lbl_directoryPathDeliveryNotes, "PathDirDeliveryNotes");
        }

        private void lbl_directoryPathShippingLabels_Click(object sender, EventArgs e)
        {
            PathBums(lbl_directoryPathShippingLabels, "PathDirShippingLabels");
        }

        private void btn_EditMarketplacesFeeStructure_Click(object sender, EventArgs e)
        {
            //FrontendFeeStructure window = new FrontendFeeStructure();
            //window.Show();
            //this.Close();
        }

        private void lbl_pathTemplateDFUReset_Click(object sender, EventArgs e)
        {
            PathBums(lbl_pathTemplateDFUReset, "PathTemplateDFUReset");
        }

        private void lbl_pathTemplateCounterOffer_Click(object sender, EventArgs e)
        {
            PathBums(lbl_pathTemplateCounterOffer, "PathTemplateCounterOffer");
        }

        private void lbl_pathTemplateLockedDevice_Click(object sender, EventArgs e)
        {
            PathBums(lbl_pathTemplateLockedDevice, "PathTemplateLockedDevice");
        }

        private void lbl_pathTemplateSimCard_Click(object sender, EventArgs e)
        {
            PathBums(lbl_pathTemplateSimCard, "PathTemplateReceivedSimCard");
        }

        private void lbl_templatePathIMEINotIdentifiable_Click(object sender, EventArgs e)
        {
            PathBums(lbl_templatePathIMEINotIdentifiable, "PathTemplateIdentifiable");
        }

        private void lbl_pathEbaySingleListing_Click(object sender, EventArgs e)
        {
            PathBums(lbl_pathEbaySingleListing, "PathTemplateEbaySingleListing");
        }
    }
}
