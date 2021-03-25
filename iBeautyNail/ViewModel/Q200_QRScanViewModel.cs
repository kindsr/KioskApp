using iBeautyNail.Datas;
using iBeautyNail.Devices.CardReader;
using iBeautyNail.Enums;
using iBeautyNail.Http;
using iBeautyNail.Http.Endpoints.ImageEndpoint.Models;
using iBeautyNail.Http.Endpoints.UserInfoEndpoint.Models;
using iBeautyNail.Messages;
using iBeautyNail.Messages.Exceptions.Enums;
using iBeautyNail.SDK;
using iBeautyNail.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace iBeautyNail.ViewModel
{
    class Q200_QRScanViewModel : BaseViewModelBase, INotifyPropertyChanged
    {
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

        public Q200_QRScanViewModel()
        {
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

            FileUtility.Instance.CloseTabTip();

            ScanText = "";
        }

        protected override void PageUnload()
        {
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyUpdate(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (scanText.Count(str => str == '*') == 7)
            {
                // Token값으로 Http Image List 호출
                string[] s = scanText.Split('*');

                DateTime dt = new DateTime();
                if (DateTime.TryParse(s[5], out dt))
                {
                    //if (dt < DateTime.Now)
                    //{
                    //    Console.WriteLine(string.Format("Expired QR code : {0}", dt));
                    //    logger.DebugFormat("{0} :: Expired QR code : {1}", CurrentViewModelName, dt);
                    //    // 기간이 만료된 QR 코드 입니다.
                    //    //PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_INFO, VALIDATION_MESSAGE.CONFIRM, VALIDATION_MESSAGE.VALIDATION_FULL_DESIGN);
                    //    ShowMessageLayerExpiredQR();
                    //    return;
                    //}

                    //Task.Run(() => GetImageList(s[4])).Wait();
                    //CommandAction(NAVIGATION_TYPE.Next);

                    //ShowMessageLayerWhileProcessing(s[4]);

                    // s[4] 가 비어있는 경우 예외처리
                    if (string.IsNullOrEmpty(s[4]))
                    {
                        return;
                    }

                    GlobalVariables.Instance.Token = s[4];
                    CommandAction(NAVIGATION_TYPE.Next);
                }
            }
        }
        #endregion

        private void ShowMessageLayerExpiredQR()
        {
            PopupMessageOption popupoption = new PopupMessageOption();
            popupoption.Button0Page = "Q200_QRScanViewModel";
            PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_WARNNING,
                         VALIDATION_MESSAGE.CONFIRM,
                         VALIDATION_MESSAGE.VALIDATION_EXPIRED_QR_CODE,
                         POPUP_QUANTITY.BUTTON_ONE, popupoption);
        }

        private void ShowMessageLayerInit()
        {
            PopupMessageOption popupoption = new PopupMessageOption();
            popupoption.Button0Page = "M100_StartPageViewModel";
            PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_WARNNING,
                         VALIDATION_MESSAGE.VALIDATION_NOT_VALID_QR_CODE,
                         VALIDATION_MESSAGE.VALIDATION_FIRST_PAGE,
                         POPUP_QUANTITY.BUTTON_ONE, popupoption);
        }
    }
}
