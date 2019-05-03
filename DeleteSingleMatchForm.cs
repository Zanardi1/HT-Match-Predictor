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
    public partial class DeleteSingleMatchForm : Form
    {
        public DeleteSingleMatchForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Procedura inchide fereastra si stocheaza numarul de identificare al meciului ce va fi sers in variabila MatchIDToDelete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveChanges(object sender, EventArgs e)
        {
            int.TryParse(MatchIDTextBox.Text, out int MatchID);
            ((Form1)this.Owner).MatchIDToDelete = MatchID;
            Close();
        }

        /// <summary>
        /// Procedura inchide fereastra, fara a salva vreo modificare. Seteaza valoarea numarului de identificare al meciului ce trebuie sters la -1, cu semnificatia "Niciun meci de sters"
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void IgnoreChanges(object sender, EventArgs e)
        {
            ((Form1)this.Owner).MatchIDToDelete = -1;
            Close();
        }
    }
}
