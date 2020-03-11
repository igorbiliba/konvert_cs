using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace KonvertIm.Components
{
    public class DB
    {
        string DB_PATH
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\db.db3";
            }
        }

        public DB()
        {
            if (!File.Exists(DB_PATH)) Init();
            else OpenConnection();
        }

        SQLiteFactory factory = null;
        SQLiteConnection connection = null;
        void OpenConnection()
        {
            if (factory != null || connection != null) return;

            factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
            connection = (SQLiteConnection)factory.CreateConnection();
            connection.ConnectionString = "Data Source = " + DB_PATH;
            connection.Open();
        }

        void Init()
        {
            SQLiteConnection.CreateFile(DB_PATH);
            OpenConnection();
        }

        public void Execute(string sql, SQLiteParameter[] parameters = null)
        {
            OpenConnection();
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                if (parameters != null) command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public string One(string sql, SQLiteParameter[] parameters = null)
        {
            OpenConnection();

            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                if (parameters != null) command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.Text;

                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader.GetValue(0).ToString();
                    }
                }
            }

            return null;
        }

        public DbDataReader All(string sql, SQLiteParameter[] parameters = null)
        {
            OpenConnection();

            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                if (parameters != null) command.Parameters.AddRange(parameters);
                command.CommandType = CommandType.Text;

                DbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return reader;
                }
            }

            return null;
        }
    }
}
