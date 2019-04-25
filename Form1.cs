using System.Windows.Forms;
using OAuth;

namespace HT_Match_Predictor
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            OAuth.Manager o = new OAuth.Manager("2BkDvCeUZL1nCIVOn5KhUb","PvSRGYlTxCwUKuw9BH9CIWP1AqutO9MB2JRDGHsVlGC");
            OAuthResponse rt = o.AcquireRequestToken("https://chpp.hattrick.org/oauth/request_token.ashx", "GET");
            var url = "https://chpp.hattrick.org/oauth/authorize.aspx";
            System.Diagnostics.Process.Start(url);
            string pin = string.Empty;
            OAuthResponse at = o.AcquireAccessToken("https://chpp.hattrick.org/oauth/access_token.ashx", "GET", pin);
        }

    }
}
