using System.Xml;
using System.Windows.Forms;

/// <summary>
/// Aceasta clasa se ocupa de urmatoarele lucruri:
/// 1. Citirea unui fisier XML;
/// 2. Prelucrarea acestuia si extragerea informatiilor utile;
/// 3. Stocarea acelor informatii in variabile.
/// Deoarece fisierele XML au continuturi diferite, in functie de informatiile necesare, deci se stocheaza si informatii diferite, fiecare tip de fisier XML va primi metoda proprie.
/// </summary>

namespace HT_Match_Predictor
{
    class ParseXMLFiles
    {
        public string UserID = string.Empty;
        public string UserName = string.Empty;
        public string UserCountryID = string.Empty;
        public string UserCountry = string.Empty;
        public string UserSupporterLevel = string.Empty;
        public string[] UserTeamNames = new string[3];
        public int[] UserTeamIDs = new int[3];
        public ParseXMLFiles()
        {

        }

        public void ParseUserFile()
        {
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                IgnoreComments = true,
                CheckCharacters = true
            };
            XmlReader Reader = XmlReader.Create(Form1.XMLFolder + "\\User.xml", settings);
            XmlDocument doc = new XmlDocument();
            doc.Load(Reader);

            XmlNode Current = doc.DocumentElement.SelectSingleNode("Manager"); //Trec la nodul "Manager
            XmlNodeList NodeList = Current.SelectNodes("*"); //Obtin nodurile-copil
            foreach (XmlNode i in NodeList)
            {
                switch (i.Name)
                {
                    case "UserId":
                        {
                            UserID = i.InnerXml;
                            break;
                        }
                    case "Loginname":
                        {
                            UserName = i.InnerXml;
                            break;
                        }
                    case "SupporterTier":
                        {
                            UserSupporterLevel = i.InnerXml;
                            break;
                        }
                    case "Country":
                        {
                            XmlNodeList CountryNodes = i.SelectNodes("*");
                            foreach (XmlNode j in CountryNodes)
                            {
                                switch (j.Name)
                                {
                                    case "CountryName":
                                        {
                                            UserCountry = j.InnerXml;
                                            break;
                                        }
                                    case "CountryId":
                                        {
                                            UserCountryID = j.InnerXml;
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case "Teams":
                        {
                            int counter = 0;
                            XmlNodeList TeamNodes = i.SelectNodes("*");
                            foreach (XmlNode j in TeamNodes)
                            {
                                XmlNodeList TeamDetails = j.SelectNodes("*");
                                foreach (XmlNode k in TeamDetails)
                                {
                                    switch (k.Name)
                                    {
                                        case "TeamName":
                                            {
                                                UserTeamNames[counter] = k.InnerXml;
                                                break;
                                            }
                                        case "TeamId":
                                            {
                                                int.TryParse(k.InnerXml, out UserTeamIDs[counter]);
                                                break;
                                            }
                                    }
                                }
                                counter++;
                            }
                            break;
                        }
                }
            }
        }

        public void ParseMatchesFile()
        {

        }

        public void ParseMatchDetailsFile()
        {

        }

        public void ParseOrdersFile()
        {

        }
    }
}
