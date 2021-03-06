﻿using Microsoft.Win32;
using OAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//todo sa citesc dintr-un fisier denumirile evaluarilor (lucru util pentru momentul in care voi introduce si alte limbi pentru interfata programului
//todo de creat o clasa care se ocupa de scrierea diferitelor erori intr-un fisier text

namespace HTMatchPredictor
{
    /// <summary>
    /// Clasa ce se ocupa de ferastra principala a programului
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Instanta de clasa ce se ocupa de conexiunea cu serverele Hattrick
        /// </summary>
        private readonly Manager o = new Manager();

        /// <summary>
        /// Obiect ce se ocupa de operatiile cu BD a programului
        /// </summary>
        private readonly DatabaseOperations Operations = new DatabaseOperations();

        /// <summary>
        /// Retin folderul in care se afla aplicatia
        /// </summary>
        static readonly string CurrentFolder = Path.GetDirectoryName(Application.ExecutablePath);

        /// <summary>
        /// Retin folderul unde vor fi descarcate fisierele XML
        /// </summary>
        readonly static public string XMLFolder = CurrentFolder + "\\XML";

        /// <summary>
        /// retine reprezentarea numerica a evaluarii selectate de catre utilizator in fereastra de selectare a abilitatilor. E un numar intre 1 si 80
        /// </summary>
        private static int ratingreturned = 0;

        /// <summary>
        /// Retine evaluarea citita de undeva sau care va fi scrisa undeva. E intre 1 si 80.
        /// </summary>
        public static int RatingReturned
        {
            get
            {
                return ratingreturned;
            }
            set
            {
                if (value < 1)
                {
                    ratingreturned = 1;
                }

                if (value > 80)
                {
                    ratingreturned = 80;
                }
                else
                {
                    ratingreturned = value;
                }
            }
        }

        /// <summary>
        /// MatchRatings este o lista de 14 numere intregi, ce retine evaluarile celor doua echipe dintr-un meci. Semnificatia numerelor de ordine din lista este urmatoarea:
        /// 0. Evaluarea la mijloc (echipa de acasa);
        /// 1. Evaluarea apararii pe dreapta (echipa de acasa);
        /// 2. Evaluarea apararii pe centru (echipa de acasa);
        /// 3. Evaluarea apararii pe stanga (echipa de acasa);
        /// 4. Evaluarea atacului pe dreapta (echipa de acasa);
        /// 5. Evaluarea atacului pe centru (echipa de acasa);
        /// 6. Evaluarea atacului pe stanga (echipa de acasa);
        /// 7. Evaluarea la mijloc (echipa din deplasare);
        /// 8. Evaluarea apararii pe dreapta(echipa de acasa);
        /// 9. Evaluarea apararii pe centru (echipa din deplasare));
        /// 10. Evaluarea apararii pe stanga (echipa din deplasare));
        /// 11. Evaluarea atacului pe dreapta (echipa din deplasare));
        /// 12. Evaluarea atacului pe centru (echipa din deplasare));
        /// 13. Evaluarea atacului pe stanga (echipa din deplasare));
        /// </summary>
        public static List<int> MatchRatings = new List<int>(14);

        /// <summary>
        /// Obiect ce se ocupa de crearea sirului care va fi transmis mai departe pentru descarcarea fisierului XML
        /// </summary>
        private readonly DownloadStringCreation DownloadString = new DownloadStringCreation();

        /// <summary>
        /// Obiect ce se ocupa de prelucrarea fisierelor XML descarcate
        /// </summary>
        private readonly ParseXMLFiles Parser = new ParseXMLFiles();

        /// <summary>
        /// Retine numarul de identificare al meciului care va fi adaugat in baza de date, ca urmare a optiunii de adaugare a unui singur meci.
        /// </summary>
        private static int matchidtoadd = 0;

        /// <summary>
        /// Numarul de identificare al meciului care va fi adaugat
        /// </summary>
        public static int MatchIDToAdd
        {
            get
            {
                return matchidtoadd;
            }
            set
            {
                matchidtoadd = value;
            }
        }

        /// <summary>
        /// Retine daca a fost apasat butonul de anulare din fereastra de progres. E true, daca a fost apasast.
        /// </summary>
        private static bool canceldatabaseadding;

        /// <summary>
        /// Retine daca s-a apasat butonul de adaugare a meciurilor in BD (atunci e true) sau nu (false)
        /// </summary>
        public static bool CancelDatabaseAdding
        {
            get
            {
                return canceldatabaseadding;
            }
            set
            {
                canceldatabaseadding = value;
            }
        }

        /// <summary>
        /// Retine daca era o conexiune la net la pornirea programului (true) sau nu (false)
        /// </summary>
        private bool InternetConnectionExistentAtStartup = true;

        /// <summary>
        /// Aduce cele 14 evaluari ale unui meci la 0
        /// </summary>
        private static void InitializeMatchRatingList()
        {
            for (int i = 0; i < 14; i++)
            {
                MatchRatings.Add(0);
            }
        }

        /// <summary>
        /// Procedura aduce la 0 toate cele 14 elemente ale listei
        /// </summary>
        private void ResetMatchRatingList()
        {
            for (int i = 0; i < MatchRatings.Count; i++)
            {
                MatchRatings[i] = 0;
            }
        }

        /// <summary>
        /// Procedura ce se ocupa de conectarea la serverele Hattrick
        /// </summary>
        private void LoginToHattrickServers()
        {
            Uri Compendium = new Uri(DownloadStringCreation.CreateManagerCompendiumString());
            InitializeAuthenticationObject();
            if (string.IsNullOrEmpty(o["token"]))
            {
                GetRequestToken();
                GetAccessToken();
            }
            SaveResponseToFile(Compendium, XMLFolder + "\\User.xml");
        }

        /// <summary>
        /// Functia intoarce valoarea lui token, citita din registri.
        /// </summary>
        /// <returns>Sirul de caractere ce reprezinta jetonul (token)</returns>
        private static string ReadTokenFromRegistry()
        {
            string Value = string.Empty;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("HTMPTK"))
            {
                if (key != null)
                {
                    Object temp = key.GetValue("Token");
                    if (temp != null)
                    {
                        Value = temp.ToString();
                    }
                }
            }
            return Value;
        }

        /// <summary>
        /// Functia intoarce valoarea lui token_secret, citita din registri
        /// </summary>
        /// <returns>Sirul de caractere ce reprezinta jetonul secret (token_secret)</returns>
        private static string ReadTokenSecretFromRegistry()
        {
            string Value = string.Empty;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("HTMPTK"))
            {
                if (key != null)
                {
                    Object temp = key.GetValue("Secret Token");
                    if (temp != null)
                    {
                        Value = temp.ToString();
                    }
                }
            }
            return Value;
        }

        /// <summary>
        /// Procedura stocheaza cele doua jetoane, token si token_secret in registri
        /// </summary>
        private void StoreTokensToRegistry()
        {
            RegistryKey Key;
            Key = Registry.CurrentUser.OpenSubKey("HTMPTK", RegistryKeyPermissionCheck.ReadWriteSubTree); //HTMPTK = HatTrick Match Predictor Token Keys
            if (Key == null)
            {
                Key = Registry.CurrentUser.CreateSubKey("HTMPTK");
            }
            Key.SetValue("Secret Token", o["token_secret"].ToString(CultureInfo.InvariantCulture));
            Key.SetValue("Token", o["token"].ToString(CultureInfo.InvariantCulture));
            Key.Close();
        }

        /// <summary>
        /// Functia verifica daca exista conectivitate la net. Metoda luata de la https://stackoverflow.com/questions/2031824/what-is-the-best-way-to-check-for-internet-connectivity-using-net
        /// </summary>
        /// <returns>true, daca exista; false altfel</returns>
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch (WebException W)
            {
                return (!(W.Status == WebExceptionStatus.NameResolutionFailure));
            }
        }

        /// <summary>
        /// Incarca cele 4 jetoane ale obiectului care se ocupa cu conectarea la site.
        /// </summary>
        private void InitializeAuthenticationObject()
        {
            o["consumer_key"] = "2BkDvCeUZL1nCIVOn5KhUb";
            o["consumer_secret"] = "PvSRGYlTxCwUKuw9BH9CIWP1AqutO9MB2JRDGHsVlGC";
            o["token"] = ReadTokenFromRegistry();
            o["token_secret"] = ReadTokenSecretFromRegistry();
        }

        /// <summary>
        /// Obtine jetonul de cerere
        /// </summary>
        private void GetRequestToken()
        {
            o.AcquireRequestToken("https://chpp.hattrick.org/oauth/request_token.ashx", "GET");
            var url = "https://chpp.hattrick.org/oauth/authorize.aspx?oauth_token=" + o["token"];
            System.Diagnostics.Process.Start(url);
        }

        /// <summary>
        /// Obtine jetonul de acces
        /// </summary>
        private void GetAccessToken()
        {
            InsertPIN I = new InsertPIN();
            I.ShowDialog(this);
            string pin = I.InsertPINTextBox.Text;
            o.AcquireAccessToken("https://chpp.hattrick.org/oauth/access_token.ashx", "GET", pin);
            StoreTokensToRegistry();
        }

        /// <summary>
        /// Lanseaza o cerere catre server si salveaza raspunsul intr-un fisier
        /// </summary>
        /// <param name="URLAddress">URL-ul catre care va fi trimisa cererea</param>
        /// <returns>Raspunsul primit</returns>
        private string GetFileContent(string URLAddress)
        {
            Uri search = new Uri(URLAddress);
            string authzHeader = o.GenerateAuthzHeader(search.ToString(), "GET");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(search); //Creeaza requestul
            request.Method = "GET";
            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = true;
            request.Headers.Add("Authorization", authzHeader);
            request.ServicePoint.Expect100Continue = false;
            request.ContentType = "x-www-form-urlencoded";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //Primeste raspunsul
                Stream receive = response.GetResponseStream(); //Il incarca intr-un flux
                StreamReader s = new StreamReader(receive, Encoding.UTF8);
                string temp = s.ReadToEnd(); //Salveaza continutul raspunsului intr-o variabila
                return temp;
            }
            catch (WebException W)
            {
                if (String.Equals(W.Message, "The request was aborted: The operation has timed out.", StringComparison.CurrentCulture))
                {
                    CheckForInternetConnection();
                    return string.Empty;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Creeaza fisierul XML cu datele necesare
        /// </summary>
        /// <param name="SourceURLAddress">URL-ul de unde descarca datele</param>
        /// <param name="DestinationFileName">Numele fisierului (inclusiv calea) care va stoca datele descarcate</param>
        public void SaveResponseToFile(Uri SourceURLAddress, string DestinationFileName)
        {
            try
            {
                File.WriteAllText(DestinationFileName, GetFileContent(SourceURLAddress.ToString()));
            }
            catch (IOException I) //todo bug din cand in cand mai primesc un mesaj de eroare cum ca fisierul Matches.xml e folosit de un alt proces.
            {
                File.WriteAllText(CurrentFolder + "\\Error.txt", DateTime.Now.ToString(CultureInfo.InvariantCulture) + "\r\n\r\n" + I.StackTrace + "\r\n\r\n" + I.Message);
            }
        }

        /// <summary>
        /// Verifica daca utilizatorul are echipe secundare. Daca da, le afiseaza
        /// </summary>
        /// <param name="Team">Numele echipei</param>
        /// <param name="btn">Butonul radio care va fi afisat</param>
        private static void CheckOtherTeam(string Team, RadioButton btn)
        {
            if (!String.IsNullOrEmpty(Team))
            {
                btn.Visible = true;
                btn.Text = Team;
            }
        }

        /// <summary>
        /// Functia activeaza sau dezactiveaza toate controalele din fereastra
        /// </summary>
        /// <param name="ControlEnabled">true, daca trebuie activate controalele, false altfel</param>
        private void AlterControlsEnable(bool ControlEnabled)
        {
            TeamListGroupBox.Enabled = ControlEnabled;
            FutureMatchesListBox.Enabled = ControlEnabled;
            HomeTeamGroupBox.Enabled = ControlEnabled;
            AwayTeamGroupBox.Enabled = ControlEnabled;
            PredictButton.Enabled = ControlEnabled;
            ResetButton.Enabled = ControlEnabled;
            optionsToolStripMenuItem.Enabled = ControlEnabled;
            aboutToolStripMenuItem.Enabled = ControlEnabled;
            deleteDatabaseToolStripMenuItem.Enabled = ControlEnabled;
            addSingleMatchToolStripMenuItem.Enabled = ControlEnabled;
            addMultipleMatchesToolStripMenuItem.Enabled = ControlEnabled;
            databaseOperationsToolStripMenuItem.Enabled = true;
            createDatabaseToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Creaza lista de echipe a utilizatorului
        /// </summary>
        /// <returns>Lista de echipe a utilizatorului</returns>
        private string CreateUserTeamList()
        {
            StringBuilder TeamList = new StringBuilder();
            TeamList.Append("Team list: \r\n");
            TeamList.Append(Parser.UserTeamNames[0]);
            TeamList.Append(" (");
            TeamList.Append(Parser.UserTeamIDs[0].ToString(CultureInfo.InvariantCulture));
            TeamList.Append(")\r\n");
            TeamList.Append(Parser.UserTeamNames[1]);
            TeamList.Append(" (");
            TeamList.Append(Parser.UserTeamIDs[1].ToString(CultureInfo.InvariantCulture));
            TeamList.Append(")\r\n");
            TeamList.Append(Parser.UserTeamNames[2]);
            TeamList.Append(" (");
            TeamList.Append(Parser.UserTeamIDs[2].ToString(CultureInfo.InvariantCulture));
            TeamList.Append(")\r\n");
            return TeamList.ToString();
        }

        /// <summary>
        /// Afiseaza detaliile despre utilizator
        /// </summary>
        private void DisplayUserDetails()
        {
            Parser.ParseUserFile();
            LoginNameLabel.Text = "User name: " + Parser.UserName + " (" + Parser.UserID + ")";
            UserCountryLabel.Text = "Country: " + Parser.UserCountry + " (" + Parser.UserCountryID + ")";
            SupporterTierLabel.Text = "Supporter: " + Parser.UserSupporterLevel;
            TeamListLabel.Text = CreateUserTeamList();
            FirstTeamRadioButton.Checked = true;
            FirstTeamRadioButton.Text = Parser.UserTeamNames[0];
            CheckOtherTeam(Parser.UserTeamNames[1], SecondTeamRadioButton);
            CheckOtherTeam(Parser.UserTeamNames[2], ThirdTeamRadioButton);
        }

        /// <summary>
        /// Functia verifica daca exista folderul "XML". Daca nu exista, il creaza.
        /// </summary>
        private static void CheckXMLFolderExistence()
        {
            if (!Directory.Exists(XMLFolder))
            {
                Directory.CreateDirectory(XMLFolder);
            }
        }

        /// <summary>
        /// Constructorul clasei
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            CheckXMLFolderExistence();
        }

        /// <summary>
        /// Afiseaza fereastra de selectare a abilitatilor
        /// </summary>
        /// <param name="sender">handler de eveniment</param>
        /// <param name="e">handler de eveniment</param>
        private void ShowSkillWindow(object sender, System.EventArgs e)
        {
            SkillSelectionWindow S = new SkillSelectionWindow
            {
                Tag = (sender as Button).Tag.ToString() //Tag este folosit pentru a determina ce buton a fost apasat pentru a afisa fereastra
            };
            S.LoadExistingSkill(MatchRatings[Convert.ToInt16(S.Tag, CultureInfo.InvariantCulture) - 1]);
            if (S.ShowDialog(this) == DialogResult.OK)
            {
                MatchRatings[Convert.ToInt16(S.Tag, CultureInfo.InvariantCulture) - 1] = RatingReturned; //atribuie evaluarea numerica primita sectorului corespunzator, in functie de eticheta butonului a carui apasare a deschis fereastra
            }
        }

        /// <summary>
        /// Afiseaza fereastra cu informatii despre program
        /// </summary>
        /// <param name="sender">Event handler</param>
        /// <param name="e">Event handler</param>
        private void ShowAboutWindow(object sender, System.EventArgs e)
        {
            AboutBox A = new AboutBox();
            A.ShowDialog();
        }

        /// <summary>
        /// Creaza baza de date cu meciuri
        /// </summary>
        /// <param name="sender">Event handler</param>
        /// <param name="e">Event handler</param>
        private void CreateMatchesDatabase(object sender, System.EventArgs e)
        {
            if (!DatabaseOperations.DatabaseExists())
            {
                HelpStatusLabel.Text = "Creating matches database...";
                Cursor = Cursors.WaitCursor;
                Operations.CreateDatabase();
                Cursor = Cursors.Default;
                HelpStatusLabel.Text = "Matches database created.";
            }
            else
            {
                HelpStatusLabel.Text = "Database is already created.";
            }
        }

        /// <summary>
        /// Sterge baza de date cu meciuri
        /// </summary>
        /// <param name="sender">Event handler</param>
        /// <param name="e">Event handler</param>
        private void DeleteMatchesDatabase(object sender, System.EventArgs e)
        {
            if (DatabaseOperations.DatabaseExists())
            {
                MessageBoxButtons Buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon Icon = MessageBoxIcon.Question;
                DialogResult Result = MessageBox.Show("Are you sure you want to delete the entire matches database? The whole content will be lost!", "Please confirm", Buttons, Icon);
                if (Result == DialogResult.Yes)
                {
                    HelpStatusLabel.Text = "Deleting matches database...";
                    Cursor = Cursors.WaitCursor;
                    Operations.DeleteDatabase();
                    HelpStatusLabel.Text = "Matches database deleted.";
                    Cursor = Cursors.Default;
                }
            }
            else
            {
                HelpStatusLabel.Text = "Database does not exist.";
            }
        }

        /// <summary>
        /// Functia reseteaza valorile etichetelor din grupul echipei gazda
        /// </summary>
        private void ResetHomeTab()
        {
            foreach (Control C in HomeTeamGroupBox.Controls)
            {
                if ((C.GetType() == typeof(Label)) && (C.TabIndex >= 14) && (C.TabIndex <= 20))
                {
                    C.ForeColor = SystemColors.ControlText;
                    C.Text = "(No rating selected!)";
                }
            }
        }

        /// <summary>
        /// Functia reseteaza valorile etichetelor din grupul echipei oaspete
        /// </summary>
        private void ResetAwayTab()
        {
            foreach (Control C in AwayTeamGroupBox.Controls)
            {
                if ((C.GetType() == typeof(Label)) && (C.TabIndex >= 21) && (C.TabIndex <= 27))
                {
                    C.ForeColor = SystemColors.ControlText;
                    C.Text = "(No rating selected!)";
                }
            }
        }

        /// <summary>
        /// Procedura aduce datele specifice simularii la valorile initiale
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void ResetData(object sender, EventArgs e)
        {
            ResetMatchRatingList();
            ResetHomeTab();
            ResetAwayTab();
        }

        /// <summary>
        /// Functia adauga un singur meci la baza de date
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void AddSingleMatchToDatabase(object sender, EventArgs e)
        {
            AddSingleMatchForm A = new AddSingleMatchForm();
            A.ShowDialog(this);
            Uri DownloadURL = new Uri(DownloadStringCreation.CreateMatchDetailsString(MatchIDToAdd));
            if (MatchIDToAdd != -1)
            {
                SaveResponseToFile(DownloadURL, XMLFolder + "\\MatchDetails.xml");
                if (Parser.ParseMatchDetailsFile(true) != -1) //Daca face parte din categoria meciurilor ce pot intra in BD
                {
                    MatchRatings = Parser.ReadMatchRatings;
                    DatabaseOperations.AddAMatch(MatchIDToAdd, MatchRatings);
                    MessageBoxButtons Buttons = MessageBoxButtons.OK;
                    MessageBoxIcon Icon = MessageBoxIcon.Information;
                    MessageBox.Show("Match inserted successfully", "Operation complete", Buttons, Icon);
                }
            }
        }

        private Task MatchesAddingByTeamEngine(int MatchID, List<int> MatchesIDList)
        {
            return Task.Run(() =>
           {
               Uri MatchDetailsURL = new Uri(DownloadStringCreation.CreateMatchDetailsString(MatchesIDList[MatchID]));
               SaveResponseToFile(MatchDetailsURL, XMLFolder + "\\MatchDetails.xml");
               switch (Parser.ParseMatchDetailsFile(false))
               {
                   case 0: //totul e in regula;
                       {
                           MatchRatings = Parser.ReadMatchRatings;
                           DatabaseOperations.AddAMatch(MatchesIDList[MatchID], MatchRatings);
                           break;
                       }
                   case -1: //meciul nu face parte din categoria celor acceptate in program
                       {
                           break;
                       }
                   case -2: //conexiunea la internet s-a intrerupt.
                       {
                           MessageBoxButtons Buttons = MessageBoxButtons.OK;
                           MessageBoxIcon Icon = MessageBoxIcon.Error;
                           MessageBox.Show("The internet connection was lost. The program will close.", "Internet connection lost", Buttons, Icon);
                           Application.Exit();
                           return;
                       }
                   default:
                       break;
               }
               MatchRatings = Parser.ResetMatchRatingsList();
           });
        }

        private void UpdateProgressWindowInterfaceForTeamAdding(StringBuilder ProgressString, ProgressWindow TheWindow, List<int> MatchesIDList, int MatchID)
        {
            ProgressString.Append("Progress... ");
            ProgressString.Append((MatchID + 1).ToString(CultureInfo.InvariantCulture));
            ProgressString.Append("/");
            ProgressString.Append(MatchesIDList.Count.ToString(CultureInfo.InvariantCulture));
            TheWindow.ProgressLabel.Text = ProgressString.ToString();
            TheWindow.TheProgressBar.Value++;
            ProgressString.Clear();
            TheWindow.ProgressLabel.Refresh();
        }

        private void ShowFinalMessageForTeamAdding(int TheNumberOfMatchesAdded, List<int> MatchesIDList)
        {
            Enabled = true;
            MessageBoxButtons Buttons = MessageBoxButtons.OK;
            MessageBoxIcon Icon = MessageBoxIcon.Information;
            MessageBox.Show($"Specified kind of matches added successfully! {TheNumberOfMatchesAdded.ToString(CultureInfo.InvariantCulture)} matches added to the database from the {MatchesIDList.Count.ToString(CultureInfo.InvariantCulture)} matches played in the selected season", "Operation complete", Buttons, Icon);
        }

        /// <summary>
        /// Functia de adaugare in baza de date a meciurilor unei echipe dintr-un anumit sezon.
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private async void AddMultipleMatchesByTeam(object sender, EventArgs e)
        {
            int NumberOfMatchesAdded = 0;
            CancelDatabaseAdding = false;
            AddMultipleMatchesByTeam AddTeam = new AddMultipleMatchesByTeam();
            if (AddTeam.ShowDialog(this) == DialogResult.OK)
            {
                Uri MatchArchiveURL = new Uri(DownloadString.CreateMatchArchiveString(AddTeam.TeamID, AddTeam.SeasonNumber));
                Enabled = false;
                SaveResponseToFile(MatchArchiveURL, XMLFolder + "\\Archive.xml");
                List<int> MatchesIDList = Parser.ParseArchiveFile(); //retine numerele de identificare ale meciurilor citite din fisier
                ProgressWindow PW = new ProgressWindow();
                PW.TheProgressBar.Maximum = MatchesIDList.Count;
                PW.Show(this);
                for (int i = 0; i < MatchesIDList.Count; i++)
                {
                    StringBuilder ProgressString = new StringBuilder();
                    await MatchesAddingByTeamEngine(i, MatchesIDList).ConfigureAwait(true);
                    NumberOfMatchesAdded++;
                    UpdateProgressWindowInterfaceForTeamAdding(ProgressString, PW, MatchesIDList, i);
                    if (CancelDatabaseAdding)
                    {
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Information;
                        MessageBox.Show("Adding cancelled. Matches prior to cancelling were inserted into the database.", "Information", Buttons, Icon);
                        break;
                    }
                }
                if (!CancelDatabaseAdding)
                {
                    ShowFinalMessageForTeamAdding(NumberOfMatchesAdded, MatchesIDList);
                }
                PW.Close();
            }
        }

        private Task MatchesAddingByIDEngine(int MatchID)
        {
            return Task.Run(() =>
            {
                Uri MatchDetailsURL = new Uri(DownloadStringCreation.CreateMatchDetailsString(MatchID));
                SaveResponseToFile(MatchDetailsURL, XMLFolder + "\\MatchDetails.xml");
                switch (Parser.ParseMatchDetailsFile(false))
                {
                    case 0: //Daca face parte din categoria meciurilor ce pot intra in BD
                        {
                            MatchRatings = Parser.ReadMatchRatings;
                            DatabaseOperations.AddAMatch(MatchID, MatchRatings);
                            break;
                        }
                    case -1: //Daca face parte din categoria meciurilor ce nu pot intra in BD
                        {
                            break;
                        }
                    case -2: //Daca s-a intrerupt conexiunea la internet in timp ce se adaugau meciurile la BD
                        {
                            MessageBoxButtons Buttons = MessageBoxButtons.OK;
                            MessageBoxIcon Icon = MessageBoxIcon.Error;
                            MessageBox.Show("The internet connection was lost. The program will close.", "Internet connection lost", Buttons, Icon);
                            Application.Exit();
                            return;
                        }
                    default:
                        break;
                }
            });
        }

        private void UpdateProgressWindowInterfaceForIDAdding(int MatchID, int LowLimit, int HighLimit, ProgressWindow PW)
        {
            StringBuilder Matches = new StringBuilder();
            Matches.Append("Progress... ");
            Matches.Append((MatchID - LowLimit + 1).ToString(CultureInfo.InvariantCulture));
            Matches.Append("/");
            Matches.Append((HighLimit - LowLimit + 1).ToString(CultureInfo.InvariantCulture));
            PW.ProgressLabel.Text = Matches.ToString();
            Matches.Clear();
            PW.TheProgressBar.Value++;
            PW.ProgressLabel.Refresh();
        }

        private void ShowFinalMessageForIDAdding(int MatchesAdded, int LowLimit, int HighLimit)
        {
            MessageBoxButtons Buttons = MessageBoxButtons.OK;
            MessageBoxIcon Icon = MessageBoxIcon.Information;
            MessageBox.Show("Specified kind of matches added successfully! " + MatchesAdded.ToString(CultureInfo.InvariantCulture) + " matches were added to the database, from the " + (HighLimit - LowLimit + 1).ToString(CultureInfo.InvariantCulture) + " matches specified.", "Operation complete", Buttons, Icon);
        }

        /// <summary>
        /// Functia de adaugare a in baza de date a meciurilor ce au numarul de identificare cuprins intre doua limite
        /// </summary>
        /// <param name="sender">Hander de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private async void AddMultipleMatchesByID(object sender, EventArgs e)
        {
            int NumberOfMatchesAdded = 0; //retine cate meciuri au fost adaugate in baza de date
            CancelDatabaseAdding = false;
            AddMultipleMatchesByMatchIDRange AddID = new AddMultipleMatchesByMatchIDRange();
            if (AddID.ShowDialog(this) == DialogResult.OK)
            {
                Enabled = false;
                ProgressWindow PW = new ProgressWindow();
                PW.Show(this);
                PW.TheProgressBar.Maximum = AddID.HighLimit - AddID.LowLimit + 1;
                for (int i = AddID.LowLimit; i <= AddID.HighLimit; i++)
                {
                    await MatchesAddingByIDEngine(i).ConfigureAwait(true);
                    NumberOfMatchesAdded++;
                    MatchRatings = Parser.ResetMatchRatingsList();
                    //Dupa fiecare meci citit se aduce la 0 lista cu evaluari ale meciului. Motivul este acela ca in baza de date, evaluarile sunt trecute ca numere de la 1 la 80. Daca urmeaza sa fie adaugat in baza de date un meci care se va disputa, el nu va avea nicio evaluare, deci elementele listei vor ramane in continuare 0. Astfel se poate testa daca meciul care ar fi introdus in BD s-a jucat sau urmeaza sa se joace.
                    UpdateProgressWindowInterfaceForIDAdding(i, AddID.LowLimit, AddID.HighLimit, PW);
                    if (CancelDatabaseAdding)
                    {
                        MessageBoxButtons Buttons = MessageBoxButtons.OK;
                        MessageBoxIcon Icon = MessageBoxIcon.Information;
                        MessageBox.Show("Adding cancelled. Matches prior to cancelling were inserted into the database.", "Information", Buttons, Icon);
                        break;
                    }
                }
                Enabled = true;
                if (!CancelDatabaseAdding)
                {
                    ShowFinalMessageForIDAdding(NumberOfMatchesAdded, AddID.LowLimit, AddID.HighLimit);
                }
                PW.Close();
            }
        }

        /// <summary>
        /// Functia descarca meciurile viitoare ale primei echipe 
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void DownloadFirstTeamFutureMatches(object sender, EventArgs e)
        {
            ParseXMLFiles.FinalFutureMatches.Clear();
            Uri DownloadURL = new Uri(DownloadString.CreateMatchesString(Parser.UserTeamIDs[0]));
            FutureMatchesListBox.Items.Clear();
            SaveResponseToFile(DownloadURL, XMLFolder + "\\Matches.xml");
            Parser.ParseMatchesFile();
            for (int i = 0; i < ParseXMLFiles.FinalFutureMatches.Count; i++)
            {
                FutureMatchesListBox.Items.Add(ParseXMLFiles.FinalFutureMatches[i].HomeTeam + " - " + ParseXMLFiles.FinalFutureMatches[i].AwayTeam);
            }
        }

        /// <summary>
        /// Functia descarca meciurile viitoare ale celei de-a doua echipe 
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void DownloadSecondTeamFutureMatches(object sender, EventArgs e)
        {
            ParseXMLFiles.FinalFutureMatches.Clear();
            Uri DownloadURL = new Uri(DownloadString.CreateMatchesString(Parser.UserTeamIDs[1]));
            FutureMatchesListBox.Items.Clear();
            SaveResponseToFile(DownloadURL, XMLFolder + "\\Matches.xml");
            Parser.ParseMatchesFile();
            for (int i = 0; i < ParseXMLFiles.FinalFutureMatches.Count; i++)
            {
                FutureMatchesListBox.Items.Add(ParseXMLFiles.FinalFutureMatches[i].HomeTeam + " - " + ParseXMLFiles.FinalFutureMatches[i].AwayTeam);
            }
        }

        /// <summary>
        /// Functia descarca meciurile viitoare ale celei de-a treia echipe 
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void DownloadThirdTeamFutureMatches(object sender, EventArgs e)
        {
            ParseXMLFiles.FinalFutureMatches.Clear();
            Uri DownloadURL = new Uri(DownloadString.CreateMatchesString(Parser.UserTeamIDs[2]));
            FutureMatchesListBox.Items.Clear();
            SaveResponseToFile(DownloadURL, XMLFolder + "\\Matches.xml");
            Parser.ParseMatchesFile();
            for (int i = 0; i < ParseXMLFiles.FinalFutureMatches.Count; i++)
            {
                FutureMatchesListBox.Items.Add(ParseXMLFiles.FinalFutureMatches[i].HomeTeam + " - " + ParseXMLFiles.FinalFutureMatches[i].AwayTeam);
            }
        }

        /// <summary>
        /// Functia coverteste abilitatea exprimata printr-un numar (de la 1 la 80) in cea exprimata sub format text
        /// </summary>
        /// <param name="Number">numarul care va fi convertit</param>
        /// <returns>abilitatea in format text</returns>
        private static string ConvertNumberToSkill(int Number)
        {
            List<string> Skill = new List<string> { "Disastrous (1)", "Wretched (2)", "Poor (3)", "Weak (4)", "Inadequate (5)", "Passable(6)", "Solid (7)", "Excellent (8)", "Formidable (9)", "Outstanding (10)", "Brilliant (11)", "Magnificent (12)", "World Class (13)", "Supernatural (14)", "Titanic (15)", "Extraterrestrial (16)", "Mythical (17)", "Magical (18)", "Utopian (19)", "Divine (20)" };
            List<string> SubSkill = new List<string> { "very low", "low", "high", "very high" };
            Number--;
            int SkillNumber = Number / 4;
            int SubSkillNumber = Number % 4;
            return Skill[SkillNumber] + " (" + SubSkill[SubSkillNumber] + ")";
        }

        /// <summary>
        /// Procedura se ocupa de afisarea in fereastra a evaluarilor pe sectoare ale echipei gazda.
        /// </summary>
        private void ShowPredictedRatings()
        {
            HomeMidfieldRatingLabel.Text = ConvertNumberToSkill(MatchRatings[0]);
            HomeRightDefenceRatingLabel.Text = ConvertNumberToSkill(MatchRatings[1]);
            HomeCentralDefenceRatingLabel.Text = ConvertNumberToSkill(MatchRatings[2]);
            HomeLeftDefenceRatingLabel.Text = ConvertNumberToSkill(MatchRatings[3]);
            HomeRightAttackRatingLabel.Text = ConvertNumberToSkill(MatchRatings[4]);
            HomeCentralAttackRatingLabel.Text = ConvertNumberToSkill(MatchRatings[5]);
            HomeLeftAttackRatingLabel.Text = ConvertNumberToSkill(MatchRatings[6]);
        }

        /// <summary>
        /// Procedura se ocupa de incarcarea evaluarilor pentru echipa gazda in lista cu evaluari.
        /// </summary>
        private void LoadMatchRatings()
        {
            for (int i = 0; i <= 6; i++)
            {
                MatchRatings[i] = Parser.ReadMatchRatings[i];
            }
        }

        /// <summary>
        /// Functia obtine numarul de identificare pentru echipa pentru care vor fi preluate meciurile
        /// </summary>
        /// <returns>Numarul de identificare pentru echipa mentionata mai sus</returns>
        private int GetUserTeamID()
        {
            int TeamID = 0;
            foreach (Control C in TeamListGroupBox.Controls)
            {
                if ((C.GetType() == typeof(RadioButton)) && (C as RadioButton).Checked)
                {
                    TeamID = Parser.UserTeamIDs[int.Parse(C.Tag.ToString(), CultureInfo.InvariantCulture)];
                }
            }
            return TeamID;
        }

        /// <summary>
        /// Functia incarca evaluarile prezise pentru meciul selectat
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void LoadPredictedRatings(object sender, EventArgs e)
        {
            if (FutureMatchesListBox.Items.Count != 0)
            {
                Uri MatchOrdersURL = new Uri(DownloadString.CreateMatchOrdersString(ParseXMLFiles.FinalFutureMatches[FutureMatchesListBox.SelectedIndex].MatchID, GetUserTeamID()));
                SaveResponseToFile(MatchOrdersURL, XMLFolder + "\\Orders.xml");
                if (Parser.ParseOrdersFile())
                {
                    LoadMatchRatings();
                    ShowPredictedRatings();
                }
            }
        }

        /// <summary>
        /// Functia stabileste culorile etichetelor care afiseaza evaluarile, in functie de existenta unei evaluari pe un anumit post
        /// </summary>
        /// <param name="RatingIndex">Evaluarea de pe un anume post</param>
        /// <returns>False, daca macar o eticheta are culoarea rosie (pe pozitia respectvia nu s-a stabilit o evaluare). True altfel</returns>
        private bool SetRatingLabelColor(int RatingIndex)
        {
            if (MatchRatings[RatingIndex] == 0)
            {
                foreach (Control C in HomeTeamGroupBox.Controls)
                {
                    if (C.TabIndex == RatingIndex + 14)
                    {
                        C.ForeColor = Color.Red;
                    }
                }
                foreach (Control C in AwayTeamGroupBox.Controls)
                {
                    if (C.TabIndex == RatingIndex + 14)
                    {
                        C.ForeColor = Color.Red;
                    }
                }
                return false;
            }
            else
            {
                foreach (Control C in HomeTeamGroupBox.Controls)
                {
                    if (C.TabIndex == RatingIndex + 14)
                    {
                        C.ForeColor = SystemColors.ControlText;
                    }
                }
                foreach (Control C in AwayTeamGroupBox.Controls)
                {
                    if (C.TabIndex == RatingIndex + 14)
                    {
                        C.ForeColor = SystemColors.ControlText;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// functia verifica daca toate datele de intrare sunt introduse corect (fiecare compartiment a primit o evaluare)
        /// </summary>
        /// <returns>true, daca fiecare compartiment a primit o evaluare, false altfel</returns>
        private bool InputsAreValid()
        {
            bool Result = true;
            bool TempResult;
            for (int i = 0; i < 14; i++)
            {
                TempResult = SetRatingLabelColor(i);
                if (!TempResult)
                {
                    Result = TempResult;
                }
            }
            return Result;
        }

        /// <summary>
        /// Functia implementeaza algoritmul de predictie
        /// </summary>
        private void PredictingEngine()
        {
            string SelectionCommand = "Select HomeTeamGoals, AwayTeamGoals from Games where HomeTeamMidfield=@HTM and HomeTeamRDefense=@HTRD and HomeTeamCDefense=@HTCD and HomeTeamLDefense=@HTLD and HomeTeamRAttack=@HTRA and HomeTeamCAttack=@HTCA and HomeTeamLAttack=@HTLA and AwayTeamMidfield=@ATM and AwayTeamRDefense=@ATRD and AwayTeamCDefense=@ATCD and AwayTeamLDefense=@ATLD and AwayTeamRAttack=@ATRA and AwayTeamCAttack=@ATCA and AwayTeamLAttack=@ATLA;";
            int HomeWins = 0;
            int Ties = 0;
            int AwayWins = 0;
            int NumberOfPlayedMatches = 0;
            int TotalNumberOfHomeGoals = 0;
            int TotalNumberOfAwayGoals = 0;
            float AverageNumberOfHomeGoals = 0.0f;
            float AverageNumberOfAwayGoals = 0.0f;
            float HomeWinPercentage = 0.0f;
            float TiePercentage = 0.0f;
            float AwayWinPercentage = 0.0f;
            SqlConnection MyConn = new SqlConnection(DatabaseOperations.CreateTableConnectionString);
            SqlCommand command = new SqlCommand(SelectionCommand, MyConn);

            command.Parameters.Add("@HTM", SqlDbType.TinyInt).Value = MatchRatings[0];
            command.Parameters.Add("@HTRD", SqlDbType.TinyInt).Value = MatchRatings[1];
            command.Parameters.Add("@HTCD", SqlDbType.TinyInt).Value = MatchRatings[2];
            command.Parameters.Add("@HTLD", SqlDbType.TinyInt).Value = MatchRatings[3];
            command.Parameters.Add("@HTRA", SqlDbType.TinyInt).Value = MatchRatings[4];
            command.Parameters.Add("@HTCA", SqlDbType.TinyInt).Value = MatchRatings[5];
            command.Parameters.Add("@HTLA", SqlDbType.TinyInt).Value = MatchRatings[6];
            command.Parameters.Add("@ATM", SqlDbType.TinyInt).Value = MatchRatings[7];
            command.Parameters.Add("@ATRD", SqlDbType.TinyInt).Value = MatchRatings[8];
            command.Parameters.Add("@ATCD", SqlDbType.TinyInt).Value = MatchRatings[9];
            command.Parameters.Add("@ATLD", SqlDbType.TinyInt).Value = MatchRatings[10];
            command.Parameters.Add("@ATRA", SqlDbType.TinyInt).Value = MatchRatings[11];
            command.Parameters.Add("@ATCA", SqlDbType.TinyInt).Value = MatchRatings[12];
            command.Parameters.Add("@ATLA", SqlDbType.TinyInt).Value = MatchRatings[13];
            MyConn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                List<int> Score = ReadSingleRow((IDataRecord)reader);
                NumberOfPlayedMatches++;
                int HomeGoals = Score[0];
                int AwayGoals = Score[1];
                if (HomeGoals > AwayGoals)
                {
                    HomeWins++;
                }

                if (HomeGoals == AwayGoals)
                {
                    Ties++;
                }

                if (HomeGoals < AwayGoals)
                {
                    AwayWins++;
                }

                TotalNumberOfHomeGoals += HomeGoals;
                TotalNumberOfAwayGoals += AwayGoals;
                AverageNumberOfHomeGoals = (float)TotalNumberOfHomeGoals / (float)NumberOfPlayedMatches;
                AverageNumberOfAwayGoals = (float)TotalNumberOfAwayGoals / (float)NumberOfPlayedMatches;
                HomeWinPercentage = ((float)HomeWins / (float)NumberOfPlayedMatches) * 100;
                TiePercentage = ((float)Ties / (float)NumberOfPlayedMatches) * 100;
                AwayWinPercentage = ((float)AwayWins / (float)NumberOfPlayedMatches) * 100;
            }
            reader.Close();
            MyConn.Close();
            HomeWinPercentageLabel.Text = "Home win %: " + HomeWinPercentage.ToString("F", CultureInfo.InvariantCulture);
            HGALabel.Text = "Home goals average: " + AverageNumberOfHomeGoals.ToString("F", CultureInfo.InvariantCulture);
            DrawPercentageLabel.Text = "Draw: " + TiePercentage.ToString("F", CultureInfo.InvariantCulture);
            AwayWinPercentageLabel.Text = "Away win %: " + AwayWinPercentage.ToString("F", CultureInfo.InvariantCulture);
            AGALabel.Text = "Away goals average: " + AverageNumberOfAwayGoals.ToString("F", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Functia care se ocupa cu efectuarea predictiilor.
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void MakeThePrediction(object sender, EventArgs e)
        {
            if (InputsAreValid())
            {
                Cursor = Cursors.WaitCursor;
                PredictingEngine();
                Cursor = Cursors.Default;
            }
            else
            {
                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                MessageBoxIcon Icon = MessageBoxIcon.Error;
                MessageBox.Show("Not all the sector evaluations added. Please check the texts written in red.", "Error", Buttons, Icon);
            }
        }

        /// <summary>
        /// Functia citeste golurile inscrise de cele doua echipe dintr-un rand al bazei de date si le adauga intr-o lista. Rezultatul va fi folosit pentru prelucrarea rezultatelor finale
        /// </summary>
        /// <param name="record">Inregistrarea din care se citeste scorul</param>
        /// <returns>Scorul convertit sub forma unei liste</returns>
        private static List<int> ReadSingleRow(IDataRecord record)
        {
            List<int> Score = new List<int> { };
            if (int.TryParse(record[0].ToString(), out int temp))
            {
                Score.Add(temp);
            }

            if (int.TryParse(record[1].ToString(), out temp))
            {
                Score.Add(temp);
            }

            return Score;
        }

        /// <summary>
        /// Functia sterge inregistrarile din registri ce contin jetoanele.
        /// </summary>
        /// <returns>true, daca stergerea a avut succes, altfel false</returns>
        private static bool DeleteTokensFromRegistryEngine()
        {
            try
            {
                Registry.CurrentUser.DeleteSubKey("HTMPTK");
            }
            catch (UnauthorizedAccessException U)
            {
                MessageBox.Show(U.Message);
                return false;
            }
            catch (SecurityException S)
            {
                MessageBox.Show(S.Message);
                return false;
            }
            finally
            {
                Registry.CurrentUser.Close();
            }
            return true;
        }

        /// <summary>
        /// Functia sterge cele 2 jetoane din registry
        /// </summary>
        /// <param name="sender">handler de eveniment</param>
        /// <param name="e">handler de eveniment</param>
        private void DeleteTokensFromRegistry(object sender, EventArgs e)
        {
            MessageBoxButtons Buttons = MessageBoxButtons.OK;
            MessageBoxIcon Icon = MessageBoxIcon.Information;
            if (!DeleteTokensFromRegistryEngine())
            {
                MessageBox.Show("Error in revoking Hattrick access!", "Error", Buttons, Icon);
            }
            else
            {
                MessageBox.Show("Access withdrawn. Next time the program runs, you will have to enter the PIN again.", "Success", Buttons, Icon);
            }
        }

        /// <summary>
        /// Functia verifica daca programul are permisiunea Hattrick de a-i accesa resursele
        /// </summary>
        /// <returns>true daca o are, altfel false</returns>
        private bool CheckPermissionEngine()
        {
            Uri PermissionUri = new Uri("https://chpp.hattrick.org/oauth/check_token.ashx");
            try
            {
                SaveResponseToFile(PermissionUri, XMLFolder + "\\Check.txt");
            }
            catch (WebException S)
            {
                if (S.Status == WebExceptionStatus.ProtocolError)
                {
                    DeleteTokensFromRegistryEngine();
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        /// <summary>
        /// Functia verifica daca programul mai are acces la resursele Hattrick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestPermissionExistence(object sender, EventArgs e)
        {
            MessageBoxButtons Buttons = MessageBoxButtons.OK;
            MessageBoxIcon Icon = MessageBoxIcon.Information;
            if (CheckPermissionEngine())
            {
                MessageBox.Show("The program has permission to access Hattrick", "Acces granted", Buttons, Icon);
            }
            else
            {
                MessageBox.Show("The program does not have permission to access Hattrick. It will now close", "Acces denied", Buttons, Icon);
                Application.Exit();
            }
        }

        /// <summary>
        /// Functia implementeaza actiunile pentru monitorizarea existentei BD si a conexiunii la net
        /// </summary>
        /// <returns>true, daca avem atat net cat si BD, false altfel</returns>
        private bool ContinuousMonitoringJobsEngine()
        {
            if ((DatabaseOperations.DatabaseExists()) && (CheckForInternetConnection()))
            {
                AlterControlsEnable(true);
                HelpStatusLabel.Text = string.Empty;
                if (!InternetConnectionExistentAtStartup)
                {
                    LoginToHattrickServers();
                    InitializeMatchRatingList();
                    DisplayUserDetails();
                    CheckXMLFolderExistence();
                    InternetConnectionExistentAtStartup = true;
                }
                return true;
            }
            else
            {
                AlterControlsEnable(false);
                HelpStatusLabel.Text = !DatabaseOperations.DatabaseExists()
                    ? "Database file not found. Controls will be disabled until the database is found or another one is created."
                    : "Internet connection not found. Controls will be disabled until internet connection is restored.";
                return false;
            }
        }

        /// <summary>
        /// Verifica o data pe secunda existenta BD si a conexiunii la net.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContinuousMonitoringJobs(object sender, EventArgs e)
        {
            ContinuousMonitoringJobsEngine();
        }

        /// <summary>
        /// Actiuni efectuate la incarcarea formularului (adica la pornirea programului)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Startup(object sender, EventArgs e)
        {
            if (!DatabaseOperations.DatabaseExists())
            {
                AlterControlsEnable(false);
                HelpStatusLabel.Text = "No databases detected. Creating a new matches database...";
                Operations.CreateDatabase();
                HelpStatusLabel.Text = "Matches database created.";
                AlterControlsEnable(true);
            }
            if (!CheckForInternetConnection())
            {
                AlterControlsEnable(false);
                HelpStatusLabel.Text = "No internet connection detected. Controls will be disabled until an existent internet connection is detected";
                InternetConnectionExistentAtStartup = false;
                return;
            }
            else
            {
                InternetConnectionExistentAtStartup = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                LoginToHattrickServers();
                InitializeMatchRatingList();
                DisplayUserDetails();
            }
        }
    }
}