using accountBook.model;
using accountBook.service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace accountBook.view
{
    public sealed partial class SaveAccount : Page, INotifyPropertyChanged
    {
        private readonly IAccountService _accountService = new AccountServiceImpl();
        private readonly ICategoryService _categoryService = new CategoryServiceImpl();
        public List<Tuple<string, Category>> IncomeCategories { get; set; } = new List<Tuple<string, Category>>();
        public List<Tuple<string, Category>> SpendingCategories { get; set; } = new List<Tuple<string, Category>>();

        private Category SelectedCategory { get; set; }

        private DateTimeOffset _createTime = DateTime.Now;

        private DateTimeOffset CreateTime
        {
            get => _createTime;
            set
            {
                if (_createTime == value) return;
                _createTime = value;
                OnPropertyChanged();
            }
        }

        private string _money = "";

        private string Money
        {
            get => _money;
            set
            {
                if (_money == value) return;
                _money = value;
                OnPropertyChanged();
            }
        }

        private string _remark = "";

        private string Remark
        {
            get => _remark;
            set
            {
                if (_remark == value) return;
                _remark = value;
                OnPropertyChanged();
            }
        }

        public SaveAccount()
        {
            InitializeComponent();
            LoadCategory();
        }

        /**
         * 加载分类列表
         */
        private async void LoadCategory()
        {
            var incomeCategories = await _categoryService.GetAllCategoryByType(ItemType.Income);
            incomeCategories.ForEach(item => IncomeCategories.Add(new Tuple<string, Category>(item.Name, item)));
            var spendingCategories = await _categoryService.GetAllCategoryByType(ItemType.Spending);
            spendingCategories.ForEach(item => SpendingCategories.Add(new Tuple<string, Category>(item.Name, item)));
        }

        private void AddIncomeAccount(object sender, RoutedEventArgs e)
        {
            if (SelectedCategory == null)
            {
                ShowMessage("未选择类型");
                return;
            }

            if (!Regex.IsMatch(Money, "^([1-9][0-9]{0,8})+(\\.[0-9]{1,2})?$"))
            {
                ShowMessage("请检查金额是否符合标准");
                return;
            }

            _accountService.AddAccount(new AccountItem
            {
                Year = CreateTime.Year,
                Month = CreateTime.Month,
                Day = CreateTime.Day,
                Remark = Remark,
                Money = Convert.ToDouble(Money),
                CategoryId = SelectedCategory.Id
            });
            ClearInput();
        }
        
        private async void ShowMessage(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        private void AddSpendingAccount(object sender, RoutedEventArgs e)
        {
            if (SelectedCategory == null)
            {
                ShowMessage("未选择类型");
                return;
            }

            if (!Regex.IsMatch(Money, "^([1-9][0-9]{0,8})+(\\.[0-9]{1,2})?$"))
            {
                ShowMessage("请检查金额是否符合标准");
                return;
            }

            _accountService.AddAccount(new AccountItem
            {
                Year = CreateTime.Year,
                Month = CreateTime.Month,
                Day = CreateTime.Day,
                Remark = Remark,
                Money = Convert.ToDouble(Money),
                CategoryId = SelectedCategory.Id
            });
            ClearInput();
        }

        /**
         * 清除输入框中的内容
         */
        private void ClearInput()
        {
            CreateTime = DateTime.Now;
            Remark = "";
            Money = "";
            SelectedCategory = null;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}