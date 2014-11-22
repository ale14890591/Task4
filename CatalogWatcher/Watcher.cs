using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConsoleApplication3;

namespace CatalogWatcher
{
    public class Watcher
    {
        public string Catalog { get; set; }
        private FileSystemWatcher catalogWatcher;

        public void Initialization(string path)
        {
            Catalog = path;
        }

        public void Start()
        {
            catalogWatcher = new FileSystemWatcher(Catalog);

            catalogWatcher.Created += null;

            IEnumerable<string> files = System.IO.Directory.GetFiles(Catalog, "*_????????.csv");

            foreach (string s in files)
            {
                string[] contents = ReadFile(s);
                string[] date = contents[0].Split('.');
                //string date = Regex.Match(s, @"\d{8}").ToString();
                //string manager = Regex.Match(s, @"{1,}").ToString();
                AddToBase(new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0])), "m", contents[1], contents[2], Convert.ToInt32(contents[3]));
            }
        }

        public string[] ReadFile(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fs, Encoding.Default))
                {
                    return reader.ReadLine().Split(',');
                }
            }
        }

        public void AddToBase(DateTime date, string manager, string client, string commodity, int price)
        {
            DatabaseWork d =
                new DatabaseWork(
                    @"Data Source=000-ПК\SQLEXPRESS; AttachDbFilename=D:\MyDatabase.mdf; Integrated Security=True");

            if (!d.ClientExists(client))
            {
                d.AddClient(client);
            }
            if (!d.CommodityExists(commodity, price))
            {
                d.AddCommodity(commodity, price);
            }
            if (!d.ManagerExists(manager))
            {
                d.AddManager(manager);
            }

            d.AddOrder(date, manager, client, commodity, price);
        }
    }
}
