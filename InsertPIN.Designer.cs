namespace HT_Match_Predictor
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
            this.InsertPINTextBox = new System.Windows.Forms.TextBox();
            this.InsertPINLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InsertPINTextBox
            // 
            this.InsertPINTextBox.Location = new System.Drawing.Point(225, 179);
            this.InsertPINTextBox.Name = "InsertPINTextBox";
            this.InsertPINTextBox.Size = new System.Drawing.Size(196, 22);
            this.InsertPINTextBox.TabIndex = 0;
            // 
            // InsertPINLabel
            // 
            this.InsertPINLabel.AutoSize = true;
            this.InsertPINLabel.Location = new System.Drawing.Point(222, 150);
            this.InsertPINLabel.Name = "InsertPINLabel";
            this.InsertPINLabel.Size = new System.Drawing.Size(177, 17);
            this.InsertPINLabel.TabIndex = 1;
            this.InsertPINLabel.Text = "Please insert the PIN here:";
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(457, 179);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.CloseWindow);
            // 
            // InsertPIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.InsertPINLabel);
            this.Controls.Add(this.InsertPINTextBox);
            this.Name = "InsertPIN";
            this.Text = "InsertPIN";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label InsertPINLabel;
        public System.Windows.Forms.TextBox InsertPINTextBox;
        private System.Windows.Forms.Button OKButton;
    }
}