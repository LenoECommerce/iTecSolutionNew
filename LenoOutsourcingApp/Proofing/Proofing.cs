using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class Proofing : Form
    {
        public static MySqlConnection conn;
        public string connString = DBManager.connString;
        public int lastSelectedEntry;
        public string internalNumber = "";
        public string imei = "";
        public string videoLink = "";
        public string nsysCertificate = "";

        public Proofing()
        {
            InitializeComponent();
            ShowProofing();
        }
        public void ShowProofing()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connString;
            conn.Open();

            string query = "SELECT * FROM `Proofing`";
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, conn);

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();

            ////Datensatz
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            //Daten anzeigen im Grid
            proofingDGV.DataSource = dataSet.Tables[0];

            proofingDGV.Columns[0].Visible = false;

            //Sortierte Ansicht
            proofingDGV.Sort(proofingDGV.Columns[1], ListSortDirection.Descending);
            conn.Close();
        }

        private void ProofingCreate()
        {
            if (textBox_IMEI.Text == "" ||
               textBox_InternalNumber.Text == "" ||
               textBox_nsysCertificate.Text == "" ||
               textBox_Video.Text == "")
            {
                MessageBox.Show("Bitte füll alle Felder aus.");
                return;
            }
            SetValues();

            string query = string.Format("INSERT INTO `Proofing`(`Intern`,`IMEI`,`Video`,`NSYS-Zertifikat`) VALUES ('{0}','{1}','{2}','{3}')"
           , internalNumber, imei, videoLink, nsysCertificate);
            var dbManager = new DBManager();
            dbManager.ExecuteQuery(query);
            MessageBox.Show("Der Eintrag wurde erfolgreich erstellt.");
            ShowProofing();
        }

        private void SetValues()
        {
            imei = textBox_IMEI.Text;
            internalNumber = textBox_InternalNumber.Text;
            nsysCertificate = textBox_nsysCertificate.Text;
            videoLink = textBox_Video.Text;
        }
        private void Proofing_Load(object sender, EventArgs e)
        {

        }

        private void proofingDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox_InternalNumber.Text = proofingDGV.SelectedRows[0].Cells[1].Value.ToString();
            textBox_IMEI.Text = proofingDGV.SelectedRows[0].Cells[2].Value.ToString();
            textBox_Video.Text = proofingDGV.SelectedRows[0].Cells[3].Value.ToString();
            textBox_nsysCertificate.Text = proofingDGV.SelectedRows[0].Cells[4].Value.ToString();

            lastSelectedEntry = (int)proofingDGV.SelectedRows[0].Cells[0].Value;
        }

        private void btn_proofingCreate_Click(object sender, EventArgs e)
        {
            ProofingCreate();
        }

        private void btn_proofingEdit_Click(object sender, EventArgs e)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus");
                return;
            }
            SetValues();

            try
            {
                string query = string.Format("UPDATE `Proofing` SET `Intern` = '{0}',`IMEI` = '{1}', `Video` = '{2}', `NSYS-Zertifikat` = '{3}' WHERE `Id` = {4}"
                , internalNumber, imei, videoLink, nsysCertificate, lastSelectedEntry);
                MessageBox.Show("Deine Änderungen wurden erfolgreich gespeichert.");
                var dbManager = new DBManager();
                dbManager.ExecuteQuery(query);
                ShowProofing();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_proofingDelete_Click(object sender, EventArgs e)
        {
            if (lastSelectedEntry == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus");
                return;
            }
            DBManager window = new DBManager();
            window.deleteEntry(lastSelectedEntry, "Proofing");
            ShowProofing();
        }
        private void btn_certificateSyncing_Click(object sender, EventArgs e)
        {
            folderBD.ShowDialog();
            try
            {
                var dbManager = new DBManager();
                string folderPath = folderBD.SelectedPath;
                string[] filesInDir = Directory.GetFiles(folderPath);
                int countChanger = 0;

                foreach (var item in filesInDir)
                {
                    string internalNumber = Path.GetFileNameWithoutExtension(item).ToString();
                    int id = dbManager.ExecuteQueryWithResult("Proofing", "Id", "Intern", internalNumber);
                    string imei = dbManager.ExecuteQueryWithResultString("Reparaturen", "IMEI", "Intern", internalNumber);
                    // create entry if does not exist
                    if (id == 0)
                    {
                        string query2 = $"INSERT INTO `Proofing` (`Intern`, `IMEI`) VALUES ({internalNumber},{imei})";
                        dbManager.ExecuteQuery(query2);
                    }
                    GoogleDrive drive = new GoogleDrive(item, "pdf");
                    string fileLink = GoogleDrive.currentLink;
                    string query = "UPDATE `Proofing` SET `NSYS-Zertifikat` = '" + fileLink + "' WHERE `Id` = '" + id.ToString() + "'";
                    dbManager.ExecuteQuery(query);
                    countChanger++;
                }
                MessageBox.Show("Es wurden " + countChanger + " Zertifikate erfolgreich hochgeladen.");
                ShowProofing();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void VideoSync()
        {
            var dbManager = new DBManager();
            ProofingVideoSyncPreparation preparation = new ProofingVideoSyncPreparation();
            preparation.Show();
            //Check new entries and copy paste intern +imei
            int highestInternalNumber = dbManager.ExecuteQueryWithResult("Config", "Nummer", "Typ", "InterneNummer");
            int dgvHighestInternalNumber = Convert.ToInt32(proofingDGV.Rows[0].Cells[1].Value);
            for (int i = dgvHighestInternalNumber; i <= highestInternalNumber; i++)
            {
                var testIfNotCreated = dbManager.ExecuteQueryWithResult("Proofing", "Id", "Intern", i.ToString());
                if (testIfNotCreated == 0)
                {
                    string newIMEI = dbManager.ExecuteQueryWithResultString("Reparaturen", "IMEI", "Intern", i.ToString());
                    string query = "INSERT INTO `Proofing` (`Intern`,`IMEI`) VALUES ('" + i.ToString() + "','" + newIMEI + "')";
                    dbManager.ExecuteQuery(query);
                }

            }
            //Check imei changes l deduct 100 because of performance
            for (int i = highestInternalNumber - 100; i <= highestInternalNumber; i++)
            {
                string newIMEICheckUp = dbManager.ExecuteQueryWithResultString("Reparaturen", "IMEI", "Intern", i.ToString());
                var testIfNotCreated = dbManager.ExecuteQueryWithResult("Proofing", "Id", "IMEI", i.ToString());
                if (testIfNotCreated == 0 & newIMEICheckUp != "")
                {
                    string query = "UPDATE `Proofing` SET `IMEI` = '" + newIMEICheckUp + "' WHERE `Intern` = '" + i.ToString() + "'";
                    dbManager.ExecuteQuery(query);
                }
            }

            string folderPath = "";
            try
            {
                folderPath = dbManager.ExecuteQueryWithResultString("ConfigUser", "PathVideosForUpload", "Nutzer", "LennartMainPC");
                string[] filesInDir = Directory.GetFiles(folderPath);
                int countChanger = 0;
                preparation.Hide();
                ProofingVideoSyncLoadingBar window = new ProofingVideoSyncLoadingBar(filesInDir.Count());
                window.Show();
                ShowProofing();
                foreach (DataGridViewRow row in proofingDGV.Rows)
                {
                    foreach (var item in filesInDir)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(item).ToString();
                        string rowValueIntern = row.Cells[1].Value.ToString();
                        if (rowValueIntern == fileName)
                        {
                            if (row.Cells[3].Value.ToString() == "")
                            {
                                countChanger++;
                                window.ChangeBar(countChanger);
                                window.lbl_matchedVideos.Text = countChanger.ToString();
                                GoogleDrive drive = new GoogleDrive(item, "mp4");
                                string fileLink = GoogleDrive.currentLink;
                                string query = "UPDATE `Proofing` SET `Video` = '" + fileLink + "' WHERE `Intern` = '" + rowValueIntern + "'";
                                dbManager.ExecuteQuery(query);
                            }
                        }
                    }
                }
                MessageBox.Show("Es wurden " + countChanger + " Videos erfolgreich hochgeladen.");
                window.Hide();
                ShowProofing();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            string today = DateTime.Now.ToString();
            dbManager.ExecuteQuery("UPDATE `Config` SET `Nummer` = '" + today + "' WHERE `Typ` = 'LastVideoUpload'");
            DeleteFilesAfterUpload(folderPath);
        }
        public void DeleteFilesAfterUpload(string folderPath)
        {
            string[] filesInDir = Directory.GetFiles(folderPath);
            foreach (var item in filesInDir)
            {
                System.IO.File.Delete(item);
            }
        }


        private void eigenbelegeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reparaturen window = new Reparaturen();
            window.Show();
            this.Hide();
        }

        private void protokollierungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Eigenbelege window = new Eigenbelege();
            window.Show();
            this.Hide();
        }

        private void protokollierungToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Protokollierung window = new Protokollierung();
            window.Show();
            this.Hide();
        }

        private void btn_CreateExcel_Click(object sender, EventArgs e)
        {
            ProofingInputOrderIDs window = new ProofingInputOrderIDs();
            window.Show();
        }

        private void auswertungenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EvaluationChoice window = new EvaluationChoice();
            window.Show();
        }

        private void sucheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new SearchAlgorithm())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    //Collecting all rows with the matching term
                    string searchTerm = form.searchTerm.ToLower();
                    int cellCounter = proofingDGV.Rows[0].Cells.Count - 1;
                    int arrayIndexCounter = 0;
                    string[] matchingRows = new string[100];
                    foreach (DataGridViewRow row in proofingDGV.Rows)
                    {
                        var pos = row.Index;
                        for (int i = 1; i <= cellCounter; i++)
                        {
                            if (row.Cells[i].Value.ToString().ToLower().Contains(searchTerm))
                            {
                                if (SearchAlgorithm.CheckIfExists(pos, matchingRows) == false)
                                {
                                    matchingRows[arrayIndexCounter] = pos.ToString();
                                }
                            }
                        }
                        //Changing the visibility
                        if (matchingRows.Contains(pos.ToString()))
                        {
                            row.Visible = true;
                        }
                        else
                        {
                            if (pos == 0)
                            {
                                proofingDGV.CurrentCell = null;
                                row.Visible = false;
                            }
                            else
                            {
                                row.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        private void serviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Service window = new Service();
            window.Show();
            this.Hide();
        }
    }
}
