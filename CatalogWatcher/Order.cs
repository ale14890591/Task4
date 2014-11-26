using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogWatcher
{
    public class Order
    {
        public DateTime Date { get; set; }
        public string Client { get; set; }
        public string Commodity { get; set; }
        public int Sum { get; set; }

        public Order(DateTime date, string client, string commodity, int sum)
        {
            this.Client = client;
            this.Commodity = commodity;
            this.Date = date;
            this.Sum = sum;
        }
    }
}
