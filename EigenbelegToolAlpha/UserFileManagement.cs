using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Org.BouncyCastle.Crypto.Prng.Drbg;

namespace EigenbelegToolAlpha
{
    public class UserFileManagement
    {
        public static string fileName = "user.txt";
        public static string currentUser = "I-Tec";

        public static string ReturnCurrentUser()
        {
            return currentUser;
        }
        public static bool CheckIfUserFileEmpty()
        {
            if (File.ReadAllText(fileName) == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckIfUserOnboarding()
        {
            string user = ReturnCurrentUser();
            var dbManager = new DBManager();
            string dbValue = dbManager.ExecuteQueryWithResultString("ConfigUser", "OnboardingMode","Nutzer", user);
            if (dbValue == "Yes")
            {
                return true;
            }
            return false;
        }

    }
}
