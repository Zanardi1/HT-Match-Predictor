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
            this.SeniorTeamIDLabel = new System.Windows.Forms.Label();
            this.SeniorTeamIDTextBox = new System.Windows.Forms.TextBox();
            this.LastMatchDateLabel = new System.Windows.Forms.Label();
            this.LastMatchDateDateTime = new System.Windows.Forms.DateTimePicker();
            this.SelectionCriteriaGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Location = new System.Drawing.Point(12, 22);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(776, 38);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = resources.GetString("ExplanationLabel.Text");
            // 
            // SelectionCriteriaGroupBox
            // 
            this.SelectionCriteriaGroupBox.Controls.Add(this.LastMatchDateDateTime);
            this.SelectionCriteriaGroupBox.Controls.Add(this.LastMatchDateLabel);
            this.SelectionCriteriaGroupBox.Controls.Add(this.SeniorTeamIDTextBox);
            this.SelectionCriteriaGroupBox.Controls.Add(this.SeniorTeamIDLabel);
            this.SelectionCriteriaGroupBox.Location = new System.Drawing.Point(15, 81);
            this.SelectionCriteriaGroupBox.Name = "SelectionCriteriaGroupBox";
            this.SelectionCriteriaGroupBox.Size = new System.Drawing.Size(737, 209);
            this.SelectionCriteriaGroupBox.TabIndex = 1;
            this.SelectionCriteriaGroupBox.TabStop = false;
            this.SelectionCriteriaGroupBox.Text = "Selection criteria";
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
            // SeniorTeamIDTextBox
            // 
            this.SeniorTeamIDTextBox.Location = new System.Drawing.Point(142, 39);
            this.SeniorTeamIDTextBox.Name = "SeniorTeamIDTextBox";
            this.SeniorTeamIDTextBox.Size = new System.Drawing.Size(200, 22);
            this.SeniorTeamIDTextBox.TabIndex = 1;
            // 
            // LastMatchDateLabel
            // 
            this.LastMatchDateLabel.AutoSize = true;
            this.LastMatchDateLabel.Location = new System.Drawing.Point(23, 116);
            this.LastMatchDateLabel.Name = "LastMatchDateLabel";
            this.LastMatchDateLabel.Size = new System.Drawing.Size(113, 17);
            this.LastMatchDateLabel.TabIndex = 2;
            this.LastMatchDateLabel.Text = "Last match date:";
            // 
            // LastMatchDateDateTime
            // 
            this.LastMatchDateDateTime.Location = new System.Drawing.Point(142, 116);
            this.LastMatchDateDateTime.Name = "LastMatchDateDateTime";
            this.LastMatchDateDateTime.Size = new System.Drawing.Size(200, 22);
            this.LastMatchDateDateTime.TabIndex = 3;
            // 
            // AddMultipleMatchesByTeam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SelectionCriteriaGroupBox);
            this.Controls.Add(this.ExplanationLabel);
            this.Name = "AddMultipleMatchesByTeam";
            this.Text = "Add multiple matches played by a team";
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
    }
}