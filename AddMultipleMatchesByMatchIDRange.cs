using System;
using System.Drawing;
using System.Globalization;
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
            int TempLowLimit, TempHighLimit;

            if (int.TryParse(LowerBoundIDTextBox.Text, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out TempLowLimit))
            {
                LowLimit = TempLowLimit;
                if (LowLimit < 0)
                {
                    return 6;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(LowerBoundIDTextBox.Text))
                {
                    return 1;
                }
                else
                {
                    return 3;
                }
            }
            if (int.TryParse(HigherBoundIDTextBox.Text, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out TempHighLimit))
            {
                HighLimit = TempHighLimit;
                if (HighLimit < 0)
                {
                    return 7;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(HigherBoundIDTextBox.Text))
                {
                    return 2;
                }
                else
                {
                    return 4;
                }
            }
            if (LowLimit >= HighLimit)
            {
                return 5;
            }
            return 0;
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
                        HigherBoundIDTextBox.BackColor = SystemColors.Window;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The field for the lower end of the matches ID scope is empty. Please insert a match ID.", "Error saving your data", Buttons, Icon);
                        break;
                    }
                case 2:
                    {
                        LowerBoundIDTextBox.BackColor = SystemColors.Window;
                        HigherBoundIDTextBox.BackColor = SystemColors.MenuHighlight;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The field for the higher end of the matches ID scope is empty. Please insert a match ID.", "Error saving your data", Buttons, Icon);
                        break;
                    }
                case 3:
                    {
                        LowerBoundIDTextBox.BackColor = SystemColors.MenuHighlight;
                        HigherBoundIDTextBox.BackColor = SystemColors.Window;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The field for the lower end of the matches must contain only numbers.", "Error saving your data", Buttons, Icon);
                        break;
                    }
                case 4:
                    {
                        LowerBoundIDTextBox.BackColor = SystemColors.Window;
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
                        HigherBoundIDTextBox.BackColor = SystemColors.Window;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The lower bound match ID must be higher than 0.", "Error saving your data", Buttons, Icon);
                        break;
                    }
                case 7:
                    {
                        LowerBoundIDTextBox.BackColor = SystemColors.Window;
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
