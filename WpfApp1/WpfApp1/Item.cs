using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Item
    {
        public string ItemName { get; set; }
        public int PriceOfItem { get; set; }
        
        public Item(string name, string price)
        {
            ItemName = name;
            PriceOfItem = int.Parse(price);
        }
    }
}
