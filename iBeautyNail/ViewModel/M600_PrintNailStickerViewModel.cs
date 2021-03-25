using iBeautyNail.Configuration;
using iBeautyNail.Datas;
using iBeautyNail.Devices.NailPrinter;
using iBeautyNail.Enums;
using iBeautyNail.Http;
using iBeautyNail.Http.Endpoints.ErrorInfoEndpoint.Models;
using iBeautyNail.Http.Endpoints.MonitoringInfoEndpoint.Models;
using iBeautyNail.Http.Endpoints.PrintInfoEndPoint.Models;
using iBeautyNail.Messages;
using iBeautyNail.Messages.Exceptions.Enums;
using iBeautyNail.SDK;
using iBeautyNail.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace iBeautyNail.ViewModel
{
    class M600_PrintNailStickerViewModel : BaseViewModelBase
    {
        private HwndSource source;
        private HwndSourceHook hook;

        int printed = 0;
        int printCount = 0;

        private int countDownSec;
        private int checkSec;
        private DispatcherTimer countdownTimer;
        private DispatcherTimer checkTimer;

        private bool isPrinting;
        private bool isStarted;
        private bool isPrinted;

        private bool isFillPaper = false;

        #region Property
        private string printedCount;
        public string PrintedCount
        {
            get { return printedCount; }
            set { Set(() => PrintedCount, ref printedCount, value); }
        }

        private string countDown;
        public string CountDown
        {
            get { return countDown; }
            set { Set(() => CountDown, ref countDown, value); }
        }

        private Visibility countDownVisibility;
        public Visibility CountDownVisibility
        {
            get { return countDownVisibility; }
            set { Set(() => CountDownVisibility, ref countDownVisibility, value); }
        }
        #endregion

        public M600_PrintNailStickerViewModel()
        {
            this.source = HwndSource.FromHwnd(App.Window.HWND);
            this.hook = new HwndSourceHook(WndProc);

            this.countdownTimer = new DispatcherTimer();
            this.countdownTimer.Interval = TimeSpan.FromMilliseconds(1000);
            this.countdownTimer.Tick += new EventHandler(this.CountdownTimerTick);

            this.checkTimer = new DispatcherTimer();
            this.checkTimer.Interval = TimeSpan.FromMilliseconds(1000);
            this.checkTimer.Tick += new EventHandler(this.CheckTimerTick);

            HomeButtonVisible = false;
            PrevButtonVisible = false;
            NextButtonVisible = false;
        }

        protected override void PageLoad()
        {
            AddHook();

            countDownSec = 140;
            checkSec = 0;
            printed = 0;
            isPrinted = false;
            isFillPaper = true;

            // Language 팝업은 Load안함
            if (GlobalVariables.Instance.LanguagePopup)
            {
                GlobalVariables.Instance.LanguagePopup = false;
                return;
            }

            // 결제 완료 된것 체크
            if (GlobalVariables.Instance.MyProduct.isPaid && SDKManager.NailPrinter.IsConnected() && DeviceConfigSection.Instance.NailPrinter.Enable)
            {
                printCount = GlobalVariables.Instance.MyProduct.qty;
                PrintedCount = String.Format("{0}/{1}", printed, printCount);

                // 카운트 시작 (프린트 장수 * 120초)
                countDownSec = 140 * printCount;
                CountDown = "Printing...";

                if (PrintNail(true) == NailPrinterWParamType.Failure)
                {
                    ShowMessageLayerError();
                }
                else
                {
                    checkTimer.Start();
                }

                //this.countdownTimer.Start(); // DLL 사용시 주석처리
                CountDownVisibility = Visibility.Visible;

                // 백업 폴더에 있는 이미지 삭제
                if (File.Exists(GlobalVariables.Instance.BackupPath))
                    File.Delete(GlobalVariables.Instance.BackupPath);

                // USB에서 선택해서 임시로 크롭된 이미지 삭제 (temp 폴더에 IMSIusbImage로 시작하는 이름)
                string[] trash = Directory.GetFiles(Path.GetDirectoryName(GlobalVariables.Instance.TempResultPath), "IMSIusbImage*", SearchOption.AllDirectories);
                foreach (string filepath in trash)
                {
                    File.Delete(filepath);
                }
            }
        }

        protected override void PageUnload()
        {
            this.countdownTimer.Stop();
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
                            logger.DebugFormat("{0} :: WndProc Start :: {1} :: {2}", CurrentViewModelName, wParam.ToInt32(), lParam.ToInt32());
                            isStarted = true;
                            isPrinted = false;
                            // 프린트 시작
                            if (countdownTimer.IsEnabled == false)
                                this.countdownTimer.Start();
                        }
                        else if (lParam == (IntPtr)0)
                        {
                            logger.DebugFormat("{0} :: WndProc Finish :: {1} :: {2}", CurrentViewModelName, wParam.ToInt32(), lParam.ToInt32());
                            isPrinted = true;
                            // 여기서 출력 건수 update call
                            Task.Run(() => UpdatePrintCount());

                            countDownSec = 140 * (printCount - printed);
                            if (countDownSec == 0) countDownSec = 5;
                            
                            logger.DebugFormat("총 출력해야할 수량 : {0}장 :: 출력된 수량 : {1}장 :: 남은 시간 : {2}초", printCount, printed, countDownSec);

                            // 용지 부족 시 다음 프린트 중지
                            if (isFillPaper == false)
                            {
                                logger.DebugFormat("{0} :: 용지부족으로 인해 중지 :: {1} :: {2}", CurrentViewModelName, wParam.ToInt32(), lParam.ToInt32());
                                
                                if (countdownTimer.IsEnabled)
                                    this.countdownTimer.Stop();
                                break;
                            }

                            if (printed < printCount)
                            {
                                if (PrintNail(false) == NailPrinterWParamType.Failure)
                                {
                                    ShowMessageLayerError();
                                }
                                else
                                {
                                    checkTimer.Start();
                                }
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
                                logger.DebugFormat("{0} :: WndProc NailPrinterManager Process Kill", CurrentViewModelName);
                                Task.Run(() => CreateErrorInfo("6003", string.Format("{0} :: WndProc NailPrinterManager Process Kill", CurrentViewModelName)));
                            }

                            ShowMessageLayerError();
                        }
                    }
                    else if (wParam == (IntPtr)WindowMessage.PCU_MSG_STATE_EMPTY_PAPER)
                    {
                        if (isFillPaper)
                        {
                            logger.ErrorFormat("{0} :: WndProc :: {1} :: {2}", CurrentViewModelName, "Empty Paper", wParam);
                            Task.Run(() => CreateErrorInfo("6004", string.Format("{0} :: WndProc :: Empty Paper! :: {1}", CurrentViewModelName, (int)wParam)));
                        }
                        isFillPaper = false;
                        ShowMessageLayerEmptyPaperError();
                    }
                    break;
                case WindowMessage.PCU_MSG_INK_VALUE:
                    logger.DebugFormat("{0} :: WndProc PCU_MSG_INK_VALUE :: {1} :: {2}", CurrentViewModelName, wParam.ToInt32(), lParam.ToInt32());
                    GlobalVariables.Instance.InkVol[wParam.ToInt32()] = lParam.ToInt32();
                    break;
            }
            return IntPtr.Zero;
        }

        private void CountdownTimerTick(object sender, EventArgs e)
        {
            if (countDownSec < 1)
            {
                this.countdownTimer.Stop();
                CountDownVisibility = System.Windows.Visibility.Collapsed;

                // 완료 화면 넘기기전 큐체크 (완료 메시지 못받은 경우만)
                if (isPrinted == false)
                {
                    var ps = new PrintServer();
                    var queues = ps.GetPrintQueues();
                    var pq = queues.Where(t => t.FullName.Contains("EPSON L805")).First();

                    if (pq != null)
                    {
                        logger.ErrorFormat("{0} :: PrintNail :: Print error before finish! :: {1}", CurrentViewModelName, "Print queue is remained even though finished printing");
                        Task.Run(() => CreateErrorInfo("6001", string.Format("{0} :: PrintNail :: {1}", CurrentViewModelName, "Print queue is remained even though finished printing")));
                        ShowMessageLayerError();
                    }
                }

                // 프린트 끝났으면 다음페이지로
                CommandAction(NAVIGATION_TYPE.Next);
            }
            else
            {
                countDownSec--;
                CountDown = String.Format("{0:D2}:{1:D2}", countDownSec / 60, countDownSec % 60);
                PrintedCount = String.Format("{0}/{1}", printed, printCount);
            }
        }

        private void CheckTimerTick(object sender, EventArgs e)
        {
            // 40초 안에 큐와 보드 신호에 문제가 있으면 오류화면으로 이동
            // 둘중 하나라도 false면 오류 화면으로 이동
            if (checkSec >= 40)
            {
                checkTimer.Stop();
                if (isPrinting == false || isStarted == false)
                {
                    logger.ErrorFormat("{0} :: PrintNail :: Print error! :: {1}", CurrentViewModelName, "Print queue or nailmananger board error");
                    Task.Run(() => CreateErrorInfo("6001", string.Format("{0} :: PrintNail :: {1}", CurrentViewModelName, "Print queue or nailmananger board error")));
                    ShowMessageLayerError();
                }
            }

            // Queue 체크
            if (isPrinting == false)
            {
                var ps = new PrintServer();
                var queues = ps.GetPrintQueues();
                var pq = queues.Where(t => t.FullName.Contains("EPSON L805")).First();

                if (pq != null)
                    isPrinting = true;
            }

            // 프린트 시작 값 받은지 체크
            if (isPrinting && isStarted)
            {
                logger.DebugFormat("Check printing : {0} sec", checkSec);
                checkTimer.Stop();
                isPrinting = false;
                isStarted = false;
                checkSec = 0;
                return;
            }

            checkSec++;
        }

        // DLL 적용 x
        //private void CountdownTimerTick(object sender, EventArgs e)
        //{
        //    if (countDownSec < 1)
        //    {
        //        this.countdownTimer.Stop();
        //        CountDownVisibility = System.Windows.Visibility.Collapsed;

        //        // 프린트 끝났으면 다음페이지로
        //        CommandAction(NAVIGATION_TYPE.Next);
        //    }
        //    else
        //    {
        //        countDownSec--;
        //        CountDown = String.Format("{0:D2}:{1:D2}", countDownSec / 60, countDownSec % 60);

        //        PrintedCount = String.Format("{0}/{1}", printCount - (countDownSec / 140), printCount);

        //        if (printed < printCount)
        //        {
        //            PrintNail(false);
        //        }
        //    }
        //}

        private int PrintNail(bool backupStatus)
        {
            if (File.Exists(GlobalVariables.Instance.ResultPath))
            {
                //Console.WriteLine("{0} :: PrintNail :: FileExist :: " + GlobalVariables.Instance.ResultPath);
                logger.InfoFormat("{0} :: PrintNail :: FileExist :: {1}", CurrentViewModelName, GlobalVariables.Instance.ResultPath);
                return NailPrinterWParamType.FileExist;
            }
            else
            {
                // QRData 의 Count 가 1이상이면 QR 이미지 리스트 프린트
                if (GlobalVariables.Instance.QRData.Count > 0)
                {
                    // 프린트하고 PrintCount - 1;
                    foreach (var d in GlobalVariables.Instance.QRData)
                    {
                        if (d.PrintCount > 0)
                        {
                            if (SDKManager.NailPrinter.Print(d.PrintImage, true) != SDKManagerStatusCode.Success)
                            {
                                logger.ErrorFormat("{0} :: PrintNail :: QRData Image Print error! :: {1}", CurrentViewModelName, d.PrintImage);
                                Task.Run(() => CreateErrorInfo("6002", string.Format("{0} :: PrintNail :: QRData Image Print error! :: {1}", CurrentViewModelName, d.PrintImage)));
                                return NailPrinterWParamType.Failure;
                            }

                            // PrintInfo 
                            Task.Run(() => CreatePrintInfo(2, printCount, printed, d.PrintImage));

                            logger.Info("PrintNail :: QRData Printed :: " + d.PrintImage);
                            d.PrintCount--;
                            break;
                        }
                    }
                }
                // 아니면 기본프로세스
                else
                {
                    if (SDKManager.NailPrinter.Print(GlobalVariables.Instance.TempResultPath, backupStatus) != SDKManagerStatusCode.Success)
                    {
                        logger.ErrorFormat("{0} :: PrintNail :: Nail Image Print error! :: {1}", CurrentViewModelName, GlobalVariables.Instance.TempResultPath);
                        Task.Run(() => CreateErrorInfo("6001", string.Format("{0} :: PrintNail :: Nail Image Print error! :: {1}", CurrentViewModelName, GlobalVariables.Instance.TempResultPath)));
                        return NailPrinterWParamType.Failure;
                    }

                    // PrintInfo 
                    Task.Run(() => CreatePrintInfo(0, printCount, printed, GlobalVariables.Instance.TempResultPath));

                    //Console.WriteLine("PrintNail :: Printed :: " + GlobalVariables.Instance.TempResultPath);
                    logger.Info("PrintNail :: Printed :: " + GlobalVariables.Instance.TempResultPath);
                }
                printed++;
                return SDKManagerStatusCode.Success;
            }
        }

        private void ShowMessageLayerError()
        {
            PopupMessageOption popupoption = new PopupMessageOption();
            popupoption.Button0Page = "E400_ErrorBaseViewModel";
            PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_ERROR,
                         VALIDATION_MESSAGE.VALIDATION_PRINT_FAILED,
                         VALIDATION_MESSAGE.CALL_THE_MANAGER,
                         POPUP_QUANTITY.BUTTON_ONE, popupoption);
        }

        private void ShowMessageLayerEmptyPaperError()
        {
            PopupMessageOption popupoption = new PopupMessageOption();
            popupoption.Button0Page = "M100_StartPageViewModel";
            PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_WARNNING,
                         VALIDATION_MESSAGE.VALIDATION_EMPTY_PAPER,
                         VALIDATION_MESSAGE.CALL_THE_MANAGER,
                         POPUP_QUANTITY.BUTTON_ONE, popupoption);
        }

        // 출력건수 카운팅
        private async Task UpdatePrintCount()
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            var req = new MonitoringInfoRequestObj { MachineID = Int32.Parse(ApplicationConfigurationSection.Instance.Machine.ID) };
            var res = await Api.MonitoringInfo.UpdateMonitoringInfoAsync(req);
        }


        // Insert PrintInfo
        private async Task CreatePrintInfo(int designType, int totalPrintCnt, int printCnt, string printFileName)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            var req = new PrintInfoRequestObj
            {
                MachineID = Int32.Parse(ApplicationConfigurationSection.Instance.Machine.ID),
                DesignType = designType,
                TotalPrintCnt = totalPrintCnt,
                PrintCnt = printCnt,
                PrintFileName = printFileName,
                FileName1  = GlobalVariables.Instance.MyDesigns.Count == 0 ? "" : GlobalVariables.Instance.MyDesigns[0].DesignInfo.DesignPath,
                FileName2  = GlobalVariables.Instance.MyDesigns.Count == 0 ? "" : GlobalVariables.Instance.MyDesigns[1].DesignInfo.DesignPath,
                FileName3  = GlobalVariables.Instance.MyDesigns.Count == 0 ? "" : GlobalVariables.Instance.MyDesigns[2].DesignInfo.DesignPath,
                FileName4  = GlobalVariables.Instance.MyDesigns.Count == 0 ? "" : GlobalVariables.Instance.MyDesigns[3].DesignInfo.DesignPath,
                FileName5  = GlobalVariables.Instance.MyDesigns.Count == 0 ? "" : GlobalVariables.Instance.MyDesigns[4].DesignInfo.DesignPath,
                FileName6  = GlobalVariables.Instance.MyDesigns.Count == 0 ? "" : GlobalVariables.Instance.MyDesigns[5].DesignInfo.DesignPath,
                FileName7  = GlobalVariables.Instance.MyDesigns.Count == 0 ? "" : GlobalVariables.Instance.MyDesigns[6].DesignInfo.DesignPath,
                FileName8  = GlobalVariables.Instance.MyDesigns.Count == 0 ? "" : GlobalVariables.Instance.MyDesigns[7].DesignInfo.DesignPath,
                FileName9  = GlobalVariables.Instance.MyDesigns.Count == 0 ? "" : GlobalVariables.Instance.MyDesigns[8].DesignInfo.DesignPath,
                FileName10 = GlobalVariables.Instance.MyDesigns.Count == 0 ? "" : GlobalVariables.Instance.MyDesigns[9].DesignInfo.DesignPath
            };
            var res = await Api.PrintInfo.CreatePrintInfoAsync(req);
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
    }
}
