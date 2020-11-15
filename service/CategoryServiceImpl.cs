using accountBook.dao;
using accountBook.model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace accountBook.service
{
    public class CategoryServiceImpl : ICategoryService
    {
        public void AddCategory(string categoryName, ItemType type)
        {
            CategoryDao.AddNewCategory(categoryName, type);
        }

        public void DeleteCategory(int categoryId)
        {
            CategoryDao.DeleteCategoryById(categoryId);
        }

        public async Task<List<Category>> GetAllCategoryByType(ItemType type)
        {
            return await CategoryDao.GetAllCategoryByType(type);
        }
    }
}