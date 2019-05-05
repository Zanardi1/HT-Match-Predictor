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

        /// <summary>
        /// Stabileste data maxima a controlului la data de azi + un sezon (deoarece niciun meci nu e creat la o distanta de mai mult de un sezon)
        /// </summary>
        private void SetMaxDate()
        {
            TimeSpan MaxPeriod = new TimeSpan(224, 0, 0, 0);
            FirstMatchDateDateTime.MinDate = DateTime.Now.Subtract(MaxPeriod);
            FirstMatchDateDateTime.MaxDate = DateTime.Now;
            LastMatchDateTime.MinDate = DateTime.Now.Subtract(MaxPeriod);
            LastMatchDateTime.MaxDate = DateTime.Now;
        }

        /// <summary>
        /// Actiuni efectuate la afisarea ferestrei
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void Startup(object sender, EventArgs e)
        {
            SetMaxDate();
        }
    }
}
