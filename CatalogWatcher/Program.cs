using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Watcher w = new Watcher();

            w.Initialization(@"D:\ProjectCatalog");
            w.Start();
        }
    }
}
