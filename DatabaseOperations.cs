using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

//Clasa ce se ocupa de operatiile cu baza de date in care vor fi stocate meciurile. In aceasta clasa vor fi programate urmatoarele functii:
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
        static readonly string DatabaseFolder = Path.GetDirectoryName(Application.ExecutablePath) + "\\db"; //Retin folderul unde va fi pusa baza de date cu meciuri
        readonly string Database = "'" + DatabaseFolder + "\\Matches.mdf'";
        readonly string Log = "'" + DatabaseFolder + "\\MatchesLog.ldf'";
        public DatabaseOperations()
        {

        }

        public bool DatabaseExists()
        {
            return true;
        }

        public void CreateDatabase()
        {
            string Str;
            SqlConnection MyConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30");
            //todo sa vad cum fac rost de sirul de conexiune (connection string)
            Str = "Create Database Matches on Primary (Name=Matches, Filename="+Database+") log on (Name=MatchesLog, Filename="+Log+")";
            SqlCommand command = new SqlCommand(Str, MyConn);
            MyConn.Open();
            command.ExecuteNonQuery();
            MessageBox.Show("Done");
        }

        public void DeleteDatabase()
        {
            //de perfectionat
            string Str;
            SqlConnection MyConn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30");
            Str = "drop database "+Database;
            SqlCommand command = new SqlCommand(Str, MyConn);
            MyConn.Open();
            command.ExecuteNonQuery();
            MessageBox.Show("Done");
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
