using System;
using System.Windows.Forms;

namespace HTMatchPredictor
{
    /// <summary>
    /// Clasa care se ocupa cu introducerea PIN
    /// </summary>
    public partial class InsertPIN : Form
    {
        /// <summary>
        /// Constructorul clasei
        /// </summary>
        public InsertPIN()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Close();
        }
    }
}
