using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using accountBook.model;
using Microsoft.Data.Sqlite;

namespace accountBook.dao
{
    internal static class AccountItemDao
    {
        /**
         * 添加新的账单记录
         */
        public static async void AddAccount(AccountItem item)
        {
            using (var connection = await DaoUtil.GetSqlConnection())
            {
                connection.Open();
                var command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = @"
                        INSERT INTO accountItem(year,month,day, remark, money, category_id)
                        VALUES (@year,@month,@day, @Remark, @Money, @CategoryId)"
                };

                command.Parameters.AddWithValue("@year", item.Year);
                command.Parameters.AddWithValue("@month", item.Month);
                command.Parameters.AddWithValue("@day", item.Day);
                command.Parameters.AddWithValue("@Remark", item.Remark);
                command.Parameters.AddWithValue("@Money", item.Money);
                command.Parameters.AddWithValue("@CategoryId", item.CategoryId);
                await command.ExecuteReaderAsync();
                connection.Close();
            }
        }


        public static async Task<List<AccountItem>> GetAccounts(int year, int month)
        {
            var ans = new List<AccountItem>();
            using (var connection = await DaoUtil.GetSqlConnection())
            {
                await connection.OpenAsync();
                var command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = @"
                        SELECT accountItem.id, year,month,day, remark, money, category_id, c.name, c.type 
                        FROM accountItem 
                        INNER JOIN category AS c on c.id = accountItem.category_id 
                        WHERE year = @year AND month = @month 
                        ORDER BY year,month,day DESC"
                };
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@month", month);
                var query = await command.ExecuteReaderAsync();
                while (await query.ReadAsync())
                {
                    ans.Add(new AccountItem
                    {
                        Id = query.GetInt32(0),
                        Year = query.GetInt32(1),
                        Month = query.GetInt32(2),
                        Day = query.GetInt32(3),
                        Remark = query.GetString(4),
                        Money = query.GetDouble(5),
                        CategoryId = query.GetInt32(6),
                        Category = new Category
                        {
                            Id = query.GetInt32(6),
                            Name = query.GetString(7),
                            Type = (ItemType) query.GetInt32(8)
                        }
                    });
                }

                connection.Close();
            }

            return ans;
        }

        public static async Task<List<AccountItem>> GetAccounts()
        {
            var ans = new List<AccountItem>();
            using (var connection = await DaoUtil.GetSqlConnection())
            {
                await connection.OpenAsync();
                var command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = @"
                        SELECT accountItem.id, year,month,day, remark, money, category_id, c.name, c.type 
                        FROM accountItem 
                        INNER JOIN category AS c on c.id = accountItem.category_id 
                        ORDER BY year,month,day DESC"
                };
                var query = await command.ExecuteReaderAsync();
                while (await query.ReadAsync())
                {
                    ans.Add(new AccountItem
                    {
                        Id = query.GetInt32(0),
                        Year = query.GetInt32(1),
                        Month = query.GetInt32(2),
                        Day = query.GetInt32(3),
                        Remark = query.GetString(4),
                        Money = query.GetDouble(5),
                        CategoryId = query.GetInt32(6),
                        Category = new Category
                        {
                            Id = query.GetInt32(6),
                            Name = query.GetString(7),
                            Type = (ItemType)query.GetInt32(8)
                        }
                    });
                }
                connection.Close();
            }
            return ans;
        }

        public static async Task<AccountItem> GetAccountItemById(int accountItemId)
        {
            using (var connection = await DaoUtil.GetSqlConnection())
            {
                await connection.OpenAsync();
                var command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = @"
                        SELECT accountItem.id, year,month,day, remark, money, category_id, c.name, c.type 
                        FROM accountItem 
                        INNER JOIN category AS c on c.id = accountItem.category_id 
                        WHERE accountItem.id = @accountItemId
                        ORDER BY year,month,day DESC"
                };
                command.Parameters.AddWithValue("@accountItemId", accountItemId);
                var query = await command.ExecuteReaderAsync();
                if (await query.ReadAsync())
                {
                    return new AccountItem
                    {
                        Id = query.GetInt32(0),
                        Year = query.GetInt32(1),
                        Month = query.GetInt32(2),
                        Day = query.GetInt32(3),
                        Remark = query.GetString(4),
                        Money = query.GetDouble(5),
                        CategoryId = query.GetInt32(6),
                        Category = new Category
                        {
                            Id = query.GetInt32(6),
                            Name = query.GetString(7),
                            Type = (ItemType) query.GetInt32(8)
                        }
                    };
                }

                return null;
            }
        }

        public static async Task<List<AccountItem>> GetAccountItemOrderByDate(int year, int month)
        {
            var ans = new List<AccountItem>();
            using (var connection = await DaoUtil.GetSqlConnection())
            {
                await connection.OpenAsync();
                var command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = @"
                        SELECT accountItem.id, year,month,day, remark, money, category_id, c.name, c.type 
                        FROM accountItem 
                        INNER JOIN category AS c on c.id = accountItem.category_id 
                        WHERE year = @year and month = @month 
                        ORDER BY year,month,day DESC"
                };

                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@month", month);

                var query = await command.ExecuteReaderAsync();
                while (await query.ReadAsync())
                {
                    ans.Add(new AccountItem
                    {
                        Id = query.GetInt32(0),
                        Year = query.GetInt32(1),
                        Month = query.GetInt32(2),
                        Day = query.GetInt32(3),
                        Remark = query.GetString(4),
                        Money = query.GetDouble(5),
                        CategoryId = query.GetInt32(6),
                        Category = new Category
                        {
                            Id = query.GetInt32(6),
                            Name = query.GetString(7),
                            Type = (ItemType) query.GetInt32(8)
                        }
                    });
                }

                connection.Close();
            }

            return ans;
        }

        public static async Task<List<AccountItem>> GetAccountByDate(DateTime date)
        {
            var ans = new List<AccountItem>();
            using (var connection = await DaoUtil.GetSqlConnection())
            {
                connection.Open();
                var command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = @"
                        SELECT id,year,month,day,remark,money,category_id FROM accountItem  
                        WHERE year =@year and month = @month and day = @day"
                };
                command.Parameters.AddWithValue("@year", date.Year);
                command.Parameters.AddWithValue("@month", date.Month);
                command.Parameters.AddWithValue("@day", date.Day);
                var query = await command.ExecuteReaderAsync();
                while (await query.ReadAsync())
                {
                    ans.Add(new AccountItem
                    {
                        Id = query.GetInt32(0),
                        Year = query.GetInt32(1),
                        Month = query.GetInt32(2),
                        Day = query.GetInt32(3),
                        Remark = query.GetString(4),
                        Money = query.GetDouble(5),
                        CategoryId = query.GetInt32(6),
                    });
                }

                connection.Close();
            }

            return ans;
        }

        public static async void DeleteById(int accountItemId)
        {
            using (var connection = await DaoUtil.GetSqlConnection())
            {
                connection.Open();
                var command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = @"DELETE FROM accountItem WHERE id =@accountItemId"
                };
                command.Parameters.AddWithValue("@accountItemId", accountItemId);
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }

        public static async void Update(AccountItem item)
        {
            using (var connection = await DaoUtil.GetSqlConnection())
            {
                connection.Open();
                var command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = @"UPDATE accountItem
                            SET year = @year,
                            month = @month ,
                            day = @day,
                            remark = @remark,
                            money = @money,
                            category_id = @categoryId
                            WHERE id =@id"
                };
                command.Parameters.AddWithValue("@year", item.Year);
                command.Parameters.AddWithValue("@month", item.Month);
                command.Parameters.AddWithValue("@day", item.Day);
                command.Parameters.AddWithValue("@remark", item.Remark);
                command.Parameters.AddWithValue("@money", item.Money);
                command.Parameters.AddWithValue("@categoryId", item.CategoryId);
                command.Parameters.AddWithValue("@id", item.Id);
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }

        public static async Task<bool> ExistByCategoryId(int categoryId)
        {
            using (var connection = await DaoUtil.GetSqlConnection())
            {
                connection.Open();
                var command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT COUNT(*) FROM accountItem WHERE category_id = @categoryId"
                };
                command.Parameters.AddWithValue("@categoryId", categoryId);
                var query = await command.ExecuteReaderAsync();
                await query.ReadAsync();
                return query.GetInt32(0) != 0;
            }
        }
    }
}