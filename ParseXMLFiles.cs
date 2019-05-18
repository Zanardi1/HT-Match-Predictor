using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

//todo sa vad daca pot scrie mai putine linii de cod pentru a face acelasi lucru cu procesarea fisierelor XML

/// <summary>
/// Aceasta clasa se ocupa de urmatoarele lucruri:
/// 1. Citirea unui fisier XML;
/// 2. Prelucrarea acestuia si extragerea informatiilor utile;
/// 3. Stocarea acelor informatii in variabile.
/// Deoarece fisierele XML au continuturi diferite, in functie de informatiile necesare, deci se stocheaza si informatii diferite, fiecare tip de fisier XML va primi metoda proprie.
/// </summary>

namespace HTMatchPredictor
{
    class ParseXMLFiles
    {
        /// <summary>
        /// Retine numarul de identificare al utilizatorului
        /// </summary>
        public string UserID = string.Empty;
        /// <summary>
        /// Retine numele de utilizator
        /// </summary>
        public string UserName = string.Empty;
        /// <summary>
        /// Retine numarul de identificare al tarii de unde provine utilizatorul
        /// </summary>
        public string UserCountryID = string.Empty;
        /// <summary>
        /// Retine tara de unde provine utilizatorul
        /// </summary>
        public string UserCountry = string.Empty;
        /// <summary>
        /// Retine ce fel de supporter e utilizatorul
        /// </summary>
        public string UserSupporterLevel = string.Empty;
        /// <summary>
        /// Retine numele echipelor pe care le are utilizatorul
        /// </summary>
        public string[] UserTeamNames = new string[3];
        /// <summary>
        /// Retine numerele de identificare ale echipelor pe care le are utilizatorul
        /// </summary>
        public int[] UserTeamIDs = new int[3];
        /// <summary>
        /// Retine tipul de meci citit
        /// </summary>
        public int ReadMatchType = 0;
        /// <summary>
        /// Retine cele 16 evaluari ale meciului. Semnificatia numerelor de ordine din lista este urmatoarea:
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
        /// 14. Numarul de goluri inscrise de echipa de acasa;
        /// 15. Numarul de goluri inscrise de echipa din deplasare
        /// </summary>
        public List<int> ReadMatchRatings = new List<int>(16) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// Structura retine numarul de identificare al unui meci (MatchID), precum si cele doua echipe care l-au jucat (HomeTeam si AwayTeam).
        /// </summary>
        public struct FutureMatches
        {
            public int MatchID;
            public string HomeTeam, AwayTeam;
        }

        /// <summary>
        /// Instanta a structurii
        /// </summary>
        private FutureMatches F;
        public static List<FutureMatches> FinalFutureMatches = new List<FutureMatches> { };

        public ParseXMLFiles()
        {

        }

        public List<int> ResetMatchRatingsList()
        {
            for (int i = 0; i < ReadMatchRatings.Count; i++)
            {
                ReadMatchRatings[i] = 0;
            }
            return ReadMatchRatings;
        }

        public void ParseUserFile()
        {
            XmlUrlResolver URLResolver = new XmlUrlResolver
            {
                Credentials = System.Net.CredentialCache.DefaultCredentials
            };
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                IgnoreComments = true,
                CheckCharacters = true,
                XmlResolver = URLResolver
            };
            XmlReader Reader = XmlReader.Create(Form1.XMLFolder + "\\User.xml", settings);
            XmlDocument doc = new XmlDocument
            {
                XmlResolver = null
            };
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
                                                if (!int.TryParse(k.InnerXml, out UserTeamIDs[counter]))
                                                {
                                                    MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                    MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                    MessageBox.Show("Parsing TeamID from XML file failed!", "Error!", Buttons, Icon);
                                                }
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
            Reader.Close();
        }

        public void ParseMatchesFile()
        {
            int TempMatchType = 0;
            XmlUrlResolver Resolver = new XmlUrlResolver()
            {
                Credentials = System.Net.CredentialCache.DefaultCredentials
            };
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                IgnoreComments = true,
                CheckCharacters = true,
                XmlResolver = Resolver
            };
            XmlReader Reader = XmlReader.Create(Form1.XMLFolder + "\\Matches.xml", settings);
            XmlDocument doc = new XmlDocument()
            {
                XmlResolver = null
            };
            doc.Load(Reader);

            XmlNode Current = doc.DocumentElement.SelectSingleNode("Team");
            XmlNode MatchListNode = Current.SelectSingleNode("MatchList");
            XmlNodeList MatchListNodeList = MatchListNode.SelectNodes("*");
            foreach (XmlNode i in MatchListNodeList)
            {
                XmlNodeList MatchNodeList = i.SelectNodes("*");
                foreach (XmlNode j in MatchNodeList)
                {
                    switch (j.Name)
                    {
                        case "MatchID":
                            {
                                if (!int.TryParse(j.InnerXml, out F.MatchID))
                                {
                                    MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                    MessageBoxIcon Icon = MessageBoxIcon.Error;
                                    MessageBox.Show("Parsing the Matc ID from the XML file failed!", "Error!", Buttons, Icon);
                                }
                                break;
                            }
                        case "HomeTeam":
                            {
                                XmlNodeList HomeTeamNodeList = j.SelectNodes("*");
                                foreach (XmlNode k in HomeTeamNodeList)
                                {
                                    if (k.Name == "HomeTeamName")
                                    {
                                        F.HomeTeam = k.InnerXml;
                                    }
                                }
                                break;
                            }
                        case "AwayTeam":
                            {
                                XmlNodeList AwayTeamNodeList = j.SelectNodes("*");
                                foreach (XmlNode k in AwayTeamNodeList)
                                {
                                    if (k.Name == "AwayTeamName")
                                    {
                                        F.AwayTeam = k.InnerXml;
                                    }
                                }
                                break;
                            }
                        case "MatchType":
                            {
                                if (!int.TryParse(j.InnerXml, out TempMatchType))
                                {
                                    MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                    MessageBoxIcon Icon = MessageBoxIcon.Error;
                                    MessageBox.Show("Parsing Match Type from XML file failed!", "Error!", Buttons, Icon);
                                }
                                break;
                            }
                        case "Status":
                            {
                                if ((j.InnerXml == "UPCOMING") && ((TempMatchType == 1) || (TempMatchType == 4) || (TempMatchType == 8)))
                                {
                                    FinalFutureMatches.Add(F);
                                }
                                break;
                            }
                    }
                }
            }
            Reader.Close();
        }

        /// <summary>
        /// Interpreteaza fisierul MatchDetails.xml
        /// </summary>
        /// <returns>-1, daca meciul nu meci de liga, amical tip normal sau amical international, tip normal; 0 daca totul e in regula</returns>
        public int ParseMatchDetailsFile(bool ShowErrorMessage)
        {
            int temp; //utilizata deoarece TryParse nu accepta ca variabila de iesire un element dintr-o lista, ci o variabila simpla
            XmlUrlResolver Resolver = new XmlUrlResolver()
            {
                Credentials = System.Net.CredentialCache.DefaultCredentials
            };
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                IgnoreComments = true,
                CheckCharacters = true,
                XmlResolver = Resolver
            };
            XmlReader Reader = XmlReader.Create(Form1.XMLFolder + "\\MatchDetails.xml", settings);
            XmlDocument doc = new XmlDocument()
            {
                XmlResolver = null
            };
            doc.Load(Reader);

            XmlNode Current = doc.DocumentElement.SelectSingleNode("Match");
            XmlNodeList MatchNodeList = Current.SelectNodes("*");
            foreach (XmlNode i in MatchNodeList)
            {
                switch (i.Name)
                {
                    case "MatchType":
                        {
                            if (int.TryParse(i.InnerXml, out ReadMatchType)) //citeste tipul meciului
                            {
                                if ((ReadMatchType != 1) && (ReadMatchType != 4) && (ReadMatchType != 8))
                                {
                                    if (ShowErrorMessage)
                                    {
                                        MessageBoxIcon Icon = MessageBoxIcon.Error;
                                        MessageBoxButtons Button = MessageBoxButtons.OK;
                                        MessageBox.Show("Only league, friendly (normal rules) and international friendly (normal rules) matches can be added into the database. The match with the chosen ID was not added.", "Match type error!", Button, Icon);
                                    }
                                    Reader.Close();
                                    return -1;
                                }
                            }
                            break;
                        }
                    case "HomeTeam":
                        {
                            XmlNodeList HomeTeamNodeList = i.SelectNodes("*");
                            foreach (XmlNode j in HomeTeamNodeList)
                            {
                                switch (j.Name)
                                {
                                    case "RatingMidfield":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[0] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the home midfield rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "RatingRightDef":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[1] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the home right defense rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "RatingMidDef":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[2] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the home central defense rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "RatingLeftDef":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[3] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the home left defense rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "RatingRightAtt":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[4] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the home right attack rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "RatingMidAtt":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[5] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the home central attack rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "RatingLeftAtt":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[6] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the home left defense attack from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "HomeGoals":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[14] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the home goals field from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case "AwayTeam":
                        {
                            XmlNodeList HomeTeamNodeList = i.SelectNodes("*");
                            foreach (XmlNode j in HomeTeamNodeList)
                            {
                                switch (j.Name)
                                {
                                    case "RatingMidfield":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[7] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the away midfield rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "RatingRightDef":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[8] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the away right defense rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "RatingMidDef":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[9] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the away central defense rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "RatingLeftDef":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[10] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the away left defense rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "RatingRightAtt":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[11] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the away right defense rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "RatingMidAtt":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[12] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the away right attack rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "RatingLeftAtt":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[13] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the away left attack rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                    case "AwayGoals":
                                        {
                                            if (int.TryParse(j.InnerXml, out temp))
                                            {
                                                ReadMatchRatings[15] = temp;
                                            }
                                            else
                                            {
                                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                                MessageBox.Show("Parsing the away goals field rating from the XML file failed!", "Error!", Buttons, Icon);
                                            }
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                }
            }
            Reader.Close();
            return 0;
        }

        public void ParseOrdersFile()
        {
            int temp;
            XmlUrlResolver Resolver = new XmlUrlResolver()
            {
                Credentials = System.Net.CredentialCache.DefaultCredentials
            };
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                IgnoreComments = true,
                CheckCharacters = true,
                XmlResolver = Resolver
            };
            XmlReader Reader = XmlReader.Create(Form1.XMLFolder + "\\Orders.xml", settings);
            XmlDocument doc = new XmlDocument()
            {
                XmlResolver = null
            };
            doc.Load(Reader);

            XmlNode Current = doc.DocumentElement.SelectSingleNode("MatchData");
            XmlNodeList MatchDataNodeList = Current.SelectNodes("*");
            foreach (XmlNode i in MatchDataNodeList)
            {
                switch (i.Name)
                {
                    case "RatingMidfield":
                        {
                            if (int.TryParse(i.InnerXml, out temp))
                            {
                                ReadMatchRatings[0] = temp;
                            }
                            else
                            {
                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                MessageBox.Show("Parsing the predicted midfield rating from the XML file failed!", "Error!", Buttons, Icon);
                            }
                            break;
                        }
                    case "RatingRightDef":
                        {
                            if (int.TryParse(i.InnerXml, out temp))
                            {
                                ReadMatchRatings[1] = temp;
                            }
                            else
                            {
                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                MessageBox.Show("Parsing the predicted right defense rating from the XML file failed!", "Error!", Buttons, Icon);
                            }
                            break;
                        }
                    case "RatingMidDef":
                        {
                            if (int.TryParse(i.InnerXml, out temp))
                            {
                                ReadMatchRatings[2] = temp;
                            }
                            else
                            {
                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                MessageBox.Show("Parsing the predicted central defense rating from the XML file failed!", "Error!", Buttons, Icon);
                            }
                            break;
                        }
                    case "RatingLeftDef":
                        {
                            if (int.TryParse(i.InnerXml, out temp))
                            {
                                ReadMatchRatings[3] = temp;
                            }
                            else
                            {
                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                MessageBox.Show("Parsing the predicted left defense rating from the XML file failed!", "Error!", Buttons, Icon);
                            }
                            break;
                        }
                    case "RatingRightAtt":
                        {
                            if (int.TryParse(i.InnerXml, out temp))
                            {
                                ReadMatchRatings[4] = temp;
                            }
                            else
                            {
                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                MessageBox.Show("Parsing the predicted right attack rating from the XML file failed!", "Error!", Buttons, Icon);
                            }
                            break;
                        }
                    case "RatingMidAtt":
                        {
                            if (int.TryParse(i.InnerXml, out temp))
                            {
                                ReadMatchRatings[5] = temp;
                            }
                            else
                            {
                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                MessageBox.Show("Parsing the predicted central attack rating from the XML file failed!", "Error!", Buttons, Icon);
                            }
                            break;
                        }
                    case "RatingLeftAtt":
                        {
                            if (int.TryParse(i.InnerXml, out temp))
                            {
                                ReadMatchRatings[6] = temp;
                            }
                            else
                            {
                                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                MessageBox.Show("Parsing the predicted left attack rating from the XML file failed!", "Error!", Buttons, Icon);
                            }
                            break;
                        }
                    default: break;
                }
            }
            Reader.Close();
        }

        /// <summary>
        /// Prelucreaza fisierul arhiva, Archive.xml
        /// </summary>
        /// <returns>O lista cu numerele de identificare ale meciurilor din arhiva</returns>
        public List<int> ParseArchiveFile()
        {
            List<int> MatchIDList = new List<int> { };
            XmlUrlResolver Resolver = new XmlUrlResolver()
            {
                Credentials = System.Net.CredentialCache.DefaultCredentials
            };
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                IgnoreComments = true,
                CheckCharacters = true,
                XmlResolver = Resolver
            };
            XmlReader Reader = XmlReader.Create(Form1.XMLFolder + "\\Archive.xml", settings);
            XmlDocument doc = new XmlDocument()
            {
                XmlResolver = null
            };
            doc.Load(Reader);

            XmlNode Current = doc.DocumentElement.SelectSingleNode("Team");
            XmlNodeList TeamIDNodeList = Current.SelectNodes("MatchList");
            foreach (XmlNode i in TeamIDNodeList)
            {
                switch (i.Name)
                {
                    case "MatchList":
                        {
                            XmlNodeList MatchNodeList = i.SelectNodes("Match");
                            foreach (XmlNode j in MatchNodeList)
                            {
                                switch (j.Name)
                                {
                                    case "Match":
                                        {
                                            XmlNodeList MatchIDNodeList = j.SelectNodes("MatchID");
                                            foreach (XmlNode k in MatchIDNodeList)
                                            {
                                                switch (k.Name)
                                                {
                                                    case "MatchID":
                                                        {
                                                            if (int.TryParse(k.InnerXml, out int temp))
                                                            {
                                                                MatchIDList.Add(temp);
                                                            }

                                                            break;
                                                        }
                                                }
                                            }

                                            break;
                                        }
                                }
                            }
                            break;
                        }
                }
            }
            return MatchIDList;
        }
    }
}
