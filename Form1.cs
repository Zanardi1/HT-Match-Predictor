using Microsoft.Win32;
using OAuth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

//todo sa scriu o rutina de compunere a URL-ului care va fi trimis catre Hattrick pentru descarcarea fisierelor
//todo sa citesc dintr-un fisier denumirile evaluarilor (lucru util pentru momentul in care voi introduce si alte limbi pentru interfata programului
//todo bug atunci cand revoc aplicatia din contul Hattrick, jetoanele raman, dar sunt inutilizabile. Din acest motiv primesc o eroare

namespace HT_Match_Predictor
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
        public int RatingReturned = 0;
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
        public int MatchIDToAdd = 0;
        /// <summary>
        /// Retine numarul de identificare al meciului care va fi eliminat din baza de date, ca urmare a optiunii de stergere a unui singur meci.
        /// </summary>
        public int MatchIDToDelete = 0;


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
            InitializeAuthenticationObject();
            if (o["token"] == string.Empty)
            {
                GetRequestToken();
                GetAccessToken();
            }
            SaveResponseToFile(DownloadString.CreateManagerCompendiumString(), XMLFolder + "\\User.xml");
        }

        /// <summary>
        /// Functia intoarce valoarea lui token, citita din registri.
        /// </summary>
        /// <returns>Sirul de caractere ce reprezinta jetonul (token)</returns>
        private string ReadTokenFromRegistry()
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
        private string ReadTokenSecretFromRegistry()
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
            string search = URLAddress;
            string authzHeader = o.GenerateAuthzHeader(search, "GET");
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

        public void SaveResponseToFile(string SourceURLAddress, string DestinationFileName)
        {
            try
            {
                File.WriteAllText(DestinationFileName, GetFileContent(SourceURLAddress));
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(XMLFolder);
                File.WriteAllText(DestinationFileName, GetFileContent(SourceURLAddress));
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
            string[] TeamPieces = new string[] { "Team list: \r\n", Parser.UserTeamNames[0], " (", Parser.UserTeamIDs[0].ToString(), ")\r\n", Parser.UserTeamNames[1], " (", Parser.UserTeamIDs[1].ToString(), ")\r\n", Parser.UserTeamNames[2], " (", Parser.UserTeamIDs[2].ToString(), ")\r\n" };
            TeamListLabel.Text = String.Concat(TeamPieces);
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

            MatchRatings[Convert.ToInt16(S.Tag) - 1] = RatingReturned; //atribuie evaluarea numerica primita sectorului corespunzator, in functie de eticheta butonului a carui apasare a deschis fereastra
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
                    C.Text = "(No rating selected!)";
                }
            }

            foreach (Control C in AwayTeamGroupBox.Controls)
            {
                if ((C.GetType() == typeof(Label)) && (C.TabIndex >= 21) && (C.TabIndex <= 27))
                {
                    C.Text = "(No rating selected!)";
                }
            }
        }

        private void AddSingleMatchToDatabase(object sender, EventArgs e)
        {
            AddSingleMatchForm A = new AddSingleMatchForm();
            A.ShowDialog(this);
            if (MatchIDToAdd != -1)
            {
                SaveResponseToFile(DownloadString.CreateMatchDetailsString(MatchIDToAdd), XMLFolder + "\\MatchDetails.xml");
                if (Parser.ParseMatchDetailsFile() != -1) //Daca face parte din categoria meciurilor ce pot intra in BD
                {
                    MatchRatings = Parser.ReadMatchRatings;
                    Operations.AddAMatch(MatchIDToAdd, MatchRatings);
                }
            }
        }

        private void DeleteSingleMatchFromDatabase(object sender, EventArgs e)
        {
            DeleteSingleMatchForm D = new DeleteSingleMatchForm();
            D.ShowDialog(this);
            if (MatchIDToDelete != -1)
            {
                MessageBoxButtons Buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon Icon = MessageBoxIcon.Question;
                DialogResult Result = MessageBox.Show("Are you sure you want to delete from the database the match with the ID: " + MatchIDToDelete.ToString() + "?", "Please confirm", Buttons, Icon);
                if (Result == DialogResult.Yes)
                    Operations.DeleteAMatch(MatchIDToDelete);
            }
        }

        private void AddMultipleMatchesByTeam(object sender, EventArgs e)
        {
            //todo cred ca aici e un bug la ei, atunci cand apelez functia matches inclusiv datele de inceput si de final
            AddMultipleMatchesByTeam AddTeam = new AddMultipleMatchesByTeam();
            AddTeam.ShowDialog(this);
            if (AddTeam.SeniorTeamIDTextBox.Text != string.Empty)
                if (int.TryParse(AddTeam.SeniorTeamIDTextBox.Text, out int TeamID))
                {
                    SaveResponseToFile(DownloadString.CreateMatchArchiveString(TeamID, AddTeam.FirstMatchDateDateTime.Value, AddTeam.LastMatchDateTime.Value), XMLFolder + "\\Archive.xml");
                }
                else
                {
                    MessageBoxButtons Buttons = MessageBoxButtons.OK;
                    MessageBoxIcon Icon = MessageBoxIcon.Error;
                    MessageBox.Show("The team ID can only contain numbers", "Error", Buttons, Icon);
                }
        }
    }
}
