using accountBook.service;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace accountBook.view
{
    public sealed partial class SettingPage : Page
    {
        private readonly IAccountService _accountService = new AccountServiceImpl();
        public SettingPage()
        {
            InitializeComponent();
        }

        private async void OutputToExcel(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "output.xlsx"
            };
            savePicker.FileTypeChoices.Add("Excel", new List<string> { ".xlsx" });
            var file = await savePicker.PickSaveFileAsync();
            if (file == null) return;
            var accounts = await _accountService.GetAllAccounts();
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("账本");
            var firstRow = sheet.CreateRow(0);
            firstRow.CreateCell(0).SetCellValue("日期");
            firstRow.CreateCell(1).SetCellValue("分类");
            firstRow.CreateCell(2).SetCellValue("类型");
            firstRow.CreateCell(3).SetCellValue("金额");
            firstRow.CreateCell(4).SetCellValue("备注");

            int index = 0;
            accounts.ForEach(item =>
            {
                index++;
                var row = sheet.CreateRow(index);
                row.CreateCell(0).SetCellValue(item.Year + "-" + item.Month + "-" + item.Day);
                row.CreateCell(1).SetCellValue(item.Category.Name);
                row.CreateCell(2).SetCellValue(item.Category.TypeDesc);
                row.CreateCell(3).SetCellValue(item.Money);
                row.CreateCell(4).SetCellValue(item.Remark);
            });

            CachedFileManager.DeferUpdates(file);
            workbook.Write(await file.OpenStreamForWriteAsync());
            await CachedFileManager.CompleteUpdatesAsync(file);
            await new MessageDialog("导出账单成功").ShowAsync();
        }

        private async void MailToDevloper(object sender, RoutedEventArgs e)
        {
            var uri= new Uri("mailto://201706021425@zjut.edu.cn");
            await Launcher.LaunchUriAsync(uri);
        }

        private async void OpenGithubPage(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("https://github.com/qian0817/accountBook");
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
