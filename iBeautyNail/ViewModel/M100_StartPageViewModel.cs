using iBeautyNail.Configuration;
using iBeautyNail.Datas;
using iBeautyNail.Devices.ReceiptPrinter;
using iBeautyNail.Enums;
using iBeautyNail.Http;
using iBeautyNail.Http.Endpoints.ErrorInfoEndpoint.Models;
using iBeautyNail.Messages;
using iBeautyNail.Messages.Exceptions.Enums;
using iBeautyNail.SDK;
using iBeautyNail.Utility;
using iBeautyNail.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace iBeautyNail.ViewModel
{
    class M100_StartPageViewModel : BaseViewModelBase
    {
        HwndSource source;
        HwndSourceHook hook;
        private IniFile settingIni = new IniFile();
        private bool isFillPaper = false;

        private string scanText;
        public string ScanText
        {
            get { return scanText; }
            set
            {
                if (scanText != value)
                {
                    scanText = value;
                    OnPropertyUpdate(nameof(ScanText));
                }
            }
        }

        private string lastBuildText;
        public string LastBuildText
        {
            get { return lastBuildText; }
            set
            {
                if (lastBuildText != value)
                {
                    lastBuildText = value;
                    OnPropertyUpdate(nameof(LastBuildText));
                }
            }
        }

        private string machineID;
        public string MachineID
        {
            get { return machineID; }
            set
            {
                if (machineID != value)
                {
                    machineID = value;
                    OnPropertyUpdate(nameof(MachineID));
                }
            }
        }

        System.Windows.Threading.DispatcherTimer timer;
        public M100_StartPageViewModel()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(0);
            timer.Tick += (object sender, EventArgs e) =>
            {
                {
                    StopTimer();
                    BaseViewModelBase.CanNavigate = true;
                }
            };
        }

        private void InitLanguageSettings()
        {
            App.LanguageMng.ChangeLanguage(ApplicationConfigurationSection.Instance.Machine.DefaultLanguage);
        }

        protected override void PageLoad()
        {
            // 글로벌 변수 초기화
            GlobalVariables.Instance.Clear();
            FileUtility.Instance.CloseTabTip();
            isFillPaper = true;

            // Load Design Price from Setting
            if (File.Exists("Configs\\PrintSetting.ini"))
            {
                settingIni.Load("Configs\\PrintSetting.ini");
                GlobalVariables.Instance.DesignPrice = settingIni["DESIGN"]["Price"].ToDouble();
                GlobalVariables.Instance.Sign1 = settingIni["SIGN"]["Sign1"].ToString();
                GlobalVariables.Instance.Sign2 = settingIni["SIGN"]["Sign2"].ToString();

                GlobalVariables.Instance.IsTTSOn = settingIni["MODE"]["TTS"].ToBool();
                GlobalVariables.Instance.IsPaymentOn = settingIni["MODE"]["Payment"].ToBool();
            }

            StartTimer();

            ScanText = "";

            // Build date
            System.Version assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(assemblyVersion.Build).AddSeconds(assemblyVersion.Revision * 2);
            LastBuildText = string.Format("[Last Build : {0}]", buildDate.ToString("yyyy-MM-dd HH:mm:ss"));
            MachineID = string.Format("#{0}", ApplicationConfigurationSection.Instance.Machine.ID);

            logger.DebugFormat("{0} :: loded StartPage at {1} :: {2}", CurrentViewModelName, MachineID, LastBuildText);

            AddHook();

            // 영수증 프린터 상태 체크
            CheckReceiptPrinter();
        }

        protected override void PageUnload()
        {
            StopTimer();
            RemoveHook();
        }

        private void StartTimer()
        {
            timer.Start();
        }

        private void StopTimer()
        {
            if (timer.IsEnabled) timer.Stop();
        }

        private void AddHook()
        {
            this.source = HwndSource.FromHwnd(App.Window.HWND);
            this.hook = new HwndSourceHook(WndProc);

            source.AddHook(hook);
        }

        private void RemoveHook()
        {
            source.RemoveHook(hook);
        }

        /// <summary>
        /// 메시지 처리
        /// </summary>
        private IntPtr WndProc(IntPtr hwnd, int windowMessage, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (windowMessage)
            {
                case WindowMessage.PCU_MSG_STATE:
                    if (wParam == (IntPtr)WindowMessage.PCU_MSG_STATE_EMPTY_PAPER)
                    {
                        if (isFillPaper)
                        {
                            logger.ErrorFormat("{0} :: WndProc :: {1} :: {2}", CurrentViewModelName, "Empty Paper", wParam);
                            Task.Run(() => CreateErrorInfo("6004", string.Format("{0} :: WndProc :: Empty Paper! :: {1}", CurrentViewModelName, (int)wParam)));
                        }
                        isFillPaper = false;
                        ShowMessageLayerError();
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyUpdate(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (ScanText.Contains("*iRoboTech*iBeautyNail*Manager*"))
            {
                GlobalVariables.Instance.IsAdmin = false;
                CommandAction("A000_AdminLoginViewModel");
            }
            else if (ScanText.Contains("*iRoboTech*iBeautyNail*Admin*"))
            {
                GlobalVariables.Instance.IsAdmin = true;
                CommandAction("A010_AdminBaseViewModel");
            }
        }
        #endregion

        private void CheckReceiptPrinter()
        {
            if (DeviceConfigSection.Instance.ReceiptPrinter.Enable == false) return;

            int res = SDKManager.ReceiptPrinter.State();
            logger.DebugFormat("{0} :: RexodReceiptPrinter State() :: {0}", res);
            if (res == ReceiptPrinterWParamType.PaperEmpty ||
                res == ReceiptPrinterWParamType.HeadUp ||
                res == ReceiptPrinterWParamType.NearEnd ||
                res == ReceiptPrinterWParamType.CutError ||
                res == ReceiptPrinterWParamType.OutSensor)
            {
                logger.ErrorFormat("{0} :: Receipt Print Empty Paper error! :: {1}", CurrentViewModelName, res);
                Task.Run(() => CreateErrorInfo("7002", string.Format("{0} :: Receipt Print Empty Paper error! :: {1}", CurrentViewModelName, res)));
                //ShowMessageLayerReceiptError();
            }
        }

        // Insert ErrorInfo
        private async Task CreateErrorInfo(string errCd, string errMsg)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            var req = new ErrorInfoRequestObj
            {
                MachineID = Int32.Parse(ApplicationConfigurationSection.Instance.Machine.ID),
                ErrorCd = errCd,
                ErrorMsg = errMsg,
                FixYn = "N"
            };
            var res = await Api.ErrorInfo.CreateErrorInfoAsync(req);
        }

        private void ShowMessageLayerError()
        {
            PopupMessageOption popupoption = new PopupMessageOption();
            popupoption.Button0Page = "M100_StartPageViewModel";
            PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_WARNNING,
                         VALIDATION_MESSAGE.VALIDATION_EMPTY_PAPER,
                         VALIDATION_MESSAGE.CALL_THE_MANAGER,
                         POPUP_QUANTITY.BUTTON_ONE, popupoption);
        }
        private void ShowMessageLayerReceiptError()
        {
            PopupMessageOption popupoption = new PopupMessageOption();
            popupoption.Button0Page = "M100_StartPageViewModel";
            PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_WARNNING,
                         VALIDATION_MESSAGE.VALIDATION_EMPTY_RECEIPT_PAPER,
                         VALIDATION_MESSAGE.CALL_THE_MANAGER,
                         POPUP_QUANTITY.BUTTON_ONE, popupoption);
        }
    }
}
