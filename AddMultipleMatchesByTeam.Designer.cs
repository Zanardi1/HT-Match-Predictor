namespace HT_Match_Predictor
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
            this.LastMatchDateDateTime = new System.Windows.Forms.DateTimePicker();
            this.LastMatchDateLabel = new System.Windows.Forms.Label();
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
            this.ExplanationLabel.Size = new System.Drawing.Size(387, 38);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = resources.GetString("ExplanationLabel.Text");
            // 
            // SelectionCriteriaGroupBox
            // 
            this.SelectionCriteriaGroupBox.Controls.Add(this.LastMatchDateDateTime);
            this.SelectionCriteriaGroupBox.Controls.Add(this.LastMatchDateLabel);
            this.SelectionCriteriaGroupBox.Controls.Add(this.SeniorTeamIDTextBox);
            this.SelectionCriteriaGroupBox.Controls.Add(this.SeniorTeamIDLabel);
            this.SelectionCriteriaGroupBox.Location = new System.Drawing.Point(15, 74);
            this.SelectionCriteriaGroupBox.Name = "SelectionCriteriaGroupBox";
            this.SelectionCriteriaGroupBox.Size = new System.Drawing.Size(384, 137);
            this.SelectionCriteriaGroupBox.TabIndex = 1;
            this.SelectionCriteriaGroupBox.TabStop = false;
            this.SelectionCriteriaGroupBox.Text = "Selection criteria";
            // 
            // LastMatchDateDateTime
            // 
            this.LastMatchDateDateTime.Location = new System.Drawing.Point(142, 89);
            this.LastMatchDateDateTime.Name = "LastMatchDateDateTime";
            this.LastMatchDateDateTime.Size = new System.Drawing.Size(200, 22);
            this.LastMatchDateDateTime.TabIndex = 3;
            // 
            // LastMatchDateLabel
            // 
            this.LastMatchDateLabel.AutoSize = true;
            this.LastMatchDateLabel.Location = new System.Drawing.Point(23, 89);
            this.LastMatchDateLabel.Name = "LastMatchDateLabel";
            this.LastMatchDateLabel.Size = new System.Drawing.Size(113, 17);
            this.LastMatchDateLabel.TabIndex = 2;
            this.LastMatchDateLabel.Text = "Last match date:";
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
            this.OKButton.Location = new System.Drawing.Point(15, 236);
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
            this.DiscardButton.Location = new System.Drawing.Point(257, 236);
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
            this.ClientSize = new System.Drawing.Size(434, 297);
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
        private System.Windows.Forms.TextBox SeniorTeamIDTextBox;
        private System.Windows.Forms.Label LastMatchDateLabel;
        private System.Windows.Forms.DateTimePicker LastMatchDateDateTime;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button DiscardButton;
    }
}