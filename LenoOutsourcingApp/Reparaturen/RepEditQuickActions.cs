using System;

namespace EigenbelegToolAlpha
{
    public class RepEditQuickActions
    {
        public static DateTime today = DateTime.Now;
        public static string currentUser = UserFileManagement.ReturnCurrentUser();

        public static void PrintDFUResetNecessary(string repNumber)
        {
            var dbManager = new DBManager();
            string path = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathTemplateDFUReset", "Nutzer", currentUser);
            bpac.Document doc = new bpac.Document();
            doc.Open(path);
            doc.SetPrinter("Brother QL-600", true);
            doc.GetObject("noted").Text = $"{today.ToShortDateString()} by {currentUser}";
            doc.GetObject("rep").Text = $"REP{repNumber}";
            MainPrintEndStuff(doc);
        }
        public static void PrintIMEINotRecognizable(string repNumber)
        {
            var dbManager = new DBManager();
            string path = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathTemplateIdentifiable", "Nutzer", currentUser);
            bpac.Document doc = new bpac.Document();
            doc.Open(path);
            doc.SetPrinter("Brother QL-600", true);
            doc.GetObject("noted").Text = $"{today.ToShortDateString()} by {currentUser}";
            doc.GetObject("rep").Text = $"REP{repNumber}";
            MainPrintEndStuff(doc);
        }
        public static void PrintCounterOffer(string reason, string repNumber)
        {
            var dbManager = new DBManager();
            string path = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathTemplateCounterOffer", "Nutzer", currentUser);
            bpac.Document doc = new bpac.Document();
            doc.Open(path);
            doc.SetPrinter("Brother QL-600", true);
            doc.GetObject("input").Text = $"{today.ToShortDateString()} by {currentUser}\r\nReason: {reason}";
            doc.GetObject("rep").Text = $"REP{repNumber}";
            MainPrintEndStuff(doc);
        }
        public static void PrintLockedDevice(string repNumber)
        {
            var dbManager = new DBManager();
            string path = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathTemplateLockedDevice", "Nutzer", currentUser);
            bpac.Document doc = new bpac.Document();
            doc.Open(path);
            doc.SetPrinter("Brother QL-600", true);
            string recyclingDate = GetDeadlineForSimCard().ToShortDateString();
            doc.GetObject("input").Text = $"{today.ToShortDateString()} by {currentUser}\r\nDeadline: {recyclingDate}";
            doc.GetObject("rep").Text = $"REP{repNumber}";
            MainPrintEndStuff(doc);
        }
        public static void PrintReceivedSimCard(string repNumber)
        {
            var dbManager = new DBManager();
            string path = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathTemplateReceivedSimCard", "Nutzer", currentUser);
            bpac.Document doc = new bpac.Document();
            doc.Open(path);
            doc.SetPrinter("Brother QL-600", true);
            string orderId = GetBuyBackOrderIdViaREPNumber(repNumber);
            string deadline = GetDeadlineForSimCard().ToShortDateString();
            doc.GetObject("input").Text = $"Orderline: {orderId}\r\n" +
                                          $"Contacted:\r\n" +
                                          $"Deadline: {deadline}";
            MainPrintEndStuff(doc);
        }

        public static void MainPrintEndStuff(bpac.Document doc)
        {
            doc.StartPrint("", bpac.PrintOptionConstants.bpoDefault);
            doc.PrintOut(1, bpac.PrintOptionConstants.bpoDefault);
            doc.EndPrint();
            doc.Close();
        }

        #region helper methods
        public static DateTime GetRecyclingDeadline()
        {
            // seems to be 7 weeks
            const int daysOfWeek = 7;
            int weeks = 7;
            int totalDays = daysOfWeek * weeks;
            return today.AddDays(daysOfWeek);
        }
        public static string GetBuyBackOrderIdViaREPNumber(string repNumber)
        {
            var dbManager = new DBManager();
            string ebNumber = dbManager.ExecuteQueryWithResultString("Reparaturen", "EBReferenz", "Intern", repNumber);
            return dbManager.ExecuteQueryWithResultString("Eigenbelege", "Referenz", "Eigenbelegnummer", ebNumber);
        }
        public static DateTime GetDeadlineForSimCard()
        {
            return today.AddMonths(1);
        }
        #endregion
    }
}
