using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace EigenbelegToolAlpha
{
    public class SKUGeneration
    {
        public string finalText = "";
        public string category = "";
        public string modell = "";
        public string color = "";
        public string condition = "";
        public string tax = "";
        public string storage = "";
        public string fiveG = "";
        public string battery100 = "";

        public static Dictionary<string, string> modelleDictionaryApple = new Dictionary<string, string>
        {
            { "iPhone SE (2016)", "5" },
            { "iPhone 6S", "6" },
            { "iPhone 6S Plus", "6.1" },
            { "iPhone 7", "7" },
            { "iPhone 7 Plus", "7.1" },
            { "iPhone 8", "8" },
            { "iPhone 8 Plus", "8.1" },
            { "iPhone SE (2020)", "2" },
            { "iPhone X", "10" },
            { "iPhone XR", "9" },
            { "iPhone XS", "10.1" },
            { "iPhone XS Max", "10.2" },
            { "iPhone 11", "11" },
            { "iPhone 11 Pro", "11.1" },
            { "iPhone 11 Pro Max", "11.2" },
            { "iPhone 12", "12" },
            { "iPhone 12 mini", "12.0" },
            { "iPhone 12 Pro", "12.1" },
            { "iPhone 12 Pro Max", "12.2" },
            { "iPhone 13", "13" },
            { "iPhone 13 Mini", "13.0" },
            { "iPhone 13 Pro", "13.1" },
            { "iPhone 13 Pro Max", "13.2" },
        };
        public static Dictionary<string, string> modelleDictionarySamsung = new Dictionary<string, string>
        {
            { "A51", "51" },
            { "A71", "71" },
            { "S10", "10" },
            { "S10 Plus", "10.1" },
            { "S20", "20" },
            { "S20 FE", "20FE" },
            { "S20 Plus", "20.1" },
            { "S20 Ultra", "20.2" },
            { "S21", "21" },
            { "S21 Plus", "21.1" },
            { "S21 Ultra", "21.2" },
            { "S22", "22" },
            { "S22 Plus", "22.1" },
            { "S22 Ultra", "22.2" },
        };

        Dictionary<string, string> conditionsDictionary = new Dictionary<string, string>
        {
            { "A Neuwertig", "A" },
            { "B Minimale Gebrauchsspuren", "B" },
            { "C Gebraucht", "C" },
            { "D Optisch Defekt", "D" },
        };

        Dictionary<string, string> taxTypeDictionary = new Dictionary<string, string>
        {
            { "Regelbesteuerung", "REG" },
            { "Differenzbesteuerung", "DIFF" },
        };

        Dictionary<string, string> storageDictionary = new Dictionary<string, string>
        {
            { "16 GB", "16" },
            { "32 GB", "32" },
            { "64 GB", "64" },
            { "128 GB", "128" },
            { "256 GB", "256" },
            { "512 GB", "512" },
            { "1000 GB", "1000" },
        };

        Dictionary<string, string> colorsDictionary = new Dictionary<string, string>
        {
            { "Schwarz / Grau", "B" },
            { "Rosa", "RO" },
            { "Jetblack", "JB" },
            { "Weiß", "W" },
            { "Rot", "RE" },
            { "Blau", "BL" },
            { "Violett", "VI" },
            { "Gelb", "YE" },
            { "Grün", "GR" },
            { "Gold", "GO" },
            { "Silber", "SI" },
            { "Cosmic Grey", "CG" },
            { "Aura Blau", "ABL" },
            { "Wolke Blau", "WB" },
            { "Orange", "OR" }
        };


        public string SKUGenerationMethod(string valueMake, string valueModell, string valueColor, string valueCondition, string valueStorage, string valueFiveG, int valueBattery)
        {
            try
            {
                category = "APL/";

                color = colorsDictionary[valueColor];
                storage = storageDictionary[valueStorage];
                condition = conditionsDictionary[valueCondition];
                battery100 = Check100Battery(valueBattery);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finalText = category + modell + fiveG + color + storage + condition + battery100;
            return finalText;
        }
        public string Check100Battery(int checkValue)
        {
            if (checkValue == 100)
            {
                return "/100%";
            }
            else
            {
                return "";
            }
        }
    }
}
