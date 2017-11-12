using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyFirstService
{
    public partial class MyFirstService : ServiceBase
    {
        private StreamWriter file;
        public MyFirstService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            file = new StreamWriter(new FileStream("MyFirstService.log", FileMode.Append));
            this.file.WriteLine(DateTime.Now.ToString() + this.ServiceName + " started!");
            this.file.Flush();
        }

        protected override void OnStop()
        {
            this.file.WriteLine(DateTime.Now.ToString() + this.ServiceName + " stopped!");
            this.file.Flush();
            this.file.Close();
        }
    }
}
