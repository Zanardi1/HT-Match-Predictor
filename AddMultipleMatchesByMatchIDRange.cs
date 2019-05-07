using System;
using System.Windows.Forms;

namespace HT_Match_Predictor
{
    public partial class AddMultipleMatchesByMatchIDRange : Form
    {
        public AddMultipleMatchesByMatchIDRange()
        {
            InitializeComponent();
        }

        private void DiscardChanges(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveChanges(object sender, EventArgs e)
        {
            Close();
        }
    }
}
