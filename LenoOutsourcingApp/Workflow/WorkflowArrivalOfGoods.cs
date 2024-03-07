using EigenbelegToolAlpha.APIs;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha.Sonstiges
{
    public class WorkflowArrivalOfGoods
    {
        public static Dictionary<string, string> NSYSColorCatalog = new Dictionary<string, string>
        {
            { "Silver", "Silber"},
            { "Spacegray", "Schwarz / Grau"},
            { "Midnight", "Schwarz / Grau"},
            { "Space Gray", "Schwarz / Grau"},
            { "Black", "Schwarz / Grau"},
            { "Graphite", "Schwarz / Grau"},
            { "Red", "Rot"},
            { "Pink", "Rosa"},
            { "Rose Gold", "Rosa"},
            { "Starlight", "Weiß"},
            { "White", "Weiß"},
            { "Green", "Grün"},
            { "Midnight Green", "Grün"},
            { "Alpine Green", "Grün"},
            { "Yellow", "Gelb"},
            { "Purple", "Violett"},
            { "Pacific Blue", "Blau"},
            { "Sierra Blue", "Blau"},
            { "Blue", "Blau"},
            { "Gold", "Gold"},
            { "Coral", "Orange" }
        };

        public static Dictionary<string, string> NSYSStorageCatalog = new Dictionary<string, string>
        {
            { "16GB", "16 GB"},
            { "32GB", "32 GB"},
            { "64GB", "64 GB"},
            { "128GB", "128 GB"},
            { "256GB", "256 GB"},
            { "512GB", "512 GB"},
            { "1000GB", "1000 GB"},
            { "16 GB", "16 GB"},
            { "32 GB", "32 GB"},
            { "64 GB", "64 GB"},
            { "128 GB", "128 GB"},
            { "256 GB", "256 GB"},
            { "512 GB", "512 GB"},
            { "1000 GB", "1000 GB"},
        };
        public static Dictionary<string, string> NSYSModelCatalog = new Dictionary<string, string>
        {
            { "iPhone SE", "iPhone SE (2016)"},
            { "iPhone SE (2nd Generation)", "iPhone SE (2020)"},
        };


        public static string model = "";
        public static string color = "";
        public static string storage = "";
        public static string imei = "";

        public static void FillOutDataByIMEI(string imei, string compareValueColor, string compareValueModel, string compareValueStorage)
        {
            iFreeiCloudAPIHandler.SetProductAttributes(imei);
            // attributes are similar to nsys i guess
            if (NSYSModelCatalog.ContainsKey(iFreeiCloudAPIHandler.model))
            {
                model = NSYSModelCatalog[iFreeiCloudAPIHandler.model];
            }
            else
            {
                model = iFreeiCloudAPIHandler.model;
            }
            storage = NSYSStorageCatalog[iFreeiCloudAPIHandler.storage];
            color = NSYSColorCatalog[iFreeiCloudAPIHandler.color];
            CheckDeviationOfAttributes(compareValueColor, compareValueModel, compareValueStorage);
        }
        //old code from NSYS

        //public static void FillOutDataByIMEI(string compareValueColor, string compareValueModel, string compareValueStorage)
        //{
        //    string imeiCheck = NSYSAPIHandler.GetDeviceIMEIFromLastMinute();
        //    string[] values = NSYSAPIHandler.ReturnValuesOfSpecificDevice(imeiCheck);
        //    if (NSYSModelCatalog.ContainsValue(values[0]))
        //    {
        //        model = NSYSModelCatalog[values[0]];
        //    }
        //    else
        //    {
        //        model = values[0];
        //    }
        //    storage = NSYSStorageCatalog[values[1]];
        //    color = NSYSColorCatalog[values[2]];
        //    imei = imeiCheck;
        //    CheckDeviationOfAttributes(compareValueColor, compareValueModel, compareValueStorage);
        //}

        public static void CheckDeviationOfAttributes(string compareValueColor, string compareValueModel, string compareValueStorage)
        {
            if (GetStorageOnlyInDigits(storage) < GetStorageOnlyInDigits(compareValueStorage))
            {
                MessageBox.Show("Der vom Kunden angegebene Speicher weicht nach unten hinten ab. Bitte festhalten für die Möglichkeit eines BuyBack Gegenangebots.");
            }
            if (compareValueModel != model)
            {
                MessageBox.Show($"Das vom Kunden angegebene Modell weicht vom eigentlichen Modell ab. Bitte festhalten für die Möglichkeit eines BuyBack Gegenangebots." +
                                $"\r\n\r\nAngegeben: "+ compareValueModel + "\r\nTatsächlich: " + model);
            }
        }

        public static int GetStorageOnlyInDigits(string capacity)
        {
            int result = 0;

            if (capacity.EndsWith("GB"))
            {
                int.TryParse(capacity.Replace("GB", "").Trim(), out result);
            }
            else if (capacity.EndsWith(" GB"))
            {
                int.TryParse(capacity.Replace(" GB", "").Trim(), out result);
            }

            return result;
        }


    }
}
