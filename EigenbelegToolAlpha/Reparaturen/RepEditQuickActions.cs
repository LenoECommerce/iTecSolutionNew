using System;

namespace EigenbelegToolAlpha
{
    public class RepEditQuickActions
    {
        public DateTime today = DateTime.Now;
        public string currentUser = UserFileManagement.ReturnCurrentUser();

        public void PrintDFUResetNecessary(string repNumber)
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

        public void PrintIMEINotRecognizable(string repNumber)
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

        public void PrintCounterOffer(string reason, string repNumber)
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
        public void PrintLockedDevice(string repNumber)
        {
            var dbManager = new DBManager();
            string path = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathTemplateLockedDevice", "Nutzer", currentUser);
            bpac.Document doc = new bpac.Document();
            doc.Open(path);
            doc.SetPrinter("Brother QL-600", true);
            string recyclingDate = GetDeadlineForLockedDevice().ToShortDateString();
            doc.GetObject("input").Text = $"{today.ToShortDateString()} by {currentUser}\r\nDeadline: {recyclingDate}";
            doc.GetObject("rep").Text = $"REP{repNumber}";
            MainPrintEndStuff(doc);
        }
        public void PrintReceivedSimCard(string repNumber)
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

        public void MainPrintEndStuff(bpac.Document doc)
        {
            doc.StartPrint("", bpac.PrintOptionConstants.bpoDefault);
            doc.PrintOut(1, bpac.PrintOptionConstants.bpoDefault);
            doc.EndPrint();
            doc.Close();
        }

        #region helper methods
        public DateTime GetRecyclingDeadline()
        {
            return today.AddDays(7);
        }

        public string GetBuyBackOrderIdViaREPNumber(string repNumber)
        {
            var dbManager = new DBManager();
            string ebNumber = dbManager.ExecuteQueryWithResultString("Reparaturen", "EBReferenz", "Intern", repNumber);
            return dbManager.ExecuteQueryWithResultString("Eigenbelege", "Referenz", "Eigenbelegnummer", ebNumber);
        }

        public DateTime GetDeadlineForSimCard()
        {
            return today.AddMonths(1);
        }

        public DateTime GetDeadlineForLockedDevice()
        {
            return today.AddDays(50);
        }
        #endregion
    }
}
