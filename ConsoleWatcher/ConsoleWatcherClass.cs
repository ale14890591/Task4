using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CatalogWatcher;

namespace ConsoleWatcher
{
    public class ConsoleWatcherClass
    {
        private CatalogWatcher.Watcher _watcher = new Watcher();

        public void Watch(string catalog)
        {
            _watcher.Initialization(catalog);
            Check();
            //_watcher.Start();

            //checkIntervalHours *= 3600000;
            //System.Timers.Timer timer = new System.Timers.Timer(checkIntervalHours);
            //timer.Elapsed += new ElapsedEventHandler(Rrr);
            //timer.Enabled = true;

            //Timer t = new System.Threading.Timer()
            //Check();
        }

        public void TimerEvent(object sender, EventArgs e)
        {
            _watcher.Start();
        }

        private void Check()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            timer.Interval = 60000;
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();
        }

        private void St()
        {
            Thread thread = new Thread(new ThreadStart(Check));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
