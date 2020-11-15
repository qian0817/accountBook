using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI.Xaml.Controls;
using accountBook.service;
using Microcharts;
using SkiaSharp;

namespace accountBook.view
{
    public sealed partial class Statistics : Page
    {

        private readonly IAccountService _accountService = new AccountServiceImpl();
        private string Year{ get ; set; } = DateTime.Now.Year.ToString();
        private string Month { get; set; } = DateTime.Now.Month.ToString();
        public Statistics()
        {
            this.InitializeComponent();
            ChangeChart();
        }

        private async void ChangeChart()
        {
            var year = Convert.ToInt32(Year);
            var month = Convert.ToInt32(Month);
            var result = await _accountService.GetAccounts(year, month);

            ShowIncomeChart(result);
            ShowSpendingChart(result);
        }

        private void ShowSpendingChart(double[,] result)
        {
            var entries = new List<ChartEntry>();
            for (var i = 0; i < result.Length / 2; i++)
            {
                var height = Convert.ToInt32(result[i, 1]);
                entries.Add(new ChartEntry(height)
                {
                    Label = i.ToString(),
                    ValueLabel = result[i, 1].ToString(CultureInfo.CurrentCulture),
                    Color = SKColor.Parse("#F08080")
                });
            }
            var chart = new LineChart
            {
                Entries = entries
            };
            SpendingChartView.Chart = chart;
        }

        private void ShowIncomeChart(double[,] result)
        {
            var entries = new List<ChartEntry>();
            for (var i = 0; i < result.Length / 2; i++)
            {
                var height = Convert.ToInt32(result[i, 0]);
                entries.Add(new ChartEntry(height)
                {
                    Label = i.ToString(),
                    ValueLabel = result[i, 0].ToString(CultureInfo.CurrentCulture),
                    Color = SKColor.Parse("#98FB98")
                });
            }
            var chart = new LineChart
            {
                Entries = entries
            };
            IncomeChartView.Chart = chart;
        }

        private void ChangeDate(object sender, SelectionChangedEventArgs e)
        {
            ChangeChart();
        }
    }
}
