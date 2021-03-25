using GalaSoft.MvvmLight.CommandWpf;
using iBeautyNail.Configuration;
using iBeautyNail.Datas;
using iBeautyNail.Devices.NailPrinter;
using iBeautyNail.Devices.ReceiptPrinter;
using iBeautyNail.Enums;
using iBeautyNail.Extensions.Converters;
using iBeautyNail.Http;
using iBeautyNail.Http.Endpoints.MonitoringInfoEndpoint.Models;
using iBeautyNail.Http.Endpoints.PaymentInfoEndpoint.Models;
using iBeautyNail.Interface;
using iBeautyNail.Messages;
using iBeautyNail.Messages.Exceptions.Enums;
using iBeautyNail.SDK;
using iBeautyNail.Utility;
using iBeautyNail.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Xml;

namespace iBeautyNail.ViewModel
{
    public class A010_AdminBaseViewModel : BaseViewModelBase
    {
        private HwndSource source;
        private HwndSourceHook hook;

        private DispatcherTimer deviceStatusTimer;
        private DispatcherTimer backupImagePrintTimer;
        private TabItem SelectedTab;
        private IniFile iniInkLimit = new IniFile();
        private EpsonNailPrinter enp = new EpsonNailPrinter();
        private PrinterInfo printerInfo;
        private IniFile headCleanIni = new IniFile();
        private IniFile templateIni = new IniFile();
        private IniFile cordinatorIni = new IniFile();
        private string headCleanTime = "03,00,00";
        private int printed = 0;
        private int printCount = 0;
        private string selectedNailPos = string.Empty;
        private bool canPrint = false;

        #region Define Variables Number 0~9
        private string number0 = "0";
        public string Number0
        {
            get { return number0; }
            set { Set(() => Number0, ref number0, value); }
        }
        private string number1 = "1";
        public string Number1
        {
            get { return number1; }
            set { Set(() => Number1, ref number1, value); }
        }
        private string number2 = "2";
        public string Number2
        {
            get { return number2; }
            set { Set(() => Number2, ref number2, value); }
        }
        private string number3 = "3";
        public string Number3
        {
            get { return number3; }
            set { Set(() => Number3, ref number3, value); }
        }
        private string number4 = "4";
        public string Number4
        {
            get { return number4; }
            set { Set(() => Number4, ref number4, value); }
        }
        private string number5 = "5";
        public string Number5
        {
            get { return number5; }
            set { Set(() => Number5, ref number5, value); }
        }
        private string number6 = "6";
        public string Number6
        {
            get { return number6; }
            set { Set(() => Number6, ref number6, value); }
        }
        private string number7 = "7";
        public string Number7
        {
            get { return number7; }
            set { Set(() => Number7, ref number7, value); }
        }
        private string number8 = "8";
        public string Number8
        {
            get { return number8; }
            set { Set(() => Number8, ref number8, value); }
        }
        private string number9 = "9";
        public string Number9
        {
            get { return number9; }
            set { Set(() => Number9, ref number9, value); }
        }
        #endregion

        #region Properties
        private StringBuilder Input { get; set; }

        private string inputValue;
        public string InputValue
        {
            get { return inputValue; }
            set { Set(() => InputValue, ref inputValue, value); }
        }

        private string selectedFile;
        public string SelectedFile
        {
            get { return selectedFile; }
            set
            {
                selectedFile = value;
                RaisePropertyChanged("SelectedFile");
            }
        }

        private string cleanHour;
        public string CleanHour
        {
            get { return cleanHour; }
            set
            {
                cleanHour = value;
                RaisePropertyChanged("CleanHour");
            }
        }

        private string cleanMinute;
        public string CleanMinute
        {
            get { return cleanMinute; }
            set
            {
                cleanMinute = value;
                RaisePropertyChanged("CleanMinute");
            }
        }

        private string cleanSecond;
        public string CleanSecond
        {
            get { return cleanSecond; }
            set
            {
                cleanSecond = value;
                RaisePropertyChanged("CleanSecond");
            }
        }

        private string itemWidth;
        public string ItemWidth
        {
            get { return itemWidth; }
            set
            {
                itemWidth = value;
                RaisePropertyChanged("ItemWidth");
            }
        }

        private string itemHeight;
        public string ItemHeight
        {
            get { return itemHeight; }
            set
            {
                itemHeight = value;
                RaisePropertyChanged("ItemHeight");
            }
        }

        private string itemRight;
        public string ItemRight
        {
            get { return itemRight; }
            set
            {
                itemRight = value;
                RaisePropertyChanged("ItemRight");
            }
        }

        private string itemBottom;
        public string ItemBottom
        {
            get { return itemBottom; }
            set
            {
                itemBottom = value;
                RaisePropertyChanged("ItemBottom");
            }
        }

        private string sign1;
        public string Sign1
        {
            get { return sign1; }
            set
            {
                sign1 = value;
                RaisePropertyChanged("Sign1");
            }
        }

        private string sign2;
        public string Sign2
        {
            get { return sign2; }
            set
            {
                sign2 = value;
                RaisePropertyChanged("Sign2");
            }
        }

        private string price;
        public string Price
        {
            get { return price; }
            set { Set(() => Price, ref price, value); }
        }

        private string cyanValue;
        public string CyanValue
        {
            get { return cyanValue; }
            set { Set(() => CyanValue, ref cyanValue, value); }
        }

        private string magentaValue;
        public string MagentaValue
        {
            get { return magentaValue; }
            set { Set(() => MagentaValue, ref magentaValue, value); }
        }

        private string yellowValue;
        public string YellowValue
        {
            get { return yellowValue; }
            set { Set(() => YellowValue, ref yellowValue, value); }
        }

        private string blackValue;
        public string BlackValue
        {
            get { return blackValue; }
            set { Set(() => BlackValue, ref blackValue, value); }
        }

        private string whiteValue;
        public string WhiteValue
        {
            get { return whiteValue; }
            set { Set(() => WhiteValue, ref whiteValue, value); }
        }

        private bool headCleanOffChecked;
        public bool HeadCleanOffChecked
        {
            get { return headCleanOffChecked; }
            set { Set(() => HeadCleanOffChecked, ref headCleanOffChecked, value); }
        }

        private bool headCleanOnChecked;
        public bool HeadCleanOnChecked
        {
            get { return headCleanOnChecked; }
            set { Set(() => HeadCleanOnChecked, ref headCleanOnChecked, value); }
        }

        private int totalPrintCount;
        public int TotalPrintCount
        {
            get { return totalPrintCount; }
            set
            {
                Set(() => TotalPrintCount, ref totalPrintCount, value);
            }
        }

        private bool adminVisible;
        public bool AdminVisible
        {
            get { return adminVisible; }
            set { Set(() => AdminVisible, ref adminVisible, value); }
        }

        private string nailLeft;
        public string NailLeft
        {
            get { return nailLeft; }
            set { Set(() => NailLeft, ref nailLeft, value); }
        }

        private string nailTop;
        public string NailTop
        {
            get { return nailTop; }
            set { Set(() => NailTop, ref nailTop, value); }
        }

        private string nailWidth;
        public string NailWidth
        {
            get { return nailWidth; }
            set { Set(() => NailWidth, ref nailWidth, value); }
        }

        private string nailHeight;
        public string NailHeight
        {
            get { return nailHeight; }
            set { Set(() => NailHeight, ref nailHeight, value); }
        }

        private string receiptPrinterStatus;
        public string ReceiptPrinterStatus
        {
            get { return receiptPrinterStatus; }
            set { Set(() => ReceiptPrinterStatus, ref receiptPrinterStatus, value); }
        }

        private string nailPrinterManagerStatus;
        public string NailPrinterManagerStatus
        {
            get { return nailPrinterManagerStatus; }
            set { Set(() => NailPrinterManagerStatus, ref nailPrinterManagerStatus, value); }
        }

        private string cardReaderStatus;
        public string CardReaderStatus
        {
            get { return cardReaderStatus; }
            set { Set(() => CardReaderStatus, ref cardReaderStatus, value); }
        }

        private int totalPrintedCount;
        public int TotalPrintedCount
        {
            get { return totalPrintedCount; }
            set
            {
                Set(() => TotalPrintedCount, ref totalPrintedCount, value);
            }
        }

        private string approvalNumber;
        public string ApprovalNumber
        {
            get { return approvalNumber; }
            set
            {
                approvalNumber = value;
                RaisePropertyChanged("ApprovalNumber");
            }
        }

        private string cancelPrice;
        public string CancelPrice
        {
            get { return cancelPrice; }
            set
            {
                cancelPrice = value;
                RaisePropertyChanged("CancelPrice");
            }
        }

        private string approvalDate;
        public string ApprovalDate
        {
            get { return approvalDate; }
            set
            {
                approvalDate = value;
                RaisePropertyChanged("ApprovalDate");
            }
        }

        private string cancelPaymentMsg;
        public string CancelPaymentMsg
        {
            get { return cancelPaymentMsg; }
            set
            {
                cancelPaymentMsg = value;
                RaisePropertyChanged("CancelPaymentMsg");
            }
        }

        private string machineID;
        public string MachineID
        {
            get { return machineID; }
            set
            {
                machineID = value;
                RaisePropertyChanged("MachineID");
            }
        }

        private string appVersion;
        public string AppVersion
        {
            get { return appVersion; }
            set
            {
                appVersion = value;
                RaisePropertyChanged("AppVersion");
            }
        }

        private string motorOnOff;
        public string MotorOnOff
        {
            get { return motorOnOff; }
            set
            {
                motorOnOff = value;
                RaisePropertyChanged("MotorOnOff");
            }
        }

        private ObservableCollection<int> inkVol = new ObservableCollection<int>();
        public ObservableCollection<int> InkVol
        {
            get { return inkVol; }
            set
            {
                Set(() => InkVol, ref inkVol, value);
            }
        }

        private string setTTS;
        public string SetTTS
        {
            get { return setTTS; }
            set
            {
                setTTS = value;
                RaisePropertyChanged("SetTTS");
            }
        }

        private string setPayment;
        public string SetPayment
        {
            get { return setPayment; }
            set
            {
                setPayment = value;
                RaisePropertyChanged("SetPayment");
            }
        }
        #endregion

        private int passwdStep { get; set; }
        private string passwd1st { get; set; }
        private string passwd2nd { get; set; }

        public A010_AdminBaseViewModel()
        {
            this.source = HwndSource.FromHwnd(App.Window.HWND);
            this.hook = new HwndSourceHook(WndProc);

            enp = new EpsonNailPrinter();
            printerInfo = new PrinterInfo();

            backupImageList = new ObservableCollection<QRImageListInfo>();

            PrevButtonVisible = false;
            NextButtonVisible = false;
        }

        protected override void PageLoad()
        {
            AddHook();

            this.Input = new StringBuilder();
            this.InputValue = string.Empty;

            this.deviceStatusTimer = new DispatcherTimer();
            this.deviceStatusTimer.Interval = TimeSpan.FromMilliseconds(5000);
            this.deviceStatusTimer.Tick += new EventHandler(this.DeviceStatusTimerTick);
            this.deviceStatusTimer.Start();

            this.backupImagePrintTimer = new DispatcherTimer();
            this.backupImagePrintTimer.Interval = TimeSpan.FromMilliseconds(1000);
            this.backupImagePrintTimer.Tick += new EventHandler(this.BackupImagePrintTimerTick);

            SelectedTab = new TabItem();
            SelectedTab.Name = "tabDevice";

            this.passwdStep = 1;

            this.printed = 0;
            this.printCount = 0;

            this.selectedNailPos = string.Empty;

            AdminVisible = GlobalVariables.Instance.IsAdmin;

            LoadBackupImages(Path.GetDirectoryName(Datas.GlobalVariables.Instance.BackupPath));

            // Device Status
            ReceiptPrinterStatus = SDKManager.ReceiptPrinter.IsConnected() ? "Connected" : "Disconnected";
            CardReaderStatus = SDKManager.CardPayment.Connect() == SDKManagerStatusCode.Success ? "OK" : "Error";
            Task.Run(async () => await GetPrintCount());

            CancelPaymentMsg = "Refund";
            MotorOnOff = "MotorOn";

            MachineID = Configuration.ApplicationConfigurationSection.Instance.Machine.ID;
            AppVersion = Configuration.ApplicationConfigurationSection.Instance.Machine.AppVersion;

            InkVol.Clear();
            for (int i = 0; i < GlobalVariables.Instance.InkVol.Length; i++)
            {
                if (GlobalVariables.Instance.InkVol[i] > 0)
                {
                    InkVol.Add(GlobalVariables.Instance.InkVol[i]);
                }
            }
        }

        protected override void PageUnload()
        {
            if (this.deviceStatusTimer.IsEnabled)
                this.deviceStatusTimer.Stop();

            if (this.backupImagePrintTimer.IsEnabled)
                this.backupImagePrintTimer.Stop();

            RemoveHook();
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
            switch (windowMessage)
            {
                case WindowMessage.PCU_MSG_STATE:
                    if (wParam == (IntPtr)WindowMessage.PCU_MSG_STATE_PRINT)
                    {
                        if (lParam == (IntPtr)1)
                        {
                            // 프린트 시작
                            logger.DebugFormat("{0} :: WndProc Start :: {1} :: {2}", CurrentViewModelName, wParam.ToInt32(), lParam.ToInt32());
                        }
                        else if (lParam == (IntPtr)0)
                        {
                            logger.DebugFormat("{0} :: WndProc Finish :: {1} :: {2}", CurrentViewModelName, wParam.ToInt32(), lParam.ToInt32());
                            canPrint = true;
                            printed++;
                            logger.DebugFormat("총 출력해야할 수량 : {0}장 :: 출력된 수량 : {1}장", printCount, printed);

                            if (totalPrintCount <= 0 && printed >= printCount)
                            {
                                backupImagePrintTimer.Stop();
                                // 출력완료
                                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                                {
                                    Button1Command();
                                }));
                            }
                        }
                    }
                    else if (wParam == (IntPtr)WindowMessage.PCU_MSG_STATE_PRINTER)
                    {
                        logger.DebugFormat("{0} :: WndProc PCU_MSG_STATE_PRINTER :: {1} :: {2}", CurrentViewModelName, wParam.ToInt32(), lParam.ToInt32());

                        // 오류 시 NailManager 종료
                        if (lParam != (IntPtr)0)
                        {
                            string progName = Path.GetFileNameWithoutExtension("NailPrinterManager.exe");
                            Process[] processList = Process.GetProcessesByName(progName);

                            if (processList.Length >= 1)
                            {
                                for (int i = 0; i < processList.Length; i++)
                                    processList[i].Kill();
                                logger.DebugFormat("{0} :: WndProc NailPrinterManager Process End", CurrentViewModelName);
                            }
                        }

                        // 팝업 
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            ShowMessageLayerError();
                        }));
                    }
                    break;
                case WindowMessage.PCU_MSG_INK_VALUE:
                    logger.DebugFormat("{0} :: WndProc PCU_MSG_INK_VALUE :: {1} :: {2}", CurrentViewModelName, wParam.ToInt32(), lParam.ToInt32());
                    GlobalVariables.Instance.InkVol[wParam.ToInt32()] = lParam.ToInt32();
                    break;
            }
            return IntPtr.Zero;
        }

        #region TabItemCommand
        private RelayCommand<object> changeTabItemCommand;
        public RelayCommand<object> ChangeTabItemCommand
        {
            get
            {
                return new RelayCommand<object>((tabItem) =>
                {
                    SelectedTab = (TabItem)tabItem;
                    DeviceStatusTimer((TabItem)tabItem);
                });
            }
        }

        private void DeviceStatusTimer(TabItem tabItem)
        {
            if (tabItem.Name.ToString() == "tabDevice")
            {
                if (deviceStatusTimer.IsEnabled == false)
                    deviceStatusTimer.Start();
            }
            else
            {
                //if (deviceStatusTimer.IsEnabled)
                this.deviceStatusTimer.Stop();

                if (tabItem.Name.ToString() == "tabPassword")
                    this.StirKeypad();
                if (tabItem.Name.ToString() == "tabSetting")
                {
                    //LoadPrintInkLimit(Datas.GlobalVariables.Instance.PrintInkLimitPath);
                    enp.LoadPrintIni(Datas.GlobalVariables.Instance.PrintInkLimitPath, ref printerInfo);
                    CyanValue = printerInfo.CyanValue;
                    MagentaValue = printerInfo.MagentaValue;
                    YellowValue = printerInfo.YellowValue;
                    BlackValue = printerInfo.BlackValue;
                    WhiteValue = printerInfo.WhiteValue;
                    if (printerInfo.HeadClean == 1)
                        HeadCleanOnChecked = true;
                    else if (printerInfo.HeadClean == 0)
                        HeadCleanOffChecked = true;

                    if (File.Exists("Configs\\PrintSetting.ini"))
                    {
                        headCleanIni.Load("Configs\\PrintSetting.ini");
                        headCleanTime = headCleanIni["EPSON"]["HeadClean"].ToString();

                        Price = headCleanIni["DESIGN"]["Price"].ToString();
                        Sign1 = headCleanIni["SIGN"]["Sign1"].ToString();
                        Sign2 = headCleanIni["SIGN"]["Sign2"].ToString();

                        SetTTS = headCleanIni["MODE"]["TTS"].ToBool() ? "On" : "Off";
                        SetPayment = headCleanIni["MODE"]["Payment"].ToBool() ? "On" : "Off";
                    }

                    if (File.Exists(Datas.GlobalVariables.Instance.PrintTemplatePath))
                    {
                        templateIni.Load(Datas.GlobalVariables.Instance.PrintTemplatePath);
                        string[] tmpItemSize = templateIni["Template11"]["Item Size"].ToString().Split(',');
                        ItemWidth = tmpItemSize[0];
                        ItemHeight = tmpItemSize[1];
                        string[] tmpItemLoc = templateIni["Template11"]["Start margin"].ToString().Split(',');
                        ItemRight = tmpItemLoc[0];
                        ItemBottom = tmpItemLoc[1];
                    }

                    // Head Clean Timeset
                    string[] tmpTime = headCleanTime.Split(',');
                    CleanHour = tmpTime[0];
                    CleanMinute = tmpTime[1];
                    CleanSecond = tmpTime[2];
                }

            }
        }

        private void DeviceStatusTimerTick(object sender, EventArgs e)
        {
            this.deviceStatusTimer.Stop();

            //DeviceStatusTimer(SelectedTab);
            // 상태 처리
            int res = SDKManager.ReceiptPrinter.State();
            switch (res)
            {
                case SDKManagerStatusCode.Success:
                    ReceiptPrinterStatus = "OK";
                    break;
                case SDKManagerStatusCode.Disconnected:
                    ReceiptPrinterStatus = "Disconnected";
                    break;
                case SDKManagerStatusCode.ConfigFailed:
                    ReceiptPrinterStatus = "ConfigFailed";
                    break;
                case ReceiptPrinterWParamType.Timeout:
                    ReceiptPrinterStatus = "Timeout";
                    break;
                default:
                    if (res == ReceiptPrinterWParamType.PaperEmpty)   //error
                        ReceiptPrinterStatus = (("PAPER_EMPTY\n"));

                    if (res == ReceiptPrinterWParamType.HeadUp)   //error
                        ReceiptPrinterStatus = (("HEAD_UP\n"));

                    if (res == ReceiptPrinterWParamType.CutError)   //error
                        ReceiptPrinterStatus = (("CUT_ERROR\n"));

                    if (res == ReceiptPrinterWParamType.NearEnd)  //Warning
                        ReceiptPrinterStatus = (("NEAR_END\n"));

                    if (res == ReceiptPrinterWParamType.OutSensor)  //Warning, error
                        ReceiptPrinterStatus = (("PR_OUT_SENSOR\n"));

                    break;
            }

            res = SDKManager.CardPayment.State();
            switch (res)
            {
                case SDKManagerStatusCode.Success:
                    CardReaderStatus = "OK";
                    break;
                case CardPaymentWParamType.CardInserted:
                    CardReaderStatus = "Inserted";
                    break;
                case CardPaymentWParamType.CardEmpty:
                    CardReaderStatus = "Empty";
                    break;
                case CardPaymentWParamType.Failure:
                    CardReaderStatus = "Error";
                    break;
                default:
                    break;
            }

            InkVol.Clear();
            for (int i = 0; i < GlobalVariables.Instance.InkVol.Length; i++)
            {
                if (GlobalVariables.Instance.InkVol[i] > 0)
                {
                    InkVol.Add(GlobalVariables.Instance.InkVol[i]);
                }
            }

            this.deviceStatusTimer.Start();
        }
        #endregion TabItemCommand


        #region ClickCommand
        private RelayCommand<string> clickCommand;
        public RelayCommand<string> ClickCommand
        {

            get
            {
                return clickCommand ?? new RelayCommand<string>((btn) =>
                {
                    if (Int32.TryParse(btn, out int btnTag))
                    {
                        PasswordProcess(btnTag);
                    }
                    else
                    {
                        switch (SelectedTab.Name.ToString())
                        {
                            case "tabDevice":
                                DeviceProcess(btn);
                                break;
                            case "tabSetting":
                                SettingProcess(btn);
                                break;
                            case "tabBackupImages":
                                BackupImageProcess(btn);
                                break;
                            case "tabExpertSetting":
                                ExpertSettingProcess(btn);
                                break;
                            default:
                                break;
                        }

                    }
                });
            }
        }

        private RelayCommand<string> nailClickCommand;
        public RelayCommand<string> NailClickCommand
        {
            get
            {
                return nailClickCommand ?? new RelayCommand<string>((btn) =>
                {
                    if (File.Exists("Configs\\PrintImageCordinator.ini"))
                        cordinatorIni.Load("Configs\\PrintImageCordinator.ini");

                    string pos = btn.Replace("recNail", "");
                    selectedNailPos = pos;
                    string[] cordPos = new string[4];
                    if (pos.Contains("Top"))
                        cordPos = cordinatorIni["TOP"][pos].ToString().Split(',');
                    else if (pos.Contains("Bottom"))
                        cordPos = cordinatorIni["BOTTOM"][pos].ToString().Split(',');
                    NailLeft = cordPos[0];
                    NailTop = cordPos[1];
                    NailWidth = cordPos[2];
                    NailHeight = cordPos[3];
                });
            }
        }
        #endregion ClickCommand

        #region FocusedCommand
        private RelayCommand gotFocusedCommand;
        public RelayCommand GotFocusedCommand
        {
            get
            {
                return gotFocusedCommand ?? new RelayCommand(() =>
                {
                    FileUtility.Instance.RunTabTip();
                });
            }
        }
        private RelayCommand lostFocusedCommand;
        public RelayCommand LostFocusedCommand
        {
            get
            {
                return lostFocusedCommand ?? new RelayCommand(() =>
                {
                    FileUtility.Instance.CloseTabTip();
                });
            }
        }
        #endregion

        #region Print Backup Image
        ObservableCollection<QRImageListInfo> backupImageList;
        public ObservableCollection<QRImageListInfo> BackupImageList
        {
            get { return backupImageList; }
            set
            {
                backupImageList = value;
                RaisePropertyChanged("BackupImageList");
            }
        }

        private List<string> _backupImage = new List<string>();
        public List<string> BackupImage
        {
            get { return _backupImage; }
            set
            {
                if (Equals(value, _backupImage)) return;
                _backupImage = value;
                RaisePropertyChanged("BackupImage");
            }
        }

        private void LoadBackupImages(string backupFolder)
        {
            // 리스트 초기화
            BackupImageList.Clear();

            if (Directory.Exists(backupFolder))
            {
                string[] backupImageFiles = Directory.GetFiles(backupFolder);

                //_backupImage = backupImageFiles.Reverse().ToList();

                foreach (var filepath in backupImageFiles.Reverse())
                {
                    BackupImageList.Add(new QRImageListInfo() { PrintImage = null, TempImagePath = filepath, PrintCount = 0, IsSelected = false });
                }
            }
        }

        private RelayCommand printBackupImageCommand;

        public RelayCommand PrintBackupImageCommand
        {
            get
            {
                return printBackupImageCommand ?? new RelayCommand(() =>
                {
                    if (File.Exists(SelectedFile))
                    {
                        Console.WriteLine("선택 파일 있음, 값 : " + SelectedFile);

                        FileInfo file = new FileInfo(SelectedFile);

                        if (File.Exists(Datas.GlobalVariables.Instance.ResultPath))
                        {
                            File.Delete(Datas.GlobalVariables.Instance.ResultPath);
                        }

                        file.CopyTo(Datas.GlobalVariables.Instance.ResultPath);
                        PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_INFO, VALIDATION_MESSAGE.CONFIRM, VALIDATION_MESSAGE.VALIDATION_WAIT_FOR_ADMIN);
                    }
                    else
                    {
                        Console.WriteLine("선택 파일 없음, 값 : " + SelectedFile);
                    }
                });
            }
        }

        private void BackupImagePrintTimerTick(object sender, EventArgs e)
        {
            //this.backupImagePrintTimer.Stop();

            if (BackupImagePrintTimer() == NailPrinterWParamType.Failure)
            {
                this.backupImagePrintTimer.Stop();
                ShowMessageLayerError();
            }

            //this.backupImagePrintTimer.Start();
        }

        private int BackupImagePrintTimer()
        {
            if (File.Exists(GlobalVariables.Instance.ResultPath))
            {
                logger.Info("PrintNail at Admin :: FileExist :: " + GlobalVariables.Instance.ResultPath);
                return NailPrinterWParamType.FileExist;
            }
            else
            {
                //logger.Info(string.Format("{0} :: PrintCount : {1}", CurrentViewModelName, totalPrintCount));
                foreach (var d in backupImageList)
                {
                    if (d.PrintCount > 0 && canPrint == true)
                    {
                        if (SDKManager.NailPrinter.Print(d.TempImagePath, false) != SDKManagerStatusCode.Success)
                        {
                            logger.Error("PrintNail at Admin :: Backup Image Print error! :: " + d.TempImagePath);
                            return NailPrinterWParamType.Failure;
                        }

                        logger.Info("PrintNail at Admin :: Backup Image Printed :: " + d.TempImagePath);
                        d.PrintCount--;
                        canPrint = false;
                        break;
                    }
                }
                return SDKManagerStatusCode.Success;
            }
        }

        private void BackupImageProcess(string btnName)
        {
            switch (btnName)
            {
                case "Print":
                    Console.WriteLine(string.Format("TotalPrintCount :: {0}", totalPrintCount));
                    if (totalPrintCount == 0 || totalPrintCount > 3)
                        break;

                    if (backupImagePrintTimer.IsEnabled == false)
                    {
                        // Popup Layer 활성
                        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_INFO, VALIDATION_MESSAGE.CONFIRM, VALIDATION_MESSAGE.VALIDATION_WAIT_FOR_ADMIN);
                        }));

                        // printCount save
                        printCount = totalPrintCount;
                        canPrint = true;
                        backupImagePrintTimer.Start();
                    }

                    break;
                default:
                    break;
            }
        }
        #endregion


        #region Password
        private void PasswordProcess(int btnTag)
        {
            switch (btnTag)
            {
                case 10:    // delete
                    if (InputValue.Equals("INCORRECT"))
                    {
                        InputValue = string.Empty;
                        this.Input.Clear();
                    }

                    if (this.Input.Length > 0)
                    {
                        logger.DebugFormat("keypad: {0}", btnTag);

                        this.Input.Remove(this.Input.Length - 1, 1);
                        InputValue = this.ToAsterisk(this.Input);
                    }
                    else logger.DebugFormat("keypad: {0} - no string to delete", btnTag);
                    break;

                case 11:    // submit
                    if (this.Input.Length > 0)
                    {
                        if (this.passwdStep == 1)
                        {
                            logger.DebugFormat("keypad: {0} - step 1", btnTag);
                            this.passwd1st = this.Input.ToString();
                            this.passwdStep++;

                            this.Input.Clear();
                            this.StirKeypad();
                            InputValue = "Enter new password again";
                        }
                        else if (this.passwdStep == 2)
                        {
                            logger.DebugFormat("keypad: {0} - step 2", btnTag);
                            this.passwd2nd = this.Input.ToString();
                            this.passwdStep++;

                            InputValue = string.Empty;
                            this.Input.Clear();
                        }

                        if (this.passwdStep == 3)
                        {
                            logger.DebugFormat("keypad: {0} - step 3", btnTag);
                            if (this.passwd1st.Equals(this.passwd2nd))
                            {
                                logger.DebugFormat("keypad: {0} - match", btnTag);
                                if (this.SavePassword(this.passwd1st))
                                {
                                    InputValue = "Password is changed";
                                }
                                else
                                {
                                    InputValue = "Failed to change password";
                                }
                            }
                            else
                            {
                                logger.DebugFormat("keypad: {0} - mismatch", btnTag);

                                this.Input.Clear();
                                //this.StirKeypad();
                                this.passwdStep = 1;
                                InputValue = "Mismatch! Enter new password (4 digit)";
                            }
                        }
                    }
                    break;
                case 12:    // close
                    CommandAction(NAVIGATION_TYPE.Previous);
                    break;
                default:
                    if (InputValue.Equals("INCORRECT"))
                    {
                        InputValue = string.Empty;
                        this.Input.Clear();
                    }

                    if (this.Input.ToString().Length < 4)
                    {
                        this.Input.Append(btnTag);
                        InputValue = this.ToAsterisk(this.Input);
                        logger.DebugFormat("keypad: {0}", btnTag);
                    }
                    else logger.DebugFormat("keypad: {0} - overflow {1}", btnTag, this.Input.Length);
                    break;
            }
        }

        private string ToAsterisk(StringBuilder sb)
        {
            try
            {
                StringBuilder asb = new StringBuilder();

                for (int i = 0; i < sb.ToString().Length; i++)
                {
                    asb.Append("*");
                }
                return asb.ToString();
            }
            catch (Exception exp)
            {
                CommonException();
                return string.Empty;
            }
        }

        private void StirKeypad()
        {
            try
            {
                Random rand = new Random((int)DateTime.Now.Ticks);
                int[] p = Enumerable.Range(0, 10).ToArray();

                int index, old;
                for (int k = 0; k < 9; k++)
                {
                    index = rand.Next(9);
                    old = p[k];
                    p[k] = p[index];
                    p[index] = old;
                }

                int i = 0;
                this.Number0 = p[i++].ToString();
                this.Number1 = p[i++].ToString();
                this.Number2 = p[i++].ToString();
                this.Number3 = p[i++].ToString();
                this.Number4 = p[i++].ToString();
                this.Number5 = p[i++].ToString();
                this.Number6 = p[i++].ToString();
                this.Number7 = p[i++].ToString();
                this.Number8 = p[i++].ToString();
                this.Number9 = p[i++].ToString();
            }
            catch (Exception exp)
            {
                CommonException();
            }
        }

        private bool SavePassword(string newPassword)
        {
            try
            {
                string passwordInFile = null;

                using (StreamReader sr = new StreamReader(Path.Combine(SystemPath.Data, "admin.auth")))
                {
                    passwordInFile = sr.ReadLine().Replace("\n", "");
                }

                using (StreamWriter sw = new StreamWriter(Path.Combine(SystemPath.Data, "admin.auth"), false))
                {
                    sw.WriteLine(DataConverter.String2md5(newPassword));
                }

                return true;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }
        }
        #endregion Password

        #region Devices
        private void DeviceProcess(string btnName)
        {
            //여기
            int res = -1;
            logger.DebugFormat("{0} :: {1}", CurrentViewModelName, btnName);
            switch (btnName)
            {
                case "ReceiptConnect":
                    res = SDKManager.ReceiptPrinter.Connect();
                    if (res != 0)
                    {
                        ReceiptPrinterStatus = "Error";
                    }
                    else
                    {
                        ReceiptPrinterStatus = "Connected";
                    }
                    break;
                case "ReceiptDisconnect":
                    res = SDKManager.ReceiptPrinter.Disconnect();
                    if (res != 0)
                    {
                        ReceiptPrinterStatus = "Error";
                    }
                    else
                    {
                        ReceiptPrinterStatus = "Disconnected";
                    }
                    break;
                case "ReceiptSample":
                    ReceiptData rd = new ReceiptData();
                    ProductInfo pi = new ProductInfo() { dcs = "NAIL STICKER", qty = 1, price = 5000, itemNum = "000", extPrice = 0 };

                    rd.companyName = "iRoboTech";
                    rd.companyRegNo = "814-87-000865";
                    rd.companyAddress = "Tera Tower, Songpadaero 167";
                    rd.companyTel = "02-881-5970";
                    rd.prodInfo.Add(pi);
                    rd.cardNo = "538720**********";
                    rd.cardCompany = "master";
                    rd.payDate = new DateTime(2020, 8, 7, 16, 53, 12);
                    rd.receiptNum = "MT-064537";

                    res = SDKManager.ReceiptPrinter.Print(rd);
                    if (res != 0)
                    {
                        ReceiptPrinterStatus = "Error";
                    }
                    else
                    {
                        ReceiptPrinterStatus = "Printed";
                    }
                    break;
                case "ReceiptStatus":
                    res = SDKManager.ReceiptPrinter.State();
                    switch (res)
                    {
                        case SDKManagerStatusCode.Success:
                            ReceiptPrinterStatus = "OK";
                            break;
                        case SDKManagerStatusCode.Disconnected:
                            ReceiptPrinterStatus = "Disconnected";
                            break;
                        case SDKManagerStatusCode.ConfigFailed:
                            ReceiptPrinterStatus = "ConfigFailed";
                            break;
                        case ReceiptPrinterWParamType.Timeout:
                            ReceiptPrinterStatus = "Timeout";
                            break;
                        default:
                            if (res == ReceiptPrinterWParamType.PaperEmpty)   //error
                                ReceiptPrinterStatus = (("PAPER_EMPTY\n"));

                            if (res == ReceiptPrinterWParamType.HeadUp)   //error
                                ReceiptPrinterStatus = (("HEAD_UP\n"));

                            if (res == ReceiptPrinterWParamType.CutError)   //error
                                ReceiptPrinterStatus = (("CUT_ERROR\n"));

                            if (res == ReceiptPrinterWParamType.NearEnd)  //Warning
                                ReceiptPrinterStatus = (("NEAR_END\n"));

                            if (res == ReceiptPrinterWParamType.OutSensor)  //Warning, error
                                ReceiptPrinterStatus = (("PR_OUT_SENSOR\n"));

                            break;
                    }
                    break;
                case "CardReaderConnection":
                    res = SDKManager.CardPayment.Connect();
                    switch (res)
                    {
                        case SDKManagerStatusCode.Success:
                            CardReaderStatus = "OK";
                            break;
                        case CardPaymentWParamType.Failure:
                            CardReaderStatus = "Error";
                            break;
                        default:
                            break;
                    }
                    break;
                case "CardReaderCheck":
                    res = SDKManager.CardPayment.State();
                    switch (res)
                    {
                        case CardPaymentWParamType.CardInserted:
                            CardReaderStatus = "Inserted";
                            break;
                        case CardPaymentWParamType.CardEmpty:
                            CardReaderStatus = "Empty";
                            break;
                        case CardPaymentWParamType.Failure:
                            CardReaderStatus = "Error";
                            break;
                        default:
                            break;
                    }
                    break;

                case "CancelPayment":
                    CancelPayment();
                    break;
                case "MotorOnOff":
                    if (MotorOnOff == "MotorOn")
                    {
                        MotorOnOff = "MotorOff";
                        SDKManager.NailPrinter.MotorOn();
                    }
                    else
                    {
                        MotorOnOff = "MotorOn";
                        SDKManager.NailPrinter.MotorOff();
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion Devices 

        #region Setting
        private void SettingProcess(string btnName)
        {
            logger.DebugFormat("{0} :: {1}", CurrentViewModelName, btnName);
            switch (btnName)
            {
                case "Keyboard":
                    FileUtility.Instance.CloseTabTip();
                    FileUtility.Instance.RunTabTip();
                    break;
                case "Save":
                    //SavePrintInkLimit(Datas.GlobalVariables.Instance.PrintInkLimitPath);
                    printerInfo.CyanValue = CyanValue;
                    printerInfo.MagentaValue = MagentaValue;
                    printerInfo.YellowValue = YellowValue;
                    printerInfo.BlackValue = BlackValue;
                    printerInfo.WhiteValue = WhiteValue;
                    enp.SavePrintIni(Datas.GlobalVariables.Instance.PrintInkLimitPath, printerInfo);
                    break;
                case "HeadCleanRun":
                    //NailPrinterLib nail = new NailPrinterLib();
                    //nail.Open(null, null, null);
                    //nail.PrinterHeadClean();
                    //enp.PreHeadClean();
                    enp.TurnExternalProgramOff("NailPrinterManager.exe");

                    printerInfo.HeadClean = 1;
                    enp.SavePrintIni(Datas.GlobalVariables.Instance.PrintInkLimitPath, printerInfo);
                    break;
                case "HeadCleanOff":
                    printerInfo.HeadClean = 0;
                    enp.SavePrintIni(Datas.GlobalVariables.Instance.PrintInkLimitPath, printerInfo);
                    break;
                case "HeadCleanOn":
                    printerInfo.HeadClean = 1;
                    enp.SavePrintIni(Datas.GlobalVariables.Instance.PrintInkLimitPath, printerInfo);
                    break;
                case "ItemUp":
                    if (double.Parse(ItemBottom) + double.Parse(ItemHeight) > 124) break;
                    ItemBottom = (double.Parse(ItemBottom) + 0.1).ToString();
                    break;
                case "ItemDown":
                    if (double.Parse(ItemBottom) <= 0) break;
                    ItemBottom = (double.Parse(ItemBottom) - 0.1).ToString();
                    break;
                case "ItemLeft":
                    if (double.Parse(ItemRight) + double.Parse(ItemWidth) > 174) break;
                    ItemRight = (double.Parse(ItemRight) + 0.1).ToString();
                    break;
                case "ItemRight":
                    if (double.Parse(ItemRight) <= 0) break;
                    ItemRight = (double.Parse(ItemRight) - 0.1).ToString();
                    break;
                case "InitCordinate":
                    ItemWidth = "145.50";
                    ItemHeight = "72.00";
                    ItemRight = "26.5";
                    ItemBottom = "13";
                    break;
                case "SaveCordinate":
                    if (File.Exists(Datas.GlobalVariables.Instance.PrintTemplatePath))
                    {
                        //templateIni.Load(Datas.GlobalVariables.Instance.PrintTemplatePath);
                        templateIni["Template11"]["Item Size"] = string.Format("{0},{1}", ItemWidth, ItemHeight);
                        templateIni["Template11"]["Start margin"] = string.Format("{0},{1}", ItemRight, ItemBottom);
                        templateIni.Save(Datas.GlobalVariables.Instance.PrintTemplatePath);
                    }
                    break;
                case "CleanTimeSet":
                    if (File.Exists("Configs\\PrintSetting.ini"))
                    {
                        //headCleanIni.Load("Configs\\PrintSetting.ini");
                        headCleanIni["EPSON"]["HeadClean"] = string.Format("{0},{1},{2}", CleanHour, CleanMinute, CleanSecond);
                        headCleanIni.Save("Configs\\PrintSetting.ini");
                    }
                    break;
                case "SavePrice":
                    if (File.Exists("Configs\\PrintSetting.ini"))
                    {
                        headCleanIni["DESIGN"]["Price"] = Price;
                        headCleanIni.Save("Configs\\PrintSetting.ini");
                    }
                    break;
                case "SaveMachineID":
                    if (File.Exists("Configs\\application.config"))
                    {
                        try
                        {
                            var filePath = "Configs\\application.config";
                            XmlDocument xmldoc = new XmlDocument();
                            xmldoc.Load(filePath);
                            XmlElement root = xmldoc.DocumentElement;

                            XmlNodeList nodes = root.ChildNodes;

                            XmlElement appNode = (XmlElement)root.SelectSingleNode("machine");
                            appNode.SetAttribute("id", MachineID);

                            root.ReplaceChild(appNode, appNode);
                            xmldoc.Save(filePath);
                        }
                        catch (IOException ex)
                        {

                        }
                    }
                    break;
                case "CancelPrintJob":
                    var ps = new PrintServer();
                    var queues = ps.GetPrintQueues();
                    var pq = queues.Where(t => t.FullName.Contains("EPSON L805")).First();

                    if (pq == null) return;

                    pq.Refresh();
                    var jobs = pq.GetPrintJobInfoCollection();

                    foreach (var job in jobs)
                    {
                        job.Cancel();
                    }
                    break;

                case "SaveSign":
                    if (File.Exists("Configs\\PrintSetting.ini"))
                    {
                        headCleanIni["SIGN"]["Sign1"] = Sign1;
                        headCleanIni["SIGN"]["Sign2"] = Sign2;
                        headCleanIni.Save("Configs\\PrintSetting.ini");
                    }
                    break;

                case "ClearSign":
                    Sign1 = "";
                    Sign2 = "";
                    break;

                case "InitSign":
                    Sign1 = "Designed by irobotech";
                    Sign2 = "";
                    break;

                case "TTS":
                    if (SetTTS == "On")
                    {
                        SetTTS = "Off";
                        GlobalVariables.Instance.IsTTSOn = false;
                    }
                    else
                    {
                        SetTTS = "On";
                        GlobalVariables.Instance.IsTTSOn = true;
                    }
                    if (File.Exists("Configs\\PrintSetting.ini"))
                    {
                        headCleanIni["MODE"]["TTS"] = GlobalVariables.Instance.IsTTSOn;
                        headCleanIni.Save("Configs\\PrintSetting.ini");
                    }
                    break;

                case "Payment":
                    if (SetPayment == "On")
                    {
                        SetPayment = "Off";
                        GlobalVariables.Instance.IsPaymentOn = false;
                    }
                    else
                    {
                        SetPayment = "On";
                        GlobalVariables.Instance.IsPaymentOn = true;
                    }
                    if (File.Exists("Configs\\PrintSetting.ini"))
                    {
                        headCleanIni["MODE"]["Payment"] = GlobalVariables.Instance.IsPaymentOn;
                        headCleanIni.Save("Configs\\PrintSetting.ini");
                    }
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region ExpertSetting
        private void ExpertSettingProcess(string btnName)
        {
            switch (btnName)
            {
                case "SaveNailCordinate":
                    if (string.IsNullOrEmpty(selectedNailPos)) return;
                    if (File.Exists("Configs\\PrintImageCordinator.ini"))
                    {
                        if (selectedNailPos.Contains("Top"))
                            cordinatorIni["TOP"][selectedNailPos] = string.Format("{0},{1},{2},{3}", NailLeft, NailTop, NailWidth, NailHeight);
                        else if (selectedNailPos.Contains("Bottom"))
                            cordinatorIni["BOTTOM"][selectedNailPos] = string.Format("{0},{1},{2},{3}", NailLeft, NailTop, NailWidth, NailHeight);

                        cordinatorIni.Save("Configs\\PrintImageCordinator.ini");
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void ShowMessageLayerError()
        {
            PopupMessageOption popupoption = new PopupMessageOption();
            popupoption.Button0Page = CurrentViewModelName;
            PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_ERROR,
                         VALIDATION_MESSAGE.CONFIRM,
                         VALIDATION_MESSAGE.VALIDATION_PRINT_FAILED,
                         POPUP_QUANTITY.BUTTON_ONE, popupoption);
        }

        #region Call Http Method
        private async Task GetPrintCount()
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            var res = await Api.MonitoringInfo.SelectMonitoringInfoAsync(Int32.Parse(ApplicationConfigurationSection.Instance.Machine.ID));
            TotalPrintedCount = res.PrintCnt;
        }

        private async Task SetCancelPayment(int machineID, double price, string approvalNo, DateTime paymentDt, string cancelApprovalNo)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            PaymentInfoRequestObj req = new PaymentInfoRequestObj { 
                MachineID = machineID, 
                Price = price, 
                ApprovalNo = approvalNo, 
                PaymentDt = paymentDt.ToString("yyyy-MM-dd"), 
                CancelApprovalNo = cancelApprovalNo 
            };
            var res = await Api.PaymentInfo.UpdateCancelPaymentAsync(req);
        }
        #endregion


        #region Refund
        private void CancelPayment()
        {
            if (ApprovalNumber != null && CancelPrice != null && ApprovalDate != null)
            {
                System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(1000);
                timer.Tick += (object sender, EventArgs e) =>
                {
                    timer.Stop();
                    Task.Run(() =>
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            logger.Debug($"환불 :: Refund Payment Try");
                            ProcessCancelCardPayment(CancelPrice, ApprovalDate, ApprovalNumber);
                        }));
                    });
                };
                timer.Start();
            }

            else
            {
                CancelPaymentMsg = "Refund : 승인번호 / 금액 / 승인날짜 중에 빈 항목이 있습니다";
            }
            
        }

        public void ProcessCancelCardPayment(string amount, string approvalDate, string approvalNumber)
        {
            try
            {
                ReceiptData receiptData = new ReceiptData();
                if (SDKManager.CardPayment.RequestCancelCardPayment(amount, approvalDate, approvalNumber, ref receiptData) == SDKManagerStatusCode.Success)
                {
                    GlobalVariables.Instance.ReceiptData = receiptData;
                    logger.DebugFormat("결제 취소 처리{0} :: Cancel Card Payment :: Done", CurrentViewModelName);
                    // 여기서 UpdateCancelPayment call
                    Task.Run(async () => await SetCancelPayment(
                        Int32.Parse(ApplicationConfigurationSection.Instance.Machine.ID), 
                        double.Parse(amount), approvalNumber, 
                        receiptData.payDate, 
                        receiptData.receiptNum)
                    );
                    CancelPaymentMsg = "Refund : 승인번호 " + ApprovalNumber + "의 결제 금액 " + amount +"원 취소 완료 되었습니다.";
                    PrintReceipt();
                }

                else
                {
                    logger.DebugFormat("결제 취소 처리 실패 {0} :: Msg : {1}", CurrentViewModelName, receiptData.extraMessage);
                    CancelPaymentMsg = "Refund : " +receiptData.extraMessage;
                }

                ApprovalNumber = "";
                CancelPrice = "";
                ApprovalDate = "";
            }

            catch (Exception ex)
            {
                //Console.WriteLine(string.Format("{0} :: {1}", CurrentViewModelName, ex.ToString()));
                logger.ErrorFormat("결제 {0} :: Card Payment Exception : {1}", CurrentViewModelName, ex.ToString());
                
            }

        }

        private void PrintReceipt()
        {
            List<ProductInfo> piList = new List<ProductInfo>();

            ProductInfo pi = new ProductInfo() { dcs = "NAIL STICKER", qty = Convert.ToInt32(CancelPrice) /5000, price = -Convert.ToDouble(CancelPrice), itemNum = "000", extPrice = 0 };

            ReceiptData receiptData = new ReceiptData()
            {
                //companyName = "iRoboTech",
                //companyRegNo = "814-87-00865",
                //companyAddress = "Tera Tower, Songpadaero 167",
                //companyTel = "02-881-5970",

                companyName = "iRoboTech",
                companyRegNo = "814-87-00865",
                companyAddress = "서울 송파대로 167, 테라타워 809",
                companyTel = "02-881-5970",

                //cardNo = "538720**********",
                //cardCompany = "master",
                //payDate = DateTime.Now,
                //receiptNum = "MT-064537"

                paymentType = "D4",
                cardNo = GlobalVariables.Instance.ReceiptData.cardNo,
                cardCompany = GlobalVariables.Instance.ReceiptData.cardCompany,
                cardType = GlobalVariables.Instance.ReceiptData.cardType,
                payDate = GlobalVariables.Instance.ReceiptData.payDate,
                receiptNum = GlobalVariables.Instance.ReceiptData.receiptNum,

                extraMessage = GlobalVariables.Instance.ReceiptData.extraMessage
            };

            receiptData.prodInfo.Add(pi);


            int result = SDKManager.ReceiptPrinter.Print(receiptData);
            if (result != 0)
            {
                logger.ErrorFormat("{0} :: Print Cancel Receipt Error! :: Approval No : {1} :: Total Price : {2}", CurrentViewModelName, ApprovalNumber, CancelPrice);

            }
            else
            {
                logger.InfoFormat("{0} :: Printed Cancel Receipt :: Approval No : {1} :: Total Price : {2}", CurrentViewModelName, ApprovalNumber, CancelPrice) ;
            }

 
        }
        #endregion

    }
}