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
            MessageBox.Show(LastMatchDateDateTime.Value.ToLongDateString());
            Close();
        }

        /// <summary>
        /// Stabileste data maxima a controlului la data de azi + un sezon (deoarece niciun meci nu e creat la o distanta de mai mult de un sezon)
        /// </summary>
        private void SetMaxDate()
        {
            TimeSpan Span = new TimeSpan(14 * 7, 0, 0, 0);
            DateTime Date = DateTime.Now;
            DateTime Combined = Date.Add(Span);
            LastMatchDateDateTime.MaxDate = Combined;
        }

        private void Startup(object sender, EventArgs e)
        {
            SetMaxDate();
        }
    }
}
