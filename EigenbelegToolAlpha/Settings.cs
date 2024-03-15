using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigenbelegToolAlpha
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            LoadValues();
        }

        private void PathBums(Label label, string dbAttribute)
        {
            openFileDialog1.ShowDialog();
            string selectedFileName = openFileDialog1.FileName;
            selectedFileName = selectedFileName.Replace(@"\", @"\\");
            var dbManager = new DBManager();
            dbManager.ExecuteQuery($"UPDATE `ConfigUser` SET `{dbAttribute}` = '{selectedFileName}' WHERE `Nutzer` = '{UserFileManagement.currentUser}'");
            label.Text = selectedFileName;
        }

        private void LoadValues()
        {
            DBManager db = new DBManager();


            string pathLabelDevice = db.ExecuteQueryWithResultString(
                "ConfigUser",
                "TemplateModell",
                "Nutzer",
                UserFileManagement.currentUser);
            lbl_pathDeviceLabel.Text = pathLabelDevice;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveChanges();
            this.Close();
        }

        private void SaveChanges()
        {
            string pathLabelDevice = lbl_pathDeviceLabel.Text;
        }

        private void lbl_pathDeviceLabel_Click(object sender, EventArgs e)
        {
            PathBums(lbl_pathDeviceLabel, "TemplateModell");
        }
    }
}
