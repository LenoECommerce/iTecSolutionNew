using EigenbelegToolAlpha.ConstsAndEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EigenbelegToolAlpha
{
    public class GoogleSheetsConfig
    {
        // Dictionary to store spreadsheet IDs based on sheet names
        private Dictionary<string, string> spreadsheetIds = new Dictionary<string, string>();
        
        public string[] headingsGrossSalesOverview = 
        {
            "",
            "BM Normal",
            "BM PayPal",
            "Ebay",
            "Wachstum"
        };

        public string[] dataAttributesGrossSalesOverview = 
        {
            "Monat",
            "GrossSalesBackMarketNormal",
            "GrossSalesBackMarketPayPal",
            "GrossSalesEbay",
            "GrowthRateSales"
        };

        public string[] headingsRunningCosts = 
        {
            "",
            "Main"
        };

        public string[] dataAttributesRunningCosts = 
        {
            "Monat",
            "RunningCosts"
        };

        public string[] headingsSoldDevices =
        {
            "",
            "Costs / Device",
            "Sold Devices",
            "Profit / Device"
        };


        public string[] dataAttributesSoldDevices = 
        {
            "Monat",
            "CostsPerDevice",
            "SoldDevices",
            "ProfitPerDevice",
        };

        public string[] headingsSourcing = 
        {
            "",
            "Ebay",
            "EK",
            "BuyBack"
        };

        public string[] dataAttributesSourcing = 
        {
            "Monat",
            "PurchaseGoodsEbay",
            "PurchaseGoodsEbayKleinanzeigen",
            "PurchaseGoodsBackMarket"
        };

        public string[] headingsProfitGrossSales =
        {
            "",
            "Gewinn",
            "Umsatz"
        };

        public string[] dataAttributesProfitGrossSales =
        {
            "Monat",
            "ProfitAfterCosts",
            "GrossSalesTotal"
        };

        public string[] headingsMargin =
        {
            "",
            "Marge"
        };

        public string[] dataAttributesMargin =
        {
            "Monat",
            "Margin"
        };

        public string[] headingsReimbursements =
        {
            "",
            "Umsatz",
            "Erstattungen",
            "Rate"
        };

        public string[] dataAttributesReimbursements =
        {
            "Monat",
            "GrossSalesTotal",
            "TotalReturnAmount",
            "ReimbursementRate"
        };

        public string[] headingsProfitStreams =
        {
            "",
            "B2C",
            "B2B",
            "Reparatur"
        };

        public string[] dataAttributesProfitStreams =
        {
            "Monat",
            "ProfitAfterCosts",
            "B2BRevenue",
            //wird zwar nicht erfasst , ist hier aber trotzdem drin
            "GrossSalesRepairOrders",
        };

        // kpi stuff testen

        public string[] headingsKPIOrderCreatedAndFinished =
        {
            "Monat",
        };

        public string[] dataAttributesKPIOrderCreatedAndFinished =
        {
            "Monat",
        };

        public GoogleSheetsConfig()
        {
            spreadsheetIds.Add("GrossSalesOverview", "1plWh8qEAIifxCXuwMWFq-48VYLVnmGiD5mmrQ1aJefA");
            spreadsheetIds.Add("RunningCosts", "1-ils2ejLgXknqACKDSIKQtCnrvozTQWJQvaSJEXYW64");
            spreadsheetIds.Add("SoldDevices", "1WKueJgZMEP04HLWx5xV9xBryZi2fVU88pzAbNYGg_7g");
            spreadsheetIds.Add("Sourcing", "1YLKX6DPyA5NjE4m8CXJwEqRsUYoD493EQG0_2se_kbI");
            spreadsheetIds.Add("ProfitGrossSales", "1hLdy8sTBihsK3fKuv33Bi34Y2qMIkW7ioiGB7WdtyUU");
            spreadsheetIds.Add("Margin", "1lMZJwOPKR0m4_KcyMaYFuvV5ZXKsC0PtsvD96-0dYVw");
            spreadsheetIds.Add("Reimbursements", "1rN-c98ZcBSh4l8BXlUPDB2CFupCv8KKFxR04bSunRvg");
            spreadsheetIds.Add("DebtCapital", "12lJaJ71Mc38Alh6k_ZQEoRxP9cFTgA9b0XTy0mh39g8");
            spreadsheetIds.Add("ProfitStreams", "1ijAzvwXWBnkRMRuVd1oUdtCWHsQG7mZ7jyvUVdHXDKA");
            spreadsheetIds.Add("KPIOrderCreated", "1JYAb8mf5-yzT4g_gElE2KhIDanKFm7ZRh6mJOr_Kscs");
            spreadsheetIds.Add("KPIOrderFinished", "1HThd4AzUKxDUWT0Qbpsq0NIVkfIObmVXhv0zGyRRDwE");
            // new KPIs
            headingsKPIOrderCreatedAndFinished = headingsKPIOrderCreatedAndFinished.Concat(MainUserConsts.possibleUsers).ToArray();
            dataAttributesKPIOrderCreatedAndFinished = dataAttributesKPIOrderCreatedAndFinished.Concat(MainUserConsts.possibleUsers).ToArray();
        }

        public string GetSpreadsheetId(string sheetName)
        {
            // Try to retrieve the spreadsheet ID from the dictionary
            if (spreadsheetIds.TryGetValue(sheetName, out string spreadsheetId))
            {
                return spreadsheetId;
            }

            // Return null if the sheet name is not found
            return null;
        }
    }
}
