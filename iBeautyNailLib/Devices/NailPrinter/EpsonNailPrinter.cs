using iBeautyNail.Http;
using iBeautyNail.Interface;
using iBeautyNail.SDK;
using iBeautyNail.Utility;
using log4net;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml;
using Unity;

namespace iBeautyNail.Devices.NailPrinter
{
    internal interface IEpsonNailPrinter : INailPrinter
    {
        string PartnerDrvPath { get; set; }
    }

    public class EpsonNailPrinter : IEpsonNailPrinter
    {
        protected readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private NailPrinterLib lib;
        private PrinterInfo printerInfo;

        private System.Threading.Thread PrinterThread;
        private System.Threading.Thread PrinterHeadCleanThread;

        private DispatcherTimer headCleanTimer;
        private DispatcherTimer motorTimer;
        private DispatcherTimer motorOffTimer;
        private IniFile printerIni = new IniFile();
        private IniFile headCleanIni = new IniFile();

        public string headCleanTime = "07,00,00";
        public int motorDuring = 30;

        private string partnerDrvPath;
        [Dependency("PartnerDrvPath")]
        public string PartnerDrvPath
        {
            get { return partnerDrvPath; }
            set { partnerDrvPath = value; }
        }

        private string printInkLimitPath;
        [Dependency("PrintInkLimitPath")]
        public string PrintInkLimitPath
        {
            get { return printInkLimitPath; }
            set { printInkLimitPath = value; }
        }

        private string printTemplatePath;
        [Dependency("PrintTemplatePath")]
        public string PrintTemplatePath
        {
            get { return printTemplatePath; }
            set { printTemplatePath = value; }
        }

        private string tempResultPath;
        [Dependency("TempResultPath")]
        public string TempResultPath
        {
            get { return tempResultPath; }
            set { tempResultPath = value; }
        }

        private string resultPath;
        [Dependency("ResultPath")]
        public string ResultPath
        {
            get { return resultPath; }
            set { resultPath = value; }
        }

        private string backupPath;
        [Dependency("BackupPath")]
        public string BackupPath
        {
            get { return backupPath; }
            set { backupPath = value; }
        }

        private string nailPrinterManagerPath;
        [Dependency("NailPrinterManagerPath")]
        public string NailPrinterManagerPath
        {
            get { return nailPrinterManagerPath; }
            set { nailPrinterManagerPath = value; }
        }

        private string qrDataPath;
        [Dependency("QrDataPath")]
        public string QrDataPath
        {
            get { return qrDataPath; }
            set { qrDataPath = value; }
        }

        public EpsonNailPrinter()
        {

        }

        public int Connect()
        {
            lib.Open(SDKManager.WindowMessageHandler.WindowHandle);
            return SDKManagerStatusCode.Success;
        }

        public int Disconnect()
        {
            lib.Close();
            return SDKManagerStatusCode.Success;
        }

        public void Initialize()
        {
            lib = new NailPrinterLib();
            lib.Create();

            printerInfo = new PrinterInfo();

            this.headCleanTimer = new DispatcherTimer();
            this.headCleanTimer.Interval = TimeSpan.FromSeconds(59);
            this.headCleanTimer.Tick += new EventHandler(this.HeadCleanTimerTick);
            this.headCleanTimer.Start();

            this.motorTimer = new DispatcherTimer();
            this.motorTimer.Interval = TimeSpan.FromMinutes(60);
            this.motorTimer.Tick += new EventHandler(this.MotorTimerTick);
            this.motorTimer.Start();

            this.motorOffTimer = new DispatcherTimer();
            this.motorOffTimer.Interval = TimeSpan.FromSeconds(motorDuring);
            this.motorOffTimer.Tick += (object sender, EventArgs e) =>
            {
                motorOffTimer.Stop();
                // motor off
                logger.InfoFormat("[NailManager] Motor Off :: {0}", DateTime.Now);
                SDKManager.NailPrinter.MotorOff();
            };

            try
            {
                TurnExternalProgramOn(PartnerDrvPath);
                TurnExternalProgramOn(NailPrinterManagerPath);

                LoadPrintIni(PrintInkLimitPath, ref printerInfo);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        private void HeadCleanTimerTick(object sender, EventArgs e)
        {
            this.headCleanTimer.Stop();


            if (File.Exists("Configs\\PrintSetting.ini"))
            {
                headCleanIni.Load("Configs\\PrintSetting.ini");
                headCleanTime = headCleanIni["EPSON"]["HeadClean"].ToString();
            }
            string[] tmpTime = headCleanTime.Split(',');

            DateTime nowTime = DateTime.Now;
            DateTime headCleanSetting = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(tmpTime[0]), int.Parse(tmpTime[1]), int.Parse(tmpTime[2]));

            // HeadClean = 1
            if (nowTime.Hour == headCleanSetting.Hour &&
                nowTime.Minute == headCleanSetting.Minute)
            {
                // HeadClean 시작전 Lib 함수 호출
                //lib.PrinterHeadClean();
                TurnExternalProgramOn(PartnerDrvPath);
                TurnExternalProgramOff(NailPrinterManagerPath);

                printerInfo.HeadClean = 1;
                SavePrintIni(PrintInkLimitPath, printerInfo);
                logger.DebugFormat("HeadClean : Now :: {0} / SetTime :: {1} / HeadClean :: {2}", nowTime, headCleanSetting, printerInfo.HeadClean);

                // 헤드클린 후 시스템 종료
                if (nowTime.DayOfWeek >= DayOfWeek.Monday && nowTime.DayOfWeek <= DayOfWeek.Friday)
                {
                    Task.Run(() => UpdateMachineStatus(GetMachineId(), "OFF"));

                    ProcessStartInfo proc = new ProcessStartInfo();
                    proc.FileName = "cmd";
                    proc.WindowStyle = ProcessWindowStyle.Hidden;
                    proc.Arguments = "/C shutdown -f -s -t 10";
                    Process.Start(proc);
                }
            }
            // 5분전에 헤드클린 한번더
            else if (Math.Abs((headCleanSetting.AddMinutes(-5) - nowTime).TotalMinutes) < 1)
            {
                TurnExternalProgramOn(PartnerDrvPath);
                TurnExternalProgramOff(NailPrinterManagerPath);

                printerInfo.HeadClean = 1;
                SavePrintIni(PrintInkLimitPath, printerInfo);
                logger.DebugFormat("HeadClean : Now :: {0} / SetTime :: {1} / HeadClean :: {2}", nowTime, headCleanSetting, printerInfo.HeadClean);
            }

            this.headCleanTimer.Start();
        }

        // Motor 1시간에 한번 30초동안
        private void MotorTimerTick(object sender, EventArgs e)
        {
            this.motorTimer.Stop();

            if (File.Exists("Configs\\PrintSetting.ini"))
            {
                headCleanIni.Load("Configs\\PrintSetting.ini");
                motorDuring = headCleanIni["EPSON"]["MotorDuring"].ToInt();
            }

            logger.InfoFormat("[NailManager] Motor On :: {0}", DateTime.Now);
            SDKManager.NailPrinter.MotorOn();

            this.motorOffTimer.Interval = TimeSpan.FromSeconds(motorDuring);
            this.motorOffTimer.Start();


            this.motorTimer.Start();
        }

        public bool IsConnected()
        {
            return true;
        }

        public int Print(object printData, bool backupStatus = true)
        {
            string fromPath = (string)printData;

            if (!File.Exists(fromPath))
            {
                Console.WriteLine("[Print] No file :: {0}", fromPath);
                logger.DebugFormat("[Print] No file :: {0}", fromPath);
                return NailPrinterWParamType.Nofile;
            }

            try
            {
                // PartnerDriveOn
                TurnExternalProgramOn(PartnerDrvPath);
                TurnExternalProgramOn(NailPrinterManagerPath);

                // printData폴더(temp or backup)의 이미지 데이터를 resultPath로 옮김
                Bitmap bmap = new Bitmap(fromPath);
                if (fromPath.Equals(TempResultPath))
                {
                    bmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    //bmap.Save(BackupPath, System.Drawing.Imaging.ImageFormat.Png);
                    if (backupStatus)
                    {
                        string CreatedFilePath = Path.Combine(Path.GetDirectoryName(BackupPath), "result_") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                        bmap.Save(CreatedFilePath, System.Drawing.Imaging.ImageFormat.Png);
                        logger.DebugFormat("백업 파일 저장 경로 :  {0}", CreatedFilePath);
                    }
                }
                else if (fromPath.Contains(QrDataPath))
                {
                    if (backupStatus)
                    {
                        string CreatedFilePath = Path.Combine(Path.GetDirectoryName(BackupPath), "result_") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                        bmap.Save(CreatedFilePath, System.Drawing.Imaging.ImageFormat.Png);
                        logger.DebugFormat("QR 백업 파일 저장 경로 :  {0}", CreatedFilePath);
                        //bmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                }

                bmap.Save(ResultPath, System.Drawing.Imaging.ImageFormat.Png);
                logger.DebugFormat("{0} -> Result 폴더  result.png 저장 완료", fromPath);
                bmap.Dispose();
                logger.Debug("!@출력");

                // 백업 폴더 이미지가 10개가 넘으면 제일 오래된 파일 1개를 삭제한다
                string dir = Path.GetDirectoryName(BackupPath);
                if (Directory.Exists(dir))
                {
                    DirectoryInfo di = new DirectoryInfo(dir);

                    if (di.GetFiles().Count() > 10)
                    {
                        FileInfo[] files = new DirectoryInfo(dir).GetFiles();

                        FileInfo fi = new FileInfo(files.First().FullName);
                        fi.Delete();
                        logger.DebugFormat("[Print] 백업 폴더 이미지 10개 넘어서 오래된 파일 삭제");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                logger.DebugFormat("[Print] 오류 : {0}", ex.ToString());
                return NailPrinterWParamType.Failure;
            }

            return SDKManagerStatusCode.Success;
        }

        public int State()
        {
            return SDKManagerStatusCode.Success;
        }

        public void MotorOn()
        {
            lib.MotorOn();
        }

        public void MotorOff()
        {
            lib.MotorOff();
        }

        public void TurnExternalProgramOn(string progPath)
        {
            ProcessStartInfo psi = new ProcessStartInfo(progPath)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            string progName = Path.GetFileNameWithoutExtension(progPath);
            Process[] processList = Process.GetProcessesByName(progName);

            if (processList.Length >= 1)
            {
                Console.WriteLine(progName + "★★★★★프로그램 이미 실행중★★★★★" + processList.Length);
                logger.Info(progName + "★★★★★프로그램 이미 실행중★★★★★" + processList.Length);
            }
            else
            {
                Console.WriteLine(progName + "★★★★★실행 중이 아님. 실행하겠습니다.★★★★★");
                logger.Info(progName + "★★★★★실행 중이 아님. 실행하겠습니다.★★★★★");
                Process.Start(psi);
            }
        }

        public void TurnExternalProgramOff(string progPath)
        {
            string progName = Path.GetFileNameWithoutExtension(progPath);
            Process[] processList = Process.GetProcessesByName(progName);

            try
            {
                if (processList.Length >= 1)
                {
                    for (int i = 0; i < processList.Length; i++)
                        processList[i].Kill();

                    logger.Info(progName + "★★★★★프로세스 종료★★★★★");
                }
            }
            catch (Exception ex)
            {

            }

        }

        public Boolean NailPrinterHeadCleanThread()
        {
            PrinterThread = new System.Threading.Thread(delegate ()
            {
                Console.WriteLine(string.Format("EpsonNailPrinter :: NailPrinterDll Thread :: {0}", SDKManager.WindowMessageHandler.WindowHandle.ToInt32()));
                logger.DebugFormat(string.Format("EpsonNailPrinter :: NailPrinterDll Thread :: {0}", SDKManager.WindowMessageHandler.WindowHandle.ToInt32()));
                lib.Open(SDKManager.WindowMessageHandler.WindowHandle);
            });

            if (PrinterThread == null)
                return false;

            PrinterThread.IsBackground = true;
            PrinterThread.Start();
            Console.WriteLine(string.Format("EpsonNailPrinter :: PrinterThread Start!"));
            logger.DebugFormat(string.Format("EpsonNailPrinter :: PrinterThread Start!"));

            return true;
        }

        public void LoadPrintIni(string path, ref PrinterInfo pi)
        {
            if (File.Exists(path) == false)
            {
                Console.WriteLine(string.Format("[Load Print Ini] Print Ini File No Exists :: {0}", path));
                logger.DebugFormat(string.Format("[Load Print Ini] Print Ini File No Exists :: {0}", path));
                return;
            }

            printerIni.Load(path);
            pi.CyanValue = printerIni["Print11"]["Cyan Limit"].ToString();
            pi.MagentaValue = printerIni["Print11"]["Magenta Limit"].ToString();
            pi.YellowValue = printerIni["Print11"]["Yellow Limit"].ToString();
            pi.BlackValue = printerIni["Print11"]["Black Limit"].ToString();
            pi.WhiteValue = printerIni["Print11"]["White Ink Limit"].ToString();

            pi.HeadClean = printerIni["Print11"]["Head Cleaning"].ToInt();

            logger.InfoFormat("LoadPrintIni :: {0}", path);
        }

        public void SavePrintIni(string path, PrinterInfo pi)
        {
            if (File.Exists(path) == false)
            {
                Console.WriteLine(string.Format("[Save Print Ini] Print Ini File No Exists :: {0}", path));
                logger.DebugFormat(string.Format("[Save Print Ini] Print Ini File No Exists :: {0}", path));
                return;
            }

            printerIni.Load(path);

            printerIni["Print11"]["Cyan Limit"] = pi.CyanValue;
            printerIni["Print11"]["Magenta Limit"] = pi.MagentaValue;
            printerIni["Print11"]["Yellow Limit"] = pi.YellowValue;
            printerIni["Print11"]["Black Limit"] = pi.BlackValue;
            printerIni["Print11"]["White Ink Limit"] = pi.WhiteValue;

            printerIni["Print11"]["Head Cleaning"] = pi.HeadClean;

            printerIni.Save(path);
            logger.InfoFormat("SavePrintIni :: {0}", path);
        }

        // UpdateMachineStatus
        private async Task UpdateMachineStatus(int machineId, string status)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            await Api.MonitoringInfo.UpdateMachineStatusAsync(machineId, status);
        }

        private int GetMachineId()
        {
            var filePath = @"C:\nailpod\Configs\application.config";
            if (File.Exists(filePath))
            {
                try
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(filePath);
                    XmlElement root = xmldoc.DocumentElement;

                    XmlNodeList nodes = root.ChildNodes;

                    XmlElement appNode = (XmlElement)root.SelectSingleNode("machine");

                    return Int32.Parse(appNode.GetAttribute("id"));
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }

            return -1;
        }
    }
}