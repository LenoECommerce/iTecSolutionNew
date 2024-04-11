using Microsoft.Office.Interop.Excel;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public class EvaluationNewApproach
    {
        public string orderId { get; set; }
        public string imei { get; set; }
        public string internalNumber { get; set; }
        public string amount { get; set; }
        public string externalCosts { get; set; }
        public string taxesType { get; set; }
        public string model { get; set; }
        public string purchaseGrade { get; set; }
        public string marketPlaceFees { get; set; }
        public string taxes { get; set; }
        public string profit { get; set; }
        public string margin { get; set; }
        public string salesVolume { get; set; }



        public EvaluationNewApproach()
        {

        }

        public void Main()
        {
            string[] orderIds = CollectAllOrderIds();

            foreach (var item in orderIds)
            {
                orderId = item;
                CollectRelevantData();

                // sometimes no imei, maybe canceled
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                CalculateEverything();
                StoreValuesToArrays();
            }

            ExcelStuff(orderIds);
            MessageBox.Show("Excel Datei erfolgreich erzeugt.");
        }

        private string[] CollectAllOrderIds()
        {
            DBManager db = new DBManager();
            return db.GetValuesFromOneAttribute("Bestellnummer", "Protokollierung");
        }

        private void ExcelStuff(string[] orderIds)
        {
            SalesDataAnalyticsXSL xsl = new SalesDataAnalyticsXSL();
            xsl.CreateXSLFile(
                orderIds,
                EvaluationsTempDataStorage.model,
                EvaluationsTempDataStorage.purchaseGrades,
                EvaluationsTempDataStorage.profits,
                EvaluationsTempDataStorage.margins);
        }

        private void StoreValuesToArrays()
        {
            EvaluationsTempDataStorage.model = AddToArray(model, EvaluationsTempDataStorage.model);
            EvaluationsTempDataStorage.purchaseGrades = AddToArray(purchaseGrade, EvaluationsTempDataStorage.purchaseGrades);
            EvaluationsTempDataStorage.orderIDs = AddToArray(orderId, EvaluationsTempDataStorage.orderIDs);
            EvaluationsTempDataStorage.internalNumbers = AddToArray(internalNumber, EvaluationsTempDataStorage.internalNumbers);
            EvaluationsTempDataStorage.amounts = AddToArray(amount, EvaluationsTempDataStorage.amounts);
            EvaluationsTempDataStorage.externalCostsArray = AddToArray(externalCosts, EvaluationsTempDataStorage.externalCostsArray);
            EvaluationsTempDataStorage.taxesTypes = AddToArray(taxesType, EvaluationsTempDataStorage.taxesTypes);
            EvaluationsTempDataStorage.taxesArray = AddToArray(taxes, EvaluationsTempDataStorage.taxesArray);
            EvaluationsTempDataStorage.marketPlaceFeesArray = AddToArray(marketPlaceFees, EvaluationsTempDataStorage.marketPlaceFeesArray);
            EvaluationsTempDataStorage.profits = AddToArray(profit.ToString(), EvaluationsTempDataStorage.profits);
            EvaluationsTempDataStorage.margins = AddToArray(margin.ToString(), EvaluationsTempDataStorage.margins);
        }

        private void CollectRelevantData()
        {
            DBManager db = new DBManager();

            imei = db.ExecuteQueryWithResultString("Protokollierung", "IMEI", "Bestellnummer", orderId);

            internalNumber = db.ExecuteQueryWithResultString("Reparaturen", "Intern", "IMEI", imei);
            model = db.ExecuteQueryWithResultString("Reparaturen", "Geraet", "Intern", internalNumber);
            purchaseGrade = db.ExecuteQueryWithResultString("Reparaturen", "BuyBackGrade", "Intern", internalNumber);
            amount = db.ExecuteQueryWithResultString("Reparaturen", "Kaufbetrag", "Intern", internalNumber);
            externalCosts = db.ExecuteQueryWithResultString("Reparaturen", "ExterneKosten", "Intern", internalNumber);
            taxesType = db.ExecuteQueryWithResultString("Reparaturen", "Besteuerung", "Intern", internalNumber);
        }

        private string[] AddToArray(string value, string[] array)
        {
            string[] newArray = new string[array.Length + 1];
            Array.Copy(array, newArray, array.Length);
            newArray[array.Length] = value;
            return newArray;
        }

        public void CalculateEverything()
        {
            DBManager db = new DBManager();
            string marketplace = db.ExecuteQueryWithResultString(
                "Protokollierung",
                "Marktplatz",
                "Bestellnummer",
                orderId);

            salesVolume = BackMarketAPIHandler.GetFieldOrder1LayerArray(orderId, "orderlines", "price");
            AdaptSalesVolume();
            CollectAllMarketPlaceFees(marketplace);

            // calculate main core figures
            taxes = RoundOneDigit(CalcTaxes()).ToString();
            profit = RoundOneDigit(CalcProfit()).ToString();
            margin = RoundOneDigit(CalcMargin()).ToString();
        }

        private void CollectAllMarketPlaceFees(string marketplace)
        {
            double tempValue = 0;

            if (marketplace == "BackMarket")
            {
                tempValue += GetShippingCosts();
                tempValue += Convert.ToDouble(salesVolume) * 0.12;
                tempValue += 5.99;
            }

            else if (marketplace == "Ebay")
            {
                // leave out because iTec only sells on BackMarket
            }

            marketPlaceFees = tempValue.ToString();
        }

        private double GetShippingCosts()
        {
            string orderCountry = BackMarketAPIHandler.GetSpecificFieldOfOrder(orderId, "country_code");
            if (orderCountry == "de-de")
            {
                return 5;
            }
            else
            {
                return 10;
            }
        }

        public double CalcTaxes()
        {
            // just for differential taxation
            double profit = Convert.ToDouble(salesVolume) - Convert.ToDouble(amount);
            double haveToPayTax = profit / 1.19 * 0.19;
            return haveToPayTax;
        }

        public double CalcProfit()
        {
            // workaround because externalCosts can be empty RN -> no motivation :c
            if (string.IsNullOrEmpty(externalCosts))
            {
                externalCosts = "0";
            }

            double profit = Convert.ToDouble(salesVolume) -
                             Convert.ToDouble(amount) -
                             Convert.ToDouble(externalCosts) -
                             Convert.ToDouble(taxes) -
                             Convert.ToDouble(marketPlaceFees);
            return profit;
        }

        public void AdaptSalesVolume()
        {
            salesVolume = salesVolume.Replace(".", ",");
        }

        public double CalcMargin()
        {
            double margin = Convert.ToDouble(profit) / Convert.ToDouble(salesVolume);
            return margin;
        }

        public double RoundOneDigit(double adaptValue)
        {
            string tempValue = adaptValue.ToString();
            if (tempValue.Contains(","))
            {
                var pos = tempValue.IndexOf(",");
                tempValue = tempValue.Substring(0, pos + 2);
                adaptValue = Convert.ToDouble(tempValue);
            }
            return adaptValue;
        }



    }
}
