using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
