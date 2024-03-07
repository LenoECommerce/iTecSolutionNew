namespace EigenbelegToolAlpha
{
    partial class ListingScanInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListingScanInput));
            this.textBox_scanInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_scanInput
            // 
            this.textBox_scanInput.Location = new System.Drawing.Point(89, 32);
            this.textBox_scanInput.Name = "textBox_scanInput";
            this.textBox_scanInput.Size = new System.Drawing.Size(209, 20);
            this.textBox_scanInput.TabIndex = 0;
            this.textBox_scanInput.TextChanged += new System.EventHandler(this.textBox_scanInput_TextChanged);
            this.textBox_scanInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDownHandler);
            // 
            // ListingScanInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(379, 91);
            this.Controls.Add(this.textBox_scanInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ListingScanInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ListingScanInput";
            this.Load += new System.EventHandler(this.ListingScanInput_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_scanInput;
    }
}