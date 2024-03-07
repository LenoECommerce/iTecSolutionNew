﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;

namespace EigenbelegToolAlpha
{

    public partial class EigenbelegCreate : Form
    {
        public EigenbelegCreate()
        {
            InitializeComponent();

        }
        public int newNumber = 0;
        public string tempEigenbelegNumber = "";

        public void button1_Click(object sender, EventArgs e)
        {
            SaveNewEntryToDatabase();
            MessageBox.Show("Dein Eintrag wurde erfolgreich erstellt.");
        }
        public void SaveNewEntryToDatabase()
        {
            tempEigenbelegNumber = textBox_eigenbelegNumber.Text;
            string tempSellerName = textBox_eigenbelegSellerName.Text;
            string tempReference = textBox_eigenbelegReference.Text;
            string tempModel = comboBox_eigenbelegeCreateDevice.Text;
            string tempDateBought = textBox_eigenbelegDateBought.Text;
            string tempTransactionAmount = textBox_eigenbelegTransactionAmount.Text;
            string tempMail = textBox_eigenbelegMail.Text;
            string tempPlatform = comboBox_eigenbelegPlatform.Text;
            string tempPaymentMethod = comboBox_eigenbelegPaymentMethod.Text;
            string tempAddress = textBox_eigenbelegAdress.Text;
            string tempCreated = comboBox_eigenbelegCreated.Text;
            string tempArrived = comboBox_eigenbelegArrived.Text;
            string tempEigenbelegStorage = comboBox_eigenbelegStorage.Text;
            string tempTransactionText = "Zahlung für " + tempPlatform + ": " + tempModel + " " + tempEigenbelegStorage;
            string[] insertValues = new string[14] { tempEigenbelegNumber, tempSellerName, tempReference, tempModel, tempDateBought, tempTransactionAmount,
                                                    tempMail, tempPlatform, tempPaymentMethod, tempAddress, tempCreated, tempArrived, tempEigenbelegStorage, tempTransactionText};
            insertValues = CheckForApostrophe(insertValues);
            string query = string.Format("INSERT INTO `Eigenbelege`(`Eigenbelegnummer`,`Verkaeufername`,`Referenz`,`Modell`,`Kaufdatum`,`Kaufbetrag`,`E-Mail`,`Plattform`,`Zahlungsmethode`,`Adresse`,`Erstellt?`,`Angekommen?`,`Transaktionstext`,`Speicher`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')"
                    , insertValues[0], insertValues[1], insertValues[2], insertValues[3], insertValues[4], insertValues[5], insertValues[6], insertValues[7],
                    insertValues[8], insertValues[9], insertValues[10], insertValues[11], insertValues[13], insertValues[12]);
            var dbManager = new DBManager();
            dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = '" + newNumber + "' WHERE `Typ` = 'Eigenbelegnummer'");
            Eigenbelege.ExecuteQuery(query);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string[] CheckForApostrophe(string[] input)
        {
            string[] returnValues = new string[0];  
            foreach (var item in input)
            {
                if (item.Contains("'"))
                {
                    string adaptedValue = RemoveApostrophe(item);
                    string[] newArray = new string[] { adaptedValue };
                    returnValues = returnValues.Concat(newArray).ToArray();
                }
                else
                {
                    string[] newArray = new string[] { item };
                    returnValues = returnValues.Concat(newArray).ToArray();
                }
            }
            return returnValues;
        }
        public string RemoveApostrophe(string input)
        {
            StringBuilder result = new StringBuilder();
            foreach (char test in input)
            {
                if (test != '\'')
                {
                    result.Append(test);
                }
            }
            return result.ToString();
        }


        private void EigenbelegCreate_Load(object sender, EventArgs e)
        {
            var dbManager = new DBManager();
            newNumber = dbManager.ExecuteQueryWithResult("Config","Nummer","Typ","Eigenbelegnummer") + 1;
            LoadModelCombobox();
            textBox_eigenbelegNumber.Text = newNumber.ToString();
            textBox_eigenbelegDateBought.Text = DateTime.Now.ToString().Substring(0, 10);
            comboBox_eigenbelegArrived.Text = "Ja";
            comboBox_eigenbelegCreated.Text = "Nein";
        }
        public void LoadModelCombobox()
        {
            foreach (var item in ProductModelCatalog.iPhoneModels)
            {
                comboBox_eigenbelegeCreateDevice.Items.Add(item.ToString());
            }
            foreach (var item in ProductModelCatalog.samsungModels)
            {
                comboBox_eigenbelegeCreateDevice.Items.Add(item.ToString());
            }
        }


        private void comboBox_eigenbelegPlatform_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_eigenbelegPlatform.Text.Equals("BackMarket"))
            {
                comboBox_eigenbelegPaymentMethod.Text = "BuyBack / Lastschrift";
            }
            else 
            {
                comboBox_eigenbelegPaymentMethod.Text = "PayPal";
            }

        }

   


        private void textBox_eigenbelegTransactionAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_eigenbelegNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_eigenbelegSellerName_TextChanged(object sender, EventArgs e)
        {
            textBox_eigenbelegAdress.Text = textBox_eigenbelegSellerName.Text;
        }

        private void btn_buybackFillOut_Click(object sender, EventArgs e)
        {
            EigenbelegCreateInputBuyBack window = new EigenbelegCreateInputBuyBack();
            window.Show();
            this.Close();
            using (var form = new EigenbelegCreateInputBuyBack())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    BackMarketAPIHandler backMarketAPIHandler = new BackMarketAPIHandler();
                    string[] feedback = backMarketAPIHandler.PullBuyBackDataFromOrderID(form.shippingNumber);
                    textBox_eigenbelegSellerName.Text = feedback[2];
                    textBox_eigenbelegTransactionAmount.Text = CalculateBackMarketAmount(feedback[4]);
                    comboBox_eigenbelegeCreateDevice.Text = feedback[0];
                    comboBox_eigenbelegStorage.Text = feedback[1];
                    textBox_eigenbelegAdress.Text = feedback[3];
                    textBox_eigenbelegReference.Text = feedback[5];
                    comboBox_eigenbelegPlatform.Text = "BackMarket";
                    comboBox_eigenbelegPaymentMethod.Text = "BuyBack / Lastschrift";
                    MessageBox.Show("Zustand Allgemein: " + feedback[6] + "\r\n\r\n" + feedback[7]);
                }
            }
        }
        public string CalculateBackMarketAmount (string adaptValue)
        {
            adaptValue = adaptValue.Replace(".",",");
            double temp = Convert.ToDouble(adaptValue) + (Convert.ToDouble(adaptValue) * 0.1) + 10.9;
            return temp.ToString(); 
        }

  
    }
}
