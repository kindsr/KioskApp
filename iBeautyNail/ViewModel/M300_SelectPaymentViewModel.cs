using iBeautyNail.Datas;
using iBeautyNail.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Interop;
using GalaSoft.MvvmLight.CommandWpf;
using iBeautyNail.Enums;
using System.Windows.Media.Imaging;
using iBeautyNail.Utility;
using System.Threading;
using System.Speech.Synthesis;
using System.Globalization;

namespace iBeautyNail.ViewModel
{
    class M300_SelectPaymentViewModel : BaseViewModelBase
    {
        Image[] printImg = new Image[22];
        Bitmap[] printResult = new Bitmap[22];

        Rectangle[] m_rcSaveImageTop = new Rectangle[11];   // 스티커 위쪽 이미지 위치들
        Rectangle[] m_rcSaveImageBottom = new Rectangle[11];    // 스티커 아래쪽 이미지 위치들
        Rectangle[] m_rcSignature = new Rectangle[2];

        // Ink Limit if it is exists "GRADATION" category
        private IniFile iniInkLimit;
        private InkLimitInfo il;

        // PrintImageCordinator 
        string[] PrintImageTop = new string[11];
        string[] PrintImageBottom = new string[11];
        string[] PrintImageSign = new string[2];

        Image[] printMaskOri = {
            ImageUtility.Instance.BitmapImage2Image(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Images/nailmask01.png", UriKind.Absolute))),
            ImageUtility.Instance.BitmapImage2Image(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Images/nailmask02.png", UriKind.Absolute))),
            ImageUtility.Instance.BitmapImage2Image(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Images/nailmask03.png", UriKind.Absolute))),
            ImageUtility.Instance.BitmapImage2Image(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Images/nailmask04.png", UriKind.Absolute))),
            ImageUtility.Instance.BitmapImage2Image(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Images/nailmask05.png", UriKind.Absolute))),
            ImageUtility.Instance.BitmapImage2Image(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Images/nailmask06.png", UriKind.Absolute))),
            ImageUtility.Instance.BitmapImage2Image(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Images/nailmask07.png", UriKind.Absolute))),
            ImageUtility.Instance.BitmapImage2Image(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Images/nailmask08.png", UriKind.Absolute))),
            ImageUtility.Instance.BitmapImage2Image(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Images/nailmask09.png", UriKind.Absolute))),
            ImageUtility.Instance.BitmapImage2Image(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Images/nailmask10.png", UriKind.Absolute))),
            ImageUtility.Instance.BitmapImage2Image(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Images/nailmask11.png", UriKind.Absolute)))
        };

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
            get
            {
                //designPrice = MyDesigns.Sum(item => Int32.Parse(item.DesignInfo.DesignCost));
                designPrice = Int32.Parse(GlobalVariables.Instance.DesignPrice.ToString()) * PrintCount;
                return designPrice;
            }
            set
            {
                designPrice = value;
                //Console.WriteLine(string.Format("DesignPrice :: {0}", designPrice));
                RaisePropertyChanged("DesignPrice");
            }
        }

        private int printCount;
        public int PrintCount
        {
            get { return printCount; }
            set
            {
                Set(() => PrintCount, ref printCount, value);
                if (printCount <= 0) printCount = 1;
                if (printCount > 3)
                {
                    PrintCount = 3;
                }
                DesignPrice = Int32.Parse(GlobalVariables.Instance.DesignPrice.ToString()) * printCount;
                //Console.WriteLine(string.Format("PrintCount : {0} :: DesignPrice : {1}", printCount, designPrice));
            }
        }

        ObservableCollection<Nail> _myDesigns = new ObservableCollection<Nail>();
        public ObservableCollection<Nail> MyDesigns
        {
            get { return _myDesigns; }
            set
            {
                if (Equals(value, _myDesigns)) return;
                _myDesigns = value;
                RaisePropertyChanged("MyDesigns");
            }
        }

        private string tempResultPath;
        public string TempResultPath
        {
            get { return tempResultPath; }
            //set { Set(() => TempResultPath, ref tempResultPath, value); }
            set
            {
                if (Equals(value, tempResultPath)) return;
                tempResultPath = value;
                RaisePropertyChanged("TempResultPath");
            }
        }

        public M300_SelectPaymentViewModel()
        {
            /* 원본
            // 20191120 2019년 11월 입고 스티커 
            m_rcSaveImageTop[0] = new Rectangle(43, 42, 109, 370);
            m_rcSaveImageTop[1] = new Rectangle(176, 42, 115, 370);
            m_rcSaveImageTop[2] = new Rectangle(316, 42, 128, 370);
            m_rcSaveImageTop[3] = new Rectangle(469, 42, 143, 370);
            m_rcSaveImageTop[4] = new Rectangle(637, 42, 143, 370);
            m_rcSaveImageTop[5] = new Rectangle(805, 42, 157, 370);
            m_rcSaveImageTop[6] = new Rectangle(986, 42, 171, 370);
            m_rcSaveImageTop[7] = new Rectangle(1182, 42, 171, 370);
            m_rcSaveImageTop[8] = new Rectangle(1378, 42, 171, 370);
            m_rcSaveImageTop[9] = new Rectangle(1573, 42, 200, 370);
            m_rcSaveImageTop[10] = new Rectangle(1797, 42, 225, 370);

            m_rcSaveImageBottom[0] = new Rectangle(43, 467, 109, 370);
            m_rcSaveImageBottom[1] = new Rectangle(176, 467, 115, 370);
            m_rcSaveImageBottom[2] = new Rectangle(316, 467, 128, 370);
            m_rcSaveImageBottom[3] = new Rectangle(469, 467, 143, 370);
            m_rcSaveImageBottom[4] = new Rectangle(637, 467, 143, 370);
            m_rcSaveImageBottom[5] = new Rectangle(805, 467, 157, 370);
            m_rcSaveImageBottom[6] = new Rectangle(986, 467, 171, 370);
            m_rcSaveImageBottom[7] = new Rectangle(1182, 467, 171, 370);
            m_rcSaveImageBottom[8] = new Rectangle(1378, 467, 171, 370);
            m_rcSaveImageBottom[9] = new Rectangle(1573, 467, 200, 370);
            m_rcSaveImageBottom[10] = new Rectangle(1797, 467, 225, 370);
            */

            //윗줄은 y좌표 -2px , 아랫줄은 y좌표 +2 px 20200907
            //m_rcSaveImageTop[0] = new Rectangle(43, 42, 104, 365);
            //m_rcSaveImageTop[1] = new Rectangle(176, 42, 110, 365);
            //m_rcSaveImageTop[2] = new Rectangle(316, 42, 124, 365);
            //m_rcSaveImageTop[3] = new Rectangle(469, 42, 139, 365);
            //m_rcSaveImageTop[4] = new Rectangle(637, 42, 139, 365);
            //m_rcSaveImageTop[5] = new Rectangle(806, 42, 152, 365);
            //m_rcSaveImageTop[6] = new Rectangle(987, 42, 167, 365);
            //m_rcSaveImageTop[7] = new Rectangle(1183, 42, 167, 365);
            //m_rcSaveImageTop[8] = new Rectangle(1379, 42, 167, 365);
            //m_rcSaveImageTop[9] = new Rectangle(1575, 42, 195, 365);
            //m_rcSaveImageTop[10] = new Rectangle(1799, 42, 220, 365);

            //m_rcSaveImageBottom[0] = new Rectangle(43, 471, 104, 365);
            //m_rcSaveImageBottom[1] = new Rectangle(176, 471, 110, 365);
            //m_rcSaveImageBottom[2] = new Rectangle(316, 471, 124, 365);
            //m_rcSaveImageBottom[3] = new Rectangle(469, 471, 139, 365);
            //m_rcSaveImageBottom[4] = new Rectangle(637, 471, 139, 365);
            //m_rcSaveImageBottom[5] = new Rectangle(806, 471, 152, 365);
            //m_rcSaveImageBottom[6] = new Rectangle(987, 471, 167, 365);
            //m_rcSaveImageBottom[7] = new Rectangle(1183, 471, 167, 365);
            //m_rcSaveImageBottom[8] = new Rectangle(1379, 471, 167, 365);
            //m_rcSaveImageBottom[9] = new Rectangle(1575, 471, 195, 365);
            //m_rcSaveImageBottom[10] = new Rectangle(1799, 471, 220, 365);


            //사방을 처음보다 총 4px씩만 깎는다 (아래는 도면 좌표 적용 이전의 값임)
            m_rcSaveImageTop[0] = new Rectangle(45, 46, 100, 361);
            m_rcSaveImageTop[1] = new Rectangle(178, 46, 106, 361);
            m_rcSaveImageTop[2] = new Rectangle(318, 46, 120, 361);
            m_rcSaveImageTop[3] = new Rectangle(471, 46, 135, 361);
            m_rcSaveImageTop[4] = new Rectangle(639, 46, 135, 361);
            m_rcSaveImageTop[5] = new Rectangle(808, 46, 148, 361);
            m_rcSaveImageTop[6] = new Rectangle(989, 46, 163, 361);
            m_rcSaveImageTop[7] = new Rectangle(1185, 46, 163, 361);
            m_rcSaveImageTop[8] = new Rectangle(1381, 46, 163, 361);
            m_rcSaveImageTop[9] = new Rectangle(1577, 46, 191, 361);
            m_rcSaveImageTop[10] = new Rectangle(1801, 46, 216, 361);

            m_rcSaveImageBottom[0] = new Rectangle(45, 475, 100, 361);
            m_rcSaveImageBottom[1] = new Rectangle(178, 475, 106, 361);
            m_rcSaveImageBottom[2] = new Rectangle(318, 475, 120, 361);
            m_rcSaveImageBottom[3] = new Rectangle(471, 475, 135, 361);
            m_rcSaveImageBottom[4] = new Rectangle(639, 475, 135, 361);
            m_rcSaveImageBottom[5] = new Rectangle(808, 475, 148, 361);
            m_rcSaveImageBottom[6] = new Rectangle(989, 475, 163, 361);
            m_rcSaveImageBottom[7] = new Rectangle(1185, 475, 163, 361);
            m_rcSaveImageBottom[8] = new Rectangle(1381, 475, 163, 361);
            m_rcSaveImageBottom[9] = new Rectangle(1577, 475, 191, 361);
            m_rcSaveImageBottom[10] = new Rectangle(1801, 475, 216, 361);

            m_rcSignature[0] = new Rectangle(50, 890, 700, 120);
            m_rcSignature[1] = new Rectangle(1200, 890, 700, 120);

            // 이후 좌표는 Configs\\PrintImageCordinator.ini파일에 기록한다 
            // 좌표Ini파일 Read
            if (File.Exists("Configs\\PrintImageCordinator.ini"))
            {
                var iniFile = new IniFile();
                iniFile.Load("Configs\\PrintImageCordinator.ini");
                PrintImageTop[0] = iniFile["TOP"]["Top0"].ToString();
                PrintImageTop[1] = iniFile["TOP"]["Top1"].ToString();
                PrintImageTop[2] = iniFile["TOP"]["Top2"].ToString();
                PrintImageTop[3] = iniFile["TOP"]["Top3"].ToString();
                PrintImageTop[4] = iniFile["TOP"]["Top4"].ToString();
                PrintImageTop[5] = iniFile["TOP"]["Top5"].ToString();
                PrintImageTop[6] = iniFile["TOP"]["Top6"].ToString();
                PrintImageTop[7] = iniFile["TOP"]["Top7"].ToString();
                PrintImageTop[8] = iniFile["TOP"]["Top8"].ToString();
                PrintImageTop[9] = iniFile["TOP"]["Top9"].ToString();
                PrintImageTop[10] = iniFile["TOP"]["Top10"].ToString();

                PrintImageBottom[0] = iniFile["BOTTOM"]["Bottom0"].ToString();
                PrintImageBottom[1] = iniFile["BOTTOM"]["Bottom1"].ToString();
                PrintImageBottom[2] = iniFile["BOTTOM"]["Bottom2"].ToString();
                PrintImageBottom[3] = iniFile["BOTTOM"]["Bottom3"].ToString();
                PrintImageBottom[4] = iniFile["BOTTOM"]["Bottom4"].ToString();
                PrintImageBottom[5] = iniFile["BOTTOM"]["Bottom5"].ToString();
                PrintImageBottom[6] = iniFile["BOTTOM"]["Bottom6"].ToString();
                PrintImageBottom[7] = iniFile["BOTTOM"]["Bottom7"].ToString();
                PrintImageBottom[8] = iniFile["BOTTOM"]["Bottom8"].ToString();
                PrintImageBottom[9] = iniFile["BOTTOM"]["Bottom9"].ToString();
                PrintImageBottom[10] = iniFile["BOTTOM"]["Bottom10"].ToString();

                PrintImageSign[0] = iniFile["SIGN"]["Sign1"].ToString();
                PrintImageSign[1] = iniFile["SIGN"]["Sign2"].ToString();

                for (int i = 0; i < 11; i++)
                {
                    string[] tmp = PrintImageTop[i].Trim().Split(',');
                    m_rcSaveImageTop[i] = new Rectangle(int.Parse(tmp[0]), int.Parse(tmp[1]), int.Parse(tmp[2]), int.Parse(tmp[3]));
                    tmp = PrintImageBottom[i].Trim().Split(',');
                    m_rcSaveImageBottom[i] = new Rectangle(int.Parse(tmp[0]), int.Parse(tmp[1]), int.Parse(tmp[2]), int.Parse(tmp[3]));
                }

                string[] tmpSign1 = PrintImageSign[0].Trim().Split(',');
                m_rcSignature[0] = new Rectangle(int.Parse(tmpSign1[0]), int.Parse(tmpSign1[1]), int.Parse(tmpSign1[2]), int.Parse(tmpSign1[3]));

                string[] tmpSign2 = PrintImageSign[1].Trim().Split(',');
                m_rcSignature[1] = new Rectangle(int.Parse(tmpSign2[0]), int.Parse(tmpSign2[1]), int.Parse(tmpSign2[2]), int.Parse(tmpSign2[3]));
            }

            iniInkLimit = new IniFile();
            il = new InkLimitInfo();

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
            WaitingImageVisibility = true;
            PrevButtonVisible = false;
            HomeButtonVisible = false;
            ControlVisibility = false;

            MyDesigns = GlobalVariables.Instance.MyDesigns.ToObservableCollection<Nail>();
            PrintCount = GlobalVariables.Instance.MyProduct.qty;

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (object sender, EventArgs e) =>
            {
                timer.Stop();
                Task.Run(() => {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        if (GlobalVariables.Instance.IsTTSOn)
                        {
                            threadDelegate = new ThreadStart(CommentWork);
                            commentThread = new Thread(threadDelegate);
                            commentThread.Start();
                        }

                        MakePrintImage();
                    }));
                });
            };
            timer.Start();
        }

        protected override void PageUnload()
        {
            if (GlobalVariables.Instance.IsTTSOn)
            {
                synthesizer.SpeakAsyncCancelAll();
                //synthesizer.Dispose();
                commentThread.Abort();
            }

            GlobalVariables.Instance.MyProduct.qty = PrintCount;
            GlobalVariables.Instance.MyProduct.price = DesignPrice;
            TempResultPath = "";
        }

        protected override void CommentWork()
        {
            try
            {
                synthesizer = new SpeechSynthesizer();
                synthesizer.Rate = 2;
                synthesizer.SetOutputToDefaultAudioDevice();

                var builder = new PromptBuilder();
                builder.StartVoice(new CultureInfo(Culture));
                builder.AppendText(App.LanguageMng.LanguageSet[Culture]["M300_ctTbDesignConfirm"].Sentence);
                builder.AppendText(",     ");
                builder.AppendText(App.LanguageMng.LanguageSet[Culture]["M300_ctTbDesignConfirm2"].Sentence);
                builder.EndVoice();
                synthesizer.SpeakAsync(builder);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("{0} :: CommentWork Exception :: {1}", CurrentViewModelName, ex.ToString());
            }
        }

        internal void MakePrintImage()
        {
            string sRootPath = "";
            Image img = null;
            Image rotImg = null;

            try
            {
                sRootPath = Path.GetDirectoryName(GlobalVariables.Instance.TempResultPath);
                if (!Directory.Exists(sRootPath))
                    Directory.CreateDirectory(sRootPath);

                GC.Collect();
                GC.WaitForPendingFinalizers();

                if (File.Exists(GlobalVariables.Instance.TempResultPath))
                    File.Delete(GlobalVariables.Instance.TempResultPath);

                if (File.Exists(Datas.GlobalVariables.Instance.PrintInkLimitPath))
                    LoadPrintInkLimit(Datas.GlobalVariables.Instance.PrintInkLimitPath, ref il);

                // GRADATION folder의 네일이미지 있을때 WhiteValue = 1
                if (MyDesigns.Where(s => s.Selected == true && s.DesignInfo.DesignPath.Contains("GRADATION")).Count() > 0)
                {
                    il.WhiteValue = "1.00";
                    logger.DebugFormat("디자인 :: MyDesigns :: GRADATION included {0}", il.WhiteValue);
                }
                else
                {
                    il.WhiteValue = "70.00";
                    logger.DebugFormat("디자인 :: MyDesigns :: {0}", il.WhiteValue);
                }

                SavePrintInkLimit(Datas.GlobalVariables.Instance.PrintInkLimitPath, il);

                img = Image.FromFile(GlobalVariables.Instance.BackgroundPath);
                using (Graphics g = Graphics.FromImage(img))
                {
                    Font drawFont = new Font("Coronet", 18);

                    g.DrawString(GlobalVariables.Instance.Sign1, drawFont, Brushes.Black, m_rcSignature[0], new StringFormat { Alignment = StringAlignment.Near });
                    g.DrawString(GlobalVariables.Instance.Sign2, drawFont, Brushes.Black, m_rcSignature[1], new StringFormat { Alignment = StringAlignment.Far });
                    
                    for (int i = 0; i < MyDesigns.Count; i++)
                    {
                        Nail nail = MyDesigns[i];
                        if (nail.Selected)
                        {
                            if (nail.Vacant == false)
                            {
                                if (nail.UsbImageYN == false)
                                {

                                    string sfilename = nail.DesignInfo.DesignPath;
                                    int imsiWidth = 224;
                                    int imsiHeight = 369;
                                    bool isRotated = nail.IsRotated;

                                    Console.WriteLine("sfilename " + i + " : " + sfilename);

                                    string fileNumber = "";
                                    string sfilenamenew = "";
                                    int sfilefoldercnt = 0;
                                    char pad = '0';

                                    string designFolderPath = Path.ChangeExtension(sfilename, null);

                                    if (Directory.Exists(designFolderPath))
                                    {
                                        DirectoryInfo di = new DirectoryInfo(designFolderPath);
                                        sfilefoldercnt = di.GetFiles().Count();
                                    }

                                    #region 10개 세트
                                    if (sfilename.Contains("ARTIST") ||
                                        sfilename.ToLower().Contains("accept"))  // Artist Pick 잘려진 22개의 이미지를 갖다 쓴다
                                    {
                                        designFolderPath = Path.GetDirectoryName(sfilename);

                                        if (i == 0)    // LeftPinky
                                        {
                                            //for (int j = i; j < i + 3; j++)
                                            //{
                                            //    fileNumber = (j + 1).ToString();
                                            //    sfilenamenew = Path.Combine(designFolderPath, fileNumber.PadLeft(2, pad) + ".png");
                                            //    g.DrawImage(DesignConvert(sfilenamenew, isRotated, printMaskOri[j], imsiWidth, imsiHeight), m_rcSaveImageTop[j]);
                                            //}
                                            if (sfilefoldercnt == 11)
                                            {
                                                for (int j = i; j < i + 3; j++)
                                                {
                                                    fileNumber = (j + 1).ToString();
                                                    sfilenamenew = Path.Combine(designFolderPath, Path.GetFileNameWithoutExtension(sfilename), fileNumber.PadLeft(2, pad) + ".png");
                                                    g.DrawImage(DesignConvert(sfilenamenew, isRotated, null, imsiWidth, imsiHeight), m_rcSaveImageTop[j]);
                                                }
                                            }
                                            else
                                            {
                                                for (int j = i; j < i + 3; j++)
                                                {
                                                    g.DrawImage(DesignConvert(sfilename, isRotated, printMaskOri[j], imsiWidth, imsiHeight), m_rcSaveImageTop[j]);
                                                }
                                            }
                                        }
                                        else if (i >= 1 && i <= 4)   // LeftRing, LeftMiddle, LeftIndex, LeftThumb
                                        {
                                            //for (int j = i * 2 + 1; j <= i * 2 + 2; j++)
                                            //{
                                            //    fileNumber = (j + 1).ToString();
                                            //    sfilenamenew = Path.Combine(designFolderPath, fileNumber.PadLeft(2, pad) + ".png");
                                            //    g.DrawImage(DesignConvert(sfilenamenew, isRotated, printMaskOri[j], imsiWidth, imsiHeight), m_rcSaveImageTop[j]);
                                            //}
                                            if (sfilefoldercnt == 11)
                                            {
                                                for (int j = i * 2 + 1; j <= i * 2 + 2; j++)
                                                {
                                                    fileNumber = (j + 1).ToString();

                                                    sfilenamenew = Path.Combine(designFolderPath, Path.GetFileNameWithoutExtension(sfilename), fileNumber.PadLeft(2, pad) + ".png");
                                                    g.DrawImage(DesignConvert(sfilenamenew, isRotated, null, imsiWidth, imsiHeight), m_rcSaveImageTop[j]);
                                                }
                                            }
                                            else
                                            {
                                                for (int j = i * 2 + 1; j <= i * 2 + 2; j++)
                                                {
                                                    g.DrawImage(DesignConvert(sfilename, isRotated, printMaskOri[j], imsiWidth, imsiHeight), m_rcSaveImageTop[j]);
                                                }
                                            }
                                        }
                                        else if (i >= 5 && i <= 8)   // RightThumb, RightIndex, RightMiddle, RightRing
                                        {
                                            //for (int j = i * 2 + 1; j <= i * 2 + 2; j++)
                                            //{
                                            //    fileNumber = (j + 1).ToString();
                                            //    sfilenamenew = Path.Combine(designFolderPath, fileNumber.PadLeft(2, pad) + ".png");
                                            //    g.DrawImage(DesignConvert(sfilenamenew, isRotated, printMaskOri[21-j], imsiWidth, imsiHeight), m_rcSaveImageBottom[21-j]);
                                            //}
                                            if (sfilefoldercnt == 11)
                                            {
                                                for (int j = i * 2 + 1; j <= i * 2 + 2; j++)
                                                {
                                                    fileNumber = (22 - j).ToString();
                                                    sfilenamenew = Path.Combine(designFolderPath, Path.GetFileNameWithoutExtension(sfilename), fileNumber.PadLeft(2, pad) + ".png");
                                                    g.DrawImage(DesignConvert(sfilenamenew, isRotated, null, imsiWidth, imsiHeight), m_rcSaveImageBottom[21 - j]);
                                                }
                                            }
                                            else
                                            {
                                                for (int j = i * 2 + 1; j <= i * 2 + 2; j++)
                                                {
                                                    g.DrawImage(DesignConvert(sfilename, isRotated, printMaskOri[21 - j], imsiWidth, imsiHeight), m_rcSaveImageBottom[21 - j]);
                                                }
                                            }
                                        }
                                        else if (i == 9)    //RightPinky
                                        {
                                            //for (int j = i * 2 + 1; j <= i * 2 + 3; j++)
                                            //{
                                            //    fileNumber = (j + 1).ToString();
                                            //    sfilenamenew = Path.Combine(designFolderPath, fileNumber.PadLeft(2, pad) + ".png");
                                            //    g.DrawImage(DesignConvert(sfilenamenew, isRotated, printMaskOri[21 - j], imsiWidth, imsiHeight), m_rcSaveImageBottom[21 - j]);
                                            //}
                                            if (sfilefoldercnt == 11)
                                            {
                                                for (int j = i * 2 + 1; j <= i * 2 + 3; j++)
                                                {
                                                    fileNumber = (22 - j).ToString();
                                                    sfilenamenew = Path.Combine(designFolderPath, Path.GetFileNameWithoutExtension(sfilename), fileNumber.PadLeft(2, pad) + ".png");
                                                    g.DrawImage(DesignConvert(sfilenamenew, isRotated, null, imsiWidth, imsiHeight), m_rcSaveImageBottom[21 - j]);
                                                }
                                            }
                                            else
                                            {
                                                for (int j = i * 2 + 1; j <= i * 2 + 3; j++)
                                                {
                                                    g.DrawImage(DesignConvert(sfilename, isRotated, printMaskOri[21 - j], imsiWidth, imsiHeight), m_rcSaveImageBottom[21 - j]);
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    #region 5개 세트
                                    else if (sfilefoldercnt == 11)  // 구 버전 (잘려진 11개의 이미지를 갖다 쓴다)
                                    {
                                        if (i == 0)    // LeftPinky
                                        {
                                            for (int j = i; j < i + 3; j++)
                                            {
                                                fileNumber = (j + 1).ToString();
                                                sfilenamenew = Path.Combine(designFolderPath, fileNumber.PadLeft(2, pad) + ".png");
                                                g.DrawImage(DesignConvert(sfilenamenew, isRotated, null, imsiWidth, imsiHeight), m_rcSaveImageTop[j]);
                                            }
                                        }
                                        else if (i >= 1 && i <= 4)   // LeftRing, LeftMiddle, LeftIndex, LeftThumb
                                        {
                                            for (int j = i * 2 + 1; j <= i * 2 + 2; j++)
                                            {
                                                fileNumber = (j + 1).ToString();

                                                sfilenamenew = Path.Combine(designFolderPath, fileNumber.PadLeft(2, pad) + ".png");
                                                g.DrawImage(DesignConvert(sfilenamenew, isRotated, null, imsiWidth, imsiHeight), m_rcSaveImageTop[j]);
                                            }
                                        }
                                        else if (i >= 5 && i <= 8)   // RightThumb, RightIndex, RightMiddle, RightRing
                                        {
                                            for (int j = i * 2 + 1; j <= i * 2 + 2; j++)
                                            {
                                                fileNumber = (22 - j).ToString();
                                                sfilenamenew = Path.Combine(designFolderPath, fileNumber.PadLeft(2, pad) + ".png");
                                                g.DrawImage(DesignConvert(sfilenamenew, isRotated, null, imsiWidth, imsiHeight), m_rcSaveImageBottom[21 - j]);
                                            }
                                        }

                                        else if (i == 9)    //RightPinky
                                        {
                                            for (int j = i * 2 + 1; j <= i * 2 + 3; j++)
                                            {
                                                fileNumber = (22 - j).ToString();
                                                sfilenamenew = Path.Combine(designFolderPath, fileNumber.PadLeft(2, pad) + ".png");
                                                g.DrawImage(DesignConvert(sfilenamenew, isRotated, null, imsiWidth, imsiHeight), m_rcSaveImageBottom[21 - j]);
                                            }
                                        }
                                    }
                                    #endregion
                                    #region 일반이미지
                                    //새 버전(하나의 이미지 파일을 손가락에 맞게 자른다)
                                    else
                                    {
                                        if (i == 0)    // LeftPinky
                                        {
                                            for (int j = i; j < i + 3; j++)
                                            {
                                                g.DrawImage(DesignConvert(sfilename, isRotated, printMaskOri[j], imsiWidth, imsiHeight), m_rcSaveImageTop[j]);
                                            }
                                        }
                                        else if (i >= 1 && i <= 4)   // LeftRing, LeftMiddle, LeftIndex, LeftThumb
                                        {
                                            for (int j = i * 2 + 1; j <= i * 2 + 2; j++)
                                            {
                                                g.DrawImage(DesignConvert(sfilename, isRotated, printMaskOri[j], imsiWidth, imsiHeight), m_rcSaveImageTop[j]);
                                            }
                                        }
                                        else if (i >= 5 && i <= 8)   // RightThumb, RightIndex, RightMiddle, RightRing
                                        {
                                            for (int j = i * 2 + 1; j <= i * 2 + 2; j++)
                                            {
                                                g.DrawImage(DesignConvert(sfilename, isRotated, printMaskOri[21 - j], imsiWidth, imsiHeight), m_rcSaveImageBottom[21 - j]);
                                            }
                                        }
                                        else if (i == 9)    //RightPinky
                                        {
                                            for (int j = i * 2 + 1; j <= i * 2 + 3; j++)
                                            {
                                                g.DrawImage(DesignConvert(sfilename, isRotated, printMaskOri[21 - j], imsiWidth, imsiHeight), m_rcSaveImageBottom[21 - j]);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                #region USB Image
                                // USB Image
                                else
                                {
                                    string sfilename = nail.DesignInfo.DesignPath;
                                    bool isRotated = nail.IsRotated;
                                    if (i == 0)    // LeftPinky
                                    {
                                        for (int j = i; j < i + 3; j++)
                                        {
                                            g.DrawImage(DesignConvert(sfilename, isRotated, printMaskOri[j], Image.FromFile(sfilename).Width, Image.FromFile(sfilename).Height), m_rcSaveImageTop[j]);
                                        }
                                    }
                                    else if (i >= 1 && i <= 4)// LeftRing, LeftMiddle, LeftIndex, LeftThumb
                                    {
                                        for (int j = i * 2 + 1; j <= i * 2 + 2; j++)
                                        {
                                            g.DrawImage(DesignConvert(sfilename, isRotated, printMaskOri[j], Image.FromFile(sfilename).Width, Image.FromFile(sfilename).Height), m_rcSaveImageTop[j]);
                                        }
                                    }

                                    else if (i >= 5 && i <= 8)    // RightThumb, RightIndex, RightMiddle, RightRing
                                    {
                                        for (int j = i * 2 + 1; j <= i * 2 + 2; j++)
                                        {
                                            g.DrawImage(DesignConvert(sfilename, isRotated, printMaskOri[21 - j], Image.FromFile(sfilename).Width, Image.FromFile(sfilename).Height), m_rcSaveImageBottom[21 - j]);
                                        }

                                    }
                                    else if (i == 9)//RightPinky
                                    {
                                        for (int j = i * 2 + 1; j <= i * 2 + 3; j++)
                                        {
                                            g.DrawImage(DesignConvert(sfilename, isRotated, printMaskOri[21 - j], Image.FromFile(sfilename).Width, Image.FromFile(sfilename).Height), m_rcSaveImageBottom[21 - j]);
                                        }

                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }

                img.Save(GlobalVariables.Instance.TempResultPath);
                logger.DebugFormat("디자인 {0} :: MyDesigns :: {1} saved.", CurrentViewModelName, GlobalVariables.Instance.TempResultPath);
                img.Dispose();

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("makeprintimage() 이미지 생성 에러 : " + ex.Message + ", ex.Data : " + ex.Data);
                logger.ErrorFormat("디자인 {0} :: makeprintimage() 이미지 생성 에러 : {1}, ex.Data : {2}", CurrentViewModelName, ex.Message, ex.Data);
                /*
                DialogMessage dlg = new DialogMessage("인쇄이미지 생성 중 에러가 발생했습니다", ex.Message.ToString(),  DialogType.Information);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Console.WriteLine("makeprintimage() 이미지 생성 에러 : " + ex.Message + ", ex.Data : " + ex.Data);
                }
                */
            }
            finally
            {
                for (int i = 0; i < 22; i++)
                {
                    printImg[i] = null;
                    printResult[i] = null;
                }
                img = null;
            }
            WaitingImageVisibility = false;
            PrevButtonVisible = true;
            HomeButtonVisible = true;
            ControlVisibility = true;

            // view 를위한 변수 추가
            TempResultPath = GlobalVariables.Instance.TempResultPath;
        }

        private RelayCommand<string> clickCommand;
        public RelayCommand<string> ClickCommand
        {
            get
            {
                return clickCommand ?? new RelayCommand<string>((selectedPaymentMethod) =>
                {
                    switch (selectedPaymentMethod)
                    {
                        case "Card":
                            logger.DebugFormat("{0} :: Card :: Go to Payment process :: PrintCount : {1} :: DesignPrice : {2}", CurrentViewModelName, PrintCount, DesignPrice);
                            if (GlobalVariables.Instance.IsPaymentOn == false)
                            {
                                GlobalVariables.Instance.MyProduct.isPaid = true;
                                CommandAction("M600_PrintNailStickerViewModel");
                            }
                            else
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
                //Console.WriteLine(string.Format("File No Exists :: {0}", path));
                logger.DebugFormat("{0} :: File No Exists :: {1}", CurrentViewModelName, path);
                return;
            }

            iniInkLimit.Load(path);
            il.CyanValue = iniInkLimit["Print11"]["Cyan Limit"].ToString();
            il.MagentaValue = iniInkLimit["Print11"]["Magenta Limit"].ToString();
            il.YellowValue = iniInkLimit["Print11"]["Yellow Limit"].ToString();
            il.BlackValue = iniInkLimit["Print11"]["Black Limit"].ToString();
            il.WhiteValue = iniInkLimit["Print11"]["White Ink Limit"].ToString();
            logger.DebugFormat("{0} :: Loaded Print Ink Limit :: Cyan {1}, Magenta {2}, Yellow {3}, Black {4}, White {5}",
                CurrentViewModelName, il.CyanValue, il.MagentaValue, il.YellowValue, il.BlackValue, il.WhiteValue);
        }

        private void SavePrintInkLimit(string path, InkLimitInfo il)
        {
            if (File.Exists(path) == false)
            {
                //Console.WriteLine(string.Format("File No Exists :: {0}", path));
                logger.DebugFormat("File No Exists :: {0}", path);
                return;
            }

            iniInkLimit["Print11"]["Cyan Limit"] = il.CyanValue;
            iniInkLimit["Print11"]["Magenta Limit"] = il.MagentaValue;
            iniInkLimit["Print11"]["Yellow Limit"] = il.YellowValue;
            iniInkLimit["Print11"]["Black Limit"] = il.BlackValue;
            iniInkLimit["Print11"]["White Ink Limit"] = il.WhiteValue;
            iniInkLimit.Save(path);
            logger.DebugFormat("{0} :: Saved Print Ink Limit :: Cyan {1}, Magenta {2}, Yellow {3}, Black {4}, White {5}",
                CurrentViewModelName, il.CyanValue, il.MagentaValue, il.YellowValue, il.BlackValue, il.WhiteValue);
        }

        private Image DesignConvert(string fileName, bool isRotated, Image maskImg, int width = 0, int height = 0)
        {
            if (!File.Exists(fileName)) return null;

            var resImg = Image.FromFile(fileName);
            if (isRotated) resImg.RotateFlip(RotateFlipType.Rotate180FlipNone);
            if (maskImg == null) return resImg;
            return ImageUtility.Instance.CropWithMask(resImg, width, height, maskImg);
        }
    }
}
