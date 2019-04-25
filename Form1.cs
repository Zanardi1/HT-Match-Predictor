using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OAuth;

namespace HT_Match_Predictor
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            OAuth.Manager o = new OAuth.Manager();
            o["consumer_key"] = "2BkDvCeUZL1nCIVOn5KhUb";
            o["consumer_secret"] = "PvSRGYlTxCwUKuw9BH9CIWP1AqutO9MB2JRDGHsVlGC";
            OAuthResponse rt = o.AcquireRequestToken("https://chpp.hattrick.org/oauth/request_token.ashx","post");
            var url = "https://chpp.hattrick.org/oauth/authorize.aspx" + o["token"];
            System.Diagnostics.Process.Start(url);
        }

    }
}
