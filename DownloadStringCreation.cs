using System;

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
        static private readonly string BaseURL = "http://chpp.hattrick.org/chppxml.ashx";
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
        public string FinalString = BaseURL + DownloadedFile + FileParameters;

        public DownloadStringCreation()
        {

        }

        private string ComposeFinalString()
        {
            FinalString = BaseURL + DownloadedFile + FileParameters;
            return FinalString;
        }

        public string CreateManagerCompendiumString()
        {
            DownloadedFile = "?file=managercompendium&version=1.2";
            return ComposeFinalString();
        }

        public string CreateMatchesString(int TeamID)
        {
            string[] pieces = { "&version=2.8&teamID=", TeamID.ToString() }; //retine parametrii
            DownloadedFile = "?file=matches";
            FileParameters = String.Concat(pieces);
            return ComposeFinalString();
        }

        public string CreateMatchDetailsString(int MatchID)
        {
            string[] pieces = { "&version=3.0&matchEvents=false&matchID=", MatchID.ToString(), "&sourceSystem=hattrick" };
            DownloadedFile = "?file=matchdetails";
            FileParameters = String.Concat(pieces);
            return ComposeFinalString();
        }

        public string CreateMatchOrdersString(int MatchID)
        {
            string[] pieces = { "&version=3.0&actionType=predictratings&matchID=", MatchID.ToString() };
            DownloadedFile = "?file=matchorders";
            FileParameters = String.Concat(pieces);
            return ComposeFinalString();
        }

        public string CreateMatchArchiveString(int TeamID, int Season)
        {
            string[] pieces = { "&version=1.4&teamID=", TeamID.ToString(), "&isYouth=false&season=", Season.ToString() };
            DownloadedFile = "?file=matchesarchive";
            FileParameters = string.Concat(pieces);
            return ComposeFinalString();
        }
    }
}
