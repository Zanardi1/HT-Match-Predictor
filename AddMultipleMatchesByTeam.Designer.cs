namespace HTMatchPredictor
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
            this.SeasonTextBox = new System.Windows.Forms.TextBox();
            this.SeasonLabel = new System.Windows.Forms.Label();
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
            this.ExplanationLabel.Size = new System.Drawing.Size(387, 142);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = resources.GetString("ExplanationLabel.Text");
            // 
            // SelectionCriteriaGroupBox
            // 
            this.SelectionCriteriaGroupBox.Controls.Add(this.SeasonTextBox);
            this.SelectionCriteriaGroupBox.Controls.Add(this.SeasonLabel);
            this.SelectionCriteriaGroupBox.Controls.Add(this.SeniorTeamIDTextBox);
            this.SelectionCriteriaGroupBox.Controls.Add(this.SeniorTeamIDLabel);
            this.SelectionCriteriaGroupBox.Location = new System.Drawing.Point(15, 168);
            this.SelectionCriteriaGroupBox.Name = "SelectionCriteriaGroupBox";
            this.SelectionCriteriaGroupBox.Size = new System.Drawing.Size(384, 124);
            this.SelectionCriteriaGroupBox.TabIndex = 1;
            this.SelectionCriteriaGroupBox.TabStop = false;
            this.SelectionCriteriaGroupBox.Text = "Selection criteria";
            // 
            // SeasonTextBox
            // 
            this.SeasonTextBox.Location = new System.Drawing.Point(142, 82);
            this.SeasonTextBox.Name = "SeasonTextBox";
            this.SeasonTextBox.Size = new System.Drawing.Size(200, 22);
            this.SeasonTextBox.TabIndex = 3;
            // 
            // SeasonLabel
            // 
            this.SeasonLabel.AutoSize = true;
            this.SeasonLabel.Location = new System.Drawing.Point(23, 82);
            this.SeasonLabel.Name = "SeasonLabel";
            this.SeasonLabel.Size = new System.Drawing.Size(60, 17);
            this.SeasonLabel.TabIndex = 2;
            this.SeasonLabel.Text = "Season:";
            // 
            // SeniorTeamIDTextBox
            // 
            this.SeniorTeamIDTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.SeniorTeamIDTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
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
            this.OKButton.Location = new System.Drawing.Point(15, 311);
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
            this.DiscardButton.Location = new System.Drawing.Point(257, 311);
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
            this.ClientSize = new System.Drawing.Size(431, 359);
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.SelectionCriteriaGroupBox);
            this.Controls.Add(this.ExplanationLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(449, 406);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(449, 406);
            this.Name = "AddMultipleMatchesByTeam";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add multiple matches played by a team";
            this.SelectionCriteriaGroupBox.ResumeLayout(false);
            this.SelectionCriteriaGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.GroupBox SelectionCriteriaGroupBox;
        private System.Windows.Forms.Label SeniorTeamIDLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button DiscardButton;
        public System.Windows.Forms.TextBox SeniorTeamIDTextBox;
        private System.Windows.Forms.Label SeasonLabel;
        public System.Windows.Forms.TextBox SeasonTextBox;
    }
}