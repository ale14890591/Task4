using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ConsoleApplication3;

namespace ConsoleWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleWatcherClass q = new ConsoleWatcherClass();

            q.Watch(@"D:\ProjectCatalog");
            
            //while (true)
            //{
            //    int i = 0;
            //    i++;
            //}
        }

        static void Func()
        {
            while (true)
            {
                int i = 0;
                i++;
            }
        }
    }
}
