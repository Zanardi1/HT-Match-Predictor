using System.Windows.Forms;

namespace HTMatchPredictor
{
    public partial class ProgressWindow : Form
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        private void CancelImport(object sender, System.EventArgs e)
        {
            Form1.CancelDatabaseAdding = true;
        }
    }
}
