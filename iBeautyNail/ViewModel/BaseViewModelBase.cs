using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Windows.Threading;
using System.Globalization;
using System.Diagnostics;
using iBeautyNail.Language;
using iBeautyNail.Enums;
using iBeautyNail.Messages;
using iBeautyNail.Configuration;
using iBeautyNail.Datas;
using iBeautyNail.Messages.Exceptions.Enums;
using System.Threading;
using System.Speech.Synthesis;

namespace iBeautyNail.ViewModel
{
    public class BaseViewModelBase : ViewModelBase, IBaseViewModel
    {
        protected readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static bool CanNavigate { get; set; }

        #region Language

        public ObservableDictionary<string, LanguageChangedImpl> Lang
        {
            get { return App.LanguageMng.CurrentLanguage; }
        }

        public string Culture
        {
            get { return App.LanguageMng.CurrentCulture; }
        }

        #endregion //Language

        #region Command

        private RelayCommand<NAVIGATION_TYPE> navigateCommand;

        public RelayCommand<NAVIGATION_TYPE> NavigateCommand
        {
            get
            {
                return navigateCommand ?? new RelayCommand<NAVIGATION_TYPE>((navigationType) =>
                {
                    CommandAction(navigationType);
                }, CanCommandAction);
            }
        }

        private RelayCommand<string> directNavigateCommand;

        public RelayCommand<string> DirectNavigateCommand
        {
            get
            {
                return directNavigateCommand ?? new RelayCommand<string>((viewModelName) =>
                {
                    CommandAction(viewModelName);
                //}, CanCommandAction);
                });
            }
        }

        private RelayCommand<string> clickCommand;

        public RelayCommand<string> ClickCommand
        {
            get
            {
                return clickCommand ?? new RelayCommand<string>((command) =>
                {
                    ClickAction(command);
                });
            }
        }

        public bool CanCommandAction(NAVIGATION_TYPE navigationType)
        {
            return BaseViewModelBase.CanNavigate;
        }

        public bool CanCommandAction(string viewModel)
        {
            return BaseViewModelBase.CanNavigate;
        }

        #endregion //Command

        #region Header

        private string time;

        public string Time
        {
            get { return time; }
            set { Set(() => Time, ref time, value); }
        }

        private string date;

        public string Date
        {
            get { return date; }
            set { Set(() => Date, ref date, value); }
        }

        #endregion // Header

        #region Footer
        private bool homeButtonVisible = true;
        public bool HomeButtonVisible
        {
            get { return homeButtonVisible; }
            set { Set(() => HomeButtonVisible, ref homeButtonVisible, value); }
        }

        private bool prevButtonVisible = true;
        public bool PrevButtonVisible
        {
            get { return prevButtonVisible; }
            set { Set(() => PrevButtonVisible, ref prevButtonVisible, value); }
        }

        private bool nextButtonVisible = true;
        public bool NextButtonVisible
        {
            get { return nextButtonVisible; }
            set { Set(() => NextButtonVisible, ref nextButtonVisible, value); }
        }
        #endregion // Footer

        DispatcherTimer timer;

        protected DispatcherTimer timeoutTimer;

        public BaseViewModelBase()
        {
            SetTime();
        }

        private void SetTime()
        {
            EventHandler handler = (object sender, EventArgs e) =>
            {
                Time = DateTime.Now.ToString("HH:mm:ss", CultureInfo.CreateSpecificCulture(""));
                Date = DateTime.Now.ToString("yyyy-MM-dd, dddd", CultureInfo.CreateSpecificCulture(""));
            };
            handler(null, null);

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += handler;
            timer.Start();
        }

        private void StartElapsedTime()
        {
            GlobalVariables.Instance.ElapsedTime.StartElapsedTime(CurrentViewModelName);
        }

        private void StopElapsedTime()
        {
            GlobalVariables.Instance.ElapsedTime.StopElapsedTime(CurrentViewModelName);
        }

        protected void CommandAction(NAVIGATION_TYPE navigationType)
        {
            CommandAction(new NavigationMessage() { CurrentViewModel = this, NavigationType = navigationType });
        }

        protected void CommandAction(string viewModelName)
        {
            CommandAction(new NavigationMessage() { CurrentViewModel = this, NavigationType = NAVIGATION_TYPE.Direct, ViewModelName = viewModelName });
        }

        protected void CommandAction(NavigationMessage navigationMessage)
        {
            Messenger.Default.Send<NavigationMessage>(navigationMessage);
        }

        protected void CommonException()
        {
            CommandAction("E400_ErrorBaseViewModel");
        }

        public string CurrentViewModelName
        {
            get { return this.GetType().Name; }
        }

        /// <summary>
        /// BaseViewModelBase를 상속받은 ViewModel로 설정된 timeout 시간에 Timeout() 함수가 호출되며 
        /// 해당 ViewModel의 타임아웃 발생시 특정 로직의 실행이 필요한 경우 override하여 구현한다.
        /// </summary>
        protected virtual void Timeout()
        {
            CommandAction(PageNavigationConfigSection.Instance.Page[CurrentViewModelName].Timeout.Page.Equals("") ? PageNavigationConfigSection.Instance.DefaultTimeout.Page : PageNavigationConfigSection.Instance.Page[CurrentViewModelName].Timeout.Page);
        }

        public void Load()
        {
            PageLoad();

            StartElapsedTime();
        }

        public void Unload()
        {
            if (timeoutTimer != null) timeoutTimer.Stop();
            PageUnload();

            StopElapsedTime();
        }

        public void SetTimeout()
        {
            var configInfo = PageNavigationConfigSection.Instance.Page[CurrentViewModelName];

            if (configInfo.Timeout.Enable)
            {
                if (timeoutTimer == null)
                {
                    timeoutTimer = new DispatcherTimer();
                    timeoutTimer.Tick += (object sender, EventArgs e) =>
                    {
                        timeoutTimer.Stop();
                        Timeout();
                        logger.Debug("Timeout");
                    };
                }

                int seconds = configInfo.Timeout.Timeout > 0 ? configInfo.Timeout.Timeout : PageNavigationConfigSection.Instance.DefaultTimeout.Timeout;
                timeoutTimer.Interval = new TimeSpan(0, 0, 0, seconds);
                timeoutTimer.Start();
            }
        }

        public void ResetTimeout()
        {
            if (timeoutTimer != null)
            {
                var configInfo = PageNavigationConfigSection.Instance.Page[CurrentViewModelName];
                int seconds = configInfo.Timeout.Timeout > 0 ? configInfo.Timeout.Timeout : PageNavigationConfigSection.Instance.DefaultTimeout.Timeout;
                timeoutTimer.Interval = new TimeSpan(0, 0, 0, seconds);
            }
        }

        #region TTS
        protected ThreadStart threadDelegate;
        protected Thread commentThread;

        protected SpeechSynthesizer synthesizer;

        protected virtual void CommentWork() { }
        #endregion

        #region POPUP
        private PopupMessageOption PopupOption { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title">타이틀 첫번째줄</param>
        /// <param name="subTitle">타이틀 두번째줄</param>
        /// <param name="message">타이틀 세번째 줄</param>
        /// <param name="quantity">팝업의 버튼 숫자</param>
        public void PopupMessage(
            VALIDATION_TITLE_MESSAGE title,
            VALIDATION_MESSAGE subtitle,
            VALIDATION_MESSAGE message,
            POPUP_QUANTITY quantity = POPUP_QUANTITY.BUTTON_DEFAULT,
            PopupMessageOption popupoption = null)
        {
            Messenger.Default.Send<PopupMessage>(new PopupMessage()
            {
                CurrentViewModel = this,
                Visibility = System.Windows.Visibility.Visible,
                Title = title,
                SubTitle = subtitle,
                Message = message,
                Button0 = new RelayCommand(Button1Command),
                Button1 = new RelayCommand(Button2Command),
                Button2 = new RelayCommand(Button3Command),
                Quantity = quantity,
                option = popupoption
            });

            PopupOption = popupoption;
        }
        /// <summary>
        /// 팝업에서 확인 누를 때 처리할 명령
        /// </summary>
        public void Button1Command()
        {
            Messenger.Default.Send<PopupMessage>(new PopupMessage()
            {
                CurrentViewModel = this,
                Visibility = System.Windows.Visibility.Collapsed,
                Pushed = POPUP_BUTTON.BUTTON_0,
                option = PopupOption
            });
            PopupOption = null;
        }
        /// <summary>
        /// 팝업에서 확인 누를 때 처리할 명령
        /// </summary>
        public void Button2Command()
        {
            Messenger.Default.Send<PopupMessage>(new PopupMessage()
            {
                CurrentViewModel = this,
                Visibility = System.Windows.Visibility.Collapsed,
                Pushed = POPUP_BUTTON.BUTTON_1,
                option = PopupOption
            });
            PopupOption = null;
        }
        /// <summary>
        /// 팝업에서 확인 누를 때 처리할 명령
        /// </summary>
        public void Button3Command()
        {
            Messenger.Default.Send<PopupMessage>(new PopupMessage()
            {
                CurrentViewModel = this,
                Visibility = System.Windows.Visibility.Collapsed,
                Pushed = POPUP_BUTTON.BUTTON_2,
                option = PopupOption
            });
            PopupOption = null;
        }
        #endregion

        /// <summary>
        /// 페이지에서 화면 시작시 특정 동작이 필요한 경우 재정하여 사용
        /// </summary>
        protected virtual void PageLoad() { }

        /// <summary>
        /// 페이지에서 화면 종료시 특정 동작이 필요한 경우 재정하여 사용
        /// </summary>
        protected virtual void PageUnload() { }

        protected virtual void ClickAction(string action) { }
    }
}
