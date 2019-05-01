using System.Data.SqlClient;
using System;
using System.IO;
using System.Windows.Forms;

//Clasa ce se ocupa de operatiile cu baza de date in care vor fi stocate meciurile. In aceasta clasa vor fi programate urmatoarele functii 
//1. Testarea existentei fisierului ce contine baza de date. Fisierul trebuie sa fie in folderul db;
//2. Crearea unei baze de date noi, in folderul db, cu toate campurile necesare;
//3. Stergerea bazei de date;
//4. Adaugarea unui meci in baza de date;
//5. Editarea unui meci din baza de date;
//6. Stergerea unui meci din baza de date;

namespace HT_Match_Predictor
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
        readonly string Database = "'" + DatabaseFolder + "\\Matches.mdf'";
        /// <summary>
        /// Retine numele fisierului jurnal
        /// </summary>
        readonly string Log = "'" + DatabaseFolder + "\\MatchesLog.ldf'";
        /// <summary>
        /// Retine sirul de conectare la baza de date. Depinde de serverul de BD pe care il am.
        /// </summary>
        readonly string CreateDatabaseConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        /// <summary>
        /// Retine sirul de conectare pentru crearea tabelei
        /// </summary>
        readonly string CreateTableConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Matches;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public DatabaseOperations()
        {

        }

        /// <summary>
        /// Verifica daca baza de date exista. Metoda luata de la https://stackoverflow.com/questions/2232227/check-if-database-exists-before-creating
        /// </summary>
        /// <returns>true, daca exista. False, altfel</returns>
        public bool DatabaseExists()
        {
            bool Exists = false;
            try
            {
                SqlConnection MyConn = new SqlConnection(CreateTableConnectionString);
                string TestDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = 'Matches'");
                using (MyConn)
                {
                    using (SqlCommand cmd = new SqlCommand(TestDBQuery, MyConn))
                    {
                        MyConn.Open();
                        object Result = cmd.ExecuteScalar();
                        int DatabaseID = 0;
                        if (Result != null)
                        {
                            int.TryParse(Result.ToString(), out DatabaseID);
                        }
                        MyConn.Close();
                        Exists = (DatabaseID > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Exists = false;
            }
            return Exists;
        }

        private void CreateDatabaseFile()
        //Creaza fisierul ce contine baza de date
        {
            string Str; //retine comenzile SQL care vor fi trimise
            SqlConnection MyConn = new SqlConnection(CreateDatabaseConnectionString);
            Str = "Create Database Matches on Primary (Name=Matches, Filename=" + Database + ") log on (Name=MatchesLog, Filename=" + Log + ")"; //creaza BD
            SqlCommand command = new SqlCommand(Str, MyConn);
            MyConn.Open();
            command.ExecuteNonQuery();
            MyConn.Close();
        }

        private void CreateDatabaseTable()
        //Creaza tabela ce va contine meciurile
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

        public void CreateDatabase()
        //Creaza baza de date ce va retine meciurile, inclusiv tabela corespunzatoare.
        {
            CreateDatabaseFile();
            CreateDatabaseTable();
        }

        public void DeleteDatabase()
        //Sterge baza de date ce va retine meciurile.
        {
            string Str;
            SqlConnection MyConn = new SqlConnection(CreateDatabaseConnectionString);
            Str = "drop database Matches";
            SqlCommand command = new SqlCommand(Str, MyConn);
            MyConn.Open();
            command.ExecuteNonQuery();
            MyConn.Close();
        }

        public void AddAMatch()
        {

        }

        public void EditAMatch(int MatchID)
        {

        }

        public void DeleteAMatch(int MatchID)
        {

        }
    }
}
