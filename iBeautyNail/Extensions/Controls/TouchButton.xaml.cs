using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using GalaSoft.MvvmLight.CommandWpf;

namespace iBeautyNail.Extensions.Controls //Image.Control.Button
{
    ///// <summary>
    ///// TouchButton.xaml에 대한 상호 작용 논리
    ///// </summary>
    public partial class TouchButton : UserControl
    {
        public bool btnFlag = false;

        public static DependencyProperty TextBlockStyleProperty =
            DependencyProperty.Register("TextBlockStyle",
                                        typeof(Style),
                                        typeof(TouchButton));

        [Description("TextBlockStyle"), Category("Common Properties")]
        public Style TextBlockStyle
        {
            get { return (Style)GetValue(TextBlockStyleProperty); }
            set { SetValue(TextBlockStyleProperty, value); }
        }

        public static DependencyProperty TextProperty
             = DependencyProperty.Register("Text", typeof(string), typeof(TouchButton), new FrameworkPropertyMetadata(OnTextPropertyChanged));

        [Description("Text"), Category("Common Properties")]
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public static DependencyProperty SourceProperty
                = DependencyProperty.Register("Source", typeof(ImageSource), typeof(TouchButton), new FrameworkPropertyMetadata(OnSourcePropertyChanged));

        [Description("Source"), Category("Common Properties")]
        public ImageSource Source
        {
            get { return (ImageSource)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }

        public static DependencyProperty SourceOverProperty
                = DependencyProperty.Register("SourceOver", typeof(ImageSource), typeof(TouchButton), new FrameworkPropertyMetadata(OnSourceOverPropertyChanged));

        [Description("SourceOver"), Category("Common Properties")]
        public ImageSource SourceOver
        {
            get { return (ImageSource)this.GetValue(SourceOverProperty); }
            set { this.SetValue(SourceOverProperty, value); }
        }

        public static DependencyProperty LineHeightProperty
        = DependencyProperty.Register("LineHeight", typeof(double), typeof(TouchButton));

        [Description("Comment"), Category("Common Properties")]
        public double LineHeight
        {
            get { return textContainer.LineHeight; }
            set { textContainer.LineHeight = value; }
        }

        public static readonly DependencyProperty CommandProperty 
            = DependencyProperty.Register("Command", typeof(ICommand), typeof(TouchButton), new UIPropertyMetadata(null));

        [Description("Command"), Category("Common Properties")]
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty
            = DependencyProperty.Register("CommandParameter", typeof(object), typeof(TouchButton), new UIPropertyMetadata(null));

        [Description("CommandParameter"), Category("Common Properties")]
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        private static void OnTextPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            TouchButton touchButton = obj as TouchButton;
            touchButton.textContainer.Text = args.NewValue?.ToString();
        }

        private static void OnSourcePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            TouchButton touchButton = obj as TouchButton;
            touchButton.imgSource.Source = args.NewValue as ImageSource;
        }

        private static void OnSourceOverPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            TouchButton touchButton = obj as TouchButton;
            touchButton.imgOverSource.Source = args.NewValue as ImageSource;
        }

        public TouchButton()
        {
            InitializeComponent();

            //MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(TouchButton_MouseLeftButtonDown);
            MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(TouchButton_MouseLeftButtonUp);
            //MouseLeave += new System.Windows.Input.MouseEventHandler(TouchButton_MouseLeave);
            // 20180702 dyddyd Admin 탭 전환시 버튼이 비활성화 되어 주석처리함
            //this.Unloaded += TouchButton_Unloaded;
        }

        void TouchButton_Unloaded(object sender, RoutedEventArgs e)
        {
            //MouseLeftButtonDown -= TouchButton_MouseLeftButtonDown;
            MouseLeftButtonUp -= TouchButton_MouseLeftButtonUp;
            //MouseLeave -= TouchButton_MouseLeave;
        }

        #region TouchButton Event
        private void TouchButton_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (textContainer.IsEnabled == true)
            {
                VisualStateManager.GoToState(this, "Pressed", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "NormalPressed", true);
            }
            btnFlag = true;
        }

        private void TouchButton_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //if (btnFlag == true)
            //{
                Command?.Execute(CommandParameter);

                RaiseClickEvent();
                //System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
                //timer.Interval = TimeSpan.FromMilliseconds(50);

                //timer.Tick += delegate
                //{
                //    timer.Stop();
                //    VisualStateManager.GoToState(this, "Normal", true);
                //};
                //timer.Start();

            //}
        }

        private void TouchButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
            btnFlag = false;
        }

        #endregion

        #region ClickEvent Routed Event
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
          "Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TouchButton));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        void RaiseClickEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(TouchButton.ClickEvent);
            RaiseEvent(newEventArgs);
        }
        #endregion
    }
}
