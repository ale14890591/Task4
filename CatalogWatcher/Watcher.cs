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
    //public class Watcher : IDisposable
    //{
    //    public string Catalog { get; set; }
    //    private FileSystemWatcher _catalogWatcher;
    //    private TaskFactory tf = new TaskFactory();
    //    private bool _isDisposed = true;

    //    private object lockerForClient = new object();
    //    private object lockerForCommodity = new object();
    //    private object lockerForManager = new object();

    //    public Watcher(string sourcePath)
    //    {
    //        Initialization(sourcePath);
    //    }

    //    public void Initialization(string path)
    //    {
    //        Catalog = path;
    //        StoreClassesDataContext db = new StoreClassesDataContext();
    //        db.CreateDatabase();
    //    }

    //    public void Start()
    //    {
    //        _catalogWatcher = new FileSystemWatcher(Catalog);
    //        var files = System.IO.Directory.GetFiles(Catalog, "*_????????.csv");
    //        foreach (var s in files)
    //        {
    //            StartFileParsingTask(s);
    //        }
            
    //        _catalogWatcher.Created += (sender, e) =>
    //        {
    //            if (e.FullPath != null)
    //            {
    //                StartFileParsingTask(e.FullPath);
    //            }
    //        };
    //        _catalogWatcher.EnableRaisingEvents = true;

    //        _isDisposed = false;
    //    }

        

    //    private void StartFileParsingTask(string fileName)
    //    {
    //            // convert to Task
    //        tf.StartNew((object fn) => { ReadFile(fn as string); }, fileName); 
    //    }

    //    public void ReadFile(string fileName)
    //    {
    //        OrderListFile file = new OrderListFile();

    //        string dateTitie = Regex.Match(fileName as string, @"\d{8}").ToString();
    //        string manager = Regex.Match(fileName as string, @"(\w{1,})(_)").ToString();
    //        manager = manager.Remove(manager.Length - 1);
    //        file.Manager = manager;

    //        using (FileStream fs = new FileStream(fileName as string, FileMode.Open))
    //        {
    //            using (StreamReader reader = new StreamReader(fs, Encoding.Default))
    //            {
    //                while (!reader.EndOfStream)
    //                {
    //                    string[] contents = reader.ReadLine().Split(',');
    //                    string[] date = contents[0].Split('.');
    //                    file.AddOrder(new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0])), contents[1], contents[2], Convert.ToInt32(contents[3]));
    //                }
    //            }
    //        }

    //        AddToBase(file, fileName as string);

    //        System.IO.Directory.Move(fileName as string, Catalog + @"\Processed\" + manager + "_" + dateTitie + ".csv");
    //    }

    //    public void AddToBase(OrderListFile file, string fileName)
    //    {
    //        DatabaseWork d = new DatabaseWork(@"Data Source=000-ПК\SQLEXPRESS; AttachDbFilename=D:\MyDatabase4.mdf; Integrated Security=True");

    //        lock (lockerForManager)
    //        {
    //            if (!d.ManagerExists(file.Manager))
    //                d.AddManager(file.Manager);
    //        }

    //        foreach (Order i in file.Orders)
    //        {
    //            lock (lockerForClient)
    //            {
    //                if (!d.ClientExists(i.Client))
    //                    d.AddClient(i.Client);
    //            }

    //            lock (lockerForCommodity)
    //            {
    //                if (!d.CommodityExists(i.Commodity))
    //                    d.AddCommodity(i.Commodity);
    //            }

    //            d.AddOrder(i.Date, file.Manager, i.Client, i.Commodity, i.Sum);
    //        }
    //    }

        

    //    public void Stop()
    //    {
    //        Dispose();
    //    }

    //    public void Dispose()
    //    {
    //        if (!_isDisposed)
    //        {
    //            if (this._catalogWatcher != null)
    //            {
    //                _catalogWatcher.Dispose();
    //            }
    //        }
    //    }
    //}




    public class Watcher : IDisposable
    {
        public string Catalog { get; set; }
        private FileSystemWatcher _catalogWatcher;
        
        private bool _isDisposed = true;

        private object lockerForClient = new object();
        private object lockerForCommodity = new object();
        private object lockerForManager = new object();

        public Watcher(string sourcePath)
        {
            Initialization(sourcePath);
        }

        public void Initialization(string path)
        {
            Catalog = path;
            StoreClassesDataContext db = new StoreClassesDataContext();
            //db.CreateDatabase();
        }

        public void Start()
        {
            _catalogWatcher = new FileSystemWatcher(Catalog);
            var files = System.IO.Directory.GetFiles(Catalog, "*_????????.csv");
            foreach (var s in files)
            {
                StartFileParsingTask(s);
            }

            _catalogWatcher.Created += (sender, e) =>
            {
                if (e.FullPath != null)
                {
                    StartFileParsingTask(e.FullPath);
                }
            };
            _catalogWatcher.EnableRaisingEvents = true;

            _isDisposed = false;
        }



        private void StartFileParsingTask(string fileName)
        {
            // convert to Task
            //tf.StartNew((object fn) => { ReadFile(fn as string); }, fileName);
            Task task = new Task(new Action<object>(ReadFile), fileName as object);
            task.Start();
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
            //System.IO.Directory.Move(fileName as string, Catalog + @"\Processed\" + manager + "_" + dateTitie + ".csv");
            try
            {
                System.IO.Directory.Move(fileName as string, Catalog + @"\Processed\" + manager + "_" + dateTitie + ".csv");
            }
            catch (Exception)
            {
                System.IO.Directory.Move(fileName as string, Catalog + @"\Processed\" + manager + "_" + dateTitie + "1.csv");
            }
        }

        public void AddToBase(OrderListFile file, string fileName)
        {
            DatabaseWork d = new DatabaseWork(@"Data Source=000-ПК\SQLEXPRESS; AttachDbFilename=D:\MyDatabase5.mdf; Integrated Security=True");

            lock (lockerForManager)
            {
                if (!d.ManagerExists(file.Manager))
                    d.AddManager(file.Manager);
            }

            foreach (Order i in file.Orders)
            {
                lock (lockerForClient)
                {
                    if (!d.ClientExists(i.Client))
                        d.AddClient(i.Client);
                }

                lock (lockerForCommodity)
                {
                    if (!d.CommodityExists(i.Commodity))
                        d.AddCommodity(i.Commodity);
                }

                d.AddOrder(i.Date, file.Manager, i.Client, i.Commodity, i.Sum);
            }
        }



        public void Stop()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (this._catalogWatcher != null)
                {
                    _catalogWatcher.Dispose();
                    _isDisposed = true;
                }
            }
        }
    }
}
