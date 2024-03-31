namespace EigenbelegToolAlpha
{
    partial class MultipleOrderlinesSelectMatchingSale
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultipleOrderlinesSelectMatchingSale));
            this.btn_Execute = new System.Windows.Forms.Button();
            this.comboBox_possibleOrderlines = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Execute
            // 
            this.btn_Execute.Location = new System.Drawing.Point(411, 156);
            this.btn_Execute.Name = "btn_Execute";
            this.btn_Execute.Size = new System.Drawing.Size(109, 37);
            this.btn_Execute.TabIndex = 0;
            this.btn_Execute.Text = "Ausführen";
            this.btn_Execute.UseVisualStyleBackColor = true;
            this.btn_Execute.Click += new System.EventHandler(this.btn_Execute_Click);
            // 
            // comboBox_possibleOrderlines
            // 
            this.comboBox_possibleOrderlines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_possibleOrderlines.FormattingEnabled = true;
            this.comboBox_possibleOrderlines.Location = new System.Drawing.Point(88, 87);
            this.comboBox_possibleOrderlines.Name = "comboBox_possibleOrderlines";
            this.comboBox_possibleOrderlines.Size = new System.Drawing.Size(432, 21);
            this.comboBox_possibleOrderlines.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(167, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(289, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Wähle den passenden Eintrag aus.";
            // 
            // MultipleOrderlinesSelectMatchingSale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 241);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_possibleOrderlines);
            this.Controls.Add(this.btn_Execute);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MultipleOrderlinesSelectMatchingSale";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sale zuordnen";
            this.Load += new System.EventHandler(this.MultipleOrderlinesSelectMatchingSale_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Execute;
        private System.Windows.Forms.ComboBox comboBox_possibleOrderlines;
        private System.Windows.Forms.Label label1;
    }
}