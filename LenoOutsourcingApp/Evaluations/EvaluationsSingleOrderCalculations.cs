using EigenbelegToolAlpha.Evaluations;
using System;

namespace EigenbelegToolAlpha
{
    public class EvaluationsSingleOrderCalculations
    {
        EvaluationsBackMarketPDF evaluationsBackMarketPDF = new EvaluationsBackMarketPDF();
        EvaluationsEbayPDF evaluationsEbay = new EvaluationsEbayPDF();
        EvaluationsFirstPage EvaluationsFirstPage = new EvaluationsFirstPage();
        public static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
        public string fullPath = desktopPath + "test.pdf";
        public double headingPosY = 30;
        public double entriesAdded = 0;
        //calcs
        public string ccbmFee;
        public string sellerCommission = "";
        public string marketplaceFees = "";
        public string salesVolume = "";
        public string pdfFound = "";
        public double paymentFees = 0;
        public double marketPlaceFeesEbay = 0;
        public static double returnAmount = 0;
        public double taxes = 0;
        public double revenue = 0;
        public double margin = 0;
        public static double revenueDisplays = 0;
        public static int counterDisplayOrders = 0;

        public static int beginLineEbay = 0;
        public double paymentFeesNotPayPalTotal = 0;
        public double OrdersNotPayPal = 0;
        public double paymentFeesNotPayPalPerOrder = 0;
        public static double backShipCosts = 0;

        //monthly report sum up values
        public static double grossSalesEbay = 0;
        public static double grossSalesBackmarketNormal = 0;
        public static double grossSalesBackmarketPayPal = 0;
        public static double grossSalesExternalSells = 0;
        public static double grossSalesDisplays = 0;
        public static double revenueTotal = 0;

        public EvaluationsSingleOrderCalculations()
        {
            var dbManager = new DBManager();
            ccbmFee = dbManager.ExecuteQueryWithResultString("ConfigFees", "Costs", "Typ", "BackMarketCCBM");
        }

        public void Main(string orderId, string internalNumber, string amount, string externalCosts, string externalCostsDiff, string taxesType)
        {
            CalculateEverything(orderId, internalNumber, amount, externalCosts, externalCostsDiff, taxesType);
            // add values to arrays
            OrderRelationPDF.orderIDs = AddToArray(orderId, OrderRelationPDF.orderIDs);
            OrderRelationPDF.internalNumbers = AddToArray(internalNumber, OrderRelationPDF.internalNumbers);
            OrderRelationPDF.amounts = AddToArray(amount, OrderRelationPDF.amounts);
            OrderRelationPDF.externalCostsArray = AddToArray(externalCosts, OrderRelationPDF.externalCostsArray);
            OrderRelationPDF.externalCostsDiffArray = AddToArray(externalCostsDiff, OrderRelationPDF.externalCostsDiffArray);
            OrderRelationPDF.taxesTypes = AddToArray(taxesType, OrderRelationPDF.taxesTypes);
            OrderRelationPDF.taxesArray = AddToArray(taxes.ToString(), OrderRelationPDF.taxesArray);
            OrderRelationPDF.marketPlaceFeesArray = AddToArray(marketplaceFees, OrderRelationPDF.marketPlaceFeesArray);
            OrderRelationPDF.profits = AddToArray(revenue.ToString(), OrderRelationPDF.profits);
            OrderRelationPDF.margins = AddToArray(margin.ToString(), OrderRelationPDF.margins);
            OrderRelationPDF.paymentFeesArray = AddToArray(paymentFees.ToString(), OrderRelationPDF.paymentFeesArray);
            OrderRelationPDF.backShipCostsArray = AddToArray(backShipCosts.ToString(), OrderRelationPDF.backShipCostsArray);
            OrderRelationPDF.sellerCommissionArray = AddToArray(sellerCommission, OrderRelationPDF.sellerCommissionArray);
        }
        public void DrawTillDawn()
        {
            OrderRelationPDF.Draw();
            EvaluationsTestClassPDF.PDFBreakDownValues(OrderRelationPDF.orderIDs, OrderRelationPDF.internalNumbers, OrderRelationPDF.amounts, OrderRelationPDF.externalCostsArray,
                OrderRelationPDF.externalCostsDiffArray, OrderRelationPDF.taxesTypes, OrderRelationPDF.taxesArray, OrderRelationPDF.marketPlaceFeesArray, OrderRelationPDF.profits,
                OrderRelationPDF.margins, OrderRelationPDF.paymentFeesArray, OrderRelationPDF.backShipCostsArray, OrderRelationPDF.sellerCommissionArray, ccbmFee);
        }


        public string[] AddToArray(string value, string[] array)
        {
            string[] newArray = new string[array.Length + 1];
            Array.Copy(array, newArray, array.Length);
            newArray[array.Length] = value;
            return newArray;
        }

        public void CalculateEverything(string orderId, string internalNumber, string amount, string externalCosts, string externalCostsDiff, string taxesType)
        {
            if (CheckMarketPlace(orderId) == "BackMarket")
            {
                SetBackShipCosts(orderId);
                CalculateFeesForBackMarket(orderId);
            }
            //external part
            else if (CheckMarketPlace(orderId) == "Extern")
            {
                CalculateFeesForExternalOrders(orderId);
            }
            //Ebay part
            else
            {
                CalculateFeesForEbay(orderId);
            }
            // calculate main core numbers
            taxes = RoundOneDigit(CalcTaxes(taxesType, salesVolume, amount, externalCosts, externalCostsDiff));
            revenue = RoundOneDigit(CalcRevenue(salesVolume, Convert.ToDouble(amount), Convert.ToDouble(externalCosts), Convert.ToDouble(externalCostsDiff), taxes, Convert.ToDouble(marketplaceFees)));
            revenueDisplays += CheckIfOrderIsDisplay(salesVolume, revenue, internalNumber);
            margin = RoundOneDigit(CalcMargin(salesVolume, revenue));
            revenueTotal += revenue;
        }
        public double CheckIfOrderIsDisplay(string salesVolume, double revenue, string internalNumber)
        {
            if (salesVolume == "N/A")
            {
                return 0;
            }
            if (internalNumber.Contains("DIS"))
            {
                grossSalesDisplays += Convert.ToDouble(salesVolume);
                counterDisplayOrders++;
                return revenue;
            }
            else
            {
                return 0;
            }
        }
        public void SetBackShipCosts(string orderId)
        {
            //check if order ID is from type "orderline"
            if (orderId.Contains("."))
            {
                orderId = orderId.Substring(0, 8);
            }
            string orderCountry = BackMarketAPIHandler.GetSpecificFieldOfOrder(orderId, "country_code");
            if (orderCountry != "de-de")
            {
                var dbManager = new DBManager();
                backShipCosts = Convert.ToDouble(dbManager.ExecuteQueryWithResultString("ConfigFees", "Costs", "Typ", "BackMarketBackShip"));
            }
            else
            {
                backShipCosts = 0;
            }
        }
        public void CalculateFeesForEbay(string orderId)
        {
            //check if order ID is from type "orderline"
            if (orderId.Contains("."))
            {
                var dbManager = new DBManager();
                salesVolume = dbManager.ExecuteQueryWithResultString("Protokollierung", "SalesVolumeOrderLine", "Bestellnummer", orderId);
                orderId = orderId.Substring(0, 14);
            }
            else
            {
                salesVolume = evaluationsEbay.GetSalesVolume(orderId);
            }
            grossSalesEbay += Convert.ToDouble(salesVolume);
            marketPlaceFeesEbay = RoundOneDigit(evaluationsEbay.GetSellerCommission(orderId) + (Convert.ToDouble(salesVolume) * 0.02));
            sellerCommission = marketPlaceFeesEbay.ToString();
            marketplaceFees = marketPlaceFeesEbay.ToString();
        }
        public void CalculateFeesForExternalOrders(string orderId)
        {
            int length = orderId.Length;
            int posSpace = orderId.IndexOf(" ");
            salesVolume = orderId.Substring(posSpace, length - posSpace - 1);
            marketplaceFees = "0";
            if (salesVolume != "N/A")
            {
                grossSalesExternalSells += Convert.ToDouble(salesVolume);
            }
        }
        public void CalculateFeesForBackMarket(string orderId)
        {
            //check if order ID is from type "orderline"
            var dbManager = new DBManager();
            if (orderId.Contains("."))
            {
                salesVolume = dbManager.ExecuteQueryWithResultString("Protokollierung", "SalesVolumeOrderLine", "Bestellnummer", orderId);
                orderId = orderId.Substring(0, 8);
            }
            else
            {
                salesVolume = evaluationsBackMarketPDF.GetSalesVolume(orderId, pdfFound);
            }

            grossSalesBackmarketNormal += Convert.ToDouble(salesVolume);
            string paymentMethod = BackMarketAPIHandler.GetSpecificFieldOfOrder(orderId, "payment_method").ToLower();
            //payment method distinction
            if (paymentMethod.Contains("oney"))
            {
                double oneyPercentage = EvaluationHelperClass.ConvertPercentageStringToDouble(dbManager.ExecuteQueryWithResultString("ConfigFees", "Costs", "Typ", "BackMarketOney"));
                paymentFees = Convert.ToDouble(salesVolume) * oneyPercentage;
            }
            else if (paymentMethod.Contains("klarna"))
            {
                double klarnaPercentage = EvaluationHelperClass.ConvertPercentageStringToDouble(dbManager.ExecuteQueryWithResultString("ConfigFees", "Costs", "Typ", "BackMarketKlarna"));
                paymentFees = Convert.ToDouble(salesVolume) * klarnaPercentage;
            }
            else if (paymentMethod.Contains("paypal"))
            {
                double paypalPercentage = EvaluationHelperClass.ConvertPercentageStringToDouble(dbManager.ExecuteQueryWithResultString("ConfigFees", "Costs", "Typ", "BackMarketPayPalPercentage"));
                double paypalFixedAmount = Convert.ToDouble(dbManager.ExecuteQueryWithResultString("ConfigFees", "Costs", "Typ", "BackMarketPayPalFixedAmount"));
                paymentFees = Convert.ToDouble(salesVolume) * paypalPercentage + paypalFixedAmount;
                grossSalesBackmarketPayPal += Convert.ToDouble(salesVolume);
                //deduct simple logic because above its added
                grossSalesBackmarketNormal -= Convert.ToDouble(salesVolume);
            }
            // sum up
            double ccbmFeeDouble = Convert.ToDouble(ccbmFee);
            double sellerCommissionDouble = evaluationsBackMarketPDF.CalculateSellerCommission(salesVolume);
            sellerCommission = sellerCommissionDouble.ToString();
            double marketplaceFeesTemp = RoundOneDigit(sellerCommissionDouble + ccbmFeeDouble + backShipCosts + paymentFees);
            marketplaceFees = marketplaceFeesTemp.ToString();
        }

        public string CheckMarketPlace(string checkValue)
        {
            if (checkValue.Contains("-"))
            {
                return "Ebay";
            }
            else if (checkValue.Contains("€"))
            {
                return "Extern";
            }
            else
            {
                return "BackMarket";
            }
        }
        public double CalcTaxes(string taxesType, string salesVolume, string deviceAmount, string external, string externalDIFF)
        {
            if (salesVolume == "N/A")
            {
                return 0;
            }
            double getBackTax = 0;
            double normalTax = 0;
            double haveToPayTax = 0;
            if (taxesType.Contains("Reg"))
            {
                normalTax += Convert.ToDouble(salesVolume) / 1.19 * 0.19;
                getBackTax += Convert.ToDouble(deviceAmount) / 1.19 * 0.19;
            }
            else
            {
                normalTax += (Convert.ToDouble(salesVolume) - Convert.ToDouble(deviceAmount) - Convert.ToDouble(externalDIFF)) / 1.19 * 0.19;
            }
            getBackTax += Convert.ToDouble(external) / 1.19 * 0.19;
            haveToPayTax = normalTax - getBackTax;
            return haveToPayTax;
        }
        public double CalcRevenue(string salesVolume, double deviceAmount, double external, double externalDIFF, double taxes, double marketplaceFees)
        {
            if (salesVolume == "N/A")
            {
                return 0;
            }
            double revenue = Convert.ToDouble(salesVolume) - deviceAmount - external - externalDIFF - taxes - marketplaceFees;
            return revenue;
        }
        public double CalcMargin(string salesVolume, double revenue)
        {
            if (salesVolume == "N/A")
            {
                return 0;
            }
            double margin = revenue / Convert.ToDouble(salesVolume) * 100;
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
