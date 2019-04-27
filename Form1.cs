using OAuth;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

//todo sa vad unde stochez token si token_secret
//todo sa scriu o rutina de compunere a URL-ului care va fi trimis catre Hattrick pentru descarcare

namespace HT_Match_Predictor
{
    public partial class Form1 : Form
    {
        private readonly Manager o = new Manager();
        static readonly string CurrentFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        readonly string XMLFolder = CurrentFolder + "\\XML";

        private void LoginToHattrickServers()
        {
            InitializeAuthenticationObject();
            if (o["token"] == string.Empty)
            {
                GetRequestToken();
                GetAccessToken();
            }
            SaveResponseToFile(XMLFolder + "\\a.xml", "http://chpp.hattrick.org/chppxml.ashx?file=matches&version=2.8&teamID=1629472");
        }

        private void InitializeAuthenticationObject()
        {
            o["consumer_key"] = "2BkDvCeUZL1nCIVOn5KhUb";
            o["consumer_secret"] = "PvSRGYlTxCwUKuw9BH9CIWP1AqutO9MB2JRDGHsVlGC";
            //o["token"] = "gWOcK5n7ZbhNAsbd";
            //o["token_secret"] = "a3GsutimIieDGlLv";
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
            //todo sa pun o fereastra in care sa pot introduce pin-ul primit de la Hattrick
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
    }
}
