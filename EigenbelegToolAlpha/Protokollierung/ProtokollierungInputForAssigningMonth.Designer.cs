namespace EigenbelegToolAlpha
{
    partial class ProtokollierungInputForAssigningMonth
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProtokollierungInputForAssigningMonth));
            this.lbl_year = new System.Windows.Forms.Label();
            this.lbl_month = new System.Windows.Forms.Label();
            this.comboBox_Months = new System.Windows.Forms.ComboBox();
            this.comboBox_year = new System.Windows.Forms.ComboBox();
            this.btn_Execute = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_year
            // 
            this.lbl_year.AutoSize = true;
            this.lbl_year.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_year.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbl_year.Location = new System.Drawing.Point(82, 110);
            this.lbl_year.Name = "lbl_year";
            this.lbl_year.Size = new System.Drawing.Size(50, 24);
            this.lbl_year.TabIndex = 0;
            this.lbl_year.Text = "Jahr";
            // 
            // lbl_month
            // 
            this.lbl_month.AutoSize = true;
            this.lbl_month.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_month.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbl_month.Location = new System.Drawing.Point(65, 53);
            this.lbl_month.Name = "lbl_month";
            this.lbl_month.Size = new System.Drawing.Size(67, 24);
            this.lbl_month.TabIndex = 1;
            this.lbl_month.Text = "Monat";
            // 
            // comboBox_Months
            // 
            this.comboBox_Months.FormattingEnabled = true;
            this.comboBox_Months.Items.AddRange(new object[] {
            "Januar",
            "Februar",
            "März",
            "April",
            "Mai",
            "Juni",
            "Juli",
            "August",
            "September",
            "Oktober",
            "November",
            "Dezember"});
            this.comboBox_Months.Location = new System.Drawing.Point(184, 53);
            this.comboBox_Months.Name = "comboBox_Months";
            this.comboBox_Months.Size = new System.Drawing.Size(178, 21);
            this.comboBox_Months.TabIndex = 65;
            // 
            // comboBox_year
            // 
            this.comboBox_year.FormattingEnabled = true;
            this.comboBox_year.Items.AddRange(new object[] {
            "2022",
            "2023",
            "2024",
            "2025"});
            this.comboBox_year.Location = new System.Drawing.Point(184, 110);
            this.comboBox_year.Name = "comboBox_year";
            this.comboBox_year.Size = new System.Drawing.Size(178, 21);
            this.comboBox_year.TabIndex = 66;
            // 
            // btn_Execute
            // 
            this.btn_Execute.Location = new System.Drawing.Point(249, 169);
            this.btn_Execute.Name = "btn_Execute";
            this.btn_Execute.Size = new System.Drawing.Size(113, 30);
            this.btn_Execute.TabIndex = 67;
            this.btn_Execute.Text = "Ausführen";
            this.btn_Execute.UseVisualStyleBackColor = true;
            this.btn_Execute.Click += new System.EventHandler(this.btn_Execute_Click);
            // 
            // ProtokollierungInputForAssigningMonth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(431, 252);
            this.Controls.Add(this.btn_Execute);
            this.Controls.Add(this.comboBox_year);
            this.Controls.Add(this.comboBox_Months);
            this.Controls.Add(this.lbl_month);
            this.Controls.Add(this.lbl_year);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ProtokollierungInputForAssigningMonth";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Monat und Jahr auswählen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_year;
        private System.Windows.Forms.Label lbl_month;
        private System.Windows.Forms.ComboBox comboBox_Months;
        private System.Windows.Forms.ComboBox comboBox_year;
        private System.Windows.Forms.Button btn_Execute;
    }
}