using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;

namespace KonvertIm.Components
{
    public class StorageModel
    {
        DB db;
        const string TABLE_NAME = "storage";

        public StorageModel(DB db)
        {
            this.db = db;
        }

        public StorageModel MigrateUp()
        {
            try
            {
                db.Execute(@"CREATE TABLE @storage (
                                key char(63) PRIMARY KEY NOT NULL UNIQUE, 
                                value char(255) NOT NULL
                            );".Replace("@storage", TABLE_NAME));
            }
            catch (Exception) { }

            return this;
        }

        public void Set(string key, string value)
        {
            try
            {
                string SQL = @"INSERT OR REPLACE INTO @storage (key, value) VALUES (@key, @value);"
                .Replace("@storage", TABLE_NAME);

                SQLiteParameter[] parameters = {
                    new SQLiteParameter("@key",   key),
                    new SQLiteParameter("@value", value)
                };

                db.Execute(SQL, parameters);
            }
            catch (Exception) { }
        }

        public string Get(string key)
        {
            List<string> listUsed = new List<string>();

            try
            {
                SQLiteParameter[] parameters = { new SQLiteParameter("@key", key) };

                DbDataReader reader = db.All(
                    @"SELECT value
                    FROM @storage
                    WHERE key LIKE @key;".Replace("@storage", TABLE_NAME)
                    , parameters
                );

                reader.Read();
                return reader.GetValue(0).ToString();
            }
            catch (Exception) { }

            return null;
        }
    }
}
