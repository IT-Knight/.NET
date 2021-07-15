using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Telegram.Bot.Types;
using System.Data;

namespace TGBOT
{
    class Database
    {
        public static string path = Directory.GetCurrentDirectory();
        public static SqlConnection con = null;
        //public static string con_string = $@"Data Source=localhost\MSSQLLocalDB;Integrated Security=True;Pooling=True";
        //public static string con_string = $@"Server=sqlserver;User Id=sa;Password=Root1234;Pooling=True";
        public static string con_string = $@"Server=(localdb)\MSSQLLocalDB;Integrated Security=True;Pooling=True";
        // public static string con_string = ConfigurationManager.ConnectionStrings["DATABASE"].ConnectionString;

        public static bool checked_exists = false;

        public Database()  // on first init should check if DB is created by flag
        {
            if (!checked_exists)
            {
                con = new SqlConnection(con_string);
                con.Open();
                // var create_db = @$"IF DB_ID('BotDB') IS NULL
                //               BEGIN
                //               CREATE DATABASE BotDB ON PRIMARY (NAME = 'TGBot', FILENAME = '{path}\TGBot_db.mdf') 
                //               LOG ON (NAME = 'TGBotLog', FILENAME = '{path}\TGBot_log.ldf')
                //               END
                //               USE BotDB;";
                var create_db = @$"IF DB_ID('BotDB') IS NULL
                              BEGIN
                              CREATE DATABASE BotDB
                              END
                              USE BotDB;";

                var command_createDB = new SqlCommand(create_db, con);
                command_createDB.ExecuteNonQuery();
                Console.WriteLine("DB Created");

                string query1 = @"USE BotDB; IF OBJECT_ID('Main', 'U') IS NULL
                                CREATE TABLE Main(ID INT PRIMARY KEY IDENTITY,
                                    Search_query NVARCHAR(2048), One NVARCHAR(1000), Two NVARCHAR(1000), Three NVARCHAR(1000), Four NVARCHAR(1000), Five NVARCHAR(1000), 
                                    Six NVARCHAR(1000), Seven NVARCHAR(1000), Eight NVARCHAR(1000), Nine NVARCHAR(1000), Ten NVARCHAR(1000), DateAdded DATETIME
                                );";
                string query2 = @"IF OBJECT_ID('Logs', 'U') IS NULL 
                               CREATE TABLE Logs (ID INT PRIMARY KEY IDENTITY, Type VARCHAR (12), Username NVARCHAR(1000), Log NVARCHAR(max), Date DATETIME);";
                SqlCommand command = new SqlCommand(query1 + query2, con);
                try
                {
                    command.ExecuteNonQuery();
                    checked_exists = true;
                    Console.WriteLine("Tables Main and Logs successfuly created");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Table not created. \n Error: {ex}");
                }
                Console.WriteLine(con.ConnectionString);
                Console.WriteLine(con.AccessToken);
                Console.WriteLine(con.DataSource);
                Console.WriteLine(con.Database);
                Console.WriteLine(con.Site);
                Console.WriteLine(con.ServerVersion);
                Console.WriteLine(con.Credential);
                Console.WriteLine(con.Container);
                con.Close();
            }
        }

        public static SqlConnection new_con()
        {
            return new SqlConnection(con_string);
        }

        public static void AddResultsToDB(string search_query, List<string> results)
        {
            con = new SqlConnection(con_string);
            con.Open();
            var query = @"use BotDB; INSERT INTO Main VALUES(@Search_query, @One, @Two, @Three, @Four, @Five, @Six, @Seven, @Eight, @Nine, @Ten, @DateAdded)";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@Search_query", $"{search_query}");  // WTF?!

            string[] params_arr = { "@One", "@Two", "@Three", "@Four", "@Five", "@Six", "@Seven", "@Eight", "@Nine", "@Ten" };

            for (int i = 0; i < results.Count; i++)
            {
                command.Parameters.AddWithValue(params_arr[i], $"{results[i]}");
            }
            for (int i = results.Count; i < 10; i++)
            {
                i = (i < 0) ? 0 : i;
                command.Parameters.AddWithValue(params_arr[i], "Null");
            }



            command.Parameters.AddWithValue("@DateAdded", DateTime.Now);
            command.ExecuteNonQuery();
            Console.WriteLine("SearchQuery: Write In");
            con.Close();
            con = null;
        }
        public static List<string> GetFromDB(string search_query)
        {
            con = new SqlConnection(con_string);
            con.Open();

            var query = @"use BotDB; Select One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten FROM [Main] Where CONVERT(nvarchar, Search_query)=@Search_query";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@Search_query", $"{search_query}");
            SqlDataReader reader = command.ExecuteReader();

            List<string> results = new();  // так тоже можно

            while (reader.Read())
            {
                var myObject = new object[reader.FieldCount];
                int colCount = reader.GetValues(myObject);
                for (int i = 0; i < colCount; i++)
                {
                    if ((string)myObject[i] != "Null")
                    {
                        results.Add((string)myObject[i]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            con.Close();

            return results;
        }

        public static void DeleteOutdatedData(string search_query = null)
        {
            con = new SqlConnection(con_string);
            con.Open();
            var query = @"use BotDB; DELETE FROM Main WHERE DATEDIFF(hour, DateAdded, GETDATE()) >= 12";
            var command = con.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
            con.Close();
        }

        public async static Task SaveLog(Message log, string type)
        {
            con = new SqlConnection(con_string);
            await con.OpenAsync();

            var query = @"use BotDB; INSERT INTO Logs VALUES(@Type, @Username, @Log, @Date)";
            string username = $"{log.Chat.FirstName} {log.Chat.LastName} ({log.Chat.Username})";

            var command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@Type", type);
            command.Parameters.AddWithValue("@Username", $"{username}");
            command.Parameters.AddWithValue("@Log", $"{log.Text}");
            command.Parameters.AddWithValue("@Date", DateTime.Now);
            await command.ExecuteNonQueryAsync();

            await con.CloseAsync();
        }
    }
}
