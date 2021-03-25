using GalaSoft.MvvmLight.CommandWpf;
using iBeautyNail.Datas;
using iBeautyNail.Enums;
using iBeautyNail.Pages;
using iBeautyNail.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Media;
using iBeautyNail.Utility;
using System.Windows.Media.Imaging;
using System.Windows;

namespace iBeautyNail.ViewModel
{
    class M290_USBLayoutViewModel : BaseViewModelBase
    {
        public string _createdFilePath;

        private ObservableCollection<DesignInfo> usbDesignInfoList = new ObservableCollection<DesignInfo>();
        public ObservableCollection<DesignInfo> USBDesignInfoList
        {
            get { return usbDesignInfoList; }
            set
            {
                if (usbDesignInfoList != value)
                    usbDesignInfoList = value;
                RaisePropertyChanged("USBDesignInfoList");
            }
        }

        private string myDesignPath;
        public string MyDesignPath
        {
            get { return myDesignPath; }
            set
            {
                if (myDesignPath != value)
                    myDesignPath = value;
                RaisePropertyChanged("MyDesignPath");
            }
        }

        private bool isUsbConnected;
        public bool IsUsbConnected
        {
            get { return isUsbConnected; }
            set
            {
                if (isUsbConnected != value)
                    isUsbConnected = value;

                RaisePropertyChanged("IsUsbConnected");
            }
        }


        private ImageSource modifyThumb;
        public ImageSource ModifyThumb  //USB 이미지 편집 시 보일 배경화면
        {
            get { return modifyThumb; }
            set
            {
                modifyThumb = value;
                RaisePropertyChanged("ModifyThumb");
            }
        }

        private ImageSource maskModifyThumb;
        public ImageSource MaskModifyThumb
        {
            get
            {
                return maskModifyThumb;
            }
            set
            {
                maskModifyThumb = value;
                RaisePropertyChanged("MaskModifyThumb");
            }
        }

        private int pbUserImageWidth;
        public int PbUserImageWidth //USB 편집시 각 손가락의 가로 폭
        {
            get
            {
                return pbUserImageWidth;
            }
            set
            {
                pbUserImageWidth = value;
                RaisePropertyChanged("PbUserImageWidth");
            }
        }

        private int imageWidth;
        public int ImageWidth //USB 편집시 각 손가락의 이미지의 가로 폭
        {
            get
            {
                return imageWidth;
            }
            set
            {
                imageWidth = value;
                RaisePropertyChanged("ImageWidth");
            }
        }

        public static int refNailPostion;

        private RelayCommand<DesignInfo> selectNailDesignCommand;
        public RelayCommand<DesignInfo> SelectNailDesignCommand
        {
            get
            {
                return selectNailDesignCommand ?? new RelayCommand<DesignInfo>((designInfo) =>
                {
                    Messenger.Default.Send<string>("ResetTimeout");

                    //Console.Write("Selected Nail Path=>{0}\n", designInfo.DesignPath);
                    logger.DebugFormat("{0} :: Selected Nail Path=>{1}\n", CurrentViewModelName, designInfo.DesignPath);
                    Messenger.Default.Send<DesignInfo>(designInfo);
                });
            }
        }

        private RelayCommand<DesignInfo> usbClickCommand;
        public RelayCommand<DesignInfo> USBClickCommand
        {
            get
            {
                return usbClickCommand ?? new RelayCommand<DesignInfo>((info) =>
                {
                    Messenger.Default.Send<string>("ResetTimeout");

                    if (new FileInfo(info.DesignPath).Attributes.HasFlag(FileAttributes.Directory))
                    {
                        string[] filter = { ".bmp", ".jpg", ".jpeg", ".png" };
                        USBDesignInfoList = CurrentPathInfo(info.DesignPath, filter);
                    }
                    else
                    {
                        // 이미지 편집으로
                        MyDesignPath = info.DesignPath;
                        //CommandManager.InvalidateRequerySuggested();

                    }
                });
            }
        }

        private Point mousePoint = new Point();
        private double _panelX;
        private double _panelY;

        public double PanelX
        {
            get { return _panelX; }
            set
            {
                if (value.Equals(_panelX)) return;
                _panelX = value;
                RaisePropertyChanged("PanelX");
            }
        }
        public double PanelY
        {
            get { return _panelY; }
            set
            {
                if (value.Equals(_panelY)) return;
                _panelY = value;
                RaisePropertyChanged("PanelY");
            }
        }

        private RelayCommand selectNailDesignDownCommand;
        public RelayCommand SelectNailDesignDownCommand
        {
            get
            {
                return selectNailDesignDownCommand ?? new RelayCommand(() =>
                {
                    //_buttonHoldStopWatch.Start();
                    //Console.WriteLine("MouseDown {0},{1}", PanelX, PanelY);
                    mousePoint.X = PanelX;
                    mousePoint.Y = PanelY;
                });
            }
        }

        private RelayCommand selectNailDesignUpCommand;
        public RelayCommand SelectNailDesignUpCommand
        {
            get
            {
                return selectNailDesignUpCommand ?? new RelayCommand(() =>
                {
                    Messenger.Default.Send<string>("ResetTimeout");

                    //Console.WriteLine("MouseUp {0},{1}", PanelX, PanelY);

                    if (PanelX == mousePoint.X && PanelY == mousePoint.Y)
                    {
                        logger.DebugFormat("{0} :: Selected Nail Path=>{1}\n", CurrentViewModelName, _createdFilePath);
                        CropUSBImage(); //마스크 씌우기 전에 선택한 USB이미지를 사각형으로 잘라놓는다

                        var di = new DesignInfo { Category = "USB", DesignPath = _createdFilePath, DesignCost = "" };
                        Messenger.Default.Send(di);

                        //Console.WriteLine("refNailPosition :    " + refNailPostion);
                        logger.DebugFormat("{0} :: refNailPosition : {1}\n", CurrentViewModelName, refNailPostion);
                        SetNewFingerInfo();

                        MyDesignPath = @"C:\nails\temp\tempImage.png";
                    }
                });
            }
        }

        public static int[,] intWidth = {
            { 333, 296 }
            ,{ 254, 254 }
            ,{ 254, 233 }
            ,{ 214, 214 }
            ,{ 190, 160 }
        };

        public void SetNewFingerInfo()
        {
            int integ = refNailPostion;
            BitmapImage OutNextModifyThumb;
            BitmapImage OutNextMaskModifyThumb;
            int OutNextPanelWidth;
            int OutNextImageWidth;

            if (integ < 5)//// 현재 입력될 위치가 왼손일때
            {

                OutNextModifyThumb = new BitmapImage(new Uri(string.Format("pack://siteoforigin:,,,/Resources/Images/bgmodifythumnew{0:D}.png", 5 - integ)));
                OutNextMaskModifyThumb = new BitmapImage(new Uri(string.Format("pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew{0:D}.png", 5 - integ)));
                OutNextPanelWidth = intWidth[4 - integ, 0];
                OutNextImageWidth = intWidth[4 - integ, 1];

            }

            else if (integ == 10)
            {
                OutNextModifyThumb = new BitmapImage(new Uri(string.Format("pack://siteoforigin:,,,/Resources/Images/bgmodifythumnew{0:D}.png", 1)));
                OutNextMaskModifyThumb = new BitmapImage(new Uri(string.Format("pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew{0:D}.png", 1)));
                OutNextPanelWidth = intWidth[0, 0];
                OutNextImageWidth = intWidth[0, 1];

            }

            else
            {
                OutNextModifyThumb = new BitmapImage(new Uri(string.Format("pack://siteoforigin:,,,/Resources/Images/bgmodifythumnew{0:D}.png", integ - 4)));
                OutNextMaskModifyThumb = new BitmapImage(new Uri(string.Format("pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew{0:D}.png", integ - 4)));
                OutNextPanelWidth = intWidth[integ - 5, 0];
                OutNextImageWidth = intWidth[integ - 5, 1];
            }

            ModifyThumb = OutNextModifyThumb;
            MaskModifyThumb = OutNextMaskModifyThumb;
            PbUserImageWidth = OutNextPanelWidth;
            ImageWidth = OutNextImageWidth;
        }

        public M290_USBLayoutViewModel()
        {
            LoadUSBDesigns();
      
        }

        public void CropUSBImage()
        {
            UIElement element = M290_USBLayout.PbUserImage2;

            if (element != null)
            {
                System.Windows.Size size = element.RenderSize;
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
                element.Measure(size);
                element.Arrange(new Rect(size));

                rtb.Render(element);
                PngBitmapEncoder png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(rtb));
                _createdFilePath = Path.Combine(Path.GetDirectoryName(GlobalVariables.Instance.TempResultPath), "IMSIusbImage_") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";

                using (Stream stm = File.Create(_createdFilePath))
                {
                    png.Save(stm);
                    stm.Dispose();
                }
            }
        }


        public void LoadUSBDesigns(string path = null)
        {
            List<FileInfo> listUserFiles = new List<FileInfo>();

            string[] filter = { ".bmp", ".jpg", ".jpeg", ".png" };


            try
            {
                USBDesignInfoList = CurrentPathInfo(path, filter);
                SetNewFingerInfo();
                IsUsbConnected = true;
            }

            catch (Exception e)
            {
                IsUsbConnected = false;
            }
        }

        public DriveInfo LoadDriveInfo()
        {
            DriveInfo maxDrive = null;

            List<DriveInfo> listDriveInfo = new List<DriveInfo>();
            List<FileInfo> listUserFiles = new List<FileInfo>();

            string[] ls_drivers = Directory.GetLogicalDrives();
            foreach (string device in ls_drivers)
            {
                DriveInfo dr = new DriveInfo(device);
                if (dr.DriveType == DriveType.Removable)
                //if (dr.DriveType == DriveType.Fixed)
                {
                    listDriveInfo.Add(dr);
                }
            }

            int nMax = 0;
            foreach (DriveInfo dr in listDriveInfo)
            {
                string sDriveLetter = dr.Name.Replace("\\", "");
                char c = char.Parse(sDriveLetter.Substring(0, 1));
                int n = c;
                if (nMax < n)
                {
                    nMax = n;
                    maxDrive = dr;
                }
            }

            return maxDrive;
        }

        public ObservableCollection<DesignInfo> CurrentPathInfo(string path, string[] filter)
        {
            ObservableCollection<DesignInfo> currentPath = new ObservableCollection<DesignInfo>();
            DirectoryInfo directoryInfo = new DirectoryInfo(path ?? LoadDriveInfo().RootDirectory.FullName);

            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                DesignInfo di = new DesignInfo();
                di.Category = "USB";
                di.DesignPath = dir.FullName;
                currentPath.Add(di);
            }

            FileInfo[] fileInfo = directoryInfo.GetFiles();

            foreach (FileInfo fi in fileInfo)
            {
                if (filter.Any(s => fi.Extension.ToLower().IndexOf(s) >= 0))
                {
                    DesignInfo di = new DesignInfo();
                    di.Category = "USB";
                    di.DesignPath = fi.FullName;
                    currentPath.Add(di);
                }
            }

            return currentPath;
        }
    }
}