using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyServiceController
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyCustomApplicationContext());
        }
    }

    public class MyCustomApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        ServiceController sc = null;
        public MyCustomApplicationContext()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.Icon1,
                ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Exit", Exit), new MenuItem("Stop service", StopService),
                new MenuItem("Start service", StartService)
            }),
                Visible = true
            };

            this.sc = GetService("MyFirstService");
            if (sc != null)
            {
                trayIcon.ContextMenu.MenuItems[2].Enabled = sc.Status == ServiceControllerStatus.Stopped;
                trayIcon.ContextMenu.MenuItems[1].Enabled = sc.Status != ServiceControllerStatus.Stopped;
            }
        }

        private ServiceController GetService(string name)
        {
            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            foreach (ServiceController scTemp in scServices)
            {

                if (scTemp.ServiceName == name)
                {
                    // Display properties for the Simple Service sample
                    // from the ServiceBase example.
                    this.sc = new ServiceController(scTemp.ServiceName);
                    return sc;
                }
            }

            return sc;
        }

        private void StartService(object sender, EventArgs e)
        {
            if (sc != null && sc.Status == ServiceControllerStatus.Stopped)
            {
                sc.Start();
                while (sc.Status == ServiceControllerStatus.Stopped)
                {
                    Thread.Sleep(1000);
                    sc.Refresh();
                }
                trayIcon.ContextMenu.MenuItems[2].Enabled = false;
                trayIcon.ContextMenu.MenuItems[1].Enabled = true;
            }
        }

        private void StopService(object sender, EventArgs e)
        {
            if (sc != null && sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
                while (sc.Status != ServiceControllerStatus.Stopped)
                {
                    Thread.Sleep(1000);
                    sc.Refresh();
                }
                trayIcon.ContextMenu.MenuItems[2].Enabled = true;
                trayIcon.ContextMenu.MenuItems[1].Enabled = false;
            }
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;
            sc = null;
            Application.Exit();
        }
    }
}
