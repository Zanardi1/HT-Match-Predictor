using Microsoft.Win32;
using OAuth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

//todo sa citesc dintr-un fisier denumirile evaluarilor (lucru util pentru momentul in care voi introduce si alte limbi pentru interfata programului
//todo bug atunci cand revoc aplicatia din contul Hattrick, jetoanele raman, dar sunt inutilizabile. Din acest motiv primesc o eroare
//todo sa adaug un buton de anulare a importului meciurilor in baza de date

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
                    ratingreturned = 1;
                if (value > 80)
                    ratingreturned = 80;
                else
                    ratingreturned = value;
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
            Uri Compendium = new Uri(DownloadString.CreateManagerCompendiumString());
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return Value;
        }

        /// <summary>
        /// Functia intoarce valoarea lui token_secret, citita din registri
        /// </summary>
        /// <returns>Sirul de caractere ce reprezinta jetonul secret (token_secret)</returns>
        private static string ReadTokenSecretFromRegistry()
        {
            string Value = string.Empty; ;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            Key.SetValue("Token", o["token"]);
            Key.SetValue("Secret Token", o["token_secret"]);
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
            catch (IOException) //in cazul in care fisierul XML este in uz, mai incearca o data. Nu stiu daca e calea cea mai potrivita, totusi
            {
                File.WriteAllText(DestinationFileName, GetFileContent(SourceURLAddress.ToString()));
            }
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
            string[] TeamPieces = new string[] { "Team list: \r\n", Parser.UserTeamNames[0], " (", Parser.UserTeamIDs[0].ToString(CultureInfo.InvariantCulture), ")\r\n", Parser.UserTeamNames[1], " (", Parser.UserTeamIDs[1].ToString(CultureInfo.InvariantCulture), ")\r\n", Parser.UserTeamNames[2], " (", Parser.UserTeamIDs[2].ToString(CultureInfo.InvariantCulture), ")\r\n" };
            TeamListLabel.Text = String.Concat(TeamPieces);
            FirstTeamRadioButton.Checked = true;
            FirstTeamRadioButton.Text = Parser.UserTeamNames[0];
            if (!String.IsNullOrEmpty(Parser.UserTeamNames[1]))
            {
                SecondTeamRadioButton.Visible = true;
                SecondTeamRadioButton.Text = Parser.UserTeamNames[1];
            }
            if (!String.IsNullOrEmpty(Parser.UserTeamNames[1]))
            {
                ThirdTeamRadioButton.Visible = true;
                ThirdTeamRadioButton.Text = Parser.UserTeamNames[2];
            }
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
        /// Procedura aduce datele specifice simularii la valorile initiale
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void ResetData(object sender, EventArgs e)
        {
            ResetMatchRatingList();

            foreach (Control C in HomeTeamGroupBox.Controls)
            {
                if ((C.GetType() == typeof(Label)) && (C.TabIndex >= 14) && (C.TabIndex <= 20))
                {
                    C.ForeColor = SystemColors.ControlText;
                    C.Text = "(No rating selected!)";
                }
            }

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

        /// <summary>
        /// Functia de adaugare in baza de date a meciurilor unei echipe dintr-un anumit sezon.
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void AddMultipleMatchesByTeam(object sender, EventArgs e)
        {
            int NumberOfMatchesAdded = 0;
            List<int> MatchesIDList = new List<int> { }; //retine numerele de identificare ale meciurilor citite din fisier
            AddMultipleMatchesByTeam AddTeam = new AddMultipleMatchesByTeam();
            Uri MatchArchiveURL = new Uri(DownloadString.CreateMatchArchiveString(AddTeam.TeamID, AddTeam.SeasonNumber));

            if (AddTeam.ShowDialog(this) == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                ProgressWindow PW = new ProgressWindow();
                PW.Show(this);
                SaveResponseToFile(MatchArchiveURL, XMLFolder + "\\Archive.xml");
                MatchesIDList = Parser.ParseArchiveFile();
                PW.TheProgressBar.Maximum = MatchesIDList.Count;
                for (int i = 0; i < MatchesIDList.Count; i++)
                {
                    Uri MatchDetailsURL = new Uri(DownloadString.CreateMatchDetailsString(MatchesIDList[i]));
                    SaveResponseToFile(MatchDetailsURL, XMLFolder + "\\MatchDetails.xml");
                    if (Parser.ParseMatchDetailsFile(false) != -1)
                    {
                        if (Parser.ReadMatchRatings[0] != 0)
                        {
                            MatchRatings = Parser.ReadMatchRatings;
                            Operations.AddAMatch(MatchesIDList[i], MatchRatings);
                            NumberOfMatchesAdded++;
                        }
                    }
                    MatchRatings = Parser.ResetMatchRatingsList();
                    PW.ProgressLabel.Text = "Progress... " + (i + 1).ToString(CultureInfo.InvariantCulture) + "/" + MatchesIDList.Count.ToString(CultureInfo.InvariantCulture);
                    PW.TheProgressBar.Value++;
                    PW.ProgressLabel.Refresh();
                }
                Cursor = Cursors.Default;
                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                MessageBoxIcon Icon = MessageBoxIcon.Information;
                MessageBox.Show($"Specified kind of matches added successfully! {NumberOfMatchesAdded.ToString(CultureInfo.InvariantCulture)} matches added to the database from the {MatchesIDList.Count.ToString(CultureInfo.InvariantCulture)} matches played in the selected season", "Operation complete", Buttons, Icon);
                PW.Close();
            }
        }

        /// <summary>
        /// Functia de adaugare a in baza de date a meciurilor ce au numarul de identificare cuprins intre doua limite
        /// </summary>
        /// <param name="sender">Hander de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void AddMultipleMatchesByID(object sender, EventArgs e)
        {
            int NumberOfMatchesAdded = 0; //retine cate meciuri au fost adaugate in baza de date
            AddMultipleMatchesByMatchIDRange AddID = new AddMultipleMatchesByMatchIDRange();
            if (AddID.ShowDialog(this) == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                ProgressWindow PW = new ProgressWindow();
                PW.Show(this);
                PW.TheProgressBar.Maximum = AddID.HighLimit - AddID.LowLimit + 1;
                for (int i = AddID.LowLimit; i <= AddID.HighLimit; i++)
                {
                    Uri MatchDetailsURL = new Uri(DownloadString.CreateMatchDetailsString(i));
                    SaveResponseToFile(MatchDetailsURL, XMLFolder + "\\MatchDetails.xml");
                    if (Parser.ParseMatchDetailsFile(false) != -1) //Daca face parte din categoria meciurilor ce pot intra in BD
                    {
                        if (Parser.ReadMatchRatings[0] != 0)
                        {
                            MatchRatings = Parser.ReadMatchRatings;
                            Operations.AddAMatch(i, MatchRatings);
                            NumberOfMatchesAdded++;
                        }
                    }
                    MatchRatings = Parser.ResetMatchRatingsList();
                    //Dupa fiecare meci citit se aduce la 0 lista cu evaluari ale meciului. Motivul este acela ca in baza de date, evaluarile sunt trecute ca numere de la 1 la 80. Daca urmeaza sa fie adaugat in baza de date un meci care se va disputa, el nu va avea nicio evaluare, deci elementele listei vor ramane in continuare 0. Astfel se poate testa daca meciul care ar fi introdus in BD s-a jucat sau urmeaza sa se joace.
                    PW.ProgressLabel.Text = "Progress... " + (i - AddID.LowLimit + 1).ToString(CultureInfo.InvariantCulture) + "/" + (AddID.HighLimit - AddID.LowLimit + 1).ToString(CultureInfo.InvariantCulture);
                    PW.TheProgressBar.Value++;
                    PW.ProgressLabel.Refresh();
                }
                Cursor = Cursors.Default;
                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                MessageBoxIcon Icon = MessageBoxIcon.Information;
                MessageBox.Show("Specified kind of matches added successfully! " + NumberOfMatchesAdded.ToString(CultureInfo.InvariantCulture) + " matches were added to the database, from the " + (AddID.HighLimit - AddID.LowLimit + 1).ToString(CultureInfo.InvariantCulture) + " matches specified.", "Operation complete", Buttons, Icon);
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
        /// Functia coverteste abilitatea exprimata printr-un numar (de la 1 la 80) in cea exprimata sb format text
        /// </summary>
        /// <param name="Number">numarul care va fi convertit</param>
        /// <returns>abilitatea in format text</returns>
        private string ConvertNumberToSkill(int Number)
        {
            List<string> Skill = new List<string> { "Disastrous (1)", "Wretched (2)", "Poor (3)", "Weak (4)", "Inadequate (5)", "Passable(6)", "Solid (7)", "Excellent (8)", "Formidable (9)", "Outstanding (10)", "Brilliant (11)", "Magnificent (12)", "World Class (13)", "Supernatural (14)", "Titanic (15)", "Extraterrestrial (16)", "Mythical (17)", "Magical (18)", "Utopian (19)", "Divine (20)" };
            List<string> SubSkill = new List<string> { "very low", "low", "high", "very high" };
            int SkillNumber = 0;
            int SubSkillNumber = 0;
            switch (Number)
            {
                case 1:
                    {
                        SkillNumber = 0;
                        SubSkillNumber = 0;
                        break;
                    }
                case 2:
                    {
                        SkillNumber = 0;
                        SubSkillNumber = 1;
                        break;
                    }
                case 3:
                    {
                        SkillNumber = 0;
                        SubSkillNumber = 2;
                        break;
                    }
                case 4:
                    {
                        SkillNumber = 0;
                        SubSkillNumber = 3;
                        break;
                    }
                case 5:
                    {
                        SkillNumber = 1;
                        SubSkillNumber = 0;
                        break;
                    }
                case 6:
                    {
                        SkillNumber = 1;
                        SubSkillNumber = 1;
                        break;
                    }
                case 7:
                    {
                        SkillNumber = 1;
                        SubSkillNumber = 2;
                        break;
                    }
                case 8:
                    {
                        SkillNumber = 1;
                        SubSkillNumber = 3;
                        break;
                    }
                case 9:
                    {
                        SkillNumber = 2;
                        SubSkillNumber = 0;
                        break;
                    }
                case 10:
                    {
                        SkillNumber = 2;
                        SubSkillNumber = 1;
                        break;
                    }
                case 11:
                    {
                        SkillNumber = 2;
                        SubSkillNumber = 2;
                        break;
                    }
                case 12:
                    {
                        SkillNumber = 2;
                        SubSkillNumber = 3;
                        break;
                    }
                case 13:
                    {
                        SkillNumber = 3;
                        SubSkillNumber = 0;
                        break;
                    }
                case 14:
                    {
                        SkillNumber = 3;
                        SubSkillNumber = 1;
                        break;
                    }
                case 15:
                    {
                        SkillNumber = 3;
                        SubSkillNumber = 2;
                        break;
                    }
                case 16:
                    {
                        SkillNumber = 3;
                        SubSkillNumber = 3;
                        break;
                    }
                case 17:
                    {
                        SkillNumber = 4;
                        SubSkillNumber = 0;
                        break;
                    }
                case 18:
                    {
                        SkillNumber = 4;
                        SubSkillNumber = 1;
                        break;
                    }
                case 19:
                    {
                        SkillNumber = 4;
                        SubSkillNumber = 2;
                        break;
                    }
                case 20:
                    {
                        SkillNumber = 4;
                        SubSkillNumber = 3;
                        break;
                    }
                case 21:
                    {
                        SkillNumber = 5;
                        SubSkillNumber = 0;
                        break;
                    }
                case 22:
                    {
                        SkillNumber = 5;
                        SubSkillNumber = 1;
                        break;
                    }
                case 23:
                    {
                        SkillNumber = 5;
                        SubSkillNumber = 2;
                        break;
                    }
                case 24:
                    {
                        SkillNumber = 5;
                        SubSkillNumber = 3;
                        break;
                    }
                case 25:
                    {
                        SkillNumber = 6;
                        SubSkillNumber = 0;
                        break;
                    }
                case 26:
                    {
                        SkillNumber = 6;
                        SubSkillNumber = 1;
                        break;
                    }
                case 27:
                    {
                        SkillNumber = 6;
                        SubSkillNumber = 2;
                        break;
                    }
                case 28:
                    {
                        SkillNumber = 6;
                        SubSkillNumber = 3;
                        break;
                    }
                case 29:
                    {
                        SkillNumber = 7;
                        SubSkillNumber = 0;
                        break;
                    }
                case 30:
                    {
                        SkillNumber = 7;
                        SubSkillNumber = 1;
                        break;
                    }
                case 31:
                    {
                        SkillNumber = 7;
                        SubSkillNumber = 2;
                        break;
                    }
                case 32:
                    {
                        SkillNumber = 7;
                        SubSkillNumber = 3;
                        break;
                    }
                case 33:
                    {
                        SkillNumber = 8;
                        SubSkillNumber = 0;
                        break;
                    }
                case 34:
                    {
                        SkillNumber = 8;
                        SubSkillNumber = 1;
                        break;
                    }
                case 35:
                    {
                        SkillNumber = 8;
                        SubSkillNumber = 2;
                        break;
                    }
                case 36:
                    {
                        SkillNumber = 8;
                        SubSkillNumber = 3;
                        break;
                    }
                case 37:
                    {
                        SkillNumber = 9;
                        SubSkillNumber = 0;
                        break;
                    }
                case 38:
                    {
                        SkillNumber = 9;
                        SubSkillNumber = 1;
                        break;
                    }
                case 39:
                    {
                        SkillNumber = 9;
                        SubSkillNumber = 2;
                        break;
                    }
                case 40:
                    {
                        SkillNumber = 9;
                        SubSkillNumber = 3;
                        break;
                    }
                case 41:
                    {
                        SkillNumber = 10;
                        SubSkillNumber = 0;
                        break;
                    }
                case 42:
                    {
                        SkillNumber = 10;
                        SubSkillNumber = 1;
                        break;
                    }
                case 43:
                    {
                        SkillNumber = 10;
                        SubSkillNumber = 2;
                        break;
                    }
                case 44:
                    {
                        SkillNumber = 10;
                        SubSkillNumber = 3;
                        break;
                    }
                case 45:
                    {
                        SkillNumber = 11;
                        SubSkillNumber = 0;
                        break;
                    }
                case 46:
                    {
                        SkillNumber = 11;
                        SubSkillNumber = 1;
                        break;
                    }
                case 47:
                    {
                        SkillNumber = 11;
                        SubSkillNumber = 2;
                        break;
                    }
                case 48:
                    {
                        SkillNumber = 11;
                        SubSkillNumber = 3;
                        break;
                    }
                case 49:
                    {
                        SkillNumber = 12;
                        SubSkillNumber = 0;
                        break;
                    }
                case 50:
                    {
                        SkillNumber = 12;
                        SubSkillNumber = 1;
                        break;
                    }
                case 51:
                    {
                        SkillNumber = 12;
                        SubSkillNumber = 2;
                        break;
                    }
                case 52:
                    {
                        SkillNumber = 12;
                        SubSkillNumber = 3;
                        break;
                    }
                case 53:
                    {
                        SkillNumber = 13;
                        SubSkillNumber = 0;
                        break;
                    }
                case 54:
                    {
                        SkillNumber = 13;
                        SubSkillNumber = 1;
                        break;
                    }
                case 55:
                    {
                        SkillNumber = 13;
                        SubSkillNumber = 2;
                        break;
                    }
                case 56:
                    {
                        SkillNumber = 13;
                        SubSkillNumber = 3;
                        break;
                    }
                case 57:
                    {
                        SkillNumber = 14;
                        SubSkillNumber = 0;
                        break;
                    }
                case 58:
                    {
                        SkillNumber = 14;
                        SubSkillNumber = 1;
                        break;
                    }
                case 59:
                    {
                        SkillNumber = 14;
                        SubSkillNumber = 2;
                        break;
                    }
                case 60:
                    {
                        SkillNumber = 14;
                        SubSkillNumber = 3;
                        break;
                    }
                case 61:
                    {
                        SkillNumber = 15;
                        SubSkillNumber = 0;
                        break;
                    }
                case 62:
                    {
                        SkillNumber = 15;
                        SubSkillNumber = 1;
                        break;
                    }
                case 63:
                    {
                        SkillNumber = 15;
                        SubSkillNumber = 2;
                        break;
                    }
                case 64:
                    {
                        SkillNumber = 15;
                        SubSkillNumber = 3;
                        break;
                    }
                case 65:
                    {
                        SkillNumber = 16;
                        SubSkillNumber = 0;
                        break;
                    }
                case 66:
                    {
                        SkillNumber = 16;
                        SubSkillNumber = 1;
                        break;
                    }
                case 67:
                    {
                        SkillNumber = 16;
                        SubSkillNumber = 2;
                        break;
                    }
                case 68:
                    {
                        SkillNumber = 16;
                        SubSkillNumber = 3;
                        break;
                    }
                case 69:
                    {
                        SkillNumber = 17;
                        SubSkillNumber = 0;
                        break;
                    }
                case 70:
                    {
                        SkillNumber = 17;
                        SubSkillNumber = 1;
                        break;
                    }
                case 71:
                    {
                        SkillNumber = 17;
                        SubSkillNumber = 2;
                        break;
                    }
                case 72:
                    {
                        SkillNumber = 17;
                        SubSkillNumber = 3;
                        break;
                    }
                case 73:
                    {
                        SkillNumber = 18;
                        SubSkillNumber = 0;
                        break;
                    }
                case 74:
                    {
                        SkillNumber = 18;
                        SubSkillNumber = 1;
                        break;
                    }
                case 75:
                    {
                        SkillNumber = 18;
                        SubSkillNumber = 2;
                        break;
                    }
                case 76:
                    {
                        SkillNumber = 18;
                        SubSkillNumber = 3;
                        break;
                    }
                case 77:
                    {
                        SkillNumber = 19;
                        SubSkillNumber = 0;
                        break;
                    }
                case 78:
                    {
                        SkillNumber = 19;
                        SubSkillNumber = 1;
                        break;
                    }
                case 79:
                    {
                        SkillNumber = 19;
                        SubSkillNumber = 2;
                        break;
                    }
                case 80:
                    {
                        SkillNumber = 19;
                        SubSkillNumber = 3;
                        break;
                    }
            }
            return Skill[SkillNumber] + " (" + SubSkill[SubSkillNumber] + ")";
        }

        /// <summary>
        /// Functia incarca evaluarile prezise pentru meciul selectat
        /// </summary>
        /// <param name="sender">Handler de eveniment</param>
        /// <param name="e">Handler de eveniment</param>
        private void LoadPredictedRatings(object sender, EventArgs e)
        {
            int UserTeamID = 0;
            if (FirstTeamRadioButton.Checked)
                UserTeamID = Parser.UserTeamIDs[0];
            if (SecondTeamRadioButton.Checked)
                UserTeamID = Parser.UserTeamIDs[1];
            if (ThirdTeamRadioButton.Checked)
                UserTeamID = Parser.UserTeamIDs[2];
            Uri MatchOrdersURL = new Uri(DownloadString.CreateMatchOrdersString(ParseXMLFiles.FinalFutureMatches[FutureMatchesListBox.SelectedIndex].MatchID, UserTeamID));
            SaveResponseToFile(MatchOrdersURL, XMLFolder + "\\Orders.xml");
            Parser.ParseOrdersFile();
            for (int i = 0; i <= 6; i++)
            {
                MatchRatings[i] = Parser.ReadMatchRatings[i];
            }
            HomeMidfieldRatingLabel.Text = ConvertNumberToSkill(MatchRatings[0]);
            HomeRightDefenceRatingLabel.Text = ConvertNumberToSkill(MatchRatings[1]);
            HomeCentralDefenceRatingLabel.Text = ConvertNumberToSkill(MatchRatings[2]);
            HomeLeftDefenceRatingLabel.Text = ConvertNumberToSkill(MatchRatings[3]);
            HomeRightAttackRatingLabel.Text = ConvertNumberToSkill(MatchRatings[4]);
            HomeCentralAttackRatingLabel.Text = ConvertNumberToSkill(MatchRatings[5]);
            HomeLeftAttackRatingLabel.Text = ConvertNumberToSkill(MatchRatings[6]);
        }

        /// <summary>
        /// functia verifica daca toate datele de intrare sunt introduse corect (fiecare compartiment a primit o evaluare)
        /// </summary>
        /// <returns>true, daca fiecare compartiment a primit o evaluare, false altfel</returns>
        private bool InputsAreValid()
        {
            bool Result = true;
            if (MatchRatings[0] == 0)
            {
                HomeMidfieldRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                HomeMidfieldRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[1] == 0)
            {
                HomeRightDefenceRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                HomeRightDefenceRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[2] == 0)
            {
                HomeCentralDefenceRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                HomeCentralDefenceRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[3] == 0)
            {
                HomeLeftDefenceRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                HomeLeftDefenceRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[4] == 0)
            {
                HomeRightAttackRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                HomeRightAttackRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[5] == 0)
            {
                HomeCentralAttackRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                HomeCentralAttackRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[6] == 0)
            {
                HomeLeftAttackRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                HomeLeftAttackRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[7] == 0)
            {
                AwayMidfieldRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                AwayMidfieldRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[8] == 0)
            {
                AwayRightDefenceRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                AwayRightDefenceRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[9] == 0)
            {
                AwayCentralDefenceRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                AwayCentralDefenceRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[10] == 0)
            {
                AwayLeftDefenceRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                AwayLeftDefenceRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[11] == 0)
            {
                AwayRightAttackRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                AwayRightAttackRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[12] == 0)
            {
                AwayCentralAttackRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                AwayCentralAttackRatingLabel.ForeColor = SystemColors.ControlText;
            }

            if (MatchRatings[13] == 0)
            {
                AwayLeftAttackRatingLabel.ForeColor = Color.Red;
            }
            else
            {
                AwayLeftAttackRatingLabel.ForeColor = SystemColors.ControlText;
            }

            foreach (Control i in HomeTeamGroupBox.Controls)
            {
                if ((i.GetType() == typeof(System.Windows.Forms.Label)) && (i.ForeColor == Color.Red))
                {
                    Result = false;
                    break;
                }
            }

            if (Result)
            {
                foreach (Control i in AwayTeamGroupBox.Controls)
                {
                    if ((i.GetType() == typeof(System.Windows.Forms.Label)) && (i.ForeColor == Color.Red))
                    {
                        Result = false;
                        break;
                    }
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
                Score.Add(temp);
            if (int.TryParse(record[1].ToString(), out temp))
                Score.Add(temp);
            return Score;
        }
    }
}