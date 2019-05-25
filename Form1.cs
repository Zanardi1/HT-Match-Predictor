using Microsoft.Win32;
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
using System.Windows.Forms;
using System.Threading.Tasks;

//todo sa citesc dintr-un fisier denumirile evaluarilor (lucru util pentru momentul in care voi introduce si alte limbi pentru interfata programului
//todo bug atunci cand revoc aplicatia din contul Hattrick, jetoanele raman, dar sunt inutilizabile. Din acest motiv primesc o eroare
//todo de creat o clasa care se ocupa de scrierea diferitelor erori intr-un fisier text
//todo sa vad daca pot inlocui AddWithValue cu Add la scrierea in BD. Probabil ca o sa am o performanta mai buna. Punct de plecare: https://stackoverflow.com/questions/56206183/how-i-can-fix-the-next-error-in-visual-studio-2010-c

namespace HTMatchPredictor
{
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
        public List<int> MatchRatings = new List<int>(14);
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
        /// Aduce cele 14 evaluari ale unui meci la 0
        /// </summary>
        private void InitializeMatchRatingList()
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
            try
            {
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
            }
            catch (ArgumentNullException A)
            {
                MessageBox.Show(A.Message);
            }
            catch (ObjectDisposedException O)
            {
                MessageBox.Show(O.Message);
            }
            catch (SecurityException S)
            {
                MessageBox.Show(S.Message);
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
            try
            {
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
            }
            catch (ArgumentNullException A)
            {
                MessageBox.Show(A.Message);
            }
            catch (ObjectDisposedException O)
            {
                MessageBox.Show(O.Message);
            }
            catch (SecurityException S)
            {
                MessageBox.Show(S.Message);
            }
            catch (IOException I)
            {
                MessageBox.Show(I.Message);
            }
            return Value;
        }

        /// <summary>
        /// Procedura stocheaza cele doua jetoane, token si token_secret in registri
        /// </summary>
        private void StoreTokensToRegistry()
        {
            RegistryKey Key;
            Key = Registry.CurrentUser.CreateSubKey("HTMPTK"); //HTMP = HatTrick Match Predictor
            try
            {
                Key.SetValue("Token", o["token"]);
                Key.SetValue("Secret Token", o["token_secret"]);
            }
            catch (ArgumentNullException A)
            {
                MessageBox.Show(A.Message);
            }
            catch (ObjectDisposedException O)
            {
                MessageBox.Show(O.Message);
            }
            catch (UnauthorizedAccessException U)
            {
                MessageBox.Show(U.Message);
            }
            catch (SecurityException S)
            {
                MessageBox.Show(S.Message);
            }
            catch (IOException I)
            {
                MessageBox.Show(I.Message);
            }
            Key.Close();
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

        private void GetRequestToken()
        {
            OAuthResponse rt = o.AcquireRequestToken("https://chpp.hattrick.org/oauth/request_token.ashx", "GET");
            var url = "https://chpp.hattrick.org/oauth/authorize.aspx?oauth_token=" + o["token"];
            System.Diagnostics.Process.Start(url);
        }

        private void GetAccessToken()
        {
            InsertPIN I = new InsertPIN();
            I.ShowDialog(this);
            string pin = I.InsertPINTextBox.Text;
            OAuthResponse at = o.AcquireAccessToken("https://chpp.hattrick.org/oauth/access_token.ashx", "GET", pin);
            StoreTokensToRegistry();
        }

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
            HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //Primeste raspunsul
            Stream receive = response.GetResponseStream(); //Il incarca intr-un flux
            StreamReader s = new StreamReader(receive, Encoding.UTF8);
            string temp = s.ReadToEnd(); //Salveaza continutul raspunsului intr-o variabila
            return temp;
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
            catch (DirectoryNotFoundException) //In cazul in care nu exista folderul XML, il creaza si mai incearca o data
            {
                Directory.CreateDirectory(XMLFolder);
                File.WriteAllText(DestinationFileName, GetFileContent(SourceURLAddress.ToString()));
            }
            catch (IOException I) //todo bug din cand in cand mai primesc un mesaj de eroare cum ca fisierul Matches.xml e folosit de un alt proces.
            {
                //MessageBox.Show(I.Message);
                File.WriteAllText(CurrentFolder + "\\Error.txt", DateTime.Now.ToString(CultureInfo.InvariantCulture)+"\r\n\r\n"+ I.StackTrace);
                Cursor = Cursors.WaitCursor;
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

        private string CreateTeamList()
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
            TeamListLabel.Text = CreateTeamList();
            FirstTeamRadioButton.Checked = true;
            FirstTeamRadioButton.Text = Parser.UserTeamNames[0];
            CheckOtherTeam(Parser.UserTeamNames[1], SecondTeamRadioButton);
            CheckOtherTeam(Parser.UserTeamNames[2], ThirdTeamRadioButton);
        }

        public Form1()
        {
            InitializeComponent();
            LoginToHattrickServers();
            InitializeMatchRatingList();
            DisplayUserDetails();
        }

        private void ShowSkillWindow(object sender, System.EventArgs e)
        {
            SkillSelectionWindow S = new SkillSelectionWindow
            {
                Tag = (sender as Button).Tag.ToString() //Tag este folosit pentru a determina ce buton a fost apasat pentru a afisa fereastra
            };
            S.ShowDialog(this);

            MatchRatings[Convert.ToInt16(S.Tag, CultureInfo.InvariantCulture) - 1] = RatingReturned; //atribuie evaluarea numerica primita sectorului corespunzator, in functie de eticheta butonului a carui apasare a deschis fereastra
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
            if (!Operations.DatabaseExists())
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
            if (Operations.DatabaseExists())
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
            Uri DownloadURL = new Uri(DownloadString.CreateMatchDetailsString(MatchIDToAdd));
            if (MatchIDToAdd != -1)
            {
                SaveResponseToFile(DownloadURL, XMLFolder + "\\MatchDetails.xml");
                if (Parser.ParseMatchDetailsFile(true) != -1) //Daca face parte din categoria meciurilor ce pot intra in BD
                {
                    MatchRatings = Parser.ReadMatchRatings;
                    Operations.AddAMatch(MatchIDToAdd, MatchRatings);
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
               Uri MatchDetailsURL = new Uri(DownloadString.CreateMatchDetailsString(MatchesIDList[MatchID]));
               SaveResponseToFile(MatchDetailsURL, XMLFolder + "\\MatchDetails.xml");
               if (Parser.ParseMatchDetailsFile(false) != -1)
               {
                   MatchRatings = Parser.ReadMatchRatings;
                   Operations.AddAMatch(MatchesIDList[MatchID], MatchRatings);
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
            Cursor = Cursors.Default;
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
            List<int> MatchesIDList = new List<int> { }; //retine numerele de identificare ale meciurilor citite din fisier
            AddMultipleMatchesByTeam AddTeam = new AddMultipleMatchesByTeam();
            if (AddTeam.ShowDialog(this) == DialogResult.OK)
            {
                Uri MatchArchiveURL = new Uri(DownloadString.CreateMatchArchiveString(AddTeam.TeamID, AddTeam.SeasonNumber));
                Cursor = Cursors.WaitCursor;
                SaveResponseToFile(MatchArchiveURL, XMLFolder + "\\Archive.xml");
                MatchesIDList = Parser.ParseArchiveFile();
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
                        MessageBox.Show("Adding cancelled", "Information", Buttons, Icon);
                        break;
                    }
                }
                ShowFinalMessageForTeamAdding(NumberOfMatchesAdded, MatchesIDList);
                PW.Close();
            }
        }

        private Task MatchesAddingByIDEngine(int MatchID)
        {
            return Task.Run(() =>
            {
                Uri MatchDetailsURL = new Uri(DownloadString.CreateMatchDetailsString(MatchID));
                SaveResponseToFile(MatchDetailsURL, XMLFolder + "\\MatchDetails.xml");
                if (Parser.ParseMatchDetailsFile(false) != -1) //Daca face parte din categoria meciurilor ce pot intra in BD
                {
                    MatchRatings = Parser.ReadMatchRatings;
                    Operations.AddAMatch(MatchID, MatchRatings);
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
                Cursor = Cursors.WaitCursor;
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
                        MessageBox.Show("Adding cancelled","Information",Buttons,Icon);
                        break;
                    }
                }
                Cursor = Cursors.Default;
                ShowFinalMessageForIDAdding(NumberOfMatchesAdded, AddID.LowLimit, AddID.HighLimit);
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
            Uri MatchOrdersURL = new Uri(DownloadString.CreateMatchOrdersString(ParseXMLFiles.FinalFutureMatches[FutureMatchesListBox.SelectedIndex].MatchID, GetUserTeamID()));
            SaveResponseToFile(MatchOrdersURL, XMLFolder + "\\Orders.xml");
            if (Parser.ParseOrdersFile())
            {
                LoadMatchRatings();
                ShowPredictedRatings();
            }
        }

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

            if (!Result)
            {
                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                MessageBoxIcon Icon = MessageBoxIcon.Error;
                MessageBox.Show("Not all the sector evaluations added. Please check the texts written in red.", "Error", Buttons, Icon);
            }
            return Result;
        }

        /// <summary>
        /// Functia implementeaza algoritmul de predictie
        /// </summary>
        private void PredictingEngine()
        {
            string SelectionCommand = "Select HomeTeamGoals, AwayTeamGoals from Games where HomeTeamMidfield=@HTM and HomeTeamRDefense=@HTRD and HomeTeamCDefense=@HTCD and HomeTeamLDefense=@HTRD and HomeTeamRAttack=@HTRA and HomeTeamCAttack=@HTCA and HomeTeamLAttack=@HTLA and AwayTeamMidfield=@ATM and AwayTeamRDefense=@ATRD and AwayTeamCDefense=@ATCD and AwayTeamLDefense=@ATLD and AwayTeamRAttack=@ATRA and AwayTeamCAttack=@ATCA and AwayTeamLAttack=@ATLA";
            List<int> Score = new List<int> { };
            int HomeWins = 0;
            int Ties = 0;
            int AwayWins = 0;
            int NumberOfPlayedMatches = 0;
            int TotalNumberOfHomeGoals = 0;
            int TotalNumberOfAwayGoals = 0;
            float AverageNumberOfHomeGoals = 0;
            float AverageNumberOfAwayGoals = 0;
            float HomeWinPercentage = 0;
            float TiePercentage = 0;
            float AwayWinPercentage = 0;
            SqlConnection MyConn = new SqlConnection(Operations.CreateTableConnectionString);
            SqlCommand Command = new SqlCommand(SelectionCommand, MyConn);
            Command.Parameters.AddWithValue("@HTM", MatchRatings[0].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@HTRD", MatchRatings[1].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@HTCD", MatchRatings[2].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@HTLD", MatchRatings[3].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@HTRA", MatchRatings[4].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@HTCA", MatchRatings[5].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@HTLA", MatchRatings[6].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@ATM", MatchRatings[7].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@ATRD", MatchRatings[8].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@ATCD", MatchRatings[9].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@ATLD", MatchRatings[10].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@ATRA", MatchRatings[11].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@ATCA", MatchRatings[12].ToString(CultureInfo.InvariantCulture));
            Command.Parameters.AddWithValue("@ATLA", MatchRatings[13].ToString(CultureInfo.InvariantCulture));
            MyConn.Open();
            SqlDataReader reader = Command.ExecuteReader();
            while (reader.Read())
            {
                Score = ReadSingleRow((IDataRecord)reader);
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
                AverageNumberOfHomeGoals = TotalNumberOfHomeGoals / NumberOfPlayedMatches;
                AverageNumberOfAwayGoals = TotalNumberOfAwayGoals / NumberOfPlayedMatches;
                HomeWinPercentage = (HomeWins / NumberOfPlayedMatches) * 100;
                TiePercentage = (Ties / NumberOfPlayedMatches) * 100;
                AwayWinPercentage = (AwayWins / NumberOfPlayedMatches) * 100;
            }
            MyConn.Close();
            HomeWinPercentageLabel.Text = "Home win %: " + HomeWinPercentage.ToString(CultureInfo.InvariantCulture);
            HGALabel.Text = "Home goals average: " + AverageNumberOfHomeGoals.ToString(CultureInfo.InvariantCulture);
            DrawPercentageLabel.Text = "Draw: " + TiePercentage.ToString(CultureInfo.InvariantCulture);
            AwayWinPercentageLabel.Text = "Away win %: " + AwayWinPercentage.ToString(CultureInfo.InvariantCulture);
            AGALabel.Text = "Away goals average: " + AverageNumberOfAwayGoals.ToString(CultureInfo.InvariantCulture);
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
                PredictingEngine();
            }
        }

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
    }
}