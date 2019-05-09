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
    public partial class FutureMatchPrediction : Form
    {
        public FutureMatchPrediction()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Procedura se ocupa de inchiderea ferestrei, fara a salva modificarile.
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
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
