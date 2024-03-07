using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public class DBManager
    {
        private MySqlConnection conn;
        public static string connString = "SERVER=212.227.140.237;PORT=3306;Initial Catalog='sql11525524';username=somedude;password=!mRAqn.Pl83.X8H0CuB28;Pooling=True;";
        public string saveLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public int backupCounter;
        public string fileName() => @"\\Backup for " + DateTime.Today.ToString().Substring(0, 10) + " " + backupCounter + " Version.sql";

        public DBManager()
        {
            backupCounter = ExecuteQueryWithResult("Config", "Nummer", "Typ", "BackUpsToday");
        }

        public void Backup()
        {
            try
            {
                if (File.Exists(saveLocation + fileName()) == true)
                {
                    return;
                }
                else
                {
                    using (MySqlConnection conn = new MySqlConnection(connString))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();
                                string pathComplete = saveLocation + fileName();
                                mb.ExportToFile(pathComplete);
                                GoogleDrive drive = new GoogleDrive(pathComplete, "sql");
                                conn.Close();
                                File.Delete(pathComplete);
                                backupCounter++;
                                ExecuteQuery("UPDATE `Config` SET `Nummer` = " + backupCounter + " WHERE `Typ` = 'BackUpsToday'");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public int CountRows1Condition(string table, string conditionColumn, string conditionValue)
        {
            string query = $"SELECT COUNT(*) AS row_count FROM `{table}` WHERE `{conditionColumn}`= '{conditionValue}'";
            conn = new MySqlConnection
            {
                ConnectionString = connString
            };
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //zwischenspeichern und danach umformen um Fehlerquelle zu vermeiden
                var firstValueGetBack = cmd.ExecuteScalar();
                int result = Convert.ToInt32(firstValueGetBack);
                return result;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public int CountRows1Condition_LikeForDateFiltering(string table, string conditionColumn, string conditionValue)
        {
            string query = $"SELECT COUNT(*) AS row_count FROM `{table}` WHERE `{conditionColumn}` LIKE '%{conditionValue}'";
            conn = new MySqlConnection
            {
                ConnectionString = connString
            };
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //zwischenspeichern und danach umformen um Fehlerquelle zu vermeiden
                var firstValueGetBack = cmd.ExecuteScalar();
                int result = Convert.ToInt32(firstValueGetBack);
                conn.Close();
                return result;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public int CountRows2Conditions_LikeForDateFiltering(string table, string conditionColumn, string conditionValue, string dateColumn, string dateValue)
        {
            string query = $"SELECT COUNT(*) AS row_count FROM `{table}` WHERE {conditionColumn} = '{conditionValue}' AND`{dateColumn}` LIKE '%{dateValue}'";
            conn = new MySqlConnection
            {
                ConnectionString = connString
            };
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //zwischenspeichern und danach umformen um Fehlerquelle zu vermeiden
                var firstValueGetBack = cmd.ExecuteScalar();
                int result = Convert.ToInt32(firstValueGetBack);
                return result;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public int CountRows2Conditions(string table, string conditionColumn1, string conditionValue1, string conditionColumn2, string conditionValue2)
        {
            string query = $"SELECT COUNT(*) AS row_count FROM `{table}` WHERE `{conditionColumn1}`= '{conditionValue1}' AND `{conditionColumn2}`= '{conditionValue2}'";
            conn = new MySqlConnection
            {
                ConnectionString = connString
            };
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //zwischenspeichern und danach umformen um Fehlerquelle zu vermeiden
                var firstValueGetBack = cmd.ExecuteScalar();
                int result = Convert.ToInt32(firstValueGetBack);
                return result;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateSpecificEntry(string table, string[] attributes, string[] values, int id)
        {
            string combinationAllAttributesAndValues = BuildStringUpdate(attributes, values);

            string query = "UPDATE `" + table + "` SET " + combinationAllAttributesAndValues + " WHERE `Id` = '" + id.ToString() + "'";
            ExecuteQuery(query);
        }

        public void CreateEntry(string table, string[] attributes, string[] values)
        {
            string combinationAllAttributes = BuildStringAttributesCreate(attributes);
            string combinationAllValues = BuildStringValuesCreate(values);
            string query = "INSERT INTO `" + table + "` (" + combinationAllAttributes + ") VALUES (" + combinationAllValues + ")";

            ExecuteQuery(query);
        }

        public string BuildStringValuesCreate(string[] input)
        {
            string returnValue = "";
            int counter = 0;
            foreach (var item in input)
            {
                counter++;
                returnValue = counter == input.Length
                    ? returnValue + "'" + item + "'"
                    : returnValue + "'" + item + "',";
            }
            return returnValue;
        }

        public string BuildStringAttributesCreate(string[] input)
        {
            string returnValue = "";
            int counter = 0;
            foreach (var item in input)
            {
                counter++;
                returnValue = counter == input.Length
                    ? returnValue + "`" + item + "`"
                    : returnValue + "`" + item + "`, ";
            }
            return returnValue;
        }

        public string BuildStringUpdate(string[] attributes, string[] values)
        {
            string returnValue = "";
            int counter = 0;

            foreach (string item in attributes)
            {
                returnValue = counter + 1 == attributes.Length
                    ? returnValue + "`" + item + "` = '" + values[counter] + "'"
                    : returnValue + "`" + item + "` = '" + values[counter] + "', ";
                counter++;
            }
            return returnValue;
        }

        public double SelectSumQuery1Condition(string table, string searchingColumn, string conditionColumn, string conditionValue)
        {
            string query = "SELECT SUM(" + searchingColumn + ") AS total_sum FROM`" + table + "` WHERE `" + conditionColumn + "` = '" + conditionValue + "'";
            conn = new MySqlConnection
            {
                ConnectionString = connString
            };
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //zwischenspeichern und danach umformen um Fehlerquelle zu vermeiden
                var firstValueGetBack = cmd.ExecuteScalar();
                double result = Convert.ToDouble(firstValueGetBack);
                return result;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public double SelectSumQuery2Conditions(string table, string searchingColumn, string conditionColumn1, string conditionValue1, string conditionColumn2, string conditionValue2)
        {
            string query = "SELECT SUM(" + searchingColumn + ") AS total_sum FROM`" + table + "` WHERE `" + conditionColumn1 + "` = '" + conditionValue1 + "' AND `" + conditionColumn2 + "` = '" + conditionValue2 + "'";
            conn = new MySqlConnection
            {
                ConnectionString = connString
            };
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //zwischenspeichern und danach umformen um Fehlerquelle zu vermeiden
                var firstValueGetBack = cmd.ExecuteScalar();
                double result = Convert.ToDouble(firstValueGetBack);
                return result;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public int ExecuteQueryWithResult(string table, string searchingColumn, string getValueOfWhere, string equalValue)
        {
            string query = "SELECT `" + searchingColumn + "` FROM `" + table + "` WHERE `" + getValueOfWhere + "` = '" + equalValue + "'";
            conn = new MySqlConnection
            {
                ConnectionString = connString
            };
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //zwischenspeichern und danach umformen um Fehlerquelle zu vermeiden
                var firstValueGetBack = cmd.ExecuteScalar();
                int result = Convert.ToInt32(firstValueGetBack);
                return result;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public int ExecuteQueryWithResultThreeConditions(string table, string searchingColumn, string condition1, string condition1EqualValue, string condition2, string condition2EqualValue, string condition3, string condition3EqualValue)
        {
            string query = "SELECT `" + searchingColumn + "` FROM `" + table + "` WHERE `" + condition1 + "` = '" + condition1EqualValue + "' AND `" + condition2 + "` = '" + condition2EqualValue + "' AND `" + condition3 + "` = '" + condition3EqualValue + "'";

            conn = new MySqlConnection
            {
                ConnectionString = connString
            };
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //zwischenspeichern und danach umformen um Fehlerquelle zu vermeiden
                var firstValueGetBack = cmd.ExecuteScalar();
                int result = Convert.ToInt32(firstValueGetBack);
                return result;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public string ExecuteQueryWithResultString(string table, string searchingColumn, string getValueOfWhere, string equalValue)
        {
            string query = "SELECT `" + searchingColumn + "` FROM `" + table + "` WHERE `" + getValueOfWhere + "` = '" + equalValue + "'";
            conn = new MySqlConnection
            {
                ConnectionString = connString
            };
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //zwischenspeichern und danach umformen um Fehlerquelle zu vermeiden
                var firstValueGetBack = cmd.ExecuteScalar();
                string result = firstValueGetBack == null ? "0" : firstValueGetBack.ToString();
                return result;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
            finally
            {
                conn.Close();
            }
        }

        public string ExecuteQueryWithResultStringTwoConditions(string table, string searchingColumn, string condition1Column, string condition2Column, string condition1EqualValue, string condition2EqualValue)
        {
            string query = "SELECT `" + searchingColumn + "` FROM `" + table + "` WHERE `" + condition1Column + "` = '" + condition1EqualValue + "' AND `" + condition2Column + "` = '" + condition2EqualValue + "'";
            conn = new MySqlConnection
            {
                ConnectionString = connString
            };
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //zwischenspeichern und danach umformen um Fehlerquelle zu vermeiden
                var firstValueGetBack = cmd.ExecuteScalar();
                string result = firstValueGetBack == null ? "0" : firstValueGetBack.ToString();
                return result;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
            finally { conn.Close(); }
        }

        public void ExecuteQuery(string query)
        {
            conn = new MySqlConnection
            {
                ConnectionString = connString
            };
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task ExecuteQueryAsync(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    await conn.OpenAsync();

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void deleteEntry(int lastSelectedEntry, string table)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle zuerst einen Eintrag aus");
                return;
            }
            string query = string.Format("DELETE FROM `" + table + "` WHERE `Id` = {0} ;", lastSelectedEntry);
            ExecuteQuery(query);
        }
    }
}
