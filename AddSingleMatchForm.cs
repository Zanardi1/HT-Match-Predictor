﻿using System;
using System.Windows.Forms;

namespace HTMatchPredictor
{
    /// <summary>
    /// Clasa ce se ocupa de adaugarea unui singur meci.
    /// </summary>
    public partial class AddSingleMatchForm : Form
    {
        /// <summary>
        /// Constructorul clasei.
        /// </summary>
        public AddSingleMatchForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Procedura inchide fereastra, fara a salva vreo modificare. Seteaza valoarea numarului de identificare al meciului de adaugare la -1, cu semnificatia "Niciun meci de adaugat"
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void IgnoreChanges(object sender, EventArgs e)
        {
            Form1.MatchIDToAdd = -1;
            Close();
        }

        /// <summary>
        /// Procedura inchide fereastra si stocheaza numarul de identificare al meciului ce va fi adaugat in variabila MatchIDToAdd.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveChanges(object sender, EventArgs e)
        {
            if (int.TryParse(MatchIDTextBox.Text, out int MatchID))
            {
                Form1.MatchIDToAdd = MatchID;
                Close();
            }
        }
    }
}
