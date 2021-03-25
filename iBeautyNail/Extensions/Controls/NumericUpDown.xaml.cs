using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iBeautyNail.Extensions.Controls
{
    /// <summary>
    /// NumericUpDown.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the NumericUpDownControl.
        /// </summary>
        public NumericUpDown()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Identifies the Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value", typeof(decimal), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(MinValue, new PropertyChangedCallback(OnValueChanged),
                                                new CoerceValueCallback(CoerceValue)));

        /// <summary>
        /// Gets or sets the value assigned to the control.
        /// </summary>
        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static DependencyProperty BoxBrushProperty
                = DependencyProperty.Register("BoxBrush", typeof(Brush), typeof(NumericUpDown), new FrameworkPropertyMetadata(OnBoxBrushPropertyChanged));

        [Description("BoxBrush"), Category("Common Properties")]
        public Brush BoxBrush
        {
            get { return (Brush)this.GetValue(BoxBrushProperty); }
            set { this.SetValue(BoxBrushProperty, value); }
        }

        public static DependencyProperty ButtonBackgroundProperty
                = DependencyProperty.Register("ButtonBackground", typeof(Brush), typeof(NumericUpDown), new FrameworkPropertyMetadata(OnButtonBackgroundPropertyChanged));

        [Description("ButtonBackground"), Category("Common Properties")]
        public Brush ButtonBackground
        {
            get { return (Brush)this.GetValue(ButtonBackgroundProperty); }
            set { this.SetValue(ButtonBackgroundProperty, value); }
        }

        public static DependencyProperty ButtonForegroundProperty
        = DependencyProperty.Register("ButtonForeground", typeof(Brush), typeof(NumericUpDown), new FrameworkPropertyMetadata(OnButtonForegroundPropertyChanged));

        [Description("ButtonForeground"), Category("Common Properties")]
        public Brush ButtonForeground
        {
            get { return (Brush)this.GetValue(ButtonForegroundProperty); }
            set { this.SetValue(ButtonForegroundProperty, value); }
        }


        public static DependencyProperty BoxTextColorProperty
                = DependencyProperty.Register("BoxTextColor", typeof(Brush), typeof(NumericUpDown), new FrameworkPropertyMetadata(OnBoxTextColorPropertyChanged));

        [Description("BoxTextColor"), Category("Common Properties")]
        public Brush BoxTextColor
        {
            get { return (Brush)this.GetValue(BoxTextColorProperty); }
            set { this.SetValue(BoxTextColorProperty, value); }
        }

        public static DependencyProperty BoxTextSizeProperty
        = DependencyProperty.Register("BoxTextSize", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(OnBoxTextSizePropertyChanged));

        [Description("BoxTextSize"), Category("Common Properties")]
        public int BoxTextSize
        {
            get { return (int)this.GetValue(BoxTextSizeProperty); }
            set { this.SetValue(BoxTextSizeProperty, value); }
        }

        private static object CoerceValue(DependencyObject element, object value)
        {
            decimal newValue = (decimal)value;
            NumericUpDown control = (NumericUpDown)element;

            newValue = Math.Max(MinValue, Math.Min(MaxValue, newValue));

            return newValue;
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown control = (NumericUpDown)obj;

            RoutedPropertyChangedEventArgs<decimal> e = new RoutedPropertyChangedEventArgs<decimal>(
                (decimal)args.OldValue, (decimal)args.NewValue, ValueChangedEvent);
            control.OnValueChanged(e);
        }

        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<decimal>), typeof(NumericUpDown));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the ValueChanged event.</param>
        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<decimal> args)
        {
            RaiseEvent(args);
        }

        private static void OnBoxBrushPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown iconButton = obj as NumericUpDown;
            iconButton.BoxBrush = args.NewValue as Brush;
        }

        private static void OnButtonBackgroundPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown iconButton = obj as NumericUpDown;
            iconButton.ButtonBackground = args.NewValue as Brush;
        }

        private static void OnButtonForegroundPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown iconButton = obj as NumericUpDown;
            iconButton.ButtonForeground = args.NewValue as Brush;
        }

        private static void OnBoxTextColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown iconButton = obj as NumericUpDown;
            iconButton.BoxTextColor = args.NewValue as Brush;
        }

        private static void OnBoxTextSizePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown iconButton = obj as NumericUpDown;
            iconButton.BoxTextSize = (int)args.NewValue;
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            Value++;
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            Value--;
        }

        private const decimal MinValue = 0, MaxValue = 3;

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            Value++;
        }

        private void downButton_Click(object sender, RoutedEventArgs e)
        {
            Value--;
        }
    }
}
