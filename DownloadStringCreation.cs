using System.Globalization;
using System.Text;

/// <summary>
/// Aceasta clasa se ocupa de generarea sirului care va fi trimis serverului Hattrick pentru descarcarea datelor si salvarea acestora in format XML. Sirul este compus din trei parti:
/// 1. Adresa URL de unde vor fi obtinute datele. Aceasta e fixa: http://chpp.hattrick.org/chppxml.ashx ;
/// 2. Fisierul care va fi descarcat;
/// 3. Datele de intrare utile acelui fisier.
/// URL-ul este separat de fisier prin "?". Parametrii au forma "nume=valoare" si sunt separati intre ei prin "&";
/// </summary>

namespace HTMatchPredictor
{
    class DownloadStringCreation
    {
        /// <summary>
        /// Retine adresa URL de unde vor fi obtinute datele
        /// </summary>
        private const string BaseURL = "http://chpp.hattrick.org/chppxml.ashx";
        /// <summary>
        /// Retine fisierul ce va fi descarcat
        /// </summary>
        static private string DownloadedFile = "";
        /// <summary>
        /// Retine datele de intrare pentru acel fisier
        /// </summary>
        static private string FileParameters = "";
        /// <summary>
        /// Retine sirul final care va fi folosit la descarcarea datelor necesare
        /// </summary>
        public string FinalString = ComposeFinalString();

        public DownloadStringCreation()
        {

        }

        private static string ComposeFinalString()
        {
            StringBuilder FinalString = new StringBuilder();
            FinalString.Append(BaseURL);
            FinalString.Append(DownloadedFile);
            FinalString.Append(FileParameters);
            return FinalString.ToString();
        }

        public static string CreateManagerCompendiumString()
        {
            DownloadedFile = "?file=managercompendium&version=1.2";
            return ComposeFinalString();
        }

        public string CreateMatchesString(int TeamID)
        {
            StringBuilder MatchesString = new StringBuilder();
            MatchesString.Append("&version=2.8&teamID=");
            MatchesString.Append(TeamID.ToString(CultureInfo.InvariantCulture));
            DownloadedFile = "?file=matches";
            FileParameters = MatchesString.ToString();
            return ComposeFinalString();
        }

        public string CreateMatchDetailsString(int MatchID)
        {
            StringBuilder Details = new StringBuilder();
            Details.Append("&version=3.0&matchEvents=false&matchID=");
            Details.Append(MatchID.ToString(CultureInfo.InvariantCulture));
            Details.Append("&sourceSystem=hattrick");
            DownloadedFile = "?file=matchdetails";
            FileParameters = Details.ToString();
            return ComposeFinalString();
        }

        public string CreateMatchOrdersString(int MatchID, int TeamID)
        {
            StringBuilder Orders = new StringBuilder();
            Orders.Append("&version=3.0&actionType=predictratings&matchID=");
            Orders.Append(MatchID.ToString(CultureInfo.InvariantCulture));
            Orders.Append("&teamId=");
            Orders.Append(TeamID.ToString(CultureInfo.InvariantCulture));
            DownloadedFile = "?file=matchorders";
            FileParameters = Orders.ToString();
            return ComposeFinalString();
        }

        public string CreateMatchArchiveString(int TeamID, int Season)
        {
            StringBuilder Archive = new StringBuilder();
            Archive.Append("&version=1.4&teamID=");
            Archive.Append(TeamID.ToString(CultureInfo.InvariantCulture));
            Archive.Append("&isYouth=false&season=");
            Archive.Append(Season.ToString(CultureInfo.InvariantCulture));
            DownloadedFile = "?file=matchesarchive";
            FileParameters = Archive.ToString();
            return ComposeFinalString();
        }
    }
}
