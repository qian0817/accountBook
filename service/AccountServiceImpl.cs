using accountBook.dao;
using accountBook.model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace accountBook.service
{
    public class AccountServiceImpl : IAccountService
    {
        public void AddAccount(AccountItem item)
        {
            
            AccountItemDao.AddAccount(item);
        }

        public async Task<List<AccountItem>> GetAccountOrderByDate(int year, int month)
        {
            return await AccountItemDao.GetAccountItemOrderByDate(year, month);
        }

        public void Update(AccountItem item)
        {
            AccountItemDao.Update(item);
        }

        public void DeleteById(int accountItemId)
        {
            AccountItemDao.DeleteById(accountItemId);
        }

        public async Task<double[,]> GetAccounts(int year, int month)
        {
            var days = DateTime.DaysInMonth(year, month);
            var ans = new double[days, 2];
            var accountLists = await AccountItemDao.GetAccounts(year, month);
            accountLists.ForEach(item =>
            {
                var index = item.Day - 1;
                if (item.Category.Type == ItemType.Income)
                {
                    ans[index, 0] += item.Money;
                }
                else
                {
                    ans[index, 1] += item.Money;
                }
            });
            return ans;
        }

        public async Task<AccountItem> GetItemById(int accountItemId)
        {
            return await AccountItemDao.GetAccountItemById(accountItemId);
        }

        public Task<bool> ExistByCategoryId(int categoryId)
        {
            return AccountItemDao.ExistByCategoryId(categoryId);
        }

        public async Task<List<AccountItem>> GetAllAccounts()
        {
            return await AccountItemDao.GetAccounts();
        }
    }
}