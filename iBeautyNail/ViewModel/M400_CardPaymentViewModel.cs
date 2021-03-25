using iBeautyNail.Configuration;
using iBeautyNail.Datas;
using iBeautyNail.Enums;
using iBeautyNail.Http;
using iBeautyNail.Http.Endpoints.ErrorInfoEndpoint.Models;
using iBeautyNail.Http.Endpoints.PaymentInfoEndpoint.Models;
using iBeautyNail.Interface;
using iBeautyNail.Messages;
using iBeautyNail.Messages.Exceptions.Enums;
using iBeautyNail.SDK;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;

namespace iBeautyNail.ViewModel
{
    class M400_CardPaymentViewModel : BaseViewModelBase, INotifyPropertyChanged
    {
        private int tryCount = 0;

        public M400_CardPaymentViewModel()
        {
            HomeButtonVisible = false;
            PrevButtonVisible = false;
            NextButtonVisible = false;
        }

        protected override void PageLoad()
        {
            // Language 팝업은 Load안함
            if (GlobalVariables.Instance.LanguagePopup)
            {
                GlobalVariables.Instance.LanguagePopup = false;
                return;
            }

            GlobalVariables.Instance.MyProduct.isPaid = false;
            tryCount = 0;

            // 네트워크 체크
            if (!IsEstablishedNetwork())
            {
                logger.ErrorFormat("결제 {0} :: Network is not available", CurrentViewModelName);
                Task.Run(() => CreateErrorInfo("1001", string.Format("결제 {0} :: Network is not available", CurrentViewModelName)));
            }

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (object sender, EventArgs e) =>
            {
                timer.Stop();
                Task.Run(() =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        if (GlobalVariables.Instance.IsTTSOn)
                        {
                            threadDelegate = new ThreadStart(CommentWork);
                            commentThread = new Thread(threadDelegate);
                            commentThread.Start();
                        }

                        logger.Debug($"결제 :: Card Payment Try : {tryCount} (Maximum count = 2) ");
                        ProcessCardPayment();
                    }));
                });
            };
            timer.Start();
        }

        protected override void PageUnload()
        {
            //GlobalVariables.Instance.MyProduct.isPaid = true;
            if (GlobalVariables.Instance.IsTTSOn)
            {
                synthesizer.SpeakAsyncCancelAll();
                //synthesizer.Dispose();
                commentThread.Abort();
            }
        }

        protected override void CommentWork()
        {
            try
            {
                synthesizer = new SpeechSynthesizer();
                synthesizer.Rate = 2;
                synthesizer.SetOutputToDefaultAudioDevice();

                var builder = new PromptBuilder();
                builder.StartVoice(new CultureInfo(App.LanguageMng.CurrentCulture));
                builder.AppendText(App.LanguageMng.LanguageSet[App.LanguageMng.CurrentCulture]["M400_ctTbPayment"].Sentence);
                builder.AppendText(",     ");
                builder.AppendText(App.LanguageMng.LanguageSet[App.LanguageMng.CurrentCulture]["M400_ctTbPayment2"].Sentence);
                builder.EndVoice();
                synthesizer.SpeakAsync(builder);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("{0} :: CommentWork Exception :: {1}", CurrentViewModelName, ex.ToString());
            }
        }

        private void ProcessCardPayment()
        {
            try
            {
                ReceiptData receiptData = new ReceiptData();
                //if (SDKManager.CardPayment.RequestCardPayment(GlobalVariables.Instance.MyProduct.price.ToString()) == SDKManagerStatusCode.Success)
                if (SDKManager.CardPayment.RequestCardPayment(GlobalVariables.Instance.MyProduct.price.ToString(), ref receiptData) == SDKManagerStatusCode.Success)
                {
                    GlobalVariables.Instance.ReceiptData = receiptData;
                    GlobalVariables.Instance.MyProduct.isPaid = true;
                    logger.DebugFormat("결제 {0} :: Card Payment :: Paid", CurrentViewModelName);
                    // 여기서 CreatePaymentInfo call
                    Task.Run(() => CreatePaymentInfo(receiptData));
                    ShowMessageLayer();
                }
                else
                {
                    logger.DebugFormat("결제실패 {0} :: Msg : {1}", CurrentViewModelName, receiptData.extraMessage);
                    if (tryCount < 2)
                    {
                        tryCount++;
                        logger.DebugFormat("결제 {0} :: Card Payment Retry : {1} (Maximum count = 2) ", CurrentViewModelName, tryCount);
                        Task.Run(() => CreateErrorInfo("4001",string.Format("결제 {0} :: Card Payment Retry : {1} (Maximum count = 2) ", CurrentViewModelName, tryCount)));
                        ProcessCardPayment();
                    }
                    else
                    {
                        logger.DebugFormat("결제 {0} :: Return to the initial page due to payment failed", CurrentViewModelName);
                        Task.Run(() => CreateErrorInfo("4002", string.Format("결제 {0} :: Return to the initial page due to payment failed", CurrentViewModelName)));
                        ShowMessageLayerInit();
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(string.Format("{0} :: {1}", CurrentViewModelName, ex.ToString()));
                logger.ErrorFormat("결제 {0} :: Card Payment Exception : {1}", CurrentViewModelName, ex.ToString());
                Task.Run(() => CreateErrorInfo("4003", string.Format("결제 {0} :: Card Payment Exception : {1}", CurrentViewModelName, ex.ToString())));
            }

        }

        private void ShowMessageLayer()
        {
            PopupMessageOption popupoption = new PopupMessageOption();
            popupoption.Button0Text = "YES";
            popupoption.Button0Page = "M500_PrintReceiptViewModel";
            popupoption.Button1Text = "NO";
            popupoption.Button1Page = "M410_EjectCardViewModel";
            PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_INFO,
                         VALIDATION_MESSAGE.CONFIRM,
                         VALIDATION_MESSAGE.VALIDATION_PRINT_RECEIPT,
                         POPUP_QUANTITY.BUTTON_TWO, popupoption);
        }

        private void ShowMessageLayerInit()
        {
            PopupMessageOption popupoption = new PopupMessageOption();
            popupoption.Button0Page = "M100_StartPageViewModel";
            PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_WARNNING,
                         VALIDATION_MESSAGE.VALIDATION_CARD_PAYMENT_FAILED,
                         VALIDATION_MESSAGE.VALIDATION_FIRST_PAGE,
                         POPUP_QUANTITY.BUTTON_ONE, popupoption);
        }

        // Insert PaymentInfo 
        public async Task CreatePaymentInfo(ReceiptData receiptData)
        {
            logger.DebugFormat("{0} :: CreatePaymentInfo() Begin : {1} :: {2}", CurrentViewModelName, receiptData.receiptNum, receiptData.cardNo);

            NailApi Api = NailApi.GetDevelopmentInstance("");
            var req = new PaymentInfoRequestObj { 
                MachineID = Int32.Parse(ApplicationConfigurationSection.Instance.Machine.ID),
                Price = GlobalVariables.Instance.MyProduct.price,
                ApprovalNo = receiptData.receiptNum,
                CardOwner = receiptData.cardNo,
                CardType = receiptData.cardType,
                CardComp = receiptData.cardCompany,
                PaymentDt = receiptData.payDate.ToString("yyyy-MM-dd HH:mm:ss"),
                ErrorCd = "",
                ErrorMsg = ""
            };
            var res = await Api.PaymentInfo.CreatePaymentInfoAsync(req);
            logger.DebugFormat("{0} :: CreatePaymentInfo() End : {1}", CurrentViewModelName, res.ToString());
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


        // 네트워크 체크 함수
        private bool IsEstablishedNetwork()
        {
            bool networkUp = NetworkInterface.GetIsNetworkAvailable();
            bool pingResult = true;
            if (networkUp)
            {
                string addr = "www.google.com";
                if (string.IsNullOrEmpty(addr))
                {
                    pingResult = true;
                }
                else
                {
                    Ping pingSender = new Ping();
                    PingReply reply = pingSender.Send(addr, 300);
                    pingResult = reply.Status == IPStatus.Success;
                }
            }
            //return networkUp & pingResult;
            return networkUp;
        }
    }
}
