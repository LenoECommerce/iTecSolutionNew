﻿namespace EigenbelegToolAlpha
{
    partial class EvaluationChoice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EvaluationChoice));
            this.btn_EvaluationsOverview = new System.Windows.Forms.Button();
            this.btn_StartNewEvaluation = new System.Windows.Forms.Button();
            this.btn_startQuarterlyReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_EvaluationsOverview
            // 
            this.btn_EvaluationsOverview.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_EvaluationsOverview.Location = new System.Drawing.Point(102, 202);
            this.btn_EvaluationsOverview.Name = "btn_EvaluationsOverview";
            this.btn_EvaluationsOverview.Size = new System.Drawing.Size(310, 55);
            this.btn_EvaluationsOverview.TabIndex = 0;
            this.btn_EvaluationsOverview.Text = "Auswertungen Übersicht";
            this.btn_EvaluationsOverview.UseVisualStyleBackColor = true;
            this.btn_EvaluationsOverview.Click += new System.EventHandler(this.btn_EvaluationsOverview_Click);
            // 
            // btn_StartNewEvaluation
            // 
            this.btn_StartNewEvaluation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_StartNewEvaluation.Location = new System.Drawing.Point(102, 40);
            this.btn_StartNewEvaluation.Name = "btn_StartNewEvaluation";
            this.btn_StartNewEvaluation.Size = new System.Drawing.Size(310, 55);
            this.btn_StartNewEvaluation.TabIndex = 1;
            this.btn_StartNewEvaluation.Text = "Monatliche Auswertung starten";
            this.btn_StartNewEvaluation.UseVisualStyleBackColor = true;
            this.btn_StartNewEvaluation.Click += new System.EventHandler(this.btn_StartNewEvaluation_Click);
            // 
            // btn_startQuarterlyReport
            // 
            this.btn_startQuarterlyReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_startQuarterlyReport.Location = new System.Drawing.Point(102, 121);
            this.btn_startQuarterlyReport.Name = "btn_startQuarterlyReport";
            this.btn_startQuarterlyReport.Size = new System.Drawing.Size(310, 55);
            this.btn_startQuarterlyReport.TabIndex = 2;
            this.btn_startQuarterlyReport.Text = "Quartalsauswertung starten";
            this.btn_startQuarterlyReport.UseVisualStyleBackColor = true;
            this.btn_startQuarterlyReport.Click += new System.EventHandler(this.btn_startQuarterlyReport_Click);
            // 
            // EvaluationChoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(523, 301);
            this.Controls.Add(this.btn_startQuarterlyReport);
            this.Controls.Add(this.btn_StartNewEvaluation);
            this.Controls.Add(this.btn_EvaluationsOverview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "EvaluationChoice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auswertung: Auswahl treffen";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_EvaluationsOverview;
        private System.Windows.Forms.Button btn_StartNewEvaluation;
        private System.Windows.Forms.Button btn_startQuarterlyReport;
    }
}