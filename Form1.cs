using OAuth;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

//todo sa vad unde stochez token si token_secret
//todo sa scriu o rutina de compunere a URL-ului care va fi trimis catre Hattrick pentru descarcarea fisierelor
//todo sa citesc dintr-un fisier denumirile evaluarilor (lucru util pentru momentul in care voi introduce si alte limbi pentru interfata programului

namespace HT_Match_Predictor
{
    public struct MatchRatings //retine evaluarile pe compartimente ale celor doua echipe
    {
        public int HomeM; //Home - acasa; Away - deplasare
        public int HomeRD; //R - dreapta; C = centru; L = stanga
        public int HomeCD; //D - aparare (defense); M - mijloc (midfield); A - atac (attack)
        public int HomeLD;
        public int HomeRA;
        public int HomeCA;
        public int HomeLA;
        public int AwayM;
        public int AwayRD;
        public int AwayCD;
        public int AwayLD;
        public int AwayRA;
        public int AwayCA;
        public int AwayLA;
    }

    public partial class Form1 : Form
    {
        private readonly Manager o = new Manager(); //instanta de clasa ce se ocupa de conexiunea cu serverele Hattrick
        static readonly string CurrentFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        readonly string XMLFolder = CurrentFolder + "\\XML";
        public int RatingReturned = 0; //retine reprezentarea numerica a evaluarii selectate de catre utilizator in fereastra de selectare a abilitatilor
        public MatchRatings Ratings = new MatchRatings();

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

        private void InitializeAuthenticationObject()
        {
            o["consumer_key"] = "2BkDvCeUZL1nCIVOn5KhUb";
            o["consumer_secret"] = "PvSRGYlTxCwUKuw9BH9CIWP1AqutO9MB2JRDGHsVlGC";
            o["token"] = "gWOcK5n7ZbhNAsbd"; //Stocate temporar sub aceasta forma. Va trebui sa vad cum se stocheaza corect aceste jetoane.
            o["token_secret"] = "a3GsutimIieDGlLv";
        }

        private void GetRequestToken()
        {
            OAuthResponse rt = o.AcquireRequestToken("https://chpp.hattrick.org/oauth/request_token.ashx", "GET");
            var url = "https://chpp.hattrick.org/oauth/authorize.aspx?oauth_token=" + o["token"];
            System.Diagnostics.Process.Start(url);
        }

        private void GetAccessToken()
        {
            string pin = string.Empty;
            InsertPIN I = new InsertPIN();
            I.ShowDialog(this);
            pin = I.InsertPINTextBox.Text;
            OAuthResponse at = o.AcquireAccessToken("https://chpp.hattrick.org/oauth/access_token.ashx", "GET", pin);
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
        }

        private void ShowSkillWindow(object sender, System.EventArgs e)
        {
            SkillSelectionWindow S = new SkillSelectionWindow
            {
                Tag = (sender as Button).Tag.ToString() //Tag este folosit pentru a determina ce buton a fost apasat pentru a afisa fereastra
            };
            S.ShowDialog(this);

            switch (S.Tag) //atribuie evaluarea numerica primita sectorului corespunzator, in functie de eticheta butonului a carui apasare a deschis fereastra
            {
                case "1":
                    {
                        Ratings.HomeM = RatingReturned;
                        break;
                    }
                case "2":
                    {
                        Ratings.HomeRD = RatingReturned;
                        break;
                    }
                case "3":
                    {
                        Ratings.HomeCD = RatingReturned;
                        break;
                    }
                case "4":
                    {
                        Ratings.HomeLD = RatingReturned;
                        break;
                    }
                case "5":
                    {
                        Ratings.HomeRA = RatingReturned;
                        break;
                    }
                case "6":
                    {
                        Ratings.HomeCA = RatingReturned;
                        break;
                    }
                case "7":
                    {
                        Ratings.HomeLA = RatingReturned;
                        break;
                    }
                case "8":
                    {
                        Ratings.AwayM = RatingReturned;
                        break;
                    }
                case "9":
                    {
                        Ratings.AwayRD = RatingReturned;
                        break;
                    }
                case "10":
                    {
                        Ratings.AwayCD = RatingReturned;
                        break;
                    }
                case "11":
                    {
                        Ratings.AwayLD = RatingReturned;
                        break;
                    }
                case "12":
                    {
                        Ratings.AwayRA = RatingReturned;
                        break;
                    }
                case "13":
                    {
                        Ratings.AwayCA = RatingReturned;
                        break;
                    }
                case "14":
                    {
                        Ratings.AwayLA = RatingReturned;
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
