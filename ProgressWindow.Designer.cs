﻿namespace HTMatchPredictor
{
    partial class ProgressWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressWindow));
            this.TheProgressBar = new System.Windows.Forms.ProgressBar();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TheProgressBar
            // 
            this.TheProgressBar.Location = new System.Drawing.Point(40, 63);
            this.TheProgressBar.Name = "TheProgressBar";
            this.TheProgressBar.Size = new System.Drawing.Size(270, 23);
            this.TheProgressBar.TabIndex = 0;
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.Location = new System.Drawing.Point(37, 29);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(77, 17);
            this.ProgressLabel.TabIndex = 1;
            this.ProgressLabel.Text = "Progress...";
            // 
            // DiscardButton
            // 
            this.DiscardButton.Location = new System.Drawing.Point(135, 123);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(75, 23);
            this.DiscardButton.TabIndex = 2;
            this.DiscardButton.Text = "Cancel";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.CancelImport);
            // 
            // ProgressWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 186);
            this.ControlBox = false;
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.TheProgressBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(378, 233);
            this.MinimumSize = new System.Drawing.Size(378, 233);
            this.Name = "ProgressWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Matches Adding Progress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ProgressBar TheProgressBar;
        public System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.Button DiscardButton;
    }
}