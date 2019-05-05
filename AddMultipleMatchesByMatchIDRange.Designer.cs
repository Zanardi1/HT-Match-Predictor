namespace HT_Match_Predictor
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
            this.HigherBoundIDTextBox = new System.Windows.Forms.TextBox();
            this.LowerBoundIDTextBox = new System.Windows.Forms.TextBox();
            this.AndLabel = new System.Windows.Forms.Label();
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
            this.ExplanationLabel.Size = new System.Drawing.Size(383, 91);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = resources.GetString("ExplanationLabel.Text");
            // 
            // MatchIDGroupBox
            // 
            this.MatchIDGroupBox.Controls.Add(this.HigherBoundIDTextBox);
            this.MatchIDGroupBox.Controls.Add(this.LowerBoundIDTextBox);
            this.MatchIDGroupBox.Controls.Add(this.AndLabel);
            this.MatchIDGroupBox.Controls.Add(this.AddLabel);
            this.MatchIDGroupBox.Location = new System.Drawing.Point(15, 118);
            this.MatchIDGroupBox.Name = "MatchIDGroupBox";
            this.MatchIDGroupBox.Size = new System.Drawing.Size(380, 195);
            this.MatchIDGroupBox.TabIndex = 1;
            this.MatchIDGroupBox.TabStop = false;
            this.MatchIDGroupBox.Text = "Select the match ID range";
            // 
            // HigherBoundIDTextBox
            // 
            this.HigherBoundIDTextBox.Location = new System.Drawing.Point(32, 146);
            this.HigherBoundIDTextBox.Name = "HigherBoundIDTextBox";
            this.HigherBoundIDTextBox.Size = new System.Drawing.Size(151, 22);
            this.HigherBoundIDTextBox.TabIndex = 3;
            // 
            // LowerBoundIDTextBox
            // 
            this.LowerBoundIDTextBox.Location = new System.Drawing.Point(32, 75);
            this.LowerBoundIDTextBox.Name = "LowerBoundIDTextBox";
            this.LowerBoundIDTextBox.Size = new System.Drawing.Size(151, 22);
            this.LowerBoundIDTextBox.TabIndex = 2;
            // 
            // AndLabel
            // 
            this.AndLabel.AutoSize = true;
            this.AndLabel.Location = new System.Drawing.Point(6, 110);
            this.AndLabel.Name = "AndLabel";
            this.AndLabel.Size = new System.Drawing.Size(36, 17);
            this.AndLabel.TabIndex = 1;
            this.AndLabel.Text = "and:";
            // 
            // AddLabel
            // 
            this.AddLabel.AutoSize = true;
            this.AddLabel.Location = new System.Drawing.Point(6, 41);
            this.AddLabel.Name = "AddLabel";
            this.AddLabel.Size = new System.Drawing.Size(363, 17);
            this.AddLabel.TabIndex = 0;
            this.AddLabel.Text = "Add into database appropriate matches with ID between:";
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(15, 330);
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
            this.DiscardButton.Location = new System.Drawing.Point(320, 330);
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
            this.ClientSize = new System.Drawing.Size(425, 365);
            this.ControlBox = false;
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.MatchIDGroupBox);
            this.Controls.Add(this.ExplanationLabel);
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
        private System.Windows.Forms.Label AndLabel;
        private System.Windows.Forms.Label AddLabel;
        public System.Windows.Forms.TextBox LowerBoundIDTextBox;
        public System.Windows.Forms.TextBox HigherBoundIDTextBox;
    }
}