using System.Windows.Forms;

namespace HTMatchPredictor
{
    public partial class SkillSelectionWindow : Form
    {
        //Converteste abilitatea si sub-abilitatea intr-o valoare numerica stabilita de Hattrick:
        //1 - dezastruos (foarte scazut);
        //2 - dezastruos (scazut);
        //---
        //80 - divin (foarte ridicat)
        private int ConvertSkillToNumber(int MainSkill, int SubSkill)
        {
            return (MainSkill * 4) + SubSkill + 1;
        }

        public void LoadExistingSkill(int SkillNumber)
        {
            if ((SkillNumber < 1) || (SkillNumber > 80))
            {
                SkillListBox.SetSelected(0, true);
                SubskillListBox.SetSelected(0, true);
            }
            else
            {
                SkillListBox.SetSelected((SkillNumber - 1) / 4, true);
                SubskillListBox.SetSelected((SkillNumber - SkillListBox.SelectedIndex * 4) - 1, true);
            }
        }

        public SkillSelectionWindow()
        {
            InitializeComponent();
        }

        private void SaveChoice(object sender, System.EventArgs e)
        {
            string RatingDenomination = SkillListBox.Text + " (" + SubskillListBox.Text + ")"; //Reprezinta modul de construire a evaluarii care va fi afisata in program
            switch (Tag)
            {
                case "1":
                    {
                        ((Form1)this.Owner).HomeMidfieldRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "2":
                    {
                        ((Form1)this.Owner).HomeRightDefenceRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "3":
                    {
                        ((Form1)this.Owner).HomeCentralDefenceRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "4":
                    {
                        ((Form1)this.Owner).HomeLeftDefenceRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "5":
                    {
                        ((Form1)this.Owner).HomeRightAttackRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "6":
                    {
                        ((Form1)this.Owner).HomeCentralAttackRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "7":
                    {
                        ((Form1)this.Owner).HomeLeftAttackRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "8":
                    {
                        ((Form1)this.Owner).AwayMidfieldRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "9":
                    {
                        ((Form1)this.Owner).AwayRightDefenceRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "10":
                    {
                        ((Form1)this.Owner).AwayCentralDefenceRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "11":
                    {
                        ((Form1)this.Owner).AwayLeftDefenceRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "12":
                    {
                        ((Form1)this.Owner).AwayRightAttackRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "13":
                    {
                        ((Form1)this.Owner).AwayCentralAttackRatingLabel.Text = RatingDenomination;
                        break;
                    }
                case "14":
                    {
                        ((Form1)this.Owner).AwayLeftAttackRatingLabel.Text = RatingDenomination;
                        break;
                    }
                default:
                    break;
            }
            Form1.RatingReturned = ConvertSkillToNumber(SkillListBox.SelectedIndex, SubskillListBox.SelectedIndex);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
