using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using CatalogWatcher;

namespace WatchService
{
    public partial class Service1 : ServiceBase
    {
        private Watcher cw = new Watcher(@"D:\ProjectCatalog");

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                cw.Start();
            }
            catch (Exception e)
            {
                AddLog(e.Message);
            }
            
        }

        protected override void OnStop()
        {
            cw.Stop();
        }

        public void AddLog(string log)
        {
            try
            {
                if (!EventLog.SourceExists("MyExampleService"))
                {
                    EventLog.CreateEventSource("MyExampleService", "MyExampleService");
                }
                eventLog1.Source = "MyExampleService";
                eventLog1.WriteEntry(log);
            }
            catch { }
        }
    }
}
