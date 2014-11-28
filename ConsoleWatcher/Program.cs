using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ConsoleApplication3;
using CatalogWatcher;

namespace ConsoleWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Watcher cw = new Watcher(@"D:\ProjectCatalog"))
            {
                cw.Start();
                Console.ReadKey();
            }
        }
    }
}
