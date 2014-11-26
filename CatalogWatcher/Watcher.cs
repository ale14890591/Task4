using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApplication3;

namespace CatalogWatcher
{
    public class Watcher
    {
        public string Catalog { get; set; }
        private FileSystemWatcher catalogWatcher;
        private static object locker = new object();

        public void Initialization(string path)
        {
            Catalog = path;
        }

        public void Start()
        {
            catalogWatcher = new FileSystemWatcher(Catalog);

            catalogWatcher.Created += null;

            IEnumerable<object> files = System.IO.Directory.GetFiles(Catalog, "*_????????.csv");

            foreach (object s in files)
            {
                Thread fileProcessingThread = new Thread(new ParameterizedThreadStart(ReadFile));
                fileProcessingThread.Start(s);
            }
        }

        public void ReadFile(object fileName)
        {
            OrderListFile file = new OrderListFile();

            string dateTitie = Regex.Match(fileName as string, @"\d{8}").ToString();
            string manager = Regex.Match(fileName as string, @"(\w{1,})(_)").ToString();
            manager = manager.Remove(manager.Length - 1);
            file.Manager = manager;

            using (FileStream fs = new FileStream(fileName as string, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fs, Encoding.Default))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] contents = reader.ReadLine().Split(',');
                        string[] date = contents[0].Split('.');
                        file.AddOrder(new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0])), contents[1], contents[2], Convert.ToInt32(contents[3]));
                    }
                }
            }

            AddToBase(file, fileName as string);

            System.IO.Directory.Move(fileName as string, Catalog + @"\Processed\" + manager + "_" + dateTitie + ".csv");
        }

        public void AddToBase(OrderListFile file, string fileName)
        {
            DatabaseWork d = new DatabaseWork(@"Data Source=000-ПК\SQLEXPRESS; AttachDbFilename=D:\MyDatabase1.mdf; Integrated Security=True");
            
            Monitor.Enter(locker);
            try
            {
                if (!d.ManagerExists(file.Manager))
                    d.AddManager(file.Manager);
            }
            finally
            {
                Monitor.Exit(locker);
            }
            
            foreach (Order i in file.Orders)
            {
                Monitor.Enter(locker);
                try
                {
                    if (!d.ClientExists(i.Client))
                        d.AddClient(i.Client);

                    if (!d.CommodityExists(i.Commodity))
                        d.AddCommodity(i.Commodity);
                }
                finally
                {
                    Monitor.Exit(locker);
                }

                d.AddOrder(i.Date, file.Manager, i.Client, i.Commodity, i.Sum);
            }
        }
    }
}
