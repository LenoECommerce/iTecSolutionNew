namespace EigenbelegToolAlpha
{
    partial class SparePartsPrintQuantityInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SparePartsPrintQuantityInput));
            this.btn_IndividualOK = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btn_5OK = new System.Windows.Forms.Button();
            this.btn_10OK = new System.Windows.Forms.Button();
            this.btn_15OK = new System.Windows.Forms.Button();
            this.btn_20OK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_IndividualOK
            // 
            this.btn_IndividualOK.Location = new System.Drawing.Point(242, 34);
            this.btn_IndividualOK.Name = "btn_IndividualOK";
            this.btn_IndividualOK.Size = new System.Drawing.Size(97, 20);
            this.btn_IndividualOK.TabIndex = 0;
            this.btn_IndividualOK.Text = "O.K.";
            this.btn_IndividualOK.UseVisualStyleBackColor = true;
            this.btn_IndividualOK.Click += new System.EventHandler(this.btn_IndividualOK_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(79, 36);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(127, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // btn_5OK
            // 
            this.btn_5OK.Location = new System.Drawing.Point(88, 100);
            this.btn_5OK.Name = "btn_5OK";
            this.btn_5OK.Size = new System.Drawing.Size(94, 20);
            this.btn_5OK.TabIndex = 3;
            this.btn_5OK.Text = "5";
            this.btn_5OK.UseVisualStyleBackColor = true;
            this.btn_5OK.Click += new System.EventHandler(this.btn_5OK_Click);
            // 
            // btn_10OK
            // 
            this.btn_10OK.Location = new System.Drawing.Point(219, 100);
            this.btn_10OK.Name = "btn_10OK";
            this.btn_10OK.Size = new System.Drawing.Size(94, 20);
            this.btn_10OK.TabIndex = 4;
            this.btn_10OK.Text = "10";
            this.btn_10OK.UseVisualStyleBackColor = true;
            this.btn_10OK.Click += new System.EventHandler(this.btn_10OK_Click);
            // 
            // btn_15OK
            // 
            this.btn_15OK.Location = new System.Drawing.Point(88, 146);
            this.btn_15OK.Name = "btn_15OK";
            this.btn_15OK.Size = new System.Drawing.Size(94, 20);
            this.btn_15OK.TabIndex = 5;
            this.btn_15OK.Text = "15";
            this.btn_15OK.UseVisualStyleBackColor = true;
            this.btn_15OK.Click += new System.EventHandler(this.btn_15OK_Click);
            // 
            // btn_20OK
            // 
            this.btn_20OK.Location = new System.Drawing.Point(219, 146);
            this.btn_20OK.Name = "btn_20OK";
            this.btn_20OK.Size = new System.Drawing.Size(94, 20);
            this.btn_20OK.TabIndex = 6;
            this.btn_20OK.Text = "20";
            this.btn_20OK.UseVisualStyleBackColor = true;
            this.btn_20OK.Click += new System.EventHandler(this.btn_20OK_Click);
            // 
            // SparePartsPrintQuantityInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 217);
            this.Controls.Add(this.btn_20OK);
            this.Controls.Add(this.btn_15OK);
            this.Controls.Add(this.btn_10OK);
            this.Controls.Add(this.btn_5OK);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.btn_IndividualOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SparePartsPrintQuantityInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Anzahl Etiketten eingeben";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_IndividualOK;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btn_5OK;
        private System.Windows.Forms.Button btn_10OK;
        private System.Windows.Forms.Button btn_15OK;
        private System.Windows.Forms.Button btn_20OK;
    }
}