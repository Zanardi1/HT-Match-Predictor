namespace HTMatchPredictor
{
    partial class AddMultipleMatchesByMatchIDRange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddMultipleMatchesByMatchIDRange));
            this.ExplanationLabel = new System.Windows.Forms.Label();
            this.MatchIDGroupBox = new System.Windows.Forms.GroupBox();
            this.HigherBoundLabel = new System.Windows.Forms.Label();
            this.LowerBoundLabel = new System.Windows.Forms.Label();
            this.HigherBoundIDTextBox = new System.Windows.Forms.TextBox();
            this.LowerBoundIDTextBox = new System.Windows.Forms.TextBox();
            this.AddLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.MatchIDGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Location = new System.Drawing.Point(12, 9);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(383, 122);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = resources.GetString("ExplanationLabel.Text");
            // 
            // MatchIDGroupBox
            // 
            this.MatchIDGroupBox.Controls.Add(this.HigherBoundLabel);
            this.MatchIDGroupBox.Controls.Add(this.LowerBoundLabel);
            this.MatchIDGroupBox.Controls.Add(this.HigherBoundIDTextBox);
            this.MatchIDGroupBox.Controls.Add(this.LowerBoundIDTextBox);
            this.MatchIDGroupBox.Controls.Add(this.AddLabel);
            this.MatchIDGroupBox.Location = new System.Drawing.Point(15, 146);
            this.MatchIDGroupBox.Name = "MatchIDGroupBox";
            this.MatchIDGroupBox.Size = new System.Drawing.Size(380, 195);
            this.MatchIDGroupBox.TabIndex = 1;
            this.MatchIDGroupBox.TabStop = false;
            this.MatchIDGroupBox.Text = "Select the match ID range";
            // 
            // HigherBoundLabel
            // 
            this.HigherBoundLabel.AutoSize = true;
            this.HigherBoundLabel.Location = new System.Drawing.Point(6, 151);
            this.HigherBoundLabel.Name = "HigherBoundLabel";
            this.HigherBoundLabel.Size = new System.Drawing.Size(98, 17);
            this.HigherBoundLabel.TabIndex = 5;
            this.HigherBoundLabel.Text = "Higher bound:";
            // 
            // LowerBoundLabel
            // 
            this.LowerBoundLabel.AutoSize = true;
            this.LowerBoundLabel.Location = new System.Drawing.Point(6, 105);
            this.LowerBoundLabel.Name = "LowerBoundLabel";
            this.LowerBoundLabel.Size = new System.Drawing.Size(94, 17);
            this.LowerBoundLabel.TabIndex = 4;
            this.LowerBoundLabel.Text = "Lower bound:";
            // 
            // HigherBoundIDTextBox
            // 
            this.HigherBoundIDTextBox.Location = new System.Drawing.Point(113, 148);
            this.HigherBoundIDTextBox.Name = "HigherBoundIDTextBox";
            this.HigherBoundIDTextBox.Size = new System.Drawing.Size(151, 22);
            this.HigherBoundIDTextBox.TabIndex = 3;
            // 
            // LowerBoundIDTextBox
            // 
            this.LowerBoundIDTextBox.Location = new System.Drawing.Point(113, 105);
            this.LowerBoundIDTextBox.Name = "LowerBoundIDTextBox";
            this.LowerBoundIDTextBox.Size = new System.Drawing.Size(151, 22);
            this.LowerBoundIDTextBox.TabIndex = 2;
            // 
            // AddLabel
            // 
            this.AddLabel.Location = new System.Drawing.Point(6, 41);
            this.AddLabel.Name = "AddLabel";
            this.AddLabel.Size = new System.Drawing.Size(363, 43);
            this.AddLabel.TabIndex = 0;
            this.AddLabel.Text = "Add into database the corresponding matches with IDs between:";
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(15, 361);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.SaveChanges);
            // 
            // DiscardButton
            // 
            this.DiscardButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DiscardButton.Location = new System.Drawing.Point(320, 361);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(75, 23);
            this.DiscardButton.TabIndex = 3;
            this.DiscardButton.Text = "Cancel";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.DiscardChanges);
            // 
            // AddMultipleMatchesByMatchIDRange
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DiscardButton;
            this.ClientSize = new System.Drawing.Size(425, 405);
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.MatchIDGroupBox);
            this.Controls.Add(this.ExplanationLabel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(443, 452);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(443, 452);
            this.Name = "AddMultipleMatchesByMatchIDRange";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Matches By Match ID Range";
            this.MatchIDGroupBox.ResumeLayout(false);
            this.MatchIDGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.GroupBox MatchIDGroupBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.Label AddLabel;
        public System.Windows.Forms.TextBox LowerBoundIDTextBox;
        public System.Windows.Forms.TextBox HigherBoundIDTextBox;
        private System.Windows.Forms.Label HigherBoundLabel;
        private System.Windows.Forms.Label LowerBoundLabel;
    }
}