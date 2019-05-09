namespace HT_Match_Predictor
{
    partial class FutureMatchPrediction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FutureMatchPrediction));
            this.ExplanationLabel = new System.Windows.Forms.Label();
            this.FirstTeamRadioButton = new System.Windows.Forms.RadioButton();
            this.SecondTeamRadioButton = new System.Windows.Forms.RadioButton();
            this.ThirdTeamRadioButton = new System.Windows.Forms.RadioButton();
            this.SelectTeamGroupBox = new System.Windows.Forms.GroupBox();
            this.FutureMatchesLabel = new System.Windows.Forms.Label();
            this.FutureMatchesListbox = new System.Windows.Forms.ListBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.SelectTeamGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Location = new System.Drawing.Point(12, 9);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(450, 105);
            this.ExplanationLabel.TabIndex = 0;
            this.ExplanationLabel.Text = resources.GetString("ExplanationLabel.Text");
            // 
            // FirstTeamRadioButton
            // 
            this.FirstTeamRadioButton.AutoSize = true;
            this.FirstTeamRadioButton.Location = new System.Drawing.Point(6, 32);
            this.FirstTeamRadioButton.Name = "FirstTeamRadioButton";
            this.FirstTeamRadioButton.Size = new System.Drawing.Size(110, 21);
            this.FirstTeamRadioButton.TabIndex = 1;
            this.FirstTeamRadioButton.TabStop = true;
            this.FirstTeamRadioButton.Text = "radioButton1";
            this.FirstTeamRadioButton.UseVisualStyleBackColor = true;
            // 
            // SecondTeamRadioButton
            // 
            this.SecondTeamRadioButton.AutoSize = true;
            this.SecondTeamRadioButton.Location = new System.Drawing.Point(6, 59);
            this.SecondTeamRadioButton.Name = "SecondTeamRadioButton";
            this.SecondTeamRadioButton.Size = new System.Drawing.Size(110, 21);
            this.SecondTeamRadioButton.TabIndex = 2;
            this.SecondTeamRadioButton.TabStop = true;
            this.SecondTeamRadioButton.Text = "radioButton2";
            this.SecondTeamRadioButton.UseVisualStyleBackColor = true;
            // 
            // ThirdTeamRadioButton
            // 
            this.ThirdTeamRadioButton.AutoSize = true;
            this.ThirdTeamRadioButton.Location = new System.Drawing.Point(6, 86);
            this.ThirdTeamRadioButton.Name = "ThirdTeamRadioButton";
            this.ThirdTeamRadioButton.Size = new System.Drawing.Size(110, 21);
            this.ThirdTeamRadioButton.TabIndex = 3;
            this.ThirdTeamRadioButton.TabStop = true;
            this.ThirdTeamRadioButton.Text = "radioButton3";
            this.ThirdTeamRadioButton.UseVisualStyleBackColor = true;
            // 
            // SelectTeamGroupBox
            // 
            this.SelectTeamGroupBox.Controls.Add(this.ThirdTeamRadioButton);
            this.SelectTeamGroupBox.Controls.Add(this.SecondTeamRadioButton);
            this.SelectTeamGroupBox.Controls.Add(this.FirstTeamRadioButton);
            this.SelectTeamGroupBox.Location = new System.Drawing.Point(15, 133);
            this.SelectTeamGroupBox.Name = "SelectTeamGroupBox";
            this.SelectTeamGroupBox.Size = new System.Drawing.Size(212, 116);
            this.SelectTeamGroupBox.TabIndex = 4;
            this.SelectTeamGroupBox.TabStop = false;
            this.SelectTeamGroupBox.Text = "Select team(s):";
            // 
            // FutureMatchesLabel
            // 
            this.FutureMatchesLabel.AutoSize = true;
            this.FutureMatchesLabel.Location = new System.Drawing.Point(259, 113);
            this.FutureMatchesLabel.Name = "FutureMatchesLabel";
            this.FutureMatchesLabel.Size = new System.Drawing.Size(131, 17);
            this.FutureMatchesLabel.TabIndex = 5;
            this.FutureMatchesLabel.Text = "Future matches for:";
            // 
            // FutureMatchesListbox
            // 
            this.FutureMatchesListbox.FormattingEnabled = true;
            this.FutureMatchesListbox.ItemHeight = 16;
            this.FutureMatchesListbox.Location = new System.Drawing.Point(256, 133);
            this.FutureMatchesListbox.Name = "FutureMatchesListbox";
            this.FutureMatchesListbox.Size = new System.Drawing.Size(206, 116);
            this.FutureMatchesListbox.TabIndex = 6;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(152, 265);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 7;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // DiscardButton
            // 
            this.DiscardButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DiscardButton.Location = new System.Drawing.Point(262, 265);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(75, 23);
            this.DiscardButton.TabIndex = 8;
            this.DiscardButton.Text = "Cancel";
            this.DiscardButton.UseVisualStyleBackColor = true;
            // 
            // FutureMatchPrediction
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DiscardButton;
            this.ClientSize = new System.Drawing.Size(492, 309);
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.FutureMatchesListbox);
            this.Controls.Add(this.FutureMatchesLabel);
            this.Controls.Add(this.SelectTeamGroupBox);
            this.Controls.Add(this.ExplanationLabel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(510, 356);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(510, 356);
            this.Name = "FutureMatchPrediction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Future Match Prediction";
            this.SelectTeamGroupBox.ResumeLayout(false);
            this.SelectTeamGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.RadioButton FirstTeamRadioButton;
        private System.Windows.Forms.RadioButton SecondTeamRadioButton;
        private System.Windows.Forms.RadioButton ThirdTeamRadioButton;
        private System.Windows.Forms.GroupBox SelectTeamGroupBox;
        private System.Windows.Forms.Label FutureMatchesLabel;
        private System.Windows.Forms.ListBox FutureMatchesListbox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button DiscardButton;
    }
}