using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text;
using OAuth;

//todo sa vad unde stochez token si token_secret

namespace HT_Match_Predictor
{
    public partial class Form1 : Form
    {
        Manager o = new Manager();

        private void LoginToHattrickServers()
        {
            InitializeAuthenticationObject();
            if (o["token"]==string.Empty)
            {
                GetRequestToken();
                InsertPin();
            }
            GetFileContent("http://chpp.hattrick.org/chppxml.ashx?file=fans&version=1.3&teamId=1187457");
        }

        private void InitializeAuthenticationObject()
        {
            o["consumer_key"] = "2BkDvCeUZL1nCIVOn5KhUb";
            o["consumer_secret"] = "PvSRGYlTxCwUKuw9BH9CIWP1AqutO9MB2JRDGHsVlGC";
            o["token"] = "gWOcK5n7ZbhNAsbd";
            o["token_secret"] = "a3GsutimIieDGlLv";
        }

        private void GetRequestToken()
        {
            OAuthResponse rt = o.AcquireRequestToken("https://chpp.hattrick.org/oauth/request_token.ashx", "GET");
            var url = "https://chpp.hattrick.org/oauth/authorize.aspx?oauth_token=" + o["token"];
            System.Diagnostics.Process.Start(url);
        }

        private void InsertPin()
        {
            string pin = string.Empty;
            OAuthResponse at = o.AcquireAccessToken("https://chpp.hattrick.org/oauth/access_token.ashx", "GET", pin);
        }

        public string GetFileContent(string FileName)
        {
            //string search = "http://chpp.hattrick.org/chppxml.ashx?file=fans&version=1.3&teamId=1187457";
            string search = FileName;
            string authzHeader = o.GenerateAuthzHeader(search, "GET");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(search);
            request.Method = "GET";
            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = true;
            request.Headers.Add("Authorization", authzHeader);
            request.ServicePoint.Expect100Continue = false;
            request.ContentType = "x-www-form-urlencoded";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receive = response.GetResponseStream();
            StreamReader s = new StreamReader(receive, Encoding.UTF8);
            MessageBox.Show(s.ReadToEnd());
            return s.ReadToEnd();
        }

        public Form1()
        {
            InitializeComponent();
            LoginToHattrickServers();


        }
    }
}
