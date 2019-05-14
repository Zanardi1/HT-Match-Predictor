using System;
using System.Drawing;
using System.Windows.Forms;

namespace HTMatchPredictor
{
    public partial class AddMultipleMatchesByMatchIDRange : Form
    {
        private int lowlimit, highlimit;
        public int LowLimit
        {
            get
            {
                return lowlimit;
            }

            set
            {
                if (value <= 0)
                    lowlimit = 1;
                else
                    lowlimit = value;
            }
        }
        public int HighLimit
        {
            get
            {
                return highlimit;
            }

            set
            {
                highlimit = value;
            }
        }
        public AddMultipleMatchesByMatchIDRange()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Functia verifica datele introduse in fereastra daca respecta formatul cerut (sa contina numai >0 iar limita inferioara sa fie mai mica decat cea superioara).
        /// </summary>
        /// <returns>0 - daca totul e in regula;
        /// 1 - daca in primul text box nu e introdus nimic;
        /// 2 - daca in al doilea text box nu e introdus nimic;
        /// 3 - daca in primul text box sunt introduse si litere;
        /// 4 - daca in al doilea text box sunt introduse si litere.
        /// 5 - daca limita inferioara nu e mai mica decat cea superioara
        /// 6 - daca limita inferioara este <=0
        /// 7 - daca limita superioara e <=0</returns>
        private int TestForDataValidity()
        {
            int Result = 0;
            int TempLowLimit, TempHighLimit;
            if (string.IsNullOrEmpty(LowerBoundIDTextBox.Text))
            {
                Result = 1;
            }
            if (string.IsNullOrEmpty(HigherBoundIDTextBox.Text))
            {
                Result = 2;
            }
            if (!int.TryParse(LowerBoundIDTextBox.Text, out TempLowLimit))
            {
                LowLimit = TempLowLimit;
                Result = 3;
            }
            if (!int.TryParse(HigherBoundIDTextBox.Text, out TempHighLimit))
            {
                HighLimit = TempHighLimit;
                Result = 4;
            }
            if (LowLimit >= HighLimit)
            {
                Result = 5;
            }
            if (LowLimit < 0)
            {
                Result = 6;
            }

            if (HighLimit < 0)
            {
                Result = 7;
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
                case 5:
                    {
                        LowerBoundIDTextBox.BackColor = SystemColors.MenuHighlight;
                        HigherBoundIDTextBox.BackColor = SystemColors.MenuHighlight;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The lower bound match ID must be lower than the higher bound match ID.", "Error saving your data", Buttons, Icon);
                        break;
                    }
                case 6:
                    {
                        LowerBoundIDTextBox.BackColor = SystemColors.MenuHighlight;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The lower bound match ID must be higher than 0.", "Error saving your data", Buttons, Icon);
                        break;
                    }
                case 7:
                    {
                        HigherBoundIDTextBox.BackColor = SystemColors.MenuHighlight;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The higher bound match ID must be higher than 0.", "Error saving your data", Buttons, Icon);
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
