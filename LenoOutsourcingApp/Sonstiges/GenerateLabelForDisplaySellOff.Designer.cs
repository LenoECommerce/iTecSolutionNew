namespace EigenbelegToolAlpha
{
    partial class GenerateLabelForDisplaySellOff
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateLabelForDisplaySellOff));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_model = new System.Windows.Forms.ComboBox();
            this.btn_Execute = new System.Windows.Forms.Button();
            this.comboBox_technicalissues = new System.Windows.Forms.ComboBox();
            this.comboBox_touchFunctionality = new System.Windows.Forms.ComboBox();
            this.comboBox_taxes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(57, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Modell";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(59, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Fehler";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(57, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Besteuerung";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(59, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 24);
            this.label4.TabIndex = 3;
            this.label4.Text = "Touch";
            // 
            // comboBox_model
            // 
            this.comboBox_model.FormattingEnabled = true;
            this.comboBox_model.Items.AddRange(new object[] {
            "8",
            "8P",
            "X",
            "XS",
            "XSM",
            "11",
            "11P",
            "11PM",
            "12/12P",
            "12M",
            "12PM",
            "13",
            "13M",
            "13P",
            "13PM"});
            this.comboBox_model.Location = new System.Drawing.Point(226, 50);
            this.comboBox_model.Name = "comboBox_model";
            this.comboBox_model.Size = new System.Drawing.Size(121, 21);
            this.comboBox_model.TabIndex = 4;
            // 
            // btn_Execute
            // 
            this.btn_Execute.Location = new System.Drawing.Point(240, 229);
            this.btn_Execute.Name = "btn_Execute";
            this.btn_Execute.Size = new System.Drawing.Size(107, 28);
            this.btn_Execute.TabIndex = 5;
            this.btn_Execute.Text = "Erstellen";
            this.btn_Execute.UseVisualStyleBackColor = true;
            // 
            // comboBox_technicalissues
            // 
            this.comboBox_technicalissues.FormattingEnabled = true;
            this.comboBox_technicalissues.Items.AddRange(new object[] {
            "keine",
            "kleiner Fleck"});
            this.comboBox_technicalissues.Location = new System.Drawing.Point(226, 94);
            this.comboBox_technicalissues.Name = "comboBox_technicalissues";
            this.comboBox_technicalissues.Size = new System.Drawing.Size(121, 21);
            this.comboBox_technicalissues.TabIndex = 6;
            // 
            // comboBox_touchFunctionality
            // 
            this.comboBox_touchFunctionality.FormattingEnabled = true;
            this.comboBox_touchFunctionality.Items.AddRange(new object[] {
            "einwandfrei",
            "defekt",
            "Ghost Touch"});
            this.comboBox_touchFunctionality.Location = new System.Drawing.Point(226, 133);
            this.comboBox_touchFunctionality.Name = "comboBox_touchFunctionality";
            this.comboBox_touchFunctionality.Size = new System.Drawing.Size(121, 21);
            this.comboBox_touchFunctionality.TabIndex = 7;
            // 
            // comboBox_taxes
            // 
            this.comboBox_taxes.FormattingEnabled = true;
            this.comboBox_taxes.Items.AddRange(new object[] {
            "DIFF",
            "REG"});
            this.comboBox_taxes.Location = new System.Drawing.Point(226, 172);
            this.comboBox_taxes.Name = "comboBox_taxes";
            this.comboBox_taxes.Size = new System.Drawing.Size(121, 21);
            this.comboBox_taxes.TabIndex = 8;
            // 
            // GenerateLabelForDisplaySellOff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(457, 292);
            this.Controls.Add(this.comboBox_taxes);
            this.Controls.Add(this.comboBox_touchFunctionality);
            this.Controls.Add(this.comboBox_technicalissues);
            this.Controls.Add(this.btn_Execute);
            this.Controls.Add(this.comboBox_model);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GenerateLabelForDisplaySellOff";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Etikett erstellen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_model;
        private System.Windows.Forms.Button btn_Execute;
        private System.Windows.Forms.ComboBox comboBox_technicalissues;
        private System.Windows.Forms.ComboBox comboBox_touchFunctionality;
        private System.Windows.Forms.ComboBox comboBox_taxes;
    }
}