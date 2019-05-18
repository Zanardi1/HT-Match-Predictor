using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

//Clasa ce se ocupa de operatiile cu baza de date in care vor fi stocate meciurile. In aceasta clasa vor fi programate urmatoarele functii 
//1. Testarea existentei fisierului ce contine baza de date. Fisierul trebuie sa fie in folderul db;
//2. Crearea unei baze de date noi, in folderul db, cu toate campurile necesare;
//3. Stergerea bazei de date;
//4. Adaugarea unui meci in baza de date;
//5. Editarea unui meci din baza de date;
//6. Stergerea unui meci din baza de date;

namespace HTMatchPredictor
{
    class DatabaseOperations
    {
        /// <summary>
        /// Retine folderul unde va fi pusa baza de date cu meciuri
        /// </summary>
        static readonly string DatabaseFolder = Path.GetDirectoryName(Application.ExecutablePath) + "\\db";
        /// <summary>
        /// Retine numele fisierului ce va retine baza de date
        /// </summary>
        readonly string DatabaseFile = DatabaseFolder + "\\Matches.mdf";
        /// <summary>
        /// Retine numele fisierului jurnal
        /// </summary>
        readonly string DatabaseLog = DatabaseFolder + "\\MatchesLog.ldf";
        /// <summary>
        /// Retine sirul de conectare la baza de date. Depinde de serverul de BD pe care il am.
        /// </summary>
        readonly public string CreateDatabaseConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        /// <summary>
        /// Retine sirul de conectare pentru crearea tabelei
        /// </summary>
        readonly public string CreateTableConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Matches;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public DatabaseOperations()
        {

        }

        /// <summary>
        /// Rutina ce verifica daca exista fisierul ce contine baza de date.
        /// </summary>
        /// <returns>true, daca fisierul exista</returns>
        public bool DatabaseExists()
        {
            return File.Exists(DatabaseFile);
        }

        /// <summary>
        /// Rutina de creare a fisierului ce va contine baza de date
        /// </summary>
        private void CreateDatabaseFile()
        {
            SqlConnection MyConn = new SqlConnection(CreateDatabaseConnectionString);
            //string Str = "Create Database Matches on Primary (Name=Matches, Filename='" + DatabaseFile + "') log on (Name=MatchesLog, Filename='" + DatabaseLog + "')"; //retine comanda SQL care creeaza BD
            string Str = "Create Database Matches on Primary (Name=Matches, Filename='@DatabaseFile') log on (Name=MatchesLog, Filename='@DatabaseLog')"; //retine comanda SQL care creeaza BD
            SqlCommand command = new SqlCommand(Str, MyConn);
            command.Parameters.AddWithValue("@DatabaseFile", DatabaseFile);
            command.Parameters.AddWithValue("@DatabaseLog", DatabaseLog);
            MyConn.Open();
            command.ExecuteNonQuery();
            MyConn.Close();
        }

        /// <summary>
        /// Rutina de creare a tabelei ce va contine meciurile
        /// </summary>
        private void CreateDatabaseTable()
        {
            string Str;
            SqlConnection MyConn = new SqlConnection(CreateTableConnectionString);
            Str = @"CREATE TABLE [dbo].[Games]
(
    [MatchID] INT NOT NULL, 
    [HomeTeamMidfield] TINYINT NOT NULL, 
    [HomeTeamRDefense] TINYINT NOT NULL, 
    [HomeTeamCDefense] TINYINT NOT NULL, 
    [HomeTeamLDefense] TINYINT NOT NULL, 
    [HomeTeamRAttack] TINYINT NOT NULL, 
    [HomeTeamCAttack] TINYINT NOT NULL, 
    [HomeTeamLAttack] TINYINT NOT NULL, 
    [AwayTeamMidfield] TINYINT NOT NULL, 
    [AwayTeamRDefense] TINYINT NOT NULL, 
    [AwayTeamCDefense] TINYINT NOT NULL, 
    [AwayTeamLDefense] TINYINT NOT NULL, 
    [AwayTeamRAttack] TINYINT NOT NULL, 
    [AwayTeamCAttack] TINYINT NOT NULL, 
    [AwayTeamLAttack] TINYINT NOT NULL, 
    [HomeTeamGoals] TINYINT NOT NULL, 
    [AwayTeamGoals] TINYINT NOT NULL, 
    PRIMARY KEY([MatchID])
)
";
            SqlCommand AnotherCommand = new SqlCommand(Str, MyConn);
            MyConn.Open();
            AnotherCommand.ExecuteNonQuery();
            MyConn.Close();
        }

        /// <summary>
        /// Rutina efectiva de creare a BD. Are doua parti: crearea fisierului ce va contine BD si crearea tabelei cu meciuri
        /// </summary>
        public void CreateDatabase()
        {
            CreateDatabaseFile();
            CreateDatabaseTable();
        }

        /// <summary>
        /// Rutina de stergere a BD. Prima data intrerupe orice conexiune la ea, apoi o sterge.
        /// </summary>
        public void DeleteDatabase()
        {
            string Str;
            SqlConnection MyConn = new SqlConnection(CreateDatabaseConnectionString);
            Str = "Alter database Matches set single_user with rollback immediate\r\ndrop database Matches";
            SqlCommand command = new SqlCommand(Str, MyConn);
            MyConn.Open();
            command.ExecuteNonQuery();
            MyConn.Close();
        }

        public void AddAMatch(int MatchIDToInsert, List<int> RatingsToInsert)
        {
            string AddMatchCommand = "Insert into Games values (@Match, @Ratings1, @Ratings2, @Ratings3, @Ratings4, @Ratings5, @Ratings6, @Ratings7, @Ratings8, @Ratings9, @Ratings10, @Ratings11, @Ratings12, @Ratings13, @Ratings14, @Ratings15, @Ratings16)";
            SqlConnection MyConn = new SqlConnection(CreateTableConnectionString);
            SqlCommand command = new SqlCommand(AddMatchCommand, MyConn);
            command.Parameters.AddWithValue("@Match", MatchIDToInsert.ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings1", RatingsToInsert[0].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings2", RatingsToInsert[1].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings3", RatingsToInsert[2].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings4", RatingsToInsert[3].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings5", RatingsToInsert[4].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings6", RatingsToInsert[5].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings7", RatingsToInsert[6].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings8", RatingsToInsert[7].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings9", RatingsToInsert[8].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings10", RatingsToInsert[9].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings11", RatingsToInsert[10].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings12", RatingsToInsert[11].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings13", RatingsToInsert[12].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings14", RatingsToInsert[13].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings15", RatingsToInsert[14].ToString(CultureInfo.InvariantCulture));
            command.Parameters.AddWithValue("@Ratings16", RatingsToInsert[15].ToString(CultureInfo.InvariantCulture));
            MyConn.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException S)
            {
                MessageBox.Show(S.Message);
                //Daca exista deja inregistrarea cu un anumit numar de identificare pentru un meci, atunci nu face nimic. Nu stiu daca asta e cea mai buna modalitate de a trata exceptia
                //todo sa ma gandesc la o modalitate mai eleganta de a trata aceasta exceptie (daca e cazul).
            }
            MyConn.Close();
        }
    }
}
