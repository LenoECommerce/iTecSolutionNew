using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EigenbelegToolAlpha
{
    public class HelperClass
    {
        //public static string GetIMEIViaInternalNumber(string internalNumber)
        //{
        //    DBManager db = new DBManager();
        //    return db.ExecuteQueryWithResultString("Reparaturen","IMEI","Intern",internalNumber);
        //}
        public static string RemoveEuroSign(string input)
        {
            if (input.Contains("€"))
            {
                input = input.Replace("€", "");
            }

            return input;
        }
        public static string GetValueWithDelimiterAdd(string input, string existingData)
        {
            if (string.IsNullOrEmpty(existingData))
            {
                return input;
            }
            else
            {
                return existingData + ";" + input;
            }
        }
    }
}
