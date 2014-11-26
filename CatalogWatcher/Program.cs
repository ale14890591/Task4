using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication3;

namespace CatalogWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            StoreClassesDataContext db = new StoreClassesDataContext(@"Data Source=000-ПК\SQLEXPRESS; AttachDbFilename=D:\MyDatabase1.mdf; Integrated Security=True");
            db.CreateDatabase();

            Watcher w = new Watcher();

            w.Initialization(@"D:\ProjectCatalog");
            w.Start();
        }
    }
}
