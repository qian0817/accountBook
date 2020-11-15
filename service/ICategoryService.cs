using System.Collections.Generic;
using System.Threading.Tasks;
using accountBook.model;

namespace accountBook.service
{
    public interface ICategoryService
    {
        /**
         *  根据分类类型获取所有的分类
         */
        Task<List<Category>> GetAllCategoryByType(ItemType type);

        /**
         * 添加新的分类
         */
        void AddCategory(string categoryName, ItemType spending);

        /**
         * 删除指定的分类
         */
        void DeleteCategory(int categoryId);
    }
}