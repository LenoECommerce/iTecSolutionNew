namespace EigenbelegToolAlpha
{
    partial class BuyBackAcceptedReportInputcs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuyBackAcceptedReportInputcs));
            this.textBox1_Input = new System.Windows.Forms.TextBox();
            this.btn_Exectute = new System.Windows.Forms.Button();
            this.lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1_Input
            // 
            this.textBox1_Input.Location = new System.Drawing.Point(138, 42);
            this.textBox1_Input.Name = "textBox1_Input";
            this.textBox1_Input.Size = new System.Drawing.Size(222, 20);
            this.textBox1_Input.TabIndex = 0;
            this.textBox1_Input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDownHandler);
            // 
            // btn_Exectute
            // 
            this.btn_Exectute.Location = new System.Drawing.Point(264, 86);
            this.btn_Exectute.Name = "btn_Exectute";
            this.btn_Exectute.Size = new System.Drawing.Size(96, 31);
            this.btn_Exectute.TabIndex = 1;
            this.btn_Exectute.Text = "Check";
            this.btn_Exectute.UseVisualStyleBackColor = true;
            this.btn_Exectute.Click += new System.EventHandler(this.btn_Exectute_Click);
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl.Location = new System.Drawing.Point(12, 38);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(104, 24);
            this.lbl.TabIndex = 3;
            this.lbl.Text = "REP Scan";
            // 
            // BuyBackAcceptedReportInputcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 154);
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.btn_Exectute);
            this.Controls.Add(this.textBox1_Input);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "BuyBackAcceptedReportInputcs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BuyBack Accepted Order Report";
            this.Load += new System.EventHandler(this.BuyBackAcceptedReportInputcs_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1_Input;
        private System.Windows.Forms.Button btn_Exectute;
        private System.Windows.Forms.Label lbl;
    }
}