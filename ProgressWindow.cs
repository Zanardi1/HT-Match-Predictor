using System.Windows.Forms;

namespace HTMatchPredictor
{
    /// <summary>
    /// Clasa ce se ocupa cu afisarea ferestrei de progres
    /// </summary>
    public partial class ProgressWindow : Form
    {
        /// <summary>
        /// Constructorul clasei
        /// </summary>
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
