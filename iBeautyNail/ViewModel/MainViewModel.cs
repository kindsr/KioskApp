using GalaSoft.MvvmLight.Messaging;
using iBeautyNail.Configuration;
using iBeautyNail.Enums;
using iBeautyNail.Messages;
using iBeautyNail.Pages;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace iBeautyNail.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : BaseViewModelBase
    {
        //HwndSource source;
        //HwndSourceHook hook;

        private string oldViewModel;
        private BaseViewModelBase currentViewModel;

        public BaseViewModelBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { Set(() => CurrentViewModel, ref currentViewModel, value); }
        }

        private Visibility popupVisibility = Visibility.Collapsed;

        public Visibility PopupVisibility
        {
            get { return popupVisibility; }
            set { Set(() => PopupVisibility, ref popupVisibility, value); }
        }

        private Grid layerArea;

        public Grid LayerArea
        {
            get { return layerArea; }
            set { Set(() => LayerArea, ref layerArea, value); }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            PopupVisibility = Visibility.Collapsed;

            Messenger.Default.Register<PopupMessage>(this, message =>
            {
                if (message.Visibility == Visibility.Visible)
                {
                    LayerArea.Children.Clear();
                    M060_MessagePopup popup = new M060_MessagePopup(message);
                    popup.DataContext = message;
                    popup.ctTbTitle.Text = App.LanguageMng.CurrentLanguage[message.Title.ToString()].Sentence;
                    popup.ctTbSubTitle.Text = App.LanguageMng.CurrentLanguage[message.SubTitle.ToString()].Sentence;
                    popup.ctTbMessage.Text = App.LanguageMng.CurrentLanguage[message.Message.ToString()].Sentence;

                    if (message.option != null)
                    {
                        popup.ctBtButton0.Text = App.LanguageMng.CurrentLanguage[message.option.Button0Text].Sentence;
                        popup.ctBtButton1.Text = App.LanguageMng.CurrentLanguage[message.option.Button1Text].Sentence;
                        popup.ctBtButton2.Text = App.LanguageMng.CurrentLanguage[message.option.Button2Text].Sentence;
                    }
                    else
                    {
                        PopupMessageOption option = new PopupMessageOption();
                        popup.ctBtButton0.Text = App.LanguageMng.CurrentLanguage[option.Button0Text].Sentence;
                        popup.ctBtButton1.Text = App.LanguageMng.CurrentLanguage[option.Button1Text].Sentence;
                        popup.ctBtButton2.Text = App.LanguageMng.CurrentLanguage[option.Button2Text].Sentence;
                    }

                    LayerArea.Children.Add(popup);
                }
                else
                {
                    var configInfo = PageNavigationConfigSection.Instance.Page[message.CurrentViewModel.GetType().Name];
                    if (message.Pushed == POPUP_BUTTON.BUTTON_0)
                    {
                        if (message.option == null || message.option.Button0Page == null)
                        {
                            CommandAction(configInfo.Popup.Button0);
                        }
                        else
                        {
                            CommandAction(message.option.Button0Page);
                        }
                    }
                    else if (message.Pushed == POPUP_BUTTON.BUTTON_1)
                    {
                        if (message.option == null || message.option.Button1Page == null)
                        {
                            CommandAction(configInfo.Popup.Button1);
                        }
                        else
                        {
                            CommandAction(message.option.Button1Page);
                        }
                    }
                    else if (message.Pushed == POPUP_BUTTON.BUTTON_2)
                    {
                        if (message.option == null || message.option.Button2Page == null)
                        {
                            CommandAction(configInfo.Popup.Button2);
                        }
                        else
                        {
                            CommandAction(message.option.Button2Page);
                        }
                    }
                }
                PopupVisibility = message.Visibility;
            });

            Messenger.Default.Register<NavigationMessage>(this, message =>
            {
                Navigate(message);
            });

            //AddHook();

            Navigate(new NavigationMessage() { CurrentViewModel = this, NavigationType = NAVIGATION_TYPE.Next });
        }

        public MainViewModel(Grid layerArea)
            : this()
        {
            LayerArea = layerArea;
        }

        //private void AddHook()
        //{
        //    this.source = HwndSource.FromHwnd(App.Window.HWND);
        //    this.hook = new HwndSourceHook(WndProc);

        //    source.AddHook(hook);
        //}

        /// <summary>
        /// 메시지 처리
        /// </summary>
        //private IntPtr WndProc(IntPtr hwnd, int windowMessage, IntPtr wParam, IntPtr lParam, ref bool handled)
        //{
        //    switch (windowMessage)
        //    {
        //        case WindowMessage.PCU_MSG_STATE:
        //            if (wParam == (IntPtr)WindowMessage.PCU_MSG_STATE_EMPTY_PAPER)
        //            {
        //                logger.ErrorFormat("{0} :: WndProc :: {1} :: {2}", CurrentViewModel, "Empty Paper", wParam);
        //                Task.Run(() => CreateErrorInfo("6004", string.Format("{0} :: WndProc :: Receipt Print error! :: {1}", CurrentViewModelName, (int)wParam)));
        //                ShowMessageLayerError();
        //            }
        //            break;
        //    }
        //    return IntPtr.Zero;
        //}

        private void Navigate(NavigationMessage message)
        {
            bool canMove = true;

            //string currnetViewModel = App.Window.contentControl.Content.ToString(); // Selfpass.ViewModel.{페이지명}
            //string currentViewName = currnetViewModel.Substring(currnetViewModel.LastIndexOf('.') +1); // 페이지명
            string currentViewModelName = message.CurrentViewModel.GetType().Name;
            var configInfo = PageNavigationConfigSection.Instance.Page[currentViewModelName];

            string navigationViewModel = null;
            string methodName = null;

            //if (!currentViewModelName.Equals("MainViewModel") && CheckDevices() != SDKManagerStatusCode.Success)
            //{
            //    //Navigate(new NavigationMessage() { CurrentViewModel = CurrentViewModel, NavigationType = NAVIGATION_TYPE.Direct, ViewModelName = "E400_ErrorBaseViewModel" });
            //    message.NavigationType = NAVIGATION_TYPE.Direct;
            //    message.ViewModelName = "E400_ErrorBaseViewModel";
            //}

            switch (message.NavigationType)
            {
                case NAVIGATION_TYPE.Next:
                    navigationViewModel = configInfo.Next;
                    methodName = configInfo.NextMethod;
                    break;

                case NAVIGATION_TYPE.Previous:
                    navigationViewModel = configInfo.Previous;
                    methodName = configInfo.PreviousMethod;
                    break;

                case NAVIGATION_TYPE.Direct:
                    navigationViewModel = message.ViewModelName;
                    //methodName = configInfo.CustomMethod;
                    break;

                case NAVIGATION_TYPE.Back:
                    navigationViewModel = oldViewModel;
                    //methodName = configInfo.CustomMethod;
                    break;
            }

            if (!string.IsNullOrEmpty(navigationViewModel))
            {
                if (configInfo != null && !string.IsNullOrEmpty(configInfo.Class) && !string.IsNullOrEmpty(methodName))
                {
                    Type t = Type.GetType(configInfo.Class);
                    object classInstance = Activator.CreateInstance(t, null);
                    canMove = (bool)GetMethod(t, methodName).Invoke(classInstance, null);
                }

                if (canMove)
                {
                    logger.Debug($"Move Page: {currentViewModelName} -> {navigationViewModel}");

                    oldViewModel = currentViewModelName;
                    if (CurrentViewModel != null) CurrentViewModel.Unload();

                    CurrentViewModel = App.Container.Resolve<IBaseViewModel>(navigationViewModel) as BaseViewModelBase;
                    CurrentViewModel.Load();
                    CurrentViewModel.SetTimeout();
                }
            }
        }

        private MethodInfo GetMethod(Type t, string name)
        {
            return t.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        //private int CheckDevices()
        //{
        //    return SDKManagerStatusCode.Success;
        //}

        //private void DeviceStep(string step)
        //{

        //}
    }
}