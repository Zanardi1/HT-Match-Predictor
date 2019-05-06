using System;
using System.Windows.Forms;

namespace HT_Match_Predictor
{
    public partial class AddMultipleMatchesByTeam : Form
    {
        public AddMultipleMatchesByTeam()
        {

            InitializeComponent();
        }

        private void IgnoreChanges(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveChanges(object sender, EventArgs e)
        {
            Close();
        }
    }
}
