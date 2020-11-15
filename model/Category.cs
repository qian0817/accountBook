using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace accountBook.model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; set; }

        public string TypeDesc { get
            {
                switch (Type)
                {
                    case ItemType.Income:return "收入";
                    case ItemType.Spending: return "支出";
                    default:throw new ArgumentException("未知类型");
                }
            } }
    }
}
