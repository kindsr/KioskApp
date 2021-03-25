using iBeautyNail.Datas;
using iBeautyNail.Extensions;
using iBeautyNail.Language;
using log4net;
using System;
using System.Windows;

namespace iBeautyNail
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static MainWindow Window { get { return App.Current.MainWindow as MainWindow; } }

        public static UnityContainerManager Container { get; private set; }

        public static LanguageMng LanguageMng { get; private set; }

        public static DesignMng DesignMng { get; private set; }

        private CommandSocketServer commandSocketServer;

        public App()
        {
            logger.DebugFormat("iBeautyNail Start :: {0}", DateTime.Now);

            this.Startup += App_Startup;
            Application.Current.Exit += Current_Exit;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            CreateContrainer();
            CreateLanguage();
            CreateDesign();
            //CreateCommandServer();
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            //commandSocketServer.Stop();
        }

        private void CreateContrainer()
        {
            Container = UnityContainerManager.Instance;
            Container.CreateContainer();
        }

        private void CreateLanguage()
        {
            LanguageMng = new LanguageMng();
        }

        private void CreateDesign()
        {
            DesignMng = new DesignMng();
        }

        private void CreateCommandServer()
        {
            commandSocketServer = new CommandSocketServer();
            commandSocketServer.ReceviedMessage += (msg) =>
            {
                int wparam = -1;

                //if (msg.Equals("Enable"))
                //{
                //    wparam = ExWindowMessage.ENABLE;
                //}
                //else if (msg.Equals("Disable"))
                //{
                //    wparam = ExWindowMessage.DISABLE;
                //}
                //else if (msg.Equals("Reboot"))
                //{
                //    wparam = ExWindowMessage.REBOOT;
                //}
                //else if (msg.Equals("Send"))
                //{
                //    wparam = ExWindowMessage.BHS;
                //}

                //if (wparam > -1)
                //{
                //    int hWnd = Win32API.FindWindow(null, "MainWindow");
                //    if (hWnd > 0)
                //    {
                //        logger.Debug($"Message From External Systems, msg: {msg} wparam: {wparam}");
                //        Win32API.SendMessage((IntPtr)hWnd, ExWindowMessage.WM_APP, (IntPtr)wparam, IntPtr.Zero);
                //    }
                //}
            };
            commandSocketServer.Start();
        }
        
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logger.Error(e.Exception.Message);

            if (e.Exception is Exception)
            {
            }
            else
            {

            }

            e.Handled = true;
        }
    }
}
