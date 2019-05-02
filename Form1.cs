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
        private readonly Manager o = new Manager();
        /// <summary>
        /// Instanta de clasa ce se ocupa de conexiunea cu serverele Hattrick
        /// </summary>
        private readonly DatabaseOperations Operations = new DatabaseOperations();
        /// <summary>
        /// Obiect ce se ocupa de operatiile cu BD a programului
        /// </summary>
        static readonly string CurrentFolder = Path.GetDirectoryName(Application.ExecutablePath);
        /// <summary>
        /// Retin folderul in care se afla aplicatia
        /// </summary>
        readonly string XMLFolder = CurrentFolder + "\\XML";
        /// <summary>
        /// Retin folderul unde vor fi descarcate fisierele XML
        /// </summary>
        public int RatingReturned = 0;
        /// <summary>
        /// retine reprezentarea numerica a evaluarii selectate de catre utilizator in fereastra de selectare a abilitatilor. E un numar intre 1 si 80
        /// </summary>
        public List<int> MatchRatings = new List<int>(14);
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
            SaveResponseToFile(XMLFolder + "\\a.xml", "http://chpp.hattrick.org/chppxml.ashx?file=matchdetails&version=3.0&matchEvents=false&matchID=638295042");
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

        public void SaveResponseToFile(string DestinationFileName, string SourceURLAddress)
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

        public Form1()
        {
            InitializeComponent();
            LoginToHattrickServers();
            InitializeMatchRatingList();
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

        private void ShowAboutWindow(object sender, System.EventArgs e)
        {
            AboutBox A = new AboutBox();
            A.ShowDialog();
        }

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

        private void DeleteMatchesDatabase(object sender, System.EventArgs e)
        {
            if (Operations.DatabaseExists())
            {
                HelpStatusLabel.Text = "Deleting matches database...";
                Cursor = Cursors.WaitCursor;
                Operations.DeleteDatabase();
                Cursor = Cursors.Default;
                HelpStatusLabel.Text = "Matches database deleted.";
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
    }
}
