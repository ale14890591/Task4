using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogWatcher
{
    public class OrderListFile
    {
        public string Manager { get; set; }
        public List<Order> Orders = new List<Order>();

        public void AddOrder(DateTime date, string client, string commodity, int sum)
        {
            this.Orders.Add(new Order(date, client, commodity, sum));
        }
    }
}
