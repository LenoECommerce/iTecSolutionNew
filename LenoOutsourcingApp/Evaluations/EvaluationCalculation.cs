using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class EvaluationCalculation : Form
    {
        public static double backMarketGrossSalesVolumeMarginalVat = 0;
        public static double backMarketGrossSalesVolumeNormalVat = 0;
        public static double backMarketReturnsMarginalVat = 0;
        public static double backMarketReturnsNormalVat = 0;
        public static double backMarketDefferedPayout = 0;
        public static double backMarketOutcome;
        public static double backMarketGrossSalesTotal;
        public static double backMarketReturnsTotal;

        public static double backMarketPayPalGrossSalesVolumeTotal = 0;
        public static double backMarketPayPalGrossSalesVolumeMarginalVat = 0;
        public static double backMarketPayPalGrossSalesVolumeNormalVat = 0;
        public static double backMarketPayPalReturnsTotal = 0;
        public static double backMarketPayPalReturnsNormalVat = 0;
        public static double backMarketPayPalReturnsMarginalVat = 0;
        public static double backMarketPayPalOutcome = 0;
        public static double backMarketPayPalFees = 0;

        public static double inputOfGoodsREG = 0;
        public static double inputOfGoodsDIFF = 0;
        public static double inputOfExternalCosts = 0;

        public static double taxesREG = 0;
        public static double taxesDIFF = 0;
        public static double taxesGetBack = 0;
        public static double taxesHaveToPay = 0;

        public static double donorDevicesAmount = 0;
        public static int donorDevicesCounter = 0;

        public static int kpiDevicesPerMonthSold = 0;
        public static int kpiDevicesPerMonth = 0;
        public static int kpiSourceCounterEbay = 0;
        public static int kpiSourceCounterEbayKleinanzeigen = 0;
        public static int kpiSourceCounterBackMarket = 0;
        public static int checkValueAddedDevicesToMatching = 0;
        public static int checkValueDevicesCountedInput = 0;

        public string[] dataSetOrder = new string[10];
        //Data Set variables
        public string orderID = "";
        public string internalNumber = "";
        public string amount = "";
        public string externalCosts = "";
        public string externalCostsDIFF = "";
        public string taxesType = "";
        public string taxesAmount = "";
        public string marketplaceFees = "";
        public string revenue = "";
        public string margin = "";

        string idS = "";
        Dictionary<string, string> monthOverview = new Dictionary<string, string>
        {
            { "Januar", "01" },
    { "Februar", "02" },
    { "März", "03" },
    { "April", "04" },
    { "Mai", "05" },
    { "Juni", "06" },
    { "Juli", "07" },
    { "August", "08" },
    { "September", "09" },
    { "Oktober", "10" },
    { "November", "11" },
    { "Dezember", "12" }
        };

        Protokollierung prot = new Protokollierung();
        Reparaturen rep = new Reparaturen();
        EvaluationsFirstPage eval = new EvaluationsFirstPage();
        OrderRelationPDF orderRelationPDF = new OrderRelationPDF();
        EvaluationsSingleOrderCalculations calculations = new EvaluationsSingleOrderCalculations();
        public string month = "";
        public string year = "";
        public EvaluationCalculation()
        {
            InitializeComponent();
            donorDevicesAmount = 0;
            donorDevicesCounter = 0;
            month = EvaluationsFirstPage.month;
            year = EvaluationsFirstPage.year;
        }

        public void RunThroughMatchingEntries()
        {
            string orderCheck = "";
            try
            {
                Matching match = new Matching();
                var dbManager = new DBManager();
                foreach (DataGridViewRow row in match.matchingDGV.Rows)
                {
                    // check if month is relevant
                    string monthCheck = row.Cells[9].Value.ToString();
                    string yearCheck = row.Cells[11].Value.ToString();
                    orderCheck = row.Cells[1].Value.ToString();
                    if (true)
                    {

                    }
                    if (monthCheck == month && yearCheck == year)
                    {
                        var id = dbManager.ExecuteQueryWithResult("Matching", "Id", "Bestellnummer", orderCheck);
                        FetchDataFromMatching(id);
                        calculations.Main(orderID, internalNumber, amount, externalCosts, externalCostsDIFF, taxesType);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Die Methode RunThroughMatchingEntries() hat einen Fehler für folgende Bestellnummer " + orderCheck + ex.Message);
            }
        }
        private void FetchDataFromMatching(int id)
        {
            try
            {
                string table = "Matching";
                string searchTerm = "Id";
                idS = id.ToString();
                var dbManager = new DBManager();
                orderID = dbManager.ExecuteQueryWithResultString(table, "Bestellnummer", searchTerm, idS);
                internalNumber = dbManager.ExecuteQueryWithResultString(table, "Intern", searchTerm, idS);
                amount = AdaptNumber(dbManager.ExecuteQueryWithResultString(table, "Kaufbetrag", searchTerm, idS));
                externalCosts = AdaptNumber(dbManager.ExecuteQueryWithResultString(table, "Externe Kosten", searchTerm, idS));
                externalCostsDIFF = AdaptNumber(dbManager.ExecuteQueryWithResultString(table, "ExterneKostenDIFF", searchTerm, idS));
                taxesType = dbManager.ExecuteQueryWithResultString(table, "Besteuerung", searchTerm, idS);
                if (taxesType.Contains("Diff"))
                {
                    taxesType = "DIFF";
                }
                else if (taxesType.Contains("Reg"))
                {
                    taxesType = "REG";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Es gibt ein Problem mit FetchDataFromMatching\r\nMatching Id: " + idS + "\r\nFehler: " + e.Message);
            }
        }



        private void MatchingAlgorithm()
        {
            string searchOrderID = "";
            try
            {
                string searchIntern = "";
                string newIMEI = "";
                string newMonth = "";
                string marketplace = "";
                string taxes = "";
                string related = "";
                double rowsInTotal = prot.protokollierungDGV.RowCount;
                double approvedRows = 0;
                int addedRows = 0;
                EvaluationsFirstPage eval = new EvaluationsFirstPage();
                string month = EvaluationsFirstPage.month;
                string[] alreadyExisting = new string[] { };
                int alreadyExistsCounter = 0;
                int alreadyExistsCounterButMatchingMonth = 0;

                foreach (DataGridViewRow row in prot.protokollierungDGV.Rows)
                {
                    string tempAmount = "";
                    string tempExternalCosts = "";
                    string tempExternalCostsDiff = "";

                    approvedRows++;
                    searchOrderID = row.Cells[1].Value.ToString();
                    newMonth = row.Cells[5].Value.ToString();
                    searchIntern = row.Cells[3].Value.ToString();
                    var dbManager = new DBManager();
                    var resultExistsInMatching = dbManager.ExecuteQueryWithResult("Matching", "Id", "Bestellnummer", searchOrderID);
                    var resultExistsInternInMatching = dbManager.ExecuteQueryWithResult("Matching", "Id", "Intern", searchIntern);
                    //Überprüfen ob der Datensatz schon in Matching vorhanden ist + Ob Monat relevant ist + ob Intern schon vorhanden ist!
                    if (resultExistsInMatching == 0 && newMonth == month && resultExistsInternInMatching == 0)
                    {
                        //Data Pull aus Protokollierung
                        newIMEI = row.Cells[2].Value.ToString();
                        marketplace = row.Cells[4].Value.ToString();
                        related = "Ja";
                        //Data Pull aus Reparaturen
                        var id = dbManager.ExecuteQueryWithResult("Reparaturen", "Id", "Intern", searchIntern);

                        //case distinction for display sells
                        if (id == 0)
                        {
                            tempAmount = "0";
                            tempExternalCosts = "0";
                            tempExternalCostsDiff = "0";
                            taxes = "Differenzbesteuerung";
                        }
                        else
                        {
                            tempAmount = AdaptNumber(dbManager.ExecuteQueryWithResultString("Reparaturen", "Kaufbetrag", "Id", id.ToString()));
                            tempExternalCosts = AdaptNumber(dbManager.ExecuteQueryWithResultString("Reparaturen", "ExterneKosten", "Id", id.ToString()));
                            tempExternalCostsDiff = AdaptNumber(dbManager.ExecuteQueryWithResultString("Reparaturen", "ExterneKostenDIFF", "Id", id.ToString()));
                            taxes = dbManager.ExecuteQueryWithResultString("Reparaturen", "Besteuerung", "Id", id.ToString());
                        }
                        string query = String.Format("INSERT INTO `Matching`(`Bestellnummer`,`IMEI`,`Intern`,`Kaufbetrag`,`Externe Kosten`,`ExterneKostenDIFF`,`Marktplatz`,`Besteuerung`,`Monat`,`Jahr`,`Zugeordnet?`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')"
                                        , searchOrderID, newIMEI, searchIntern, tempAmount, tempExternalCosts, tempExternalCostsDiff, marketplace, taxes, newMonth, year, related);
                        addedRows++;
                        dbManager.ExecuteQuery(query);
                    }
                    else if (newMonth != month)
                    {
                        // add to array
                        string[] newArray = new string[] { searchOrderID };
                        alreadyExisting = alreadyExisting.Concat(newArray).ToArray();
                        alreadyExistsCounter++;
                    }
                    else if (newMonth == month)
                    {
                        alreadyExistsCounterButMatchingMonth++;
                    }
                }
                checkValueAddedDevicesToMatching = addedRows;
                MessageBox.Show("Der Matching Algorithmus wurde mit folgendem Ergebnis ausgeführt: \r\n- Einträge insgesamt: " + rowsInTotal + "\r\n- Durchlaufen: " + approvedRows + "\r\n- Passende Einträge: " + addedRows + "\r\n- Bereits existierende Aufträge: " + alreadyExistsCounter + "\r\n- Bereits existierende Aufträge (Monat matcht): " + alreadyExistsCounterButMatchingMonth);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Der Matching Algorithmus hat ein Problem mit {searchOrderID}: " + ex.Message);
            }

        }
        private string AdaptNumber(string checkValue)
        {
            string amount = checkValue;
            if (checkValue == "")
            {
                amount = "0";
            }
            if (checkValue.Contains("€"))
            {
                var length = checkValue.Length;
                amount = checkValue.Substring(0, length - 1);
            }
            return amount;
        }






        public double getValueOfOneLine(int index, string[] array, int lengthOfTheFirstPos, string firstPos, string secondPos)
        {
            string newValue = "";
            string tempSave = array[index].ToString();
            var fullLength = tempSave.Length;
            var posFirst = tempSave.IndexOf(firstPos);
            var posSecond = tempSave.IndexOf(secondPos);
            string tempValue = tempSave.Substring(posFirst + lengthOfTheFirstPos, posSecond - posFirst - lengthOfTheFirstPos - 1);
            //Erweiterung für Tausenderbeträge mit Leerzeichen
            if (checkSpaceSign(tempValue) == true)
            {
                if (checkMinusSign(tempValue) == true)
                {
                    var posSpace = tempValue.IndexOf(" ");
                    var length = tempValue.Length;
                    string temp1 = tempValue.Substring(1, 1);
                    string temp2 = tempValue.Substring(posSpace + 1, length - posSpace - 1);
                    tempValue = temp1 + temp2;
                }
                else
                {
                    var posSpace = tempValue.IndexOf(" ");
                    var length = tempValue.Length;
                    string temp1 = tempValue.Substring(0, 1);
                    string temp2 = tempValue.Substring(posSpace + 1, length - posSpace - 1);
                    tempValue = temp1 + temp2;
                }
            }
            if (checkMinusSign(tempValue) == true)
            {
                var length2 = tempValue.Length;
                tempValue = tempValue.Substring(0, length2 - 1);
            }
            if (tempValue == "0,00")
            {
                tempValue = "0";
            }

            newValue = tempValue;
            double value = Convert.ToDouble(newValue);
            return value;
        }
        public string getValueOfOneLineDefferedPayOut(int index, string[] array, int lengthOfTheFirstPos, string firstPos, string secondPos)
        {
            string tempSave = array[index].ToString();
            var fullLength = tempSave.Length;
            var posFirst = tempSave.IndexOf(firstPos);
            var posSecond = tempSave.IndexOf(secondPos);
            string tempValue = tempSave.Substring(posFirst + lengthOfTheFirstPos, posSecond - posFirst - lengthOfTheFirstPos - 1);
            return tempValue;
        }
        public bool checkZeroAmount(string tempValue)
        {
            if (tempValue == "0,00")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkMinusSign(string tempValue)
        {
            if (tempValue.Contains("-"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkSpaceSign(string tempValue)
        {
            if (tempValue.Contains(" "))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkReg(string[] array, int index)
        {
            string tempSave = array[index].ToString();
            if (tempSave.Contains("REG"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int findLine(string[] array, string searchValue)
        {
            int backValue = 0;
            for (int i = 1; i < array.Count(); i++)
            {
                if (array[i].Contains(searchValue))
                {
                    backValue = i + 1;
                    break;
                }
            }
            return backValue;
        }

        public static string ExtractTextFromPdf(string path)
        {
            try
            {
                using (PdfReader reader = new PdfReader(path))
                {
                    StringBuilder text = new StringBuilder();

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                    }

                    return text.ToString();
                }
            }
            catch (Exception ex)
            {
                return "";
                MessageBox.Show(ex.Message);
            }
        }

        public string EuroCheck(string checkValue)
        {
            if (checkValue.Contains("€"))
            {
                var length = checkValue.Length;
                var newValue = checkValue.Substring(0, length - 1);
                return newValue;
            }
            else
            {
                return checkValue;
            }
        }
        public bool CheckTaxesForInputs(string checkValue)
        {
            if (checkValue == "Differenzbesteuerung")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CollectAndSetKPIs()
        {
            // donor devices
            var dbManager = new DBManager();
            try
            {
                donorDevicesAmount = dbManager.SelectSumQuery2Conditions(
                    table: "Reparaturen",
                    searchingColumn: "Kaufbetrag",
                    conditionColumn1: "Spendermonat",
                    conditionValue1: month,
                    conditionColumn2: "Spenderjahr",
                    conditionValue2: year);
                donorDevicesCounter = dbManager.CountRows2Conditions(
                    table: "Reparaturen",
                    conditionColumn1: "Spendermonat",
                    conditionValue1: month,
                    conditionColumn2: "Spenderjahr",
                    conditionValue2: year);

                MessageBox.Show("Der Spender Algorithmus wurde mit folgendem Ergebnis ausgeführt: \r\n- Betrag insgesamt: " + donorDevicesAmount + "\r\n- Geräte insgesamt: " + donorDevicesCounter);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Der Spender Algorithmus hat folgenden Fehler: " + ex.Message);
            }


            // rest KPIs
            string errorHandlingReference = "";
            try
            {
                // reset, I think because of bullshit object creation logic, haha xd
                kpiDevicesPerMonth = 0;
                // get devices handled by the infrastructure of every month | every device that arrives
                string monthYearFilter = $"{monthOverview[month]}.{year}";
                kpiDevicesPerMonth = dbManager.CountRows1Condition_LikeForDateFiltering(
                    table: "Eigenbelege",
                    conditionColumn: "Kaufdatum",
                    conditionValue: monthYearFilter);
                // distingiush between different sources
                kpiSourceCounterEbayKleinanzeigen = dbManager.CountRows2Conditions_LikeForDateFiltering(
                    table: "Eigenbelege",
                    conditionColumn: "Plattform",
                    conditionValue: "Ebay Kleinanzeigen",
                    dateColumn: "Kaufdatum",
                    dateValue: monthYearFilter);
                kpiSourceCounterEbay = dbManager.CountRows2Conditions_LikeForDateFiltering(
                    table: "Eigenbelege",
                    conditionColumn: "Plattform",
                    conditionValue: "Ebay",
                    dateColumn: "Kaufdatum",
                    dateValue: monthYearFilter);
                kpiSourceCounterBackMarket = dbManager.CountRows2Conditions_LikeForDateFiltering(
                    table: "Eigenbelege",
                    conditionColumn: "Plattform",
                    conditionValue: "BackMarket",
                    dateColumn: "Kaufdatum",
                    dateValue: monthYearFilter);

                //sold devices per month
                kpiDevicesPerMonthSold = dbManager.CountRows2Conditions(
                    table: "Matching",
                    conditionColumn1: "Monat",
                    conditionValue1: month,
                    conditionColumn2: "Jahr",
                    conditionValue2: year);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Der weitere KPI Algorithmus hat ein Problem für folgende Order: " + ex.Message);

            }

        }
        private void btn_TaxCalculation_Click(object sender, EventArgs e)
        {

        }

        public string RoundNumber(string tempNumber)
        {
            var length = tempNumber.Length;
            var indexComma = tempNumber.IndexOf(",");
            string preComma = tempNumber.Substring(0, indexComma);
            string afterComma = tempNumber.Substring(indexComma + 1, 2);
            string newNumber = preComma + "," + afterComma;
            return newNumber;
        }

        private void EvaluationCalculation_Load(object sender, EventArgs e)
        {

        }


        public void UploadPDF()
        {
            var dbManager = new DBManager();
            int returnValue = Convert.ToInt32(dbManager.ExecuteQueryWithResultStringTwoConditions("Evaluations", "Id", "Monat", "Jahr", month, year));
            GoogleDrive drive = new GoogleDrive(MonthlyReportPDF.monthlyReportFinishedPath, "pdf");
            if (returnValue == 0)
            {
                string query = string.Format("INSERT INTO `Evaluations` (`Monat`,`Jahr`,`Link`) VALUES ('{0}','{1}','{2}')"
                                , month, year, GoogleDrive.currentLink);
                dbManager.ExecuteQuery(query);
                MessageBox.Show("Der Monat " + month + " wurde erstmalig angelegt.");
            }
            else
            {
                string query = "UPDATE `Evaluations` SET `Link` = '" + GoogleDrive.currentLink + "' WHERE `Id` = " + returnValue;
                dbManager.ExecuteQuery(query);
                MessageBox.Show("Der Monat " + month + " wurde überschrieben.");
            }
            this.Close();
        }
        private void btn_ExecuteAllAlgorithms_Click(object sender, EventArgs e)
        {
            MatchingAlgorithm();
            CollectAndSetKPIs();
            RunThroughMatchingEntries();
            try
            {
                calculations.DrawTillDawn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erstellung der Order Relation PDF Datei hat ein Problem: " + ex.Message);
            }
            try
            {
                MonthlyReportPDF.CreatePDFFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erstellung der Monthly Report PDF Datei hat ein Problem: " + ex.Message);
            }
            try
            {
                UploadPDF();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Uploading Methode hat ein Problem: " + ex.Message);
            }
        }
    }
}