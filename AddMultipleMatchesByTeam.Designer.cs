﻿namespace HT_Match_Predictor
{
    partial class AddMultipleMatchesByTeam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddMultipleMatchesByTeam));
            this.ExplanationLabel = new System.Windows.Forms.Label();
            this.SelectionCriteriaGroupBox = new System.Windows.Forms.GroupBox();
            this.LastMatchDateTime = new System.Windows.Forms.DateTimePicker();
            this.AndLabel = new System.Windows.Forms.Label();
            this.FirstMatchDateDateTime = new System.Windows.Forms.DateTimePicker();
            this.DownloadMatchesLabel = new System.Windows.Forms.Label();
            this.SeniorTeamIDTextBox = new System.Windows.Forms.TextBox();
            this.SeniorTeamIDLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.SelectionCriteriaGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Location = new System.Drawing.Point(12, 22);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(387, 94);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = resources.GetString("ExplanationLabel.Text");
            // 
            // SelectionCriteriaGroupBox
            // 
            this.SelectionCriteriaGroupBox.Controls.Add(this.LastMatchDateTime);
            this.SelectionCriteriaGroupBox.Controls.Add(this.AndLabel);
            this.SelectionCriteriaGroupBox.Controls.Add(this.FirstMatchDateDateTime);
            this.SelectionCriteriaGroupBox.Controls.Add(this.DownloadMatchesLabel);
            this.SelectionCriteriaGroupBox.Controls.Add(this.SeniorTeamIDTextBox);
            this.SelectionCriteriaGroupBox.Controls.Add(this.SeniorTeamIDLabel);
            this.SelectionCriteriaGroupBox.Location = new System.Drawing.Point(15, 119);
            this.SelectionCriteriaGroupBox.Name = "SelectionCriteriaGroupBox";
            this.SelectionCriteriaGroupBox.Size = new System.Drawing.Size(384, 240);
            this.SelectionCriteriaGroupBox.TabIndex = 1;
            this.SelectionCriteriaGroupBox.TabStop = false;
            this.SelectionCriteriaGroupBox.Text = "Selection criteria";
            // 
            // LastMatchDateTime
            // 
            this.LastMatchDateTime.Location = new System.Drawing.Point(79, 197);
            this.LastMatchDateTime.Name = "LastMatchDateTime";
            this.LastMatchDateTime.Size = new System.Drawing.Size(200, 22);
            this.LastMatchDateTime.TabIndex = 5;
            // 
            // AndLabel
            // 
            this.AndLabel.AutoSize = true;
            this.AndLabel.Location = new System.Drawing.Point(23, 163);
            this.AndLabel.Name = "AndLabel";
            this.AndLabel.Size = new System.Drawing.Size(36, 17);
            this.AndLabel.TabIndex = 4;
            this.AndLabel.Text = "and:";
            // 
            // FirstMatchDateDateTime
            // 
            this.FirstMatchDateDateTime.Location = new System.Drawing.Point(79, 124);
            this.FirstMatchDateDateTime.Name = "FirstMatchDateDateTime";
            this.FirstMatchDateDateTime.Size = new System.Drawing.Size(200, 22);
            this.FirstMatchDateDateTime.TabIndex = 3;
            // 
            // DownloadMatchesLabel
            // 
            this.DownloadMatchesLabel.AutoSize = true;
            this.DownloadMatchesLabel.Location = new System.Drawing.Point(23, 89);
            this.DownloadMatchesLabel.Name = "DownloadMatchesLabel";
            this.DownloadMatchesLabel.Size = new System.Drawing.Size(188, 17);
            this.DownloadMatchesLabel.TabIndex = 2;
            this.DownloadMatchesLabel.Text = "Download matches between:";
            // 
            // SeniorTeamIDTextBox
            // 
            this.SeniorTeamIDTextBox.Location = new System.Drawing.Point(142, 39);
            this.SeniorTeamIDTextBox.Name = "SeniorTeamIDTextBox";
            this.SeniorTeamIDTextBox.Size = new System.Drawing.Size(200, 22);
            this.SeniorTeamIDTextBox.TabIndex = 1;
            // 
            // SeniorTeamIDLabel
            // 
            this.SeniorTeamIDLabel.AutoSize = true;
            this.SeniorTeamIDLabel.Location = new System.Drawing.Point(23, 42);
            this.SeniorTeamIDLabel.Name = "SeniorTeamIDLabel";
            this.SeniorTeamIDLabel.Size = new System.Drawing.Size(105, 17);
            this.SeniorTeamIDLabel.TabIndex = 0;
            this.SeniorTeamIDLabel.Text = "Senior team ID:";
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(15, 385);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(142, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.SaveChanges);
            // 
            // DiscardButton
            // 
            this.DiscardButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DiscardButton.Location = new System.Drawing.Point(257, 385);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(142, 23);
            this.DiscardButton.TabIndex = 3;
            this.DiscardButton.Text = "Cancel";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.IgnoreChanges);
            // 
            // AddMultipleMatchesByTeam
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DiscardButton;
            this.ClientSize = new System.Drawing.Size(434, 433);
            this.ControlBox = false;
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.SelectionCriteriaGroupBox);
            this.Controls.Add(this.ExplanationLabel);
            this.Name = "AddMultipleMatchesByTeam";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add multiple matches played by a team";
            this.Activated += new System.EventHandler(this.Startup);
            this.SelectionCriteriaGroupBox.ResumeLayout(false);
            this.SelectionCriteriaGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.GroupBox SelectionCriteriaGroupBox;
        private System.Windows.Forms.Label SeniorTeamIDLabel;
        private System.Windows.Forms.Label DownloadMatchesLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.Label AndLabel;
        public System.Windows.Forms.TextBox SeniorTeamIDTextBox;
        public System.Windows.Forms.DateTimePicker FirstMatchDateDateTime;
        public System.Windows.Forms.DateTimePicker LastMatchDateTime;
    }
}