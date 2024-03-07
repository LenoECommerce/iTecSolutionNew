namespace EigenbelegToolAlpha.Evaluations
{
    partial class QuartleryReportCreationInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuartleryReportCreationInput));
            this.btn_Execute = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_quarterSelection = new System.Windows.Forms.ComboBox();
            this.comboBox_yearSelection = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Execute
            // 
            this.btn_Execute.Location = new System.Drawing.Point(222, 150);
            this.btn_Execute.Name = "btn_Execute";
            this.btn_Execute.Size = new System.Drawing.Size(121, 31);
            this.btn_Execute.TabIndex = 0;
            this.btn_Execute.Text = "Ausführen";
            this.btn_Execute.UseVisualStyleBackColor = true;
            this.btn_Execute.Click += new System.EventHandler(this.btn_Execute_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(65, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Quartal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(83, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "Jahr";
            // 
            // comboBox_quarterSelection
            // 
            this.comboBox_quarterSelection.FormattingEnabled = true;
            this.comboBox_quarterSelection.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.comboBox_quarterSelection.Location = new System.Drawing.Point(222, 46);
            this.comboBox_quarterSelection.Name = "comboBox_quarterSelection";
            this.comboBox_quarterSelection.Size = new System.Drawing.Size(121, 21);
            this.comboBox_quarterSelection.TabIndex = 3;
            // 
            // comboBox_yearSelection
            // 
            this.comboBox_yearSelection.FormattingEnabled = true;
            this.comboBox_yearSelection.Location = new System.Drawing.Point(222, 96);
            this.comboBox_yearSelection.Name = "comboBox_yearSelection";
            this.comboBox_yearSelection.Size = new System.Drawing.Size(121, 21);
            this.comboBox_yearSelection.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(128, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(215, 40);
            this.label3.TabIndex = 5;
            this.label3.Text = "WICHTIG: Nicht mehr als \r\neinen Report pro Session";
            // 
            // QuartleryReportCreationInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(413, 243);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_yearSelection);
            this.Controls.Add(this.comboBox_quarterSelection);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Execute);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "QuartleryReportCreationInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Felder ausfüllen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Execute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_quarterSelection;
        private System.Windows.Forms.ComboBox comboBox_yearSelection;
        private System.Windows.Forms.Label label3;
    }
}