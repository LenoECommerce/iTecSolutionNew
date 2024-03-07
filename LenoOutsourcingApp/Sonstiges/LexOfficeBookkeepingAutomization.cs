using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using WindowsInput;
using static Microsoft.FSharp.Core.ByRefKinds;
using Timer = System.Timers.Timer;

namespace EigenbelegToolAlpha
{
    public class LexOfficeBookkeepingAutomization
    {

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }
        public static string url = "https://app.lexoffice.de/fis/olb4/#/transactionswop";
        public static bool foundPDF = false;
        public static bool existsOrderID = false;
        public static string orderID = "";
        public static void Main(int amount)
        {
            try
            {
                for (int i = 1; i <= amount; i++)
                {
                    MainProcess(i);
                    // check if the file exists
                    if (foundPDF == false)
                    {
                        MessageBox.Show($"Für die Order {orderID} wurde leider kein Eigenbeleg gefunden, daher wurde der Prozess gestoppt.");
                        return;
                    }
                    if (existsOrderID == false)
                    {
                        MessageBox.Show($"Für die Order {orderID} wurde leider kein Datenbankeintrag gefunden, daher wurde der Prozess gestoppt.");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Es gab einen Fehler:" + ex.Message);
                return;
            }

        }
        public static void MainProcess(int amount)
        {
            int abstandNachUnten = 0;
            //Web Browser öffnen
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
            Thread.Sleep(2000);
            //Feld Umsätze suchen anklicken
            SetCursorPos(533, 291);
            NormalClick();
            //Adyen Suchbegriff eintippen
            SendKeys.Send("Adyen");
            SendKeys.Send("{ENTER}");
            Thread.Sleep(2000);
            //Ersten Eintrag auswählen
            SetPosClickAnd1SecondSleep(485, 375);
            // “Neuen Beleg erfasse und zuordnen” klicken
            SetCursorPos(1295, 405);
            NormalClick();
            Thread.Sleep(2000);
            if (amount > 1)
            {
                // Alten Beleg löschen
                SetPosClickAnd1SecondSleep(1429, 855);
                SetPosClickAnd1SecondSleep(957, 232);
                SetPosClickAnd1SecondSleep(1371, 248);
                SetPosClickAnd1SecondSleep(1736, 124);
            }
            
            // Die Referenznummer kopieren
            SetCursorPos(1394, 368);
            DoubleClick();
            SendKeys.Send("^c");
            string clipboard = Clipboard.GetText();
            //Er checkt, ob es eine 8 Digit Nummer ist
            //bool isValid = Regex.IsMatch(clipboard, @"^\d{8}$");
            clipboard = CheckForSpaces(clipboard);
            orderID = clipboard;
            Thread.Sleep(2000);
            string[] values = GetDataBaseValues(clipboard);
            FindPDFAndUpload(values[0]);
            Thread.Sleep(5000);
            SendKeys.Send("{F5}");
            Thread.Sleep(2000);
            // “Hochgeladene auswählen”
            SetPosClickAnd1SecondSleep(1075, 676);
            // Beleg auswählen
            SetPosClickAnd1SecondSleep(613, 297);
            // Grüner Button
            SetPosClickAnd1SecondSleep(1305, 538);
            // PopUp: Ja alle Werte überschreiben
            SetPosClickAnd1SecondSleep(1118, 321);

            // check if UI Button exists
            SetCursorPos(1337, 255);
            DoubleClick();
            SendKeys.Send("^c");
            string temp = Clipboard.GetText();
            if (temp != "Belegnummer")
            {
                abstandNachUnten = 45;
            }
            //Lieferant auswählen: Sammellieferant
            SetPosClickAnd1SecondSleep(1576, 206);
            SetPosClickAnd1SecondSleep(1616, 530);
            // Alte Belegnummer löschen
            SetPosClickAnd1SecondSleep(1385, 286+abstandNachUnten);
            SendKeys.SendWait("{BACKSPACE}{BACKSPACE}{BACKSPACE}{BACKSPACE}");
            // Belegnummer einsetzen
            SetPosClickAnd1SecondSleep(1385, 286 + abstandNachUnten);
            SendKeys.Send(values[0]);
            //case distinction if the eigenbelegnumber is less then 3 digits
            if (Convert.ToInt32(values[0]) < 1000)
            {
                // Art der Ausgabe: Wareneinkauf
                Thread.Sleep(2000);
                SetPosClickAnd1SecondSleep(1419, 526 + abstandNachUnten);
                SendKeys.Send("Wareneinkauf");
                Thread.Sleep(2000);
                SetPosClickAnd1SecondSleep(1419, 608 + abstandNachUnten);
                // Beleg Datum festlegen
                SetPosClickAnd1SecondSleep(1784, 287 + abstandNachUnten);
                SendKeys.Send(values[1]);
                Thread.Sleep(2000);
                // Steuer 
                SetPosClickAnd1SecondSleep(1650, 526 + abstandNachUnten);
                Thread.Sleep(2000);
                SetPosClickAnd1SecondSleep(1684, 617 + abstandNachUnten);
            }
            else
            {
                Thread.Sleep(2000);
                // Art der Ausgabe: Wareneinkauf
                SetPosClickAnd1SecondSleep(1435, 472 + abstandNachUnten);
                SendKeys.Send("Wareneinkauf");
                Thread.Sleep(2000);
                SetPosClickAnd1SecondSleep(1429, 553 + abstandNachUnten);
                // Beleg Datum festlegen
                SetPosClickAnd1SecondSleep(1784, 287 + abstandNachUnten);
                SendKeys.Send(values[1]);
                Thread.Sleep(2000);
                // Steuer 
                SetPosClickAnd1SecondSleep(1651, 465 + abstandNachUnten);
                Thread.Sleep(2000);
                SetPosClickAnd1SecondSleep(1679, 553 + abstandNachUnten);
            }
            // Speichern + Zuordnen
            SetPosClickAnd1SecondSleep(1843, 1008);
            SendKeys.Send("^w");
        }

        public static void SetPosClickAnd1SecondSleep(int x, int y)
        {
            SetCursorPos(x, y);
            NormalClick();
            Thread.Sleep(1000);
        }
        public static void FindPDFAndUpload(string eigenbelegNumber)
        {
            foundPDF = false;
            string currentUser = UserFileManagement.ReturnCurrentUser();
            var dbManager = new DBManager();
            string folderPath = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathSaveLocationEB", "Nutzer", currentUser);
            string[] filesInDir = Directory.GetFiles(folderPath);
            foreach (var item in filesInDir)
            {
                string fileName = Path.GetFileName(item);
                if (String.Compare(ReturnNumberFromFileName(fileName),eigenbelegNumber) == 0)
                {
                    foundPDF = true;
                    LexOfficeAPIHandler lexOfficeAPIHandler = new LexOfficeAPIHandler();
                    lexOfficeAPIHandler.UploadBasicFile(item);
                }
            }
        }
        public static string CheckForSpaces(string input)
        {
            string result = "";
            foreach (char c in input)
            {
                if (c != ' ')
                {
                    result += c;
                }
            }
            return result;
        }
        public static string ReturnNumberFromFileName(string inputString)
        {
            int startIndex = "Eigenbeleg".Length;
            int endIndex = inputString.IndexOf(".pdf");

            if (startIndex < endIndex && endIndex != -1)
            {
                string numberString = inputString.Substring(startIndex, endIndex - startIndex);
                return numberString;
            }

            return "";
        }

        public static string[] GetDataBaseValues(string referenceOrderNumber)
        {
            existsOrderID = false;
            string[] values = new string[2];
            try
            {
                var dbManager = new DBManager();
                int id = dbManager.ExecuteQueryWithResult("Eigenbelege", "Id", "Referenz", referenceOrderNumber);
                if (id != 0)
                {
                    existsOrderID = true;
                }
                values[0] = dbManager.ExecuteQueryWithResultString("Eigenbelege", "Eigenbelegnummer", "Referenz", referenceOrderNumber);
                values[1] = dbManager.ExecuteQueryWithResultString("Eigenbelege", "BankPaymentDate", "Referenz", referenceOrderNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Es gab ein Problem beim Datenbankzugriff für "+referenceOrderNumber + ex.Message);
            }
            return values;

        }
        public static void NormalClick()
        {
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
        }
        public static void DoubleClick()
        {
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
        }
        public static void HelpMethodShowCursorPosition()
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

        }
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Point cursorPosition = Cursor.Position;
            int mouseX = cursorPosition.X;
            int mouseY = cursorPosition.Y;
            MessageBox.Show(mouseX.ToString() + mouseY.ToString());
        }
    }
}
