using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using accountBook.model;
using accountBook.service;

namespace accountBook.view
{
    public sealed partial class AccountBook : Page, INotifyPropertyChanged
    {
        private readonly IAccountService _accountService = new AccountServiceImpl();
        private string Year { get; set; } = DateTime.Now.Year.ToString();
        private string Month { get; set; } = DateTime.Now.Month.ToString();

        private string _incomeMoney = "0";

        public string IncomeMoney
        {
            get => _incomeMoney;
            set
            {
                if (_incomeMoney == value) return;
                _incomeMoney = value;
                OnPropertyChanged();
            }
        }

        private string _spendingMoney = "0";

        public string SpendingMoney
        {
            get => _spendingMoney;
            set
            {
                if (_spendingMoney == value) return;
                _spendingMoney = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AccountItem> AccountList { get; set; } = new ObservableCollection<AccountItem>();

        public AccountBook()
        {
            InitializeComponent();
            LoadAccountBook();
        }

        /**
         * 加载账单内容
         */
        private async void LoadAccountBook()
        {
            var year = Convert.ToInt32(Year);
            var month = Convert.ToInt32(Month);
            var ans = await _accountService.GetAccountOrderByDate(year, month);
            AccountList.Clear();
            // 存储所有收益总和
            var incomeMoney = 0.0;
            // 存储所有花费总和
            var spendingMoney = 0.0;
            ans.ForEach(item =>
            {
                if (item.Category.Type == ItemType.Income)
                {
                    incomeMoney += item.Money;
                }
                else
                {
                    spendingMoney += item.Money;
                }

                AccountList.Add(item);
            });
            IncomeMoney = incomeMoney.ToString(CultureInfo.CurrentCulture);
            SpendingMoney = spendingMoney.ToString(CultureInfo.CurrentCulture);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAccountBook();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void ShowUpdateAccountItemDialog(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var accountItemId = Convert.ToInt32(button.Tag);
            var dialog = new UpdateAccountItemDialog(accountItemId);
            var result = await dialog.ShowAsync();
            switch (result)
            {
                case ContentDialogResult.Primary:
                    var item = new AccountItem
                    {
                        Id = accountItemId,
                        Year = dialog.CreateTime.Year,
                        Month = dialog.CreateTime.Month,
                        Day = dialog.CreateTime.Day,
                        Remark = dialog.Remark,
                        Money = Convert.ToDouble(dialog.Money),
                        CategoryId = dialog.SelectedCategory.Id
                    };
                    _accountService.Update(item);
                    LoadAccountBook();
                    break;
                case ContentDialogResult.Secondary:
                    _accountService.DeleteById(accountItemId);
                    LoadAccountBook();
                    break;
                case ContentDialogResult.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}