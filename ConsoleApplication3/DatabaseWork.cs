using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public class DatabaseWork
    {
        public StoreClassesDataContext db { get; set; }

        public DatabaseWork(string connectionString)
        {
            db = new StoreClassesDataContext(connectionString);
        }

        public void AddClient(string name)
        {
            Client c = new Client() {Name = name};
            db.Client.InsertOnSubmit(c);
            db.SubmitChanges();
        }

        public bool ClientExists(string name)
        {
            return db.Client.Contains(new Client() {Name = name}, new ClientComparer());
        }

        public void AddCommodity(string name, int price)
        {
            Commodity c = new Commodity() {Name = name, Price = price};
            db.Commodity.InsertOnSubmit(c);
            db.SubmitChanges();
        }

        public bool CommodityExists(string name, int price)
        {
            return db.Commodity.Contains(new Commodity() { Name = name, Price = price});
        }

        public void AddManager(string name)
        {
            Manager m = new Manager() {Name = name};
            db.Manager.InsertOnSubmit(m);
            db.SubmitChanges();
        }

        public bool ManagerExists(string name)
        {
            return db.Manager.Contains(new Manager() { Name = name });
        }
        
        public void AddOrder(DateTime date, string manager, string client, string commodity, int price)
        {
            Manager m = db.Manager.First(x => x.Name == manager);
            Client c = db.Client.First(x => x.Name == client);
            Commodity cm = db.Commodity.First(x => x.Name == commodity && x.Price == price);

            Order order = new Order()
            {
                Client = c.IdClient,
                Commodity = cm.IdCommodity,
                Manager = m.IdManager,
                Date = date
            };

            db.Order.InsertOnSubmit(order);
            db.SubmitChanges();
        }
    }
}
