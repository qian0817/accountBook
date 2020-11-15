
using System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace accountBook.model
{
    public class AccountItem
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Remark { get; set; }
        public double Money { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public DateTime CreateTime => new DateTime(Year, Month, Day);

        public string MoneyToString
        {
            get
            {
                if (Category.Type == ItemType.Income)
                {
                    return "+" + Money;
                }
                return "-" + Money;
            }
        }

        public Brush TextColor
        {
            get
            {
                if (Category.Type == ItemType.Income)
                {
                    var color = Color.FromArgb(0xff, 0x5a, 0xb9, 0x8c);
                    return new SolidColorBrush(color);
                }
                else
                {
                    var color = Color.FromArgb(0xff, 0xd9, 0x6b, 0x68);
                    return new SolidColorBrush(color);
                }
            }
        }
    }
}
