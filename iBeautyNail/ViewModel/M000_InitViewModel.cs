using iBeautyNail.Configuration;
using iBeautyNail.Devices.NailPrinter;
using iBeautyNail.Enums;
using iBeautyNail.Http;
using iBeautyNail.Language;
using iBeautyNail.SDK;
using iBeautyNail.SDK.Device.Printer;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace iBeautyNail.ViewModel
{
    public class M000_InitViewModel : BaseViewModelBase
    {
        private HwndSource source;

        private HwndSourceHook hook;

        private int result = SDKManagerStatusCode.InitRequired;

        //NailPrinterLib nailPrinter;

        public M000_InitViewModel()
        {
            this.source = HwndSource.FromHwnd(App.Window.HWND);
            this.hook = new HwndSourceHook(WndProc);
        }

        protected override void PageLoad()
        {
            //result = Selfpass.SDK.SDKManagerStatusCode.InitRequired;
            AddHook();

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += (object sender, EventArgs e) =>
            {
                timer.Stop();

                InitDevices();

                // Log file zip 압축
                BackupLogs();
            };
            timer.Start();
        }

        protected override void PageUnload()
        {
            RemoveHook();
        }

        private void InitDevices()
        {
            //nailPrinter = new NailPrinterLib();
            //nailPrinter.Create();
            //nailPrinter.Open(App.Window.HWND);

            // Initialize Devices
            SDKManager.Initialize(ApplicationConfigurationSection.Instance.Machine.ID, App.Window.HWND);
            //Console.Write("SDK Initialize completed.");
            logger.DebugFormat("{0} :: SDK Initialize completed.", CurrentViewModelName);

            result = SDKManager.Connect();
            
            if (result == SDKManagerStatusCode.Success)
            {
                //Console.Write("SDK Connect completed.");
                logger.DebugFormat("{0} :: SDK Connect completed.", CurrentViewModelName);
                Task.Run(() => UpdateMachineStatus(Int32.Parse(ApplicationConfigurationSection.Instance.Machine.ID), "ON"));
                CommandAction(NAVIGATION_TYPE.Next);
               
            }
            else
            {
                //Console.Write("SDK Connect failed.");
                logger.DebugFormat("{0} :: SDK Connect failed.", CurrentViewModelName);
                CommonException();
            }
            //CommandAction(NAVIGATION_TYPE.Next);
        }
        
        private void AddHook()
        {
            source.AddHook(hook);
        }

        private void RemoveHook()
        {
            source.RemoveHook(hook);
        }

        private IntPtr WndProc(IntPtr hwnd, int windowMessage, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            return IntPtr.Zero;
        }

        string logPath = Path.Combine(SystemPath.Base, "Logs");
        private void BackupLogs()
        {
            string targetDate = DateTime.Now.AddMonths(-2).ToString("yyyyMM");

            string[] dirs = Directory.GetDirectories(logPath); //20201002, 20201003 ...
            List<string> folderList1 = new List<string>();

            var r = dirs.Select(x => Path.GetFileName(x).Substring(0, 6)).Distinct();//202009, 202010, 202011

            foreach (string s in r)
            {

                if (Convert.ToInt32(s) <= Convert.ToInt32(targetDate))
                {
                    folderList1.Clear();
                    foreach (string d in dirs)
                    {
                        if (Path.GetFileName(d).StartsWith(s))
                        {
                            if (Path.GetFileName(d) != s)
                                folderList1.Add(d);
                        }
                    }
                    FileCompression_Multiple(folderList1, s);
                }
            }

        }

        private void FileCompression_Multiple(List<string> folderList, string zipFolderName)
        {
            string fileName = Path.Combine(logPath, zipFolderName);
            DirectoryInfo di = new DirectoryInfo(fileName);

            if (!di.Exists)
            {
                di.Create();
            }

            foreach (string a in folderList)
            {
                MoveFolder(a, fileName);
            }

            if (Directory.GetDirectories(fileName).Count() == folderList.Count)
            {
                try
                {
                    ZipFile.CreateFromDirectory(fileName, fileName + ".zip");
                    Directory.Delete(fileName, true);
                }

                catch (Exception ex)
                {

                }
            }
        }

        public void MoveFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(Path.Combine(destFolder, Path.GetFileName(sourceFolder))))
                Directory.Move(sourceFolder, Path.Combine(destFolder, Path.GetFileName(sourceFolder)));
        }

        // UpdateMachineStatus
        private async Task UpdateMachineStatus(int machineId, string status)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            await Api.MonitoringInfo.UpdateMachineStatusAsync(machineId, status);
        }
    }
}
