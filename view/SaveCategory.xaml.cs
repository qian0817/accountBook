using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using accountBook.model;
using accountBook.service;
using Windows.UI.Popups;

namespace accountBook.view
{
    public sealed partial class SaveCategory: Page, INotifyPropertyChanged
    {
        private string _categoryName = "";
        private string CategoryName
        {
            get => _categoryName;
            set
            {
                if (_categoryName == value) return;
                _categoryName = value;
                OnPropertyChanged();
            }
        }
        private readonly ICategoryService _categoryService = new CategoryServiceImpl();
        private readonly IAccountService _accountService = new AccountServiceImpl();
        private ObservableCollection<Category> SpendingCategories { get; set; } = new ObservableCollection<Category>();
        private ObservableCollection<Category> IncomeCategories { get; set; } = new ObservableCollection<Category>();

        public SaveCategory()
        {
            InitializeComponent();
            LoadCategory();
        }

        private async void LoadCategory()
        {
            SpendingCategories.Clear();
            IncomeCategories.Clear();
            var spending =await _categoryService.GetAllCategoryByType(ItemType.Spending);
            var income = await _categoryService.GetAllCategoryByType(ItemType.Income);
            income.ForEach(item => IncomeCategories.Add(item));
            spending.ForEach(item => SpendingCategories.Add(item));
        }

        private void AddSpendingCategory(object sender, RoutedEventArgs e)
        {
            if (CategoryName.Length == 0)
            {
                return;
            }
            _categoryService.AddCategory(CategoryName, ItemType.Spending);
            LoadCategory();
            CategoryName = "";
        }

        private void AddIncomeCategory(object sender, RoutedEventArgs e)
        {
            if (CategoryName.Length == 0)
            {
                return;
            }
            _categoryService.AddCategory(CategoryName, ItemType.Income);
            LoadCategory();
            CategoryName = "";
        }

        private async void DeleteCategory(object sender, RoutedEventArgs e)
        {
            var button=(Button)sender;
            var categoryId = Convert.ToInt32(button.Tag);
            var exist =await _accountService.ExistByCategoryId(categoryId);
            if (exist)
            {
                await new MessageDialog("该分类下存在账单，不可删除").ShowAsync();
                return;
            }
            _categoryService.DeleteCategory(categoryId);
            LoadCategory();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
