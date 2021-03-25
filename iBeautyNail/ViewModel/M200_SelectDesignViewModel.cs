using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using iBeautyNail.Datas;
using iBeautyNail.Enums;
using iBeautyNail.Pages;
using iBeautyNail.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows;
using System.Collections.ObjectModel;
using iBeautyNail.Messages.Exceptions.Enums;
using iBeautyNail.Extensions;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Runtime.InteropServices;
using System.Threading;
using System.Speech.Synthesis;
using System.Globalization;

namespace iBeautyNail.ViewModel
{
    public class M200_SelectDesignViewModel : BaseViewModelBase, INotifyPropertyChanged
    {
        private HwndSource source;
        private HwndSourceHook hook;
        MediaPlayer player;

        #region ProcessView
        private FrameworkElement _contentControlView;
        public FrameworkElement ContentControlView
        {
            get { return _contentControlView; }
            set
            {
                _contentControlView = value;
                RaisePropertyChanged("ContentControlView");
            }
        }
        #endregion

        #region Variable
        public List<string> CategoryList
        {
            get
            {
                List<string> categories = new List<string>();

                categories = App.DesignMng.CategorySet.Keys.ToList();
                // 20210111 kindsr 이사님 요청에 의해 USB막음
                //categories.Add("USB");

                //return App.DesignMng.CategorySet.Keys.ToList();
                return categories;
            }
        }

        public List<List<DesignInfo>> DesignList
        {
            get
            {
                return App.DesignMng.DesignSet.Values.ToList();
            }
        }

        public List<DesignInfo> SelectedCategoryDesignList
        {
            get
            {
                List<DesignInfo> selectedCategoryDesignList = new List<DesignInfo>();

                App.DesignMng.DesignSet.TryGetValue(CurrentCategory, out selectedCategoryDesignList);

                return selectedCategoryDesignList;
            }
        }

        private string _currentCategory;
        public string CurrentCategory
        {
            get { return _currentCategory; }
            set
            {
                Set(() => CurrentCategory, ref _currentCategory, value);
            }
        }

        private int _currentNailPosition;
        public int CurrentNailPosition
        {
            get
            {
                _currentNailPosition = MyDesigns.Count;

                for (int i = 0; i < MyDesigns.Count; i++)
                {
                    if (MyDesigns[i].Selected == false && MyDesigns[i].Vacant == true)
                    {
                        _currentNailPosition = i;
                        break;
                    }
                }

                return _currentNailPosition;
            }
            set
            {
                Set(() => CurrentNailPosition, ref _currentNailPosition, value);
            }
        }

        ObservableCollection<Nail> _myDesigns = new ObservableCollection<Nail>();
        public ObservableCollection<Nail> MyDesigns
        {
            get { return _myDesigns; }
            set
            {
                if (_myDesigns != value)
                    _myDesigns = value;

                RaisePropertyChanged("MyDesigns");
            }
        }

        private string designPrice;
        public string DesignPrice
        {
            get
            {
                //return designPrice;
                //return "5000";
                return GlobalVariables.Instance.DesignPrice.ToString();
            }
            set { Set(() => DesignPrice, ref designPrice, value); }
        }

        private int designCount;
        public int DesignCount
        {
            get
            {
                designCount = MyDesigns.Count(item => item.Selected);
                curDesignCount = designCount;
                return designCount;
            }
            set //{ Set(() => DesignCount, ref designCount, value); }
            {
                if (Equals(value, designCount)) return;
                designCount = value;
                RaisePropertyChanged("DesignCount");
            }
        }

        public static int curDesignCount;

        public static int nexNailPosition;

        public void forNextPosition()
        {
            M290_USBLayoutViewModel.refNailPostion = CurrentNailPosition;
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
        #endregion

        public M200_SelectDesignViewModel()
        {
            this.source = HwndSource.FromHwnd(App.Window.HWND);
            this.hook = new HwndSourceHook(WndProc);

            // 선택한 카테고리의 Layout 으로 View 이동
            //Messenger.Default.Register<NavigationMessage>(this, (navigationMessage) =>
            //{
            //    SwitchView(navigationMessage.ViewModelName);
            //});

            // 선택한 nail의 이미지경로 받는 곳
            Messenger.Default.Register<DesignInfo>(this, OnReceiveMessageAction);

            // 전체삭제 메세지 받는곳
            Messenger.Default.Register<int>(this, OnReceiveClearAllAction);

            // 타임아웃 리셋 
            Messenger.Default.Register<string>(this, OnReceiveResetTimeout);

            PrevButtonVisible = false;
        }

        protected override void PageLoad()
        {
            AddHook();

            // 선택 디자인이 없을때 CanNavigate = false;
            if (DesignCount <= 0) CanNavigate = false;

            if (GlobalVariables.Instance.IsTTSOn)
            {
                System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Tick += (object sender, EventArgs e) =>
                {
                    timer.Stop();
                    Task.Run(() =>
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            //threadDelegate = new ThreadStart(CommentWork);
                            //commentThread = new Thread(threadDelegate);
                            //commentThread.Start();

                            Uri uri = new Uri(@"pack://siteoforigin:,,,/Resources/Data/TTS/001.wav");
                            player = new MediaPlayer();
                            player.Open(uri);
                            player.Play();
                        }));
                    });
                };
                timer.Start();
            }

            // Language 팝업은 Load안함
            if (GlobalVariables.Instance.LanguagePopup)
            {
                GlobalVariables.Instance.LanguagePopup = false;
                return;
            }

            MyDesigns = GlobalVariables.Instance.MyDesigns.ToObservableCollection<Nail>();

            CurrentCategory = CategoryList.First();
            SwitchView(CurrentCategory);
        }

        protected override void PageUnload()
        {
            if (GlobalVariables.Instance.IsTTSOn)
            {
                //synthesizer.SpeakAsyncCancelAll();
                ////synthesizer.Dispose();
                //commentThread.Abort();
                player.Stop();
            }

            RemoveHook();

            GlobalVariables.Instance.MyDesigns = MyDesigns.ToList<Nail>();
            MyDesigns.Clear();
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
                builder.AppendText(App.LanguageMng.LanguageSet[Culture]["M200_Comment1"].Sentence);
                builder.AppendText("     ");
                builder.AppendText(App.LanguageMng.LanguageSet[Culture]["M200_Comment2"].Sentence);
                builder.AppendText("     ");
                //builder.AppendText(App.LanguageMng.LanguageSet[App.LanguageMng.CurrentCulture]["M200_Comment3"].Sentence);
                //builder.AppendText("     ");
                builder.AppendText(App.LanguageMng.LanguageSet[Culture]["M200_Comment4"].Sentence);
                builder.EndVoice();
                synthesizer.SpeakAsync(builder);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("{0} :: CommentWork Exception :: {1}", CurrentViewModelName, ex.ToString());
            }
        }

        private void SwitchView(string viewName)
        {
            switch (viewName)
            {
                case "FULL":
                    ContentControlView = new M211_FullLayout();
                    ContentControlView.DataContext = new M211_FullLayoutViewModel(SelectedCategoryDesignList);
                    break;
                case "GRADATION":
                case "CUTE":
                case "GLITTER":
                case "FRENCH":
                case "PATTERN":
                case "ART":
                case "STRIPE":
                case "WATER MARBLE":
                case "HALLOWEEN":
                case "CHRISTMAS":
                    ContentControlView = new M230_WeeklyPickLayout();
                    ContentControlView.DataContext = new DesignBaseViewModel(SelectedCategoryDesignList);
                    break;
                case "ARTIST PICK":
                    ContentControlView = new M240_ArtistPickLayout();
                    ContentControlView.DataContext = new M240_ArtistPickLayoutViewModel(SelectedCategoryDesignList);
                    break;
                case "USB":
                    ContentControlView = new M290_USBLayout();
                    ContentControlView.DataContext = new M290_USBLayoutViewModel();
                    break;
                default:
                    ContentControlView = new M240_ArtistPickLayout();
                    ContentControlView.DataContext = new M240_ArtistPickLayoutViewModel(SelectedCategoryDesignList);
                    break;
            }
        }

        private void OnReceiveMessageAction(DesignInfo obj)
        {
            // 나의 선택에 뿌려줄 디자인 정보를 View에 바인딩
            //SelectedDesignList.Add(obj);
            int curNP = CurrentNailPosition;
            if (curNP >= 10)
            {
                // 디자인 선택 완료 팝업
                ShowMessageLayer();
                return;
            }
            //MyDesigns.Insert(curNP, new Nail() { DesignInfo = obj, Selected = true, Vacant = false, Position = (FINGER_POSITION)curNP, MaskInfo = FingerMask((FINGER_POSITION)curNP), RotAngle = 0 });
            if (curNP != MyDesigns.Count) MyDesigns.RemoveAt(curNP);
            MyDesigns.Insert(curNP, new Nail()
            {
                UsbImageYN = obj.Category == "USB" ? true : false,
                DesignInfo = obj,
                Selected = true,
                Vacant = false,
                Position = (FINGER_POSITION)curNP,
                IsRotated = false,
                RotAngle = 0,
                MaskInfo = FingerMask((FINGER_POSITION)curNP)
            });
            DesignCount++;
            CanNavigate = true;
            logger.DebugFormat("Current Finger number : {0} :: DesignPath : {1}", (FINGER_POSITION)curNP,  obj.DesignPath);

            forNextPosition();
        }

        private void OnReceiveClearAllAction(int num)
        {
            MyDesigns.Clear();
            DesignCount = 0;
            CanNavigate = false;
            forNextPosition();
        }

        private void OnReceiveResetTimeout(string action)
        {
            if (action.Equals("ResetTimeout"))
                ResetTimeout();
        }

        // Dynamic TouchButton Event
        private RelayCommand<string> changeCategoryCommand;
        public RelayCommand<string> ChangeCategoryCommand
        {
            get
            {
                return changeCategoryCommand ?? new RelayCommand<string>((selectedCategory) =>
                {
                    //if (selectedCategory == CurrentCategory)
                    //    return;
                    //Console.Write("category=>{0}\n", selectedCategory);
                    logger.DebugFormat("{0} :: Selected view : {1}\n", CurrentViewModelName, selectedCategory);
                    CurrentCategory = selectedCategory;

                    SwitchView(CurrentCategory);
                });
            }
        }

        private RelayCommand<string> selectCategoryDownCommand;
        public RelayCommand<string> SelectCategoryDownCommand
        {
            get
            {
                return selectCategoryDownCommand ?? new RelayCommand<string>((selectedCategory) =>
                {
                    //Console.WriteLine("MouseDown {0},{1}", PanelX, PanelY);
                    mousePoint.X = PanelX;
                    mousePoint.Y = PanelY;
                });
            }
        }

        private RelayCommand<string> selectCategoryUpCommand;
        public RelayCommand<string> SelectCategoryUpCommand
        {
            get
            {
                return selectCategoryUpCommand ?? new RelayCommand<string>((selectedCategory) =>
                {
                    ResetTimeout();
                    //Console.WriteLine("MouseUp {0},{1}", PanelX, PanelY);

                    if (PanelX == mousePoint.X && PanelY == mousePoint.Y)
                    {
                        //Console.Write("category=>{0}\n", selectedCategory);
                        logger.DebugFormat("{0} :: Selected view : {1}\n", CurrentViewModelName, selectedCategory);
                        CurrentCategory = selectedCategory;

                        SwitchView(CurrentCategory);
                    }
                });
            }
        }

        private RelayCommand<int> myDesignsClickCommand;
        public RelayCommand<int> MyDesignsClickCommand
        {
            get
            {
                return myDesignsClickCommand ?? new RelayCommand<int>((selectedNailPosition) =>
                {
                    ResetTimeout();

                    MyDesigns.RemoveAt(selectedNailPosition);
                    MyDesigns.Insert(selectedNailPosition, new Nail() { DesignInfo = null, Selected = false, Vacant = true, Position = (FINGER_POSITION)selectedNailPosition, RotAngle = 0 });
                    DesignCount--;
                    
                    if (DesignCount <= 0) CanNavigate = false;

                    forNextPosition();

                    if (CurrentCategory == "USB")
                    {
                        //forNextPosition();
                        ContentControlView = new M290_USBLayout();
                        ContentControlView.DataContext = new M290_USBLayoutViewModel();
                    }
                });
            }
        }

        private RelayCommand<int> rotationClickCommand;
        public RelayCommand<int> RotationClickCommand
        {
            get
            {
                return rotationClickCommand ?? new RelayCommand<int>((selectedNailPosition) =>
                {
                    ResetTimeout();

                    MyDesigns[selectedNailPosition].IsRotated = !MyDesigns[selectedNailPosition].IsRotated;
                    if (MyDesigns[selectedNailPosition].IsRotated)
                    {
                        MyDesigns[selectedNailPosition].RotAngle = 180;
                    }
                    else
                    {
                        MyDesigns[selectedNailPosition].RotAngle = 0;
                    }
                    Console.Write("rotate nail =>{0} :: {1}\n", selectedNailPosition, MyDesigns[selectedNailPosition].RotAngle);
                    //RaisePropertyChanged("MyDesigns");
                });
            }
        }

        private RelayCommand myDesignsAllRemoveCommand;
        public RelayCommand MyDesignsAllRemoveCommand
        {
            get
            {
                return myDesignsAllRemoveCommand ?? new RelayCommand(() =>
                {
                    ResetTimeout();

                    MyDesigns.Clear();
                    DesignCount = 0;
                    CanNavigate = false;
                    forNextPosition();

                    if (CurrentCategory == "USB")
                    {
                        //forNextPosition();
                        ContentControlView = new M290_USBLayout();
                        ContentControlView.DataContext = new M290_USBLayoutViewModel();
                    }
                });
            }
        }

        private string fingerMaskPath;
        public string FingerMaskPath
        {
            get { return fingerMaskPath; }
            set
            {
                fingerMaskPath = value;
                RaisePropertyChanged("FingerMaskPath");
            }
        }

        public string FingerMask(FINGER_POSITION pos)
        {
            var path = string.Empty;
            switch (pos)
            {
                case FINGER_POSITION.LEFT_PINKY: //왼 새끼
                    path = "pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew5.png";
                    break;
                case FINGER_POSITION.LEFT_RING: //왼 약지
                    path = "pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew4.png";
                    break;
                case FINGER_POSITION.LEFT_MIDDLE: //왼 중지
                    path = "pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew3.png";
                    break;
                case FINGER_POSITION.LEFT_INDEX: //왼 검지
                    path = "pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew2.png";
                    break;
                case FINGER_POSITION.LEFT_THUMB: //왼 엄지
                    path = "pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew1.png";
                    break;
                case FINGER_POSITION.RIGHT_THUMB: //오 엄지
                    path = "pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew1.png";
                    break;
                case FINGER_POSITION.RIGHT_INDEX: //오 검지
                    path = "pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew2.png";
                    break;
                case FINGER_POSITION.RIGHT_MIDDLE: //오 중지
                    path = "pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew3.png";
                    break;
                case FINGER_POSITION.RIGHT_RING: //오 약지
                    path = "pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew4.png";
                    break;
                case FINGER_POSITION.RIGHT_PINKY: //오 새끼
                    path = "pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew5.png";
                    break;
                default:
                    path = "pack://siteoforigin:,,,/Resources/Images/maskmodifythumbnew5.png";
                    break;
            }

            return path;
        }

        private void ShowMessageLayer()
        {
            PopupMessage(VALIDATION_TITLE_MESSAGE.VALIDATION_TITLE_INFO, VALIDATION_MESSAGE.CONFIRM, VALIDATION_MESSAGE.VALIDATION_FULL_DESIGN);
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
            UInt32 uiDevCHANGE = 0x0219;
            UInt32 uiDevVOLUME = 0x02;
            UInt32 uiDevARRIVAL = 0x8000;
            UInt32 uiDevREMOVE = 0x8004;

            if ((windowMessage == uiDevCHANGE) && (wParam.ToInt32() == uiDevARRIVAL)) //디바이스 연결
            {
                int devType = Marshal.ReadInt32(lParam, 4);
                if (devType == uiDevVOLUME)
                {
                    try
                    {
                        Console.WriteLine("디바이스 연결");
                        if (CurrentCategory == "USB")
                        {
                            forNextPosition();
                            ContentControlView = new M290_USBLayout();
                            ContentControlView.DataContext = new M290_USBLayoutViewModel();
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            if ((windowMessage == uiDevCHANGE) && (wParam.ToInt32() == uiDevREMOVE)) //디바이스 연결 해제
            {
                int devType = Marshal.ReadInt32(lParam, 4);
                if (devType == uiDevVOLUME)
                {
                    try
                    {
                        if (CurrentCategory == "USB")
                        {
                            ContentControlView = new M290_USBLayout();
                            ContentControlView.DataContext = new M290_USBLayoutViewModel();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return IntPtr.Zero;
        }
    }
}
