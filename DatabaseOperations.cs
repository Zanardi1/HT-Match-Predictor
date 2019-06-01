﻿using System;
using System.Collections.Generic;
using System.Data;
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
        /// <returns>true, daca baza de date a fost creata, false altfel</returns>
        private bool CreateDatabaseFile()
        {
            SqlConnection MyConn = new SqlConnection(CreateDatabaseConnectionString);
            string Str = "Create Database Matches on Primary (Name=Matches, Filename='@DatabaseFile') log on (Name=MatchesLog, Filename='@DatabaseLog')"; //retine comanda SQL care creeaza BD
            SqlCommand DatabaseCreationCommand = new SqlCommand(Str, MyConn);
            DatabaseCreationCommand.Parameters.Add("@DatabaseFile", SqlDbType.Text).Value = DatabaseFile;
            DatabaseCreationCommand.Parameters.Add("@DatabaseLog", SqlDbType.Text).Value = DatabaseLog;
            try
            {
                MyConn.Open();
                DatabaseCreationCommand.ExecuteNonQuery();
            }
            catch (SqlException S)
            {
                MessageBox.Show(S.Message);
                return false;
            }
            catch (IOException I)
            {
                MessageBox.Show(I.Message);
                return false;
            }
            catch (InvalidOperationException I)
            {
                MessageBox.Show(I.Message);
                return false;
            }
            catch (InvalidCastException I)
            {
                MessageBox.Show(I.Message);
                return false;
            }
            finally
            {
                MyConn.Close();
            }
            return true;
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
            SqlCommand TableCreationCommand = new SqlCommand(Str, MyConn);
            try
            {
                MyConn.Open();
                TableCreationCommand.ExecuteNonQuery();
            }
            catch (SqlException S)
            {
                MessageBox.Show(S.Message);
            }
            catch (IOException I)
            {
                MessageBox.Show(I.Message);
            }
            catch (InvalidOperationException I)
            {
                MessageBox.Show(I.Message);
            }
            catch (InvalidCastException I)
            {
                MessageBox.Show(I.Message);
            }
            finally
            {
                MyConn.Close();
            }
        }

        /// <summary>
        /// Rutina efectiva de creare a BD. Are doua parti: crearea fisierului ce va contine BD si crearea tabelei cu meciuri
        /// </summary>
        public void CreateDatabase()
        {
            if (CreateDatabaseFile())
            {
                CreateDatabaseTable();
            }
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
            try
            {
                MyConn.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException S)
            {
                MessageBox.Show(S.Message);
            }
            catch (IOException I)
            {
                MessageBox.Show(I.Message);
            }
            catch (InvalidOperationException I)
            {
                MessageBox.Show(I.Message);
            }
            catch (InvalidCastException I)
            {
                MessageBox.Show(I.Message);
            }
            finally
            {
                MyConn.Close();
            }
        }

        public void AddAMatch(int MatchIDToInsert, List<int> RatingsToInsert)
        {
            string AddMatchCommand = "Insert into Games select @Match, @Ratings1, @Ratings2, @Ratings3, @Ratings4, @Ratings5, @Ratings6, @Ratings7, @Ratings8, @Ratings9, @Ratings10, @Ratings11, @Ratings12, @Ratings13, @Ratings14, @Ratings15, @Ratings16 where not exists (select 1 from Games g where g.MatchID=@Match)";

            SqlConnection MyConn = new SqlConnection(CreateTableConnectionString);
            SqlCommand command = new SqlCommand(AddMatchCommand, MyConn);
            command.Parameters.Add("@Match", SqlDbType.Int).Value = MatchIDToInsert.ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings1", SqlDbType.TinyInt).Value = RatingsToInsert[0].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings2", SqlDbType.TinyInt).Value = RatingsToInsert[1].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings3", SqlDbType.TinyInt).Value = RatingsToInsert[2].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings4", SqlDbType.TinyInt).Value = RatingsToInsert[3].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings5", SqlDbType.TinyInt).Value = RatingsToInsert[4].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings6", SqlDbType.TinyInt).Value = RatingsToInsert[5].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings7", SqlDbType.TinyInt).Value = RatingsToInsert[6].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings8", SqlDbType.TinyInt).Value = RatingsToInsert[7].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings9", SqlDbType.TinyInt).Value = RatingsToInsert[8].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings10", SqlDbType.TinyInt).Value = RatingsToInsert[9].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings11", SqlDbType.TinyInt).Value = RatingsToInsert[10].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings12", SqlDbType.TinyInt).Value = RatingsToInsert[11].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings13", SqlDbType.TinyInt).Value = RatingsToInsert[12].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings14", SqlDbType.TinyInt).Value = RatingsToInsert[13].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings15", SqlDbType.TinyInt).Value = RatingsToInsert[14].ToString(CultureInfo.InvariantCulture);
            command.Parameters.Add("@Ratings16", SqlDbType.TinyInt).Value = RatingsToInsert[15].ToString(CultureInfo.InvariantCulture);
            try
            {
                MyConn.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException S)
            {
                MessageBox.Show(S.Message);
            }
            catch (IOException I)
            {
                MessageBox.Show(I.Message);
            }
            catch (InvalidOperationException I)
            {
                MessageBox.Show(I.Message);
            }
            catch (InvalidCastException I)
            {
                MessageBox.Show(I.Message);
            }
            MyConn.Close();
        }
    }
}
