﻿using System.Collections.Generic;
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
            int temp; //utilizata deoarece TryParse nu accepta ca variabila de iesire un element dintr-o lista, ci o variabila simpla
            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                IgnoreComments = true,
                CheckCharacters = true
            };
            XmlReader Reader = XmlReader.Create(Form1.XMLFolder + "\\MatchDetails.xml", settings);
            XmlDocument doc = new XmlDocument();
            doc.Load(Reader);

            XmlNode Current = doc.DocumentElement.SelectSingleNode("Match");
            XmlNodeList MatchNodeList = Current.SelectNodes("*");
            foreach (XmlNode i in MatchNodeList)
            {
                switch (i.Name)
                {
                    case "MatchType":
                        {
                            int.TryParse(i.InnerXml, out ReadMatchType); //citeste tipul meciului
                            if ((ReadMatchType != 1) && (ReadMatchType != 4) && (ReadMatchType != 8))
                            {
                                MessageBoxIcon Icon = MessageBoxIcon.Error;
                                MessageBoxButtons Button = MessageBoxButtons.OK;
                                MessageBox.Show("Only league, friendly (normal rules) and international friendly (normal rules) matches can be added into the database. The match with the chosen ID was not added.", "Match type error!", Button, Icon);
                                return;
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
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[0] = temp;
                                            break;
                                        }
                                    case "RatingRightDef":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[1] = temp;
                                            break;
                                        }
                                    case "RatingMidDef":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[2] = temp;
                                            break;
                                        }
                                    case "RatingLeftDef":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[3] = temp;
                                            break;
                                        }
                                    case "RatingRightAtt":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[4] = temp;
                                            break;
                                        }
                                    case "RatingMidAtt":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[5] = temp;
                                            break;
                                        }
                                    case "RatingLeftAtt":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[6] = temp;
                                            break;
                                        }
                                    case "HomeGoals":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[14] = temp;
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
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[7] = temp;
                                            break;
                                        }
                                    case "RatingRightDef":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[8] = temp;
                                            break;
                                        }
                                    case "RatingMidDef":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[9] = temp;
                                            break;
                                        }
                                    case "RatingLeftDef":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[10] = temp;
                                            break;
                                        }
                                    case "RatingRightAtt":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[11] = temp;
                                            break;
                                        }
                                    case "RatingMidAtt":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[12] = temp;
                                            break;
                                        }
                                    case "RatingLeftAtt":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[13] = temp;
                                            break;
                                        }
                                    case "AwayGoals":
                                        {
                                            int.TryParse(j.InnerXml, out temp);
                                            ReadMatchRatings[15] = temp;
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                }
            }
        }

        public void ParseOrdersFile()
        {

        }
    }
}
