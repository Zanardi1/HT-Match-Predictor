using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

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
        /// <summary>
        /// Lista ce contine date despre meciurile viitoare
        /// </summary>
        public static List<FutureMatches> FinalFutureMatches = new List<FutureMatches> { };

        /// <summary>
        /// Constructorul clasei
        /// </summary>
        public ParseXMLFiles()
        {

        }

        /// <summary>
        /// Aduna toate instructiunile pentru afisarea unei casute cu un mesaj intr-o singura functie.
        /// </summary>
        /// <param name="message">Mesajul ce va fi scris</param>
        private static void ShowErrorMessageBox(string message)
        {
            MessageBoxButtons Buttons = MessageBoxButtons.OK;
            MessageBoxIcon Icon = MessageBoxIcon.Error;
            MessageBox.Show(message, "Error!", Buttons, Icon);
        }

        /// <summary>
        /// Alta varianta de adunare a instructiunilor pentru afisarea unei casute cu mesaj intr-o singura functie.
        /// </summary>
        /// <param name="message">Mesajul ce va fi scris</param>
        /// <param name="errormessage">Titlul casutei</param>
        private static void ShowErrorMessageBox(string message, string errormessage)
        {
            MessageBoxIcon Icon = MessageBoxIcon.Error;
            MessageBoxButtons Button = MessageBoxButtons.OK;
            MessageBox.Show(message, errormessage, Button, Icon);
        }

        public List<int> ResetMatchRatingsList()
        {
            for (int i = 0; i < ReadMatchRatings.Count; i++)
            {
                ReadMatchRatings[i] = 0;
            }
            return ReadMatchRatings;
        }

        /// <summary>
        /// Citeste fisierul User.xml si pune datele citite in program
        /// </summary>
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
            XmlNode Node = Current.SelectSingleNode("UserId");
            UserID = Node.InnerXml;
            Node = Current.SelectSingleNode("Loginname");
            UserName = Node.InnerXml;
            Node = Current.SelectSingleNode("SupporterTier");
            UserSupporterLevel = Node.InnerXml;
            Node = Current.SelectSingleNode("Country");
            XmlNodeList Nodes = Node.SelectNodes("*[self::CountryName or self::CountryId]");
            foreach (XmlNode j in Nodes)
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
            Node = Current.SelectSingleNode("Teams");
            int counter = 0;
            Nodes = Node.SelectNodes("*");
            foreach (XmlNode j in Nodes)
            {
                XmlNodeList TeamDetails = j.SelectNodes("*[self::TeamId or self::TeamName]");
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
                                    ShowErrorMessageBox("Parsing TeamID from XML file failed!");
                                }
                                break;
                            }
                    }
                }
                counter++;
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
            XmlNodeList MatchListNodeList = MatchListNode.SelectNodes("Match");
            foreach (XmlNode i in MatchListNodeList)
            {
                XmlNodeList MatchNodeList = i.SelectNodes("*[self::MatchID or self::HomeTeam or self::AwayTeam or self::MatchType or self::Status]");
                foreach (XmlNode j in MatchNodeList)
                {
                    switch (j.Name)
                    {
                        case "MatchID":
                            {
                                if (!int.TryParse(j.InnerXml, out F.MatchID))
                                {
                                    ShowErrorMessageBox("Parsing the Match ID from the XML file failed!", "Error!");
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
                                    ShowErrorMessageBox("Parsing Match Type from XML file failed!", "Error!");
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
        /// <returns>-2, daca fisierul nu are un nod radacina, -1, daca meciul nu meci de liga, amical tip normal sau amical international, tip normal; 0 daca totul e in regula</returns>
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
            try
            {
                doc.Load(Reader);
            }
            catch (XmlException X)
            {
                if (string.Equals(X.Message, "Root element is missing.", System.StringComparison.CurrentCulture))
                {
                    Reader.Close();
                    return -2;
                }
                else
                    throw;
            }

            XmlNode Current = doc.DocumentElement.SelectSingleNode("Match");
            //Din cand in cand mai sunt probleme cu API-ul Hattrick, iar fisierul intoarce un mesaj de eroare. Din acest motiv, Current e null. Codul de mai jos trateaza acest caz
            if (Current == null)
            {
                Reader.Close();
                return -1;
            }
            XmlNodeList MatchNodeList = Current.SelectNodes("*[self::MatchType or self::HomeTeam or self::AwayTeam]");
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
                                        ShowErrorMessageBox("Only league, friendly (normal rules) and international friendly (normal rules) matches can be added into the database. The match with the chosen ID was not added.", "Match type error!");
                                    }
                                    Reader.Close();
                                    return -1;
                                }
                            }
                            break;
                        }
                    case "HomeTeam":
                        {
                            XmlNodeList HomeTeamNodeList = i.SelectNodes("*[self::RatingMidfield or self::RatingRightDef or self::RatingMidDef or self::RatingLeftDef or self::RatingRightAtt or self::RatingMidAtt or self::RatingLeftAtt or self::HomeGoals]");
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
                                                ShowErrorMessageBox("Parsing the home midfield rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the home right defense rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the home central defense rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the home left defense rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the home right attack rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the home central attack rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the home left defense attack from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the home goals field from the XML file failed!");
                                            }
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case "AwayTeam":
                        {
                            XmlNodeList HomeTeamNodeList = i.SelectNodes("*[self::RatingMidfield or self::RatingRightDef or self::RatingMidDef or self::RatingLeftDef or self::RatingRightAtt or self::RatingMidAtt or self::RatingLeftAtt or self::AwayGoals]");
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
                                                ShowErrorMessageBox("Parsing the away midfield rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the away right defense rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the away central defense rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the away left defense rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the away right defense rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the away right attack rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the away left attack rating from the XML file failed!");
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
                                                ShowErrorMessageBox("Parsing the away goals field rating from the XML file failed!");
                                            }
                                            break;
                                        }
                                }
                            }
                            break;
                        }

                    default:
                        break;
                }
            }
            Reader.Close();
            return 0;
        }

        public bool ParseOrdersFile()
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

            XmlNode Current = doc.DocumentElement.SelectSingleNode("Error"); //Din motive necunoscute mie, unele XML-uri vin cu mesaje de eroare, desi toti parametrii sunt transmisi corect. Aici se testeaza accest caz. Daca nu e niciun mesaj de eroare, Current va fi null si functia trece mai departe. Altfel, afiseaza un mesaj si iese.
            if (Current != null)
            {
                ShowErrorMessageBox("The predicted ratings for this match could not be loaded, because of a Hattrick server fault. " + Current.InnerXml);
                Reader.Close();
                return false;
            }

            Current = doc.DocumentElement.SelectSingleNode("MatchData");
            XmlNodeList MatchDataNodeList = Current.SelectNodes("*[self::RatingMidfield or self::RatingRightDef or self::RatingMidDef or self::RatingLeftDef or self::RatingRightAtt or self::RatingMidAtt or self::RatingLeftAtt]");
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
                                ShowErrorMessageBox("Parsing the predicted midfield rating from the XML file failed!");
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
                                ShowErrorMessageBox("Parsing the predicted right defense rating from the XML file failed!");
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
                                ShowErrorMessageBox("Parsing the predicted central defense rating from the XML file failed!");
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
                                ShowErrorMessageBox("Parsing the predicted left defense rating from the XML file failed!");
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
                                ShowErrorMessageBox("Parsing the predicted right attack rating from the XML file failed!");
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
                                ShowErrorMessageBox("Parsing the predicted central attack rating from the XML file failed!");
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
                                ShowErrorMessageBox("Parsing the predicted left attack rating from the XML file failed!");
                            }
                            break;
                        }
                    default: break;
                }
            }
            Reader.Close();
            return true;
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
            XmlNode TeamIDNode = Current.SelectSingleNode("MatchList");
            XmlNodeList MatchNodeList = TeamIDNode.SelectNodes("Match");
            foreach (XmlNode j in MatchNodeList)
            {
                switch (j.Name)
                {
                    case "Match":
                        {
                            XmlNode MatchIDNode = j.SelectSingleNode("MatchID");
                            if (int.TryParse(MatchIDNode.InnerXml, out int temp))
                            {
                                MatchIDList.Add(temp);
                            }
                            break;
                        }
                }
            }
            Reader.Close();
            return MatchIDList;
        }
    }
}
