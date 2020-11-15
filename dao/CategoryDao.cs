using System.Collections.Generic;
using System.Threading.Tasks;
using accountBook.model;
using Microsoft.Data.Sqlite;

namespace accountBook.dao
{
    static class CategoryDao
    {
        public static async Task<List<Category>> GetAllCategoryByType(ItemType type)
        {
            var ans = new List<Category>();
            using (var connection = await DaoUtil.GetSqlConnection())
            {
                await connection.OpenAsync();
                var command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT id, name, type FROM category WHERE type = @type"
                };
                command.Parameters.AddWithValue("@type", (int)type);
                var query = await command.ExecuteReaderAsync();
                while (await query.ReadAsync())
                {
                    ans.Add(new Category
                    {
                        Id = query.GetInt32(0),
                        Name = query.GetString(1),
                        Type = (ItemType)query.GetInt32(2)
                    });
                }
                connection.Close();
            }
            return ans;
        }

        public static async void DeleteCategoryById(int categoryId)
        {
            using (var connection = await DaoUtil.GetSqlConnection())
            {
                await connection.OpenAsync();
                var command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = @"DELETE FROM category WHERE id = @categoryId"
                };
                command.Parameters.AddWithValue("@categoryId", categoryId);
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }

        public static async void AddNewCategory(string categoryName, ItemType type)
        {
            using (var connection = await DaoUtil.GetSqlConnection())
            {
                await connection.OpenAsync();
                var command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = @"INSERT INTO category(name, type) VALUES (@name,@type)"
                };
                command.Parameters.AddWithValue("@name", categoryName);
                command.Parameters.AddWithValue("@type", type);
                await command.ExecuteReaderAsync();
                connection.Close();
            }
        }
    }
}
