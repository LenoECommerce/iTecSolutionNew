using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace EigenbelegToolAlpha.BackMarket
{
    public partial class BuyBackWeeklyVolumes : Form
    {
        public static MySqlConnection conn;
        public static string connString = "SERVER=212.227.140.237;PORT=3306;Initial Catalog='sql11525524';username=somedude;password=!mRAqn.Pl83.X8H0CuB28;Pooling=True;";
        public static DateTime today = DateTime.Now;
        //public static DateTime today = Convert.ToDateTime("13.01.2023");
        public BuyBackWeeklyVolumes()
        {
            InitializeComponent();
            ShowWeeks();
            ShowCurrentValues();
        }
        public static Dictionary<int, int> dayOfTimeSpaceBegin = new Dictionary<int, int>
        {
            {1,26},
            {2,6},
            {3,16},
        };
        public static void ShowCurrentValues()
        {
            string[] values = GetTemporaryCalculation();
            MessageBox.Show($"Aktuelle Werte\r\n\r\nVolumen: {values[1]}\r\nOrders: {values[0]}");
        }
        public static string[] GetTemporaryCalculation()
        {
            string[] returnValuesString = new string[2];
            int day = dayOfTimeSpaceBegin[GiveNumberOfTimeSpace()];
            DateTime beginCurrentTimeSpace = GetLastDateForDay(day);
            DateTime endDate = DateTime.Today;
            BackMarketAPIHandler backmarketAPI = new BackMarketAPIHandler();

            double[] returnValues = backmarketAPI.GetVolumeOfSpecificWeek(beginCurrentTimeSpace, endDate);
            var dbManager = new DBManager();
            returnValuesString[0] = (returnValues[0] * Convert.ToDouble(dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "BuyBackArrivalRate"))).ToString();
            returnValuesString[1] = AddPaymentAndArrivalRate(returnValues[1]).ToString();
            return returnValuesString;
        }
        public static void MainCalc()
        {
            DateTime beginOfLastTimeSpace = GiveBeginOfLastTimeSpace();
            DateTime endOfLastTimeSpace = GiveEndOfLastTimeSpace();
            BackMarketAPIHandler backmarketAPI = new BackMarketAPIHandler();
            var dbManager = new DBManager();
            int id = dbManager.ExecuteQueryWithResult("BuyBackWeeklyVolumes", "Id", "StartOfWeek", beginOfLastTimeSpace.ToString());
            if (id == 0)
            {
                //Datenbankeintrag erstellen
                double[] returnValues = backmarketAPI.GetVolumeOfSpecificWeek(beginOfLastTimeSpace, endOfLastTimeSpace);
                string totalOrders = (returnValues[0] * Convert.ToDouble(dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "BuyBackArrivalRate"))).ToString();
                string totalOrdersWithoutDecution = returnValues[0].ToString();
                string totalVolume = AddPaymentAndArrivalRate(returnValues[1]).ToString();
                string query = string.Format("INSERT INTO `BuyBackWeeklyVolumes` (`StartOfWeek`,`TotalVolume`,`TotalOrders`,`TotalOrdersWithoutDeduction`) VALUES ('{0}','{1}','{2}','{3}')"
                                             , beginOfLastTimeSpace, totalVolume, totalOrders, totalOrdersWithoutDecution);
                dbManager.ExecuteQuery(query);
            }
        }
        public static DateTime GetLastDateForDay(int day)
        {
            DateTime currentDate = DateTime.Now; // You can replace this with your desired date

            // Create a new DateTime object with the given day, using the current year and month
            DateTime inputDate = new DateTime(currentDate.Year, currentDate.Month, day);

            // If the input date is greater than the current date, subtract one month to get the last date for the given day
            if (inputDate > currentDate)
            {
                inputDate = inputDate.AddMonths(-1);
            }

            DateTime lastDate = inputDate;

            return lastDate;
        }
        public static DateTime GiveBeginOfLastTimeSpace()
        {
            var numberOfTimeSpace = GiveNumberOfTimeSpace();
            int dayLastSpace = 0;
            if (numberOfTimeSpace == 1)
            {
                dayLastSpace = dayOfTimeSpaceBegin[3];
            }
            else
            {
                dayLastSpace = dayOfTimeSpaceBegin[numberOfTimeSpace - 1];
            }
            return ReturnTheLastDateWhereDayMatchs(dayLastSpace);
        }
        public static DateTime GiveEndOfLastTimeSpace()
        {
            var numberOfTimeSpace = GiveNumberOfTimeSpace();
            int dayLastSpace = dayOfTimeSpaceBegin[numberOfTimeSpace];
            return ReturnTheLastDateWhereDayMatchs(dayLastSpace);
        }
        public static DateTime ReturnTheLastDateWhereDayMatchs(int dayOfMonth)
        {

            // Check if the day is in the current month
            if (dayOfMonth <= today.Day)
            {
                DateTime lastDate = new DateTime(today.Year, today.Month, dayOfMonth);

                // Check if the last date is in the future
                if (lastDate > today)
                {
                    lastDate = lastDate.AddMonths(-1);
                }

                return lastDate;
            }
            else
            {
                // The last date is in the previous month
                DateTime lastDate = new DateTime(today.Year, today.Month, 1).AddDays(-1);
                lastDate = new DateTime(lastDate.Year, lastDate.Month, dayOfMonth);
                return lastDate;
            }
        }
        public static int GiveNumberOfTimeSpace()
        {
            var day = today.Day;
            if (day < 6)
            {
                return 1;
            }
            else if (day < 16)
            {
                return 2;
            }
            else if (day < 26)
            {
                return 3;
            }
            else
            {
                return 1;
            }
        }
        public static double AddPaymentAndArrivalRate(double inputValue)
        {
            var dbManager = new DBManager();
            double arrivalRate = Convert.ToDouble(dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "BuyBackArrivalRate"));
            double paymentRate = Convert.ToDouble(dbManager.ExecuteQueryWithResultString("Config", "Nummer", "Typ", "BuyBackPaymentRate"));
            return inputValue * arrivalRate * paymentRate;
        }
        public static DateTime AddSixDaysAndTwentyThreeHours(DateTime date)
        {
            // Add six days to the input date
            DateTime newDate = date.AddDays(6);

            // Add 23 hours to the new date
            newDate = newDate.AddHours(23);

            return newDate;
        }
        public static string GetLastWeekMondayAsString()
        {
            DateTime today = DateTime.Today;
            int daysSinceMonday = ((int)today.DayOfWeek - 1 + 7) % 7;
            DateTime lastMonday = today.AddDays(-daysSinceMonday - 7);
            return lastMonday.ToString("dd.MM.yyyy");
        }

        public void ShowWeeks()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connString;
            conn.Open();

            string query = "SELECT * FROM `BuyBackWeeklyVolumes`";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            ////Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            //Daten anzeigen im Grid
            buyBackWeeklyNumbersDGV.DataSource = dataSet.Tables[0];

            buyBackWeeklyNumbersDGV.Columns[0].Visible = false;

            //Sortierte Ansicht
            buyBackWeeklyNumbersDGV.Sort(buyBackWeeklyNumbersDGV.Columns[0], ListSortDirection.Descending);
            conn.Close();
        }

        private void BuyBackWeeklyVolumes_Load(object sender, EventArgs e)
        {

        }
    }
}
