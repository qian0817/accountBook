using accountBook.view;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace accountBook
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(AccountBook));
        }

        private void NavigationView_SelectionChanged(
            NavigationView sender,
            NavigationViewSelectionChangedEventArgs args
        )
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(SettingPage));
            }
            else
            {
                var selectedItem = (NavigationViewItem)args.SelectedItem;
                sender.Header = selectedItem.Tag;
                var page = GetPageFromTag((string)selectedItem.Tag);
                ContentFrame.Navigate(page);
            }
        }

        private Type GetPageFromTag(string tag)
        {
            switch (tag)
            {
                case "我的账本":
                    return typeof(AccountBook);
                case "添加记录":
                    return typeof(SaveAccount);
                case "统计信息":
                    return typeof(Statistics);
                case "分类管理":
                    return typeof(SaveCategory);
            }
            return  null;
        }
    }
}
