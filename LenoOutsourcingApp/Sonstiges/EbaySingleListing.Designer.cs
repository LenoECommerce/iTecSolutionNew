namespace EigenbelegToolAlpha
{
    partial class EbaySingleListing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EbaySingleListing));
            this.comboBox_Battery = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_lack = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_Execute = new System.Windows.Forms.Button();
            this.comboBox_notification = new System.Windows.Forms.ComboBox();
            this.comboBox_pictureRequired = new System.Windows.Forms.ComboBox();
            this.textBox_price = new System.Windows.Forms.TextBox();
            this.textBox_SKUNumber = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // comboBox_Battery
            // 
            this.comboBox_Battery.FormattingEnabled = true;
            this.comboBox_Battery.Items.AddRange(new object[] {
            "keine",
            "Glasbruch",
            "kleiner Pixelfehler",
            "kleiner Glasbruch"});
            this.comboBox_Battery.Location = new System.Drawing.Point(275, 102);
            this.comboBox_Battery.Name = "comboBox_Battery";
            this.comboBox_Battery.Size = new System.Drawing.Size(195, 21);
            this.comboBox_Battery.TabIndex = 9;
            this.comboBox_Battery.Text = "100%";
            this.comboBox_Battery.SelectedIndexChanged += new System.EventHandler(this.comboBox_Battery_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(102, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "Akku";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(102, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Mangel";
            // 
            // textBox_lack
            // 
            this.textBox_lack.Location = new System.Drawing.Point(275, 29);
            this.textBox_lack.Multiline = true;
            this.textBox_lack.Name = "textBox_lack";
            this.textBox_lack.Size = new System.Drawing.Size(195, 49);
            this.textBox_lack.TabIndex = 10;
            this.textBox_lack.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(102, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 24);
            this.label3.TabIndex = 11;
            this.label3.Text = "Meldung";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(102, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 24);
            this.label4.TabIndex = 12;
            this.label4.Text = "Bilder relevant?";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(102, 274);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 24);
            this.label5.TabIndex = 13;
            this.label5.Text = "Preis";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label6.Location = new System.Drawing.Point(102, 317);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 24);
            this.label6.TabIndex = 14;
            this.label6.Text = "SKU Nummer";
            // 
            // btn_Execute
            // 
            this.btn_Execute.Location = new System.Drawing.Point(374, 362);
            this.btn_Execute.Name = "btn_Execute";
            this.btn_Execute.Size = new System.Drawing.Size(96, 33);
            this.btn_Execute.TabIndex = 15;
            this.btn_Execute.Text = "O.K.";
            this.btn_Execute.UseVisualStyleBackColor = true;
            this.btn_Execute.Click += new System.EventHandler(this.btn_Execute_Click);
            // 
            // comboBox_notification
            // 
            this.comboBox_notification.FormattingEnabled = true;
            this.comboBox_notification.Items.AddRange(new object[] {
            "Ja",
            "Nein"});
            this.comboBox_notification.Location = new System.Drawing.Point(275, 163);
            this.comboBox_notification.Name = "comboBox_notification";
            this.comboBox_notification.Size = new System.Drawing.Size(195, 21);
            this.comboBox_notification.TabIndex = 16;
            this.comboBox_notification.Text = "Nein";
            // 
            // comboBox_pictureRequired
            // 
            this.comboBox_pictureRequired.FormattingEnabled = true;
            this.comboBox_pictureRequired.Items.AddRange(new object[] {
            "Ja",
            "Nein"});
            this.comboBox_pictureRequired.Location = new System.Drawing.Point(275, 225);
            this.comboBox_pictureRequired.Name = "comboBox_pictureRequired";
            this.comboBox_pictureRequired.Size = new System.Drawing.Size(195, 21);
            this.comboBox_pictureRequired.TabIndex = 17;
            this.comboBox_pictureRequired.Text = "Nein";
            // 
            // textBox_price
            // 
            this.textBox_price.Location = new System.Drawing.Point(275, 274);
            this.textBox_price.Name = "textBox_price";
            this.textBox_price.Size = new System.Drawing.Size(195, 20);
            this.textBox_price.TabIndex = 18;
            // 
            // textBox_SKUNumber
            // 
            this.textBox_SKUNumber.Location = new System.Drawing.Point(275, 317);
            this.textBox_SKUNumber.Name = "textBox_SKUNumber";
            this.textBox_SKUNumber.Size = new System.Drawing.Size(195, 20);
            this.textBox_SKUNumber.TabIndex = 19;
            // 
            // EbaySingleListing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(580, 427);
            this.Controls.Add(this.textBox_SKUNumber);
            this.Controls.Add(this.textBox_price);
            this.Controls.Add(this.comboBox_pictureRequired);
            this.Controls.Add(this.comboBox_notification);
            this.Controls.Add(this.btn_Execute);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_lack);
            this.Controls.Add(this.comboBox_Battery);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "EbaySingleListing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Eingabe";
            this.Load += new System.EventHandler(this.EbaySingleListing_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_Battery;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_lack;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_Execute;
        private System.Windows.Forms.ComboBox comboBox_notification;
        private System.Windows.Forms.ComboBox comboBox_pictureRequired;
        private System.Windows.Forms.TextBox textBox_price;
        private System.Windows.Forms.TextBox textBox_SKUNumber;
    }
}