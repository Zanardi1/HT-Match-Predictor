using System.Windows.Forms;

namespace HTMatchPredictor
{
    public partial class ProgressWindow : Form
    {
        public bool StopAddingMatches = false;
        public ProgressWindow()
        {
            InitializeComponent();
        }

        private void CancelImport(object sender, System.EventArgs e)
        {
            StopAddingMatches = true;
        }
    }
}
