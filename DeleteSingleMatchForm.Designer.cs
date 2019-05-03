namespace HT_Match_Predictor
{
    partial class DeleteSingleMatchForm
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
            this.DiscardButton = new System.Windows.Forms.Button();
            this.MatchIDTextBox = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.ExplanationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // DiscardButton
            // 
            this.DiscardButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DiscardButton.Location = new System.Drawing.Point(284, 46);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(75, 23);
            this.DiscardButton.TabIndex = 7;
            this.DiscardButton.Text = "Cancel";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.IgnoreChanges);
            // 
            // MatchIDTextBox
            // 
            this.MatchIDTextBox.Location = new System.Drawing.Point(15, 47);
            this.MatchIDTextBox.Name = "MatchIDTextBox";
            this.MatchIDTextBox.Size = new System.Drawing.Size(126, 22);
            this.MatchIDTextBox.TabIndex = 6;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(175, 46);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 5;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.SaveChanges);
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.AutoSize = true;
            this.ExplanationLabel.Location = new System.Drawing.Point(12, 9);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(455, 17);
            this.ExplanationLabel.TabIndex = 4;
            this.ExplanationLabel.Text = "Please insert the ID of the match you want to delete from the database.";
            // 
            // DeleteSingleMatchForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DiscardButton;
            this.ClientSize = new System.Drawing.Size(473, 94);
            this.ControlBox = false;
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.MatchIDTextBox);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.ExplanationLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteSingleMatchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DeleteSingleMatchForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.TextBox MatchIDTextBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label ExplanationLabel;
    }
}