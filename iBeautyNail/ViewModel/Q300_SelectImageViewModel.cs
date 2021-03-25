using GalaSoft.MvvmLight.CommandWpf;
using iBeautyNail.Datas;
using iBeautyNail.Devices.CardReader;
using iBeautyNail.Enums;
using iBeautyNail.Extensions;
using iBeautyNail.Http;
using iBeautyNail.Http.Endpoints.UserInfoEndpoint.Models;
using iBeautyNail.Messages;
using iBeautyNail.Messages.Exceptions.Enums;
using iBeautyNail.SDK;
using iBeautyNail.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace iBeautyNail.ViewModel
{
    class Q300_SelectImageViewModel : BaseViewModelBase
    {
        private IniFile iniInkLimit = new IniFile();
        private InkLimitInfo il = new InkLimitInfo();

        private bool waitingImageVisibility = true;
        public bool WaitingImageVisibility
        {
            get { return waitingImageVisibility; }
            set { Set(() => WaitingImageVisibility, ref waitingImageVisibility, value); }
        }

        private bool controlVisibility = false;
        public bool ControlVisibility
        {
            get { return controlVisibility; }
            set { Set(() => ControlVisibility, ref controlVisibility, value); }
        }

        private int designPrice;
        public int DesignPrice
        {
            get { return designPrice; }
            set
            {
                designPrice = value;
                RaisePropertyChanged("DesignPrice");
            }
        }

        private int totalPrintCount;
        public int TotalPrintCount
        {
            get { return totalPrintCount; }
            set
            {
                Set(() => TotalPrintCount, ref totalPrintCount, value);
                DesignPrice = Int32.Parse(GlobalVariables.Instance.DesignPrice.ToString()) * totalPrintCount;
            }
        }

        ObservableCollection<QRImageListInfo> qrImageList;
        public ObservableCollection<QRImageListInfo> QRImageList
        {
            get { return qrImageList; }
            set
            {
                qrImageList = value;
                RaisePropertyChanged("QRImageList");
            }
        }

        System.Windows.Threading.DispatcherTimer timer;
        public Q300_SelectImageViewModel()
        {
            qrImageList = new ObservableCollection<QRImageListInfo>();

        }

        protected override void PageLoad()
        {
            // Language 팝업은 Load안함
            if (GlobalVariables.Instance.LanguagePopup)
            {
                GlobalVariables.Instance.LanguagePopup = false;
                return;
            }

            PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_INFO, VALIDATION_MESSAGE.CONFIRM, VALIDATION_MESSAGE.IMAGE_DOWNLOADING);

            PrevButtonVisible = false;
            NextButtonVisible = false;
            HomeButtonVisible = false;

            // 초기화 
            DesignPrice = 0;
            TotalPrintCount = 0;
            GlobalVariables.Instance.QRData.Clear();
            
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (object sender, EventArgs e) =>
            {
                {
                    if (timer.IsEnabled) 
                        timer.Stop();

                    Task.Run(() =>
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            timer.Stop();

                            Task.Run(() => GetImageList(GlobalVariables.Instance.Token)).Wait();

                            QRImageList = GlobalVariables.Instance.QRData.ToObservableCollection();
                            Button1Command();

                            // 흰색 잉크 70으로
                            if (File.Exists(Datas.GlobalVariables.Instance.PrintInkLimitPath))
                            {
                                LoadPrintInkLimit(Datas.GlobalVariables.Instance.PrintInkLimitPath, ref il);

                                il.WhiteValue = "70.00";
                                SavePrintInkLimit(Datas.GlobalVariables.Instance.PrintInkLimitPath, il);
                            }

                            PrevButtonVisible = true;
                            ControlVisibility = true;
                            HomeButtonVisible = true;
                        }));
                    });
                }
            };

            timer.Start();
        }

        protected override void PageUnload()
        {
            GlobalVariables.Instance.MyProduct.price = DesignPrice;
        }

        private async Task GetImageList(string Token)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            var req = new UserInfoRequestObj { Username = "admin", Password = "dkdlfhqh!" };
            var res = await Api.UserInfo.GetUserInfoByLoginAsync(req);

            //Console.WriteLine(string.Format("Admin Logged in :: {0} :: {1}", res.Token, Token));
            logger.DebugFormat("{0} :: Admin Logged in :: {1} :: {2}", CurrentViewModelName, res.Token, Token);

            Api = NailApi.GetDevelopmentInstance(res.Token);
            var resImageList = await Api.ImageList.GetImageListTokenAsync(Token);

            // 이미지 역순 정렬
            resImageList.Datas.Reverse();

            foreach (var d in resImageList.Datas)
            {
                //string thumbnail = d.Thumbnail;
                // 실제 이미지 받음
                var resImage = await Api.Image.GetImageAsync(d.Username, d.Index);

                string thumbnail = resImage.Datas[0].Thumbnail;
                string imagedata = resImage.Datas[0].ImageData;

                string filepath = string.Format(@"C:\nails\temp\testThumbnail_{0}", d.Index);
                string filepathImg = string.Format(@"C:\nails\temp\nailpan_{0}", d.Index);

                BitmapSource bs = ImageUtility.Instance.DecodeBase64Image(thumbnail);
                ImageUtility.Instance.SavePNGFile(bs, filepath + ".png", PngInterlaceOption.Default);

                BitmapSource bsImg = ImageUtility.Instance.DecodeBase64Image(imagedata);
                ImageUtility.Instance.SavePNGFile(bsImg, filepathImg + ".png", PngInterlaceOption.Default);

                GlobalVariables.Instance.QRData.Add(new QRImageListInfo() { PrintImage = filepathImg + ".png", TempImagePath = filepath + ".png", PrintCount = 0, IsSelected = false });
            }
        }

        private RelayCommand<string> clickCommand;
        public RelayCommand<string> ClickCommand
        {
            get
            {
                return clickCommand ?? new RelayCommand<string>((selectedPaymentMethod) =>
                {
                    //Console.WriteLine(string.Format("TotalPrintCount :: {0}", totalPrintCount));
                    logger.DebugFormat("{0} :: TotalPrintCount : {1}", CurrentViewModelName, totalPrintCount);
                    if (totalPrintCount == 0 || totalPrintCount > 3)
                        return;

                    switch (selectedPaymentMethod)
                    {
                        case "Card":
                            logger.DebugFormat("{0} :: Go to payment process", CurrentViewModelName);
                            CommandAction(NAVIGATION_TYPE.Next);
                            break;
                        case "Pay":
                            break;
                        case "QR":
                            break;
                        default:
                            break;
                    }
                });
            }
        }

        private void LoadPrintInkLimit(string path, ref InkLimitInfo il)
        {
            if (File.Exists(path) == false)
            {
                Console.WriteLine(string.Format("File No Exists :: {0}", path));
                return;
            }

            iniInkLimit.Load(path);
            il.CyanValue = iniInkLimit["Print11"]["Cyan Limit"].ToString();
            il.MagentaValue = iniInkLimit["Print11"]["Magenta Limit"].ToString();
            il.YellowValue = iniInkLimit["Print11"]["Yellow Limit"].ToString();
            il.BlackValue = iniInkLimit["Print11"]["Black Limit"].ToString();
            il.WhiteValue = iniInkLimit["Print11"]["White Ink Limit"].ToString();
            logger.InfoFormat("{0} :: Ini Load :: C {1} M {2} Y {3} K {4} W {5}", CurrentViewModelName, il.CyanValue, il.MagentaValue, il.YellowValue, il.BlackValue, il.WhiteValue);
        }

        private void SavePrintInkLimit(string path, InkLimitInfo il)
        {
            if (File.Exists(path) == false)
            {
                Console.WriteLine(string.Format("File No Exists :: {0}", path));
                return;
            }

            iniInkLimit["Print11"]["Cyan Limit"] = il.CyanValue;
            iniInkLimit["Print11"]["Magenta Limit"] = il.MagentaValue;
            iniInkLimit["Print11"]["Yellow Limit"] = il.YellowValue;
            iniInkLimit["Print11"]["Black Limit"] = il.BlackValue;
            iniInkLimit["Print11"]["White Ink Limit"] = il.WhiteValue;
            iniInkLimit.Save(path);
            logger.InfoFormat("{0} :: Ini Save :: C {1} M {2} Y {3} K {4} W {5}", CurrentViewModelName, il.CyanValue, il.MagentaValue, il.YellowValue, il.BlackValue, il.WhiteValue);
        }
    }
}
