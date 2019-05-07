using System;
using System.Drawing;
using System.Windows.Forms;

namespace HT_Match_Predictor
{
    public partial class AddMultipleMatchesByMatchIDRange : Form
    {
        public AddMultipleMatchesByMatchIDRange()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Functia verifica datele introduse in fereastra daca respecta formatul cerut (sa contina numai numere).
        /// </summary>
        /// <returns>0 - daca totul e in regula;
        /// 1 - daca in primul text box nu e introdus nimic;
        /// 2 - daca in al doilea text box nu e introdus nimic;
        /// 3 - daca in primul text box sunt introduse si litere;
        /// 4 - daca in al doilea text box sunt introduse si litere.</returns>
        private int TestForDataValidity()
        {
            int Result = 0;
            LowerBoundIDTextBox.Text.Trim();
            HigherBoundIDTextBox.Text.Trim();
            if (LowerBoundIDTextBox.Text == string.Empty)
            {
                Result = 1;
            }
            if (HigherBoundIDTextBox.Text == string.Empty)
            {
                Result = 2;
            }
            if (!int.TryParse(LowerBoundIDTextBox.Text, out int temp))
            {
                Result = 3;
            }
            if (!int.TryParse(HigherBoundIDTextBox.Text, out temp))
            {
                Result = 4;
            }
            return Result;
        }

        private void DiscardChanges(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SaveChanges(object sender, EventArgs e)
        {
            int TestResult = TestForDataValidity();
            switch (TestResult)
            {
                case 0:
                    {
                        LowerBoundIDTextBox.BackColor = SystemColors.Window;
                        HigherBoundIDTextBox.BackColor = SystemColors.Window;
                        DialogResult = DialogResult.OK;
                        Close();
                        break;
                    }
                case 1:
                    {
                        LowerBoundIDTextBox.BackColor = SystemColors.MenuHighlight;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The field for the lower end of the matches ID scope is empty. Please insert a match ID.", "Error saving your data", Buttons, Icon);
                        break;
                    }
                case 2:
                    {
                        HigherBoundIDTextBox.BackColor = SystemColors.MenuHighlight;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The field for the higher end of the matches ID scope is empty. Please insert a match ID.", "Error saving your data", Buttons, Icon);
                        break;
                    }
                case 3:
                    {
                        LowerBoundIDTextBox.BackColor = SystemColors.MenuHighlight;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The field for the lower end of the matches must contain only numbers.", "Error saving your data", Buttons, Icon);
                        break;
                    }
                case 4:
                    {
                        HigherBoundIDTextBox.BackColor = SystemColors.MenuHighlight;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The field for the higher end of the matches must contain only numbers.", "Error saving your data", Buttons, Icon);
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
