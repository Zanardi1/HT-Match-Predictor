using System;
using System.Drawing;
using System.Windows.Forms;

namespace HTMatchPredictor
{
    public partial class AddMultipleMatchesByTeam : Form
    {
        public AddMultipleMatchesByTeam()
        {

            InitializeComponent();
        }

        private int teamid, seasonnumber;
        public int TeamID
        {
            get
            {
                return teamid;
            }
            set
            {
                if (value <= 0)
                {
                    teamid = 1;
                }
                else
                {
                    teamid = value;
                }
            }
        }
        public int SeasonNumber
        {
            get
            {
                return seasonnumber;
            }
            set
            {
                if (value <= 0)
                {
                    seasonnumber = 1;
                }
                else
                {
                    seasonnumber = value;
                }
            }
        }

        /// <summary>
        /// Testeaza daca datele de intrare sunt valide (adica sa contina numai cifre)
        /// </summary>
        /// <returns>0, daca totul e in regula;
        /// 1 in cazul in care casuta cu numarul de identificare al echipei e goala;
        /// 2 in cazul in care casuta cu numarul sezonului e goala;
        /// 3 in cazul in care casuta cu numarul de identificare al echipei contine si litere;
        /// 4 in cazul in care casuta cu numarul sezonului contine si litere</returns>
        private int TestDataValidity()
        {
            int Result = 0; //retine rezultatul functiei
            int TempTeamID, TempSeasonNumber;
            SeniorTeamIDTextBox.Text = SeniorTeamIDTextBox.Text.Trim();
            SeasonTextBox.Text = SeasonTextBox.Text.Trim();
            if (string.IsNullOrEmpty(SeniorTeamIDTextBox.Text))
            {
                Result = 1;
            }

            if (string.IsNullOrEmpty(SeasonTextBox.Text))
            {
                Result = 2;
            }

            if (int.TryParse(SeniorTeamIDTextBox.Text, out TempTeamID))
            {
                TeamID = TempTeamID;
            }
            else
            {
                Result = 3;
            }

            if (int.TryParse(SeasonTextBox.Text, out TempSeasonNumber))
            {
                SeasonNumber = TempSeasonNumber;
            }
            else
            {
                Result = 4;
            }

            return Result;
        }

        /// <summary>
        /// Inchide fereastra, deoarece orice modificare este ignorata
        /// </summary>
        /// <param name="sender">Event handler</param>
        /// <param name="e">Event handler</param>
        private void IgnoreChanges(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Testeaza validitatea datelor introduse si, daca acestea sunt valide, inchide fereastra
        /// </summary>
        /// <param name="sender">Event handler</param>
        /// <param name="e">Event handler</param>
        private void SaveChanges(object sender, EventArgs e)
        {
            int TestResult = TestDataValidity();
            switch (TestResult)
            {
                case 0:
                    {
                        SeniorTeamIDTextBox.BackColor = SystemColors.Window;
                        SeasonTextBox.BackColor = SystemColors.Window;
                        DialogResult = DialogResult.OK;
                        break;
                    }
                case 1:
                    {
                        SeniorTeamIDTextBox.BackColor = SystemColors.MenuHighlight;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The field of the team ID is empty. Please insert the team ID and try again", "Error saving your data", Buttons, Icon);
                        break;
                    }
                case 2:
                    {
                        SeasonTextBox.BackColor = SystemColors.MenuHighlight;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The season field is empty. Please insert the team ID and try again", "Error saving your data", Buttons, Icon);
                        break;
                    }
                case 3:
                    {
                        SeniorTeamIDTextBox.BackColor = SystemColors.MenuHighlight;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The field of the team ID must have only numbers. Please insert the team ID and try again", "Error saving your data", Buttons, Icon);
                        break;
                    }
                case 4:
                    {
                        SeasonTextBox.BackColor = SystemColors.MenuHighlight;
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                        MessageBox.Show("The season field must have only numbers. Please insert the team ID and try again", "Error saving your data", Buttons, Icon);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
