using iBeautyNail.Configuration;
using iBeautyNail.Datas;
using iBeautyNail.Http;
using iBeautyNail.Http.Endpoints.ErrorInfoEndpoint.Models;
using iBeautyNail.Interface;
using iBeautyNail.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iBeautyNail.ViewModel
{
    class M500_PrintReceiptViewModel : BaseViewModelBase
    {
        public M500_PrintReceiptViewModel()
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
            logger.DebugFormat("{0} :: ReceiptPrinter :: Connected Check", CurrentViewModelName);
            if (SDKManager.ReceiptPrinter.IsConnected() == false)
            {
                logger.ErrorFormat("{0} :: ReceiptPrinter :: Connected Error!", CurrentViewModelName);
                Task.Run(() => CreateErrorInfo("7001", string.Format("{0} :: ReceiptPrinter :: Connected Error!", CurrentViewModelName)));
                CommonException();
            }

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (object sender, EventArgs e) =>
            {
                timer.Stop();
                if (GlobalVariables.Instance.MyProduct.isPaid && DeviceConfigSection.Instance.ReceiptPrinter.Enable)
                {
                    Task.Run(() =>
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            PrintReceipt();
                        }));
                    });
                }
            };
            timer.Start();
        }

        protected override void PageUnload()
        {
            // 카드리더기 없을 시 테스트를 위한 코드 :: 배포시 주석 처리 요함
            //GlobalVariables.Instance.MyProduct.isPaid = true;
        }

        private void PrintReceipt()
        {
            List<ProductInfo> piList = new List<ProductInfo>();

            ProductInfo pi = new ProductInfo() { dcs = "NAIL STICKER", qty = GlobalVariables.Instance.MyProduct.qty, price = GlobalVariables.Instance.MyProduct.price, itemNum = "000", extPrice = 0 };

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

                cardNo = GlobalVariables.Instance.ReceiptData.cardNo,
                cardCompany = GlobalVariables.Instance.ReceiptData.cardCompany,
                cardType = GlobalVariables.Instance.ReceiptData.cardType,
                payDate = GlobalVariables.Instance.ReceiptData.payDate,
                receiptNum = GlobalVariables.Instance.ReceiptData.receiptNum,

                extraMessage = GlobalVariables.Instance.ReceiptData.extraMessage
            };

            receiptData.prodInfo.Add(pi);

            //System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            //{
            int result = SDKManager.ReceiptPrinter.Print(receiptData);
            if (result != 0)
            {
                logger.ErrorFormat("{0} :: Print Receipt Error", CurrentViewModelName);
                Task.Run(() => CreateErrorInfo("7001", string.Format("{0} :: ReceiptPrinter :: Print Receipt Error!", CurrentViewModelName)));
                CommonException();
            }
            else
            {
                logger.InfoFormat("{0} :: Printed Receipt! :: {1} :: {2}", CurrentViewModelName, receiptData.prodInfo.First().qty, receiptData.prodInfo.First().price);
            }

            //}));
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
