namespace EigenbelegToolAlpha
{
    partial class Eigenbelege
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Eigenbelege));
            this.lbl_foundPath = new System.Windows.Forms.Label();
            this.eigenbelegeDGV = new System.Windows.Forms.DataGridView();
            this.btn_eigenbelegCreate = new System.Windows.Forms.Button();
            this.btn_eigenbelegEdit = new System.Windows.Forms.Button();
            this.btn_eigenbelegRemove = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.openFD = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_PushToRep = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.btn_SwitchToRelatedReparatur = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fensterwechselToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eigenbelegeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.warenausgangToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sucheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_workflowArrivalOfGoods = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.mySqlDataAdapter1 = new MySql.Data.MySqlClient.MySqlDataAdapter();
            this.button2 = new System.Windows.Forms.Button();
            this.auswertungGenerierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.eigenbelegeDGV)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_foundPath
            // 
            this.lbl_foundPath.AutoSize = true;
            this.lbl_foundPath.Location = new System.Drawing.Point(47, 373);
            this.lbl_foundPath.Name = "lbl_foundPath";
            this.lbl_foundPath.Size = new System.Drawing.Size(0, 13);
            this.lbl_foundPath.TabIndex = 1;
            // 
            // eigenbelegeDGV
            // 
            this.eigenbelegeDGV.AllowUserToAddRows = false;
            this.eigenbelegeDGV.AllowUserToDeleteRows = false;
            this.eigenbelegeDGV.AllowUserToResizeColumns = false;
            this.eigenbelegeDGV.AllowUserToResizeRows = false;
            this.eigenbelegeDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.eigenbelegeDGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.eigenbelegeDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.eigenbelegeDGV.Location = new System.Drawing.Point(61, 225);
            this.eigenbelegeDGV.Name = "eigenbelegeDGV";
            this.eigenbelegeDGV.ReadOnly = true;
            this.eigenbelegeDGV.RowHeadersVisible = false;
            this.eigenbelegeDGV.RowHeadersWidth = 62;
            this.eigenbelegeDGV.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.eigenbelegeDGV.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.eigenbelegeDGV.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.eigenbelegeDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.eigenbelegeDGV.Size = new System.Drawing.Size(1153, 533);
            this.eigenbelegeDGV.TabIndex = 5;
            this.eigenbelegeDGV.TabStop = false;
            this.eigenbelegeDGV.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.eigenbelegeDGV_CellClick);
            this.eigenbelegeDGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.eigenbelegeDGV_CellContentClick);
            // 
            // btn_eigenbelegCreate
            // 
            this.btn_eigenbelegCreate.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_eigenbelegCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_eigenbelegCreate.Location = new System.Drawing.Point(79, 179);
            this.btn_eigenbelegCreate.Name = "btn_eigenbelegCreate";
            this.btn_eigenbelegCreate.Size = new System.Drawing.Size(121, 26);
            this.btn_eigenbelegCreate.TabIndex = 22;
            this.btn_eigenbelegCreate.Text = "Erstellen";
            this.btn_eigenbelegCreate.UseVisualStyleBackColor = false;
            this.btn_eigenbelegCreate.Click += new System.EventHandler(this.btn_eigenbelegCreate_Click);
            // 
            // btn_eigenbelegEdit
            // 
            this.btn_eigenbelegEdit.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_eigenbelegEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_eigenbelegEdit.Location = new System.Drawing.Point(223, 179);
            this.btn_eigenbelegEdit.Name = "btn_eigenbelegEdit";
            this.btn_eigenbelegEdit.Size = new System.Drawing.Size(121, 26);
            this.btn_eigenbelegEdit.TabIndex = 23;
            this.btn_eigenbelegEdit.Text = "Bearbeiten";
            this.btn_eigenbelegEdit.UseVisualStyleBackColor = false;
            this.btn_eigenbelegEdit.Click += new System.EventHandler(this.btn_eigenbelegEdit_Click);
            // 
            // btn_eigenbelegRemove
            // 
            this.btn_eigenbelegRemove.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_eigenbelegRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_eigenbelegRemove.Location = new System.Drawing.Point(370, 179);
            this.btn_eigenbelegRemove.Name = "btn_eigenbelegRemove";
            this.btn_eigenbelegRemove.Size = new System.Drawing.Size(121, 26);
            this.btn_eigenbelegRemove.TabIndex = 24;
            this.btn_eigenbelegRemove.Text = "Löschen";
            this.btn_eigenbelegRemove.UseVisualStyleBackColor = false;
            this.btn_eigenbelegRemove.Click += new System.EventHandler(this.btn_eigenbelegRemove_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Location = new System.Drawing.Point(79, 80);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(134, 26);
            this.button3.TabIndex = 26;
            this.button3.Text = "Eigenbeleg erstellen";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // openFD
            // 
            this.openFD.FileName = "openFD";
            // 
            // btn_PushToRep
            // 
            this.btn_PushToRep.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_PushToRep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_PushToRep.ForeColor = System.Drawing.Color.Black;
            this.btn_PushToRep.Location = new System.Drawing.Point(254, 80);
            this.btn_PushToRep.Name = "btn_PushToRep";
            this.btn_PushToRep.Size = new System.Drawing.Size(134, 26);
            this.btn_PushToRep.TabIndex = 35;
            this.btn_PushToRep.Text = "Als Reparatur erfassen";
            this.btn_PushToRep.UseVisualStyleBackColor = false;
            this.btn_PushToRep.Click += new System.EventHandler(this.btn_PushToRep_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.ForeColor = System.Drawing.Color.Black;
            this.button5.Location = new System.Drawing.Point(1080, 79);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(134, 27);
            this.button5.TabIndex = 37;
            this.button5.Text = "Aktualisieren";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // btn_SwitchToRelatedReparatur
            // 
            this.btn_SwitchToRelatedReparatur.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_SwitchToRelatedReparatur.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SwitchToRelatedReparatur.ForeColor = System.Drawing.Color.Black;
            this.btn_SwitchToRelatedReparatur.Location = new System.Drawing.Point(430, 80);
            this.btn_SwitchToRelatedReparatur.Name = "btn_SwitchToRelatedReparatur";
            this.btn_SwitchToRelatedReparatur.Size = new System.Drawing.Size(134, 26);
            this.btn_SwitchToRelatedReparatur.TabIndex = 38;
            this.btn_SwitchToRelatedReparatur.Text = "Zu Reparatur springen";
            this.btn_SwitchToRelatedReparatur.UseVisualStyleBackColor = false;
            this.btn_SwitchToRelatedReparatur.Click += new System.EventHandler(this.btn_SwitchToRelatedReparatur_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(74, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(242, 25);
            this.label2.TabIndex = 44;
            this.label2.Text = "Erweiterte Funktionen";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(74, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 25);
            this.label1.TabIndex = 45;
            this.label1.Text = "DB - Hauptfunktionen";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fensterwechselToolStripMenuItem,
            this.sucheToolStripMenuItem,
            this.auswertungGenerierenToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(1270, 30);
            this.menuStrip1.TabIndex = 48;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fensterwechselToolStripMenuItem
            // 
            this.fensterwechselToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eigenbelegeToolStripMenuItem,
            this.warenausgangToolStripMenuItem});
            this.fensterwechselToolStripMenuItem.Image = global::EigenbelegToolAlpha.Properties.Resources.windowsvg;
            this.fensterwechselToolStripMenuItem.Name = "fensterwechselToolStripMenuItem";
            this.fensterwechselToolStripMenuItem.Size = new System.Drawing.Size(123, 28);
            this.fensterwechselToolStripMenuItem.Text = "Fensterwechsel";
            // 
            // eigenbelegeToolStripMenuItem
            // 
            this.eigenbelegeToolStripMenuItem.Name = "eigenbelegeToolStripMenuItem";
            this.eigenbelegeToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.eigenbelegeToolStripMenuItem.Text = "Reparaturen";
            this.eigenbelegeToolStripMenuItem.Click += new System.EventHandler(this.eigenbelegeToolStripMenuItem_Click);
            // 
            // warenausgangToolStripMenuItem
            // 
            this.warenausgangToolStripMenuItem.Name = "warenausgangToolStripMenuItem";
            this.warenausgangToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.warenausgangToolStripMenuItem.Text = "Warenausgang";
            this.warenausgangToolStripMenuItem.Click += new System.EventHandler(this.warenausgangToolStripMenuItem_Click);
            // 
            // sucheToolStripMenuItem
            // 
            this.sucheToolStripMenuItem.Image = global::EigenbelegToolAlpha.Properties.Resources.suche;
            this.sucheToolStripMenuItem.Name = "sucheToolStripMenuItem";
            this.sucheToolStripMenuItem.Size = new System.Drawing.Size(75, 28);
            this.sucheToolStripMenuItem.Text = "Suche";
            this.sucheToolStripMenuItem.Click += new System.EventHandler(this.sucheToolStripMenuItem_Click);
            // 
            // btn_workflowArrivalOfGoods
            // 
            this.btn_workflowArrivalOfGoods.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_workflowArrivalOfGoods.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_workflowArrivalOfGoods.Location = new System.Drawing.Point(598, 169);
            this.btn_workflowArrivalOfGoods.Name = "btn_workflowArrivalOfGoods";
            this.btn_workflowArrivalOfGoods.Size = new System.Drawing.Size(187, 36);
            this.btn_workflowArrivalOfGoods.TabIndex = 54;
            this.btn_workflowArrivalOfGoods.Text = "Warenannahme Workflow";
            this.btn_workflowArrivalOfGoods.UseVisualStyleBackColor = false;
            this.btn_workflowArrivalOfGoods.Click += new System.EventHandler(this.btn_workflowArrivalOfGoods_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(593, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 25);
            this.label4.TabIndex = 57;
            this.label4.Text = "Workflows";
            // 
            // mySqlDataAdapter1
            // 
            this.mySqlDataAdapter1.DeleteCommand = null;
            this.mySqlDataAdapter1.InsertCommand = null;
            this.mySqlDataAdapter1.SelectCommand = null;
            this.mySqlDataAdapter1.UpdateCommand = null;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(1080, 112);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(134, 26);
            this.button2.TabIndex = 58;
            this.button2.Text = "Einstellungen";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // auswertungGenerierenToolStripMenuItem
            // 
            this.auswertungGenerierenToolStripMenuItem.Name = "auswertungGenerierenToolStripMenuItem";
            this.auswertungGenerierenToolStripMenuItem.Size = new System.Drawing.Size(142, 28);
            this.auswertungGenerierenToolStripMenuItem.Text = "Auswertung generieren";
            this.auswertungGenerierenToolStripMenuItem.Click += new System.EventHandler(this.auswertungGenerierenToolStripMenuItem_Click);
            // 
            // Eigenbelege
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1270, 682);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_workflowArrivalOfGoods);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_SwitchToRelatedReparatur);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.btn_PushToRep);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btn_eigenbelegRemove);
            this.Controls.Add(this.btn_eigenbelegEdit);
            this.Controls.Add(this.btn_eigenbelegCreate);
            this.Controls.Add(this.eigenbelegeDGV);
            this.Controls.Add(this.lbl_foundPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Eigenbelege";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Eigenbelege";
            this.Load += new System.EventHandler(this.Hauptmenü_Load);
            ((System.ComponentModel.ISupportInitialize)(this.eigenbelegeDGV)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl_foundPath;
        public System.Windows.Forms.DataGridView eigenbelegeDGV;
        private System.Windows.Forms.Button btn_eigenbelegCreate;
        private System.Windows.Forms.Button btn_eigenbelegEdit;
        private System.Windows.Forms.Button btn_eigenbelegRemove;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.OpenFileDialog openFD;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btn_PushToRep;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btn_SwitchToRelatedReparatur;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fensterwechselToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eigenbelegeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sucheToolStripMenuItem;
        public System.Windows.Forms.Button btn_workflowArrivalOfGoods;
        private System.Windows.Forms.Label label4;
        private MySql.Data.MySqlClient.MySqlDataAdapter mySqlDataAdapter1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem warenausgangToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem auswertungGenerierenToolStripMenuItem;
    }
}

