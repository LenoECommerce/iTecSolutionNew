namespace EigenbelegToolAlpha
{
    partial class ListingFrontend
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListingFrontend));
            this.textBox_SKU = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox_amount = new System.Windows.Forms.ComboBox();
            this.textBox_information = new System.Windows.Forms.TextBox();
            this.textBox_Price = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listingsDGV = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Edit = new System.Windows.Forms.Button();
            this.btn_Create = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.zurückZuFensterEigenbelegeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBox_marketPlace = new System.Windows.Forms.ComboBox();
            this.comboBox_finished = new System.Windows.Forms.ComboBox();
            this.comboBox_filterFinished = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_Scan = new System.Windows.Forms.Button();
            this.btn_DoneSpecificListing = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.listingsDGV)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_SKU
            // 
            this.textBox_SKU.Location = new System.Drawing.Point(523, 95);
            this.textBox_SKU.Name = "textBox_SKU";
            this.textBox_SKU.Size = new System.Drawing.Size(178, 20);
            this.textBox_SKU.TabIndex = 115;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(757, 128);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 16);
            this.label9.TabIndex = 113;
            this.label9.Text = "Infos";
            // 
            // comboBox_amount
            // 
            this.comboBox_amount.FormattingEnabled = true;
            this.comboBox_amount.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.comboBox_amount.Location = new System.Drawing.Point(202, 94);
            this.comboBox_amount.Name = "comboBox_amount";
            this.comboBox_amount.Size = new System.Drawing.Size(178, 21);
            this.comboBox_amount.TabIndex = 112;
            this.comboBox_amount.Text = "1";
            // 
            // textBox_information
            // 
            this.textBox_information.Location = new System.Drawing.Point(868, 128);
            this.textBox_information.Multiline = true;
            this.textBox_information.Name = "textBox_information";
            this.textBox_information.Size = new System.Drawing.Size(298, 74);
            this.textBox_information.TabIndex = 110;
            this.textBox_information.Text = "/";
            // 
            // textBox_Price
            // 
            this.textBox_Price.Location = new System.Drawing.Point(202, 126);
            this.textBox_Price.Name = "textBox_Price";
            this.textBox_Price.Size = new System.Drawing.Size(178, 20);
            this.textBox_Price.TabIndex = 109;
            this.textBox_Price.Text = "/";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(406, 127);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 16);
            this.label7.TabIndex = 108;
            this.label7.Text = "Erledigt?";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(757, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 16);
            this.label6.TabIndex = 107;
            this.label6.Text = "Marktplatz";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(406, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 16);
            this.label5.TabIndex = 106;
            this.label5.Text = "SKU";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(72, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 16);
            this.label4.TabIndex = 105;
            this.label4.Text = "Preis";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(72, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 16);
            this.label3.TabIndex = 104;
            this.label3.Text = "Anzahl";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Atlanta", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(70, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 28);
            this.label2.TabIndex = 103;
            this.label2.Text = "Felder";
            // 
            // listingsDGV
            // 
            this.listingsDGV.AllowUserToAddRows = false;
            this.listingsDGV.AllowUserToDeleteRows = false;
            this.listingsDGV.AllowUserToResizeColumns = false;
            this.listingsDGV.AllowUserToResizeRows = false;
            this.listingsDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.listingsDGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.listingsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.listingsDGV.Location = new System.Drawing.Point(59, 276);
            this.listingsDGV.Name = "listingsDGV";
            this.listingsDGV.ReadOnly = true;
            this.listingsDGV.RowHeadersVisible = false;
            this.listingsDGV.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.listingsDGV.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.listingsDGV.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.listingsDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.listingsDGV.Size = new System.Drawing.Size(1153, 540);
            this.listingsDGV.TabIndex = 102;
            this.listingsDGV.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.listingsDGV_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Atlanta", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(70, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 28);
            this.label1.TabIndex = 101;
            this.label1.Text = "Hauptfunktionen";
            // 
            // btn_Delete
            // 
            this.btn_Delete.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_Delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Delete.Location = new System.Drawing.Point(393, 228);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(134, 26);
            this.btn_Delete.TabIndex = 100;
            this.btn_Delete.Text = "Löschen";
            this.btn_Delete.UseVisualStyleBackColor = false;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Edit
            // 
            this.btn_Edit.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_Edit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Edit.Location = new System.Drawing.Point(236, 228);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(134, 26);
            this.btn_Edit.TabIndex = 99;
            this.btn_Edit.Text = "Bearbeiten";
            this.btn_Edit.UseVisualStyleBackColor = false;
            this.btn_Edit.Click += new System.EventHandler(this.btn_Edit_Click);
            // 
            // btn_Create
            // 
            this.btn_Create.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_Create.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Create.Location = new System.Drawing.Point(75, 228);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(134, 26);
            this.btn_Create.TabIndex = 98;
            this.btn_Create.Text = "Erstellen";
            this.btn_Create.UseVisualStyleBackColor = false;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zurückZuFensterEigenbelegeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1270, 24);
            this.menuStrip1.TabIndex = 116;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // zurückZuFensterEigenbelegeToolStripMenuItem
            // 
            this.zurückZuFensterEigenbelegeToolStripMenuItem.Name = "zurückZuFensterEigenbelegeToolStripMenuItem";
            this.zurückZuFensterEigenbelegeToolStripMenuItem.Size = new System.Drawing.Size(179, 20);
            this.zurückZuFensterEigenbelegeToolStripMenuItem.Text = "Zurück zu Fenster Eigenbelege";
            this.zurückZuFensterEigenbelegeToolStripMenuItem.Click += new System.EventHandler(this.zurückZuFensterEigenbelegeToolStripMenuItem_Click);
            // 
            // comboBox_marketPlace
            // 
            this.comboBox_marketPlace.FormattingEnabled = true;
            this.comboBox_marketPlace.Items.AddRange(new object[] {
            "Ebay",
            "BackMarket"});
            this.comboBox_marketPlace.Location = new System.Drawing.Point(868, 96);
            this.comboBox_marketPlace.Name = "comboBox_marketPlace";
            this.comboBox_marketPlace.Size = new System.Drawing.Size(178, 21);
            this.comboBox_marketPlace.TabIndex = 117;
            this.comboBox_marketPlace.Text = "/";
            // 
            // comboBox_finished
            // 
            this.comboBox_finished.FormattingEnabled = true;
            this.comboBox_finished.Items.AddRange(new object[] {
            "Ja",
            "Nein"});
            this.comboBox_finished.Location = new System.Drawing.Point(523, 128);
            this.comboBox_finished.Name = "comboBox_finished";
            this.comboBox_finished.Size = new System.Drawing.Size(178, 21);
            this.comboBox_finished.TabIndex = 118;
            this.comboBox_finished.Text = "Nein";
            // 
            // comboBox_filterFinished
            // 
            this.comboBox_filterFinished.FormattingEnabled = true;
            this.comboBox_filterFinished.Items.AddRange(new object[] {
            "Ja",
            "Nein"});
            this.comboBox_filterFinished.Location = new System.Drawing.Point(868, 42);
            this.comboBox_filterFinished.Name = "comboBox_filterFinished";
            this.comboBox_filterFinished.Size = new System.Drawing.Size(178, 21);
            this.comboBox_filterFinished.TabIndex = 119;
            this.comboBox_filterFinished.Text = "Nein";
            this.comboBox_filterFinished.SelectedIndexChanged += new System.EventHandler(this.comboBox_filterFinished_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(756, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 24);
            this.label8.TabIndex = 120;
            this.label8.Text = "Filter";
            // 
            // btn_Scan
            // 
            this.btn_Scan.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_Scan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Scan.Location = new System.Drawing.Point(599, 219);
            this.btn_Scan.Name = "btn_Scan";
            this.btn_Scan.Size = new System.Drawing.Size(139, 35);
            this.btn_Scan.TabIndex = 121;
            this.btn_Scan.Text = "Scan";
            this.btn_Scan.UseVisualStyleBackColor = false;
            this.btn_Scan.Click += new System.EventHandler(this.btn_Scan_Click);
            // 
            // btn_DoneSpecificListing
            // 
            this.btn_DoneSpecificListing.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_DoneSpecificListing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DoneSpecificListing.Location = new System.Drawing.Point(771, 219);
            this.btn_DoneSpecificListing.Name = "btn_DoneSpecificListing";
            this.btn_DoneSpecificListing.Size = new System.Drawing.Size(139, 35);
            this.btn_DoneSpecificListing.TabIndex = 122;
            this.btn_DoneSpecificListing.Text = "Erledigt";
            this.btn_DoneSpecificListing.UseVisualStyleBackColor = false;
            this.btn_DoneSpecificListing.Click += new System.EventHandler(this.btn_DoneSpecificListing_Click);
            // 
            // ListingFrontend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1270, 836);
            this.Controls.Add(this.btn_DoneSpecificListing);
            this.Controls.Add(this.btn_Scan);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBox_filterFinished);
            this.Controls.Add(this.comboBox_finished);
            this.Controls.Add(this.comboBox_marketPlace);
            this.Controls.Add(this.textBox_SKU);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBox_amount);
            this.Controls.Add(this.textBox_information);
            this.Controls.Add(this.textBox_Price);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listingsDGV);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Delete);
            this.Controls.Add(this.btn_Edit);
            this.Controls.Add(this.btn_Create);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ListingFrontend";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Listing";
            this.Load += new System.EventHandler(this.ListingFrontend_Load);
            ((System.ComponentModel.ISupportInitialize)(this.listingsDGV)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_SKU;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox_amount;
        private System.Windows.Forms.TextBox textBox_information;
        private System.Windows.Forms.TextBox textBox_Price;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.DataGridView listingsDGV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Edit;
        private System.Windows.Forms.Button btn_Create;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem zurückZuFensterEigenbelegeToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox_marketPlace;
        private System.Windows.Forms.ComboBox comboBox_finished;
        private System.Windows.Forms.ComboBox comboBox_filterFinished;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_Scan;
        private System.Windows.Forms.Button btn_DoneSpecificListing;
    }
}