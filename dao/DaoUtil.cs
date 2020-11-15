using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace accountBook.dao
{
    public static class DaoUtil
    {
        private const string DbName = "accoutBook.db";

        public static async Task<SqliteConnection> GetSqlConnection()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(
                DbName,
                CreationCollisionOption.OpenIfExists
            );
            var dbpath = Path.Combine(
                ApplicationData.Current.LocalFolder.Path,
                DbName
            );
            return new SqliteConnection($"Filename={dbpath}");
        }

        /**
         * 初始化数据库
         */
        public static async void InitializeDatabase()
        {
            using (var db = await GetSqlConnection())
            {
                db.Open();
                const string createTableCommand = @"CREATE TABLE IF NOT EXISTS accountItem( 
                                                      id          INTEGER PRIMARY KEY AUTOINCREMENT,
                                                      year        INTEGER           NOT NULL,
                                                      month       INTEGER           NOT NULL,
                                                      day         INTEGER           NOT NULL,
                                                      remark      VARCHAR(255)   NOT NULL DEFAULT '',
                                                      money       DECIMAL(10, 2) NOT NULL,
                                                      category_id INTEGER        NOT NULL
                                                  );
                                                  CREATE TABLE IF NOT EXISTS category( 
                                                      id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                      name VARCHAR(20) NOT NULL,
                                                      type INTEGER NOT NULL
                                                  );";

                var createTable = new SqliteCommand(createTableCommand, db);
                await createTable.ExecuteReaderAsync();
                db.Close();
            }
        }
    }
}