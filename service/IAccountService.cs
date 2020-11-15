using System.Collections.Generic;
using System.Threading.Tasks;
using accountBook.model;

namespace accountBook.service
{
    public interface IAccountService
    {
        /**
         * 添加新的账单项
         */
        void AddAccount(AccountItem item);

        /**
         * 获知[year]年[month]月的账单列表，按照日期进行排序
         */
        Task<List<AccountItem>> GetAccountOrderByDate(int year, int month);

        /**
         * 获知[year]年[month]月的账单列表，
         * 第一行为收入项
         * 第二行为支出项
         */
        Task<double[,]> GetAccounts(int year, int month);

        /**
         * 根据[accountItemId]获取对应的账单项
         */
        Task<AccountItem> GetItemById(int accountItemId);

        /**
         * 更新账单项
         */
        void Update(AccountItem item);

        /**
         * 删除指定id的账单项
         */
        void DeleteById(int accountItemId);

        /**
         * 是否存在指定分类下的账单项
         */
        Task<bool> ExistByCategoryId(int categoryId);
        Task<List<AccountItem>> GetAllAccounts();
    }
}