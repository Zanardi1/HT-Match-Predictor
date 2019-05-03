namespace HT_Match_Predictor
{
    partial class SkillSelectionWindow
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
            this.OKButton = new System.Windows.Forms.Button();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.SubskillLevelLabel = new System.Windows.Forms.Label();
            this.SkillLevelLabel = new System.Windows.Forms.Label();
            this.SubskillListBox = new System.Windows.Forms.ListBox();
            this.SkillListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(222, 173);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(120, 23);
            this.OKButton.TabIndex = 14;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.SaveChoice);
            // 
            // DiscardButton
            // 
            this.DiscardButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DiscardButton.Location = new System.Drawing.Point(222, 233);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(120, 23);
            this.DiscardButton.TabIndex = 15;
            this.DiscardButton.Text = "Cancel";
            this.DiscardButton.UseVisualStyleBackColor = true;
            // 
            // SubskillLevelLabel
            // 
            this.SubskillLevelLabel.AutoSize = true;
            this.SubskillLevelLabel.Location = new System.Drawing.Point(219, 34);
            this.SubskillLevelLabel.Name = "SubskillLevelLabel";
            this.SubskillLevelLabel.Size = new System.Drawing.Size(56, 17);
            this.SubskillLevelLabel.TabIndex = 19;
            this.SubskillLevelLabel.Text = "Subskill";
            // 
            // SkillLevelLabel
            // 
            this.SkillLevelLabel.AutoSize = true;
            this.SkillLevelLabel.Location = new System.Drawing.Point(20, 34);
            this.SkillLevelLabel.Name = "SkillLevelLabel";
            this.SkillLevelLabel.Size = new System.Drawing.Size(66, 17);
            this.SkillLevelLabel.TabIndex = 18;
            this.SkillLevelLabel.Text = "Skill level";
            // 
            // SubskillListBox
            // 
            this.SubskillListBox.FormattingEnabled = true;
            this.SubskillListBox.ItemHeight = 16;
            this.SubskillListBox.Items.AddRange(new object[] {
            "very low",
            "low",
            "high",
            "very high"});
            this.SubskillListBox.Location = new System.Drawing.Point(222, 66);
            this.SubskillListBox.Name = "SubskillListBox";
            this.SubskillListBox.Size = new System.Drawing.Size(120, 68);
            this.SubskillListBox.TabIndex = 17;
            // 
            // SkillListBox
            // 
            this.SkillListBox.FormattingEnabled = true;
            this.SkillListBox.ItemHeight = 16;
            this.SkillListBox.Items.AddRange(new object[] {
            "Disastruous(1)",
            "Wretched (2)",
            "Poor (3)",
            "Weak (4)",
            "Inadequate (5)",
            "Passable (6)",
            "Solid (7)",
            "Excellent (8)",
            "Formidable (9)",
            "Outstanding (10)",
            "Brilliant (11)",
            "Magnificent (12)",
            "World Class (13)",
            "Supernatural (14)",
            "Titanic (15)",
            "Extraterrestrial (16)",
            "Mythical (17)",
            "Magical (18)",
            "Utopian (19)",
            "Divine (20)"});
            this.SkillListBox.Location = new System.Drawing.Point(23, 66);
            this.SkillListBox.Name = "SkillListBox";
            this.SkillListBox.Size = new System.Drawing.Size(159, 324);
            this.SkillListBox.TabIndex = 16;
            // 
            // SkillSelectionWindow
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DiscardButton;
            this.ClientSize = new System.Drawing.Size(372, 431);
            this.ControlBox = false;
            this.Controls.Add(this.SubskillLevelLabel);
            this.Controls.Add(this.SkillLevelLabel);
            this.Controls.Add(this.SubskillListBox);
            this.Controls.Add(this.SkillListBox);
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.OKButton);
            this.Name = "SkillSelectionWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SkillSelectionWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.Label SubskillLevelLabel;
        private System.Windows.Forms.Label SkillLevelLabel;
        private System.Windows.Forms.ListBox SubskillListBox;
        private System.Windows.Forms.ListBox SkillListBox;
    }
}