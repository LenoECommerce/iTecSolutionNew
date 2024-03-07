namespace EigenbelegToolAlpha
{
    partial class ReparturenEditSparePartsDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReparturenEditSparePartsDetails));
            this.usedSparePartsDGV = new System.Windows.Forms.DataGridView();
            this.btn_saveChanges = new System.Windows.Forms.Button();
            this.btn_usedSparePartsDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.usedSparePartsDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // usedSparePartsDGV
            // 
            this.usedSparePartsDGV.AllowUserToAddRows = false;
            this.usedSparePartsDGV.AllowUserToDeleteRows = false;
            this.usedSparePartsDGV.AllowUserToResizeColumns = false;
            this.usedSparePartsDGV.AllowUserToResizeRows = false;
            this.usedSparePartsDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.usedSparePartsDGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.usedSparePartsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.usedSparePartsDGV.Location = new System.Drawing.Point(72, 57);
            this.usedSparePartsDGV.Name = "usedSparePartsDGV";
            this.usedSparePartsDGV.ReadOnly = true;
            this.usedSparePartsDGV.RowHeadersVisible = false;
            this.usedSparePartsDGV.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.usedSparePartsDGV.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.usedSparePartsDGV.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.usedSparePartsDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.usedSparePartsDGV.Size = new System.Drawing.Size(643, 314);
            this.usedSparePartsDGV.TabIndex = 30;
            this.usedSparePartsDGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.usedSparePartsDGV_CellContentClick);
            // 
            // btn_saveChanges
            // 
            this.btn_saveChanges.Location = new System.Drawing.Point(576, 390);
            this.btn_saveChanges.Name = "btn_saveChanges";
            this.btn_saveChanges.Size = new System.Drawing.Size(139, 37);
            this.btn_saveChanges.TabIndex = 31;
            this.btn_saveChanges.Text = "Änderungen speichern";
            this.btn_saveChanges.UseVisualStyleBackColor = true;
            this.btn_saveChanges.Click += new System.EventHandler(this.btn_saveChanges_Click);
            // 
            // btn_usedSparePartsDelete
            // 
            this.btn_usedSparePartsDelete.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_usedSparePartsDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_usedSparePartsDelete.Location = new System.Drawing.Point(581, 12);
            this.btn_usedSparePartsDelete.Name = "btn_usedSparePartsDelete";
            this.btn_usedSparePartsDelete.Size = new System.Drawing.Size(134, 26);
            this.btn_usedSparePartsDelete.TabIndex = 36;
            this.btn_usedSparePartsDelete.Text = "Eintrag löschen";
            this.btn_usedSparePartsDelete.UseVisualStyleBackColor = false;
            this.btn_usedSparePartsDelete.Click += new System.EventHandler(this.btn_usedSparePartsDelete_Click);
            // 
            // ReparturenEditSparePartsDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_usedSparePartsDelete);
            this.Controls.Add(this.btn_saveChanges);
            this.Controls.Add(this.usedSparePartsDGV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ReparturenEditSparePartsDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Verbaute Ersatzteile Detailansicht";
            this.Load += new System.EventHandler(this.ReparturenEditSparePartsDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.usedSparePartsDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView usedSparePartsDGV;
        private System.Windows.Forms.Button btn_saveChanges;
        private System.Windows.Forms.Button btn_usedSparePartsDelete;
    }
}