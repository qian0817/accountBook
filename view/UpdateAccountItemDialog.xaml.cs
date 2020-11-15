using accountBook.model;
using accountBook.service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;

namespace accountBook.view
{
    public sealed partial class UpdateAccountItemDialog: ContentDialog, INotifyPropertyChanged
    {

        private readonly IAccountService _accountService = new AccountServiceImpl();
        private readonly ICategoryService _categoryService = new CategoryServiceImpl();
        public List<Tuple<string, Category>> Categories { get; set; } = new List<Tuple<string, Category>>();
        public Category SelectedCategory { get; set; }
        private DateTimeOffset _createTime = DateTime.Now;
        public DateTimeOffset CreateTime
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

        public string Money
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

        public string Remark
        {
            get => _remark;
            set
            {
                if (_remark == value) return;
                _remark = value;
                OnPropertyChanged();
            }
        }

        public UpdateAccountItemDialog(int accountItemId)
        {
            InitializeComponent();
            LoadAccountItem(accountItemId);
        }

        private async void LoadAccountItem(int accountItemId)
        {
            var accountItem =await _accountService.GetItemById(accountItemId);
            var list = await _categoryService.GetAllCategoryByType(accountItem.Category.Type);
            Categories.Clear();
            list.ForEach(item => Categories.Add(new Tuple<string, Category>(item.Name, item)));
            Money = accountItem.Money.ToString(CultureInfo.CurrentCulture);
            Remark = accountItem.Remark;
            CreateTime = accountItem.CreateTime;
            SelectedCategory = Categories.Find(item=>item.Item2.Id==accountItem.CategoryId).Item2;
        }




        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
