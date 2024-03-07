using System;
using System.IO;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public class UserFileManagement
    {
        public static string fileName = "user.txt";

        public static string ReturnCurrentUser()
        {
            string returnValue = "";
            try
            {
                returnValue = File.ReadAllText(fileName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return returnValue;
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
            string dbValue = dbManager.ExecuteQueryWithResultString("ConfigUser", "OnboardingMode", "Nutzer", user);
            if (dbValue == "Yes")
            {
                return true;
            }
            return false;
        }

    }
}
