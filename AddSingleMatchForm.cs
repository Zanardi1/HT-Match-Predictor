using System;
using System.Windows.Forms;

namespace HT_Match_Predictor
{
    public partial class AddSingleMatchForm : Form
    {
        public AddSingleMatchForm()
        {
            InitializeComponent();
        }

        private void IgnoreChanges(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveChanges(object sender, EventArgs e)
        {
            int.TryParse(MatchIDTextBox.Text, out int MatchID);
            ((Form1)this.Owner).MatchIDToAdd = MatchID;
            Close();
        }
    }
}
