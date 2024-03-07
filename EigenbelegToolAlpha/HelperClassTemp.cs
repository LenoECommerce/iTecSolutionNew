using ExcelLibrary.BinaryFileFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EigenbelegToolAlpha
{
    public class HelperClassTemp
    {
        public static string GetIMEIViaInternalNumber(string internalNumber)
        {
            DBManager db = new DBManager();
            return db.ExecuteQueryWithResultString("Reparaturen", "IMEI", "Intern", internalNumber);
        }

        public static int ReturnInternalNumberFromBarcode(string input)
        {
            int firstFiveDigits = Convert.ToInt32(input.Substring(0, 5));
            string formattedNumber = firstFiveDigits.ToString("D5");
            return int.Parse(formattedNumber);
        }

    }
}
