using System.Windows.Forms;
using System.Net;
using OAuth;

namespace HT_Match_Predictor
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            Manager o = new Manager();
            o["consumer_key"] = "2BkDvCeUZL1nCIVOn5KhUb";
            o["consumer_secret"] = "PvSRGYlTxCwUKuw9BH9CIWP1AqutO9MB2JRDGHsVlGC";
            //o["token"] = "NGcEpaQg2I4QYLWy";
            //o["token_secret"] = "riboptAJpTEUadcr";
            OAuthResponse rt = o.AcquireRequestToken("https://chpp.hattrick.org/oauth/request_token.ashx", "GET");
            var url = "https://chpp.hattrick.org/oauth/authorize.aspx?oauth_token=" + o["token"];
            System.Diagnostics.Process.Start(url);
            string pin = string.Empty;
            OAuthResponse at = o.AcquireAccessToken("https://chpp.hattrick.org/oauth/access_token.ashx", "GET", pin);
            var search = "http://chpp.hattrick.org/chppxml.ashx?file=fans&version=1.3";
            var authzHeader = o.GenerateAuthzHeader(search, "GET");
            var request = (HttpWebRequest)WebRequest.Create(search);
            request.Method = "GET";
            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = true;
            request.Headers.Add("Authorization", authzHeader);
            var response = (HttpWebResponse)request.GetResponse();
            MessageBox.Show(response.StatusCode.ToString());
            System.Diagnostics.Process.Start(search);
        }

    }
}
