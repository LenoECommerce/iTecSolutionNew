namespace EigenbelegToolAlpha
{
    partial class UIWorkflowShippingProcess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UIWorkflowShippingProcess));
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lbl_foundOrders = new System.Windows.Forms.Label();
            this.btn_Start = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_DeliveryNotes = new System.Windows.Forms.Label();
            this.lbl_shippingLabels = new System.Windows.Forms.Label();
            this.lbl_merge = new System.Windows.Forms.Label();
            this.lbl_upload = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Atlanta", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(91, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 28);
            this.label2.TabIndex = 45;
            this.label2.Text = "Gefundene Orders";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(96, 59);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(633, 23);
            this.progressBar1.TabIndex = 46;
            // 
            // lbl_foundOrders
            // 
            this.lbl_foundOrders.AutoSize = true;
            this.lbl_foundOrders.BackColor = System.Drawing.Color.Transparent;
            this.lbl_foundOrders.Font = new System.Drawing.Font("Atlanta", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_foundOrders.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbl_foundOrders.Location = new System.Drawing.Point(693, 106);
            this.lbl_foundOrders.Name = "lbl_foundOrders";
            this.lbl_foundOrders.Size = new System.Drawing.Size(24, 28);
            this.lbl_foundOrders.TabIndex = 47;
            this.lbl_foundOrders.Text = "0";
            // 
            // btn_Start
            // 
            this.btn_Start.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Start.Location = new System.Drawing.Point(312, 12);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(204, 28);
            this.btn_Start.TabIndex = 56;
            this.btn_Start.Text = "Starten";
            this.btn_Start.UseVisualStyleBackColor = false;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Atlanta", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(91, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 28);
            this.label1.TabIndex = 57;
            this.label1.Text = "Lieferscheine";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Atlanta", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(91, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 28);
            this.label3.TabIndex = 58;
            this.label3.Text = "Etiketten";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Atlanta", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(91, 250);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 28);
            this.label4.TabIndex = 59;
            this.label4.Text = "Merge";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Atlanta", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(91, 299);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 28);
            this.label5.TabIndex = 60;
            this.label5.Text = "Upload";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // lbl_DeliveryNotes
            // 
            this.lbl_DeliveryNotes.AutoSize = true;
            this.lbl_DeliveryNotes.BackColor = System.Drawing.Color.Transparent;
            this.lbl_DeliveryNotes.Font = new System.Drawing.Font("Atlanta", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_DeliveryNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbl_DeliveryNotes.Location = new System.Drawing.Point(693, 154);
            this.lbl_DeliveryNotes.Name = "lbl_DeliveryNotes";
            this.lbl_DeliveryNotes.Size = new System.Drawing.Size(24, 28);
            this.lbl_DeliveryNotes.TabIndex = 61;
            this.lbl_DeliveryNotes.Text = "0";
            // 
            // lbl_shippingLabels
            // 
            this.lbl_shippingLabels.AutoSize = true;
            this.lbl_shippingLabels.BackColor = System.Drawing.Color.Transparent;
            this.lbl_shippingLabels.Font = new System.Drawing.Font("Atlanta", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_shippingLabels.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbl_shippingLabels.Location = new System.Drawing.Point(693, 203);
            this.lbl_shippingLabels.Name = "lbl_shippingLabels";
            this.lbl_shippingLabels.Size = new System.Drawing.Size(24, 28);
            this.lbl_shippingLabels.TabIndex = 62;
            this.lbl_shippingLabels.Text = "0";
            // 
            // lbl_merge
            // 
            this.lbl_merge.AutoSize = true;
            this.lbl_merge.BackColor = System.Drawing.Color.Transparent;
            this.lbl_merge.Font = new System.Drawing.Font("Atlanta", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_merge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbl_merge.Location = new System.Drawing.Point(610, 250);
            this.lbl_merge.Name = "lbl_merge";
            this.lbl_merge.Size = new System.Drawing.Size(117, 28);
            this.lbl_merge.TabIndex = 63;
            this.lbl_merge.Text = "      not yet";
            // 
            // lbl_upload
            // 
            this.lbl_upload.AutoSize = true;
            this.lbl_upload.BackColor = System.Drawing.Color.Transparent;
            this.lbl_upload.Font = new System.Drawing.Font("Atlanta", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_upload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbl_upload.Location = new System.Drawing.Point(610, 299);
            this.lbl_upload.Name = "lbl_upload";
            this.lbl_upload.Size = new System.Drawing.Size(117, 28);
            this.lbl_upload.TabIndex = 64;
            this.lbl_upload.Text = "      not yet";
            // 
            // UIWorkflowShippingProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 418);
            this.Controls.Add(this.lbl_upload);
            this.Controls.Add(this.lbl_merge);
            this.Controls.Add(this.lbl_shippingLabels);
            this.Controls.Add(this.lbl_DeliveryNotes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.lbl_foundOrders);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UIWorkflowShippingProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UIWorkflowShippingProcess";
            this.Load += new System.EventHandler(this.UIWorkflowShippingProcess_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.ProgressBar progressBar1;
        public System.Windows.Forms.Label lbl_foundOrders;
        public System.Windows.Forms.Button btn_Start;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label lbl_DeliveryNotes;
        public System.Windows.Forms.Label lbl_shippingLabels;
        public System.Windows.Forms.Label lbl_merge;
        public System.Windows.Forms.Label lbl_upload;
    }
}