﻿namespace HTMatchPredictor
{
    partial class InsertPIN
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsertPIN));
            this.InsertPINTextBox = new System.Windows.Forms.TextBox();
            this.InsertPINLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.ExplanationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InsertPINTextBox
            // 
            this.InsertPINTextBox.Location = new System.Drawing.Point(16, 99);
            this.InsertPINTextBox.Name = "InsertPINTextBox";
            this.InsertPINTextBox.Size = new System.Drawing.Size(196, 22);
            this.InsertPINTextBox.TabIndex = 0;
            // 
            // InsertPINLabel
            // 
            this.InsertPINLabel.AutoSize = true;
            this.InsertPINLabel.Location = new System.Drawing.Point(13, 75);
            this.InsertPINLabel.Name = "InsertPINLabel";
            this.InsertPINLabel.Size = new System.Drawing.Size(177, 17);
            this.InsertPINLabel.TabIndex = 1;
            this.InsertPINLabel.Text = "Please insert the PIN here:";
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(248, 99);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.CloseWindow);
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.Location = new System.Drawing.Point(10, 12);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(434, 53);
            this.ExplanationLabel.TabIndex = 3;
            this.ExplanationLabel.Text = "It appears it\'s the first time you login from this application, or the previous c" +
    "onnection expired. You will have to enter the PIN which was provided to you by H" +
    "attrick.";
            // 
            // InsertPIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 140);
            this.ControlBox = false;
            this.Controls.Add(this.ExplanationLabel);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.InsertPINLabel);
            this.Controls.Add(this.InsertPINTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(461, 187);
            this.MinimumSize = new System.Drawing.Size(461, 187);
            this.Name = "InsertPIN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Insert PIN";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label InsertPINLabel;
        public System.Windows.Forms.TextBox InsertPINTextBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label ExplanationLabel;
    }
}