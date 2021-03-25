using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace iBeautyNail.Pages
{
    /// <summary>
    /// M290_USBLayout.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M290_USBLayout : UserControl, INotifyPropertyChanged
    {
        BitmapImage myBitmapImage = new BitmapImage();

        double rotate1;

        public event PropertyChangedEventHandler PropertyChanged;

        public M290_USBLayout()
        {
            InitializeComponent();
            this.DataContext = this;
            imageTransform = new MatrixTransform();
        }

        public static Image MyImage2;
        public static Grid PbUserImage2;

        private Image myImage1;
        public Image MyImage1
        {
            get
            {
                myImage1 = (Image)this.FindName("myImage");
                MyImage2 = myImage1;
                return myImage1;
            }
        }

        private Grid pbUserImage1;
        public Grid PbUserImage1
        {
            get
            {
                pbUserImage1 = (Grid)this.FindName("pbUserImage");
                PbUserImage2 = pbUserImage1;
                return pbUserImage1;
            }
        }

        private MatrixTransform imageTransform;
        public MatrixTransform ImageTransform
        {
            get { return imageTransform; }
            set
            {
                if (value != imageTransform)
                {
                    imageTransform = value;
                    RaisePropertyChanged("ImageTransform");
                }
            }
        }

        Vector trans;
        //double rotate;
        Vector scale;
        System.Windows.Point center;

        private void Image_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            ManipulationDelta md = e.DeltaManipulation;
            trans = md.Translation;
            //rotate = md.Rotation;
            scale = md.Scale;

            Console.WriteLine(string.Format("Manipulation => Translation : {0}, {1}", trans.X, trans.Y));
            //Console.WriteLine(string.Format("Manipulation => Rotate : {0}", rotate));
            Console.WriteLine(string.Format("Manipulation => Scale : {0}, {1}", scale.X, scale.Y));

            Matrix m = imageTransform.Matrix;

            // Find center of element and then transform to get current location of center
            FrameworkElement fe = e.Source as FrameworkElement;
            center = new System.Windows.Point(fe.ActualWidth / 2, fe.ActualHeight / 2);
            center = m.Transform(center);

            // Update matrix to reflect translation/rotation
            m.Translate(trans.X, trans.Y);
            //m.RotateAt(rotate, center.X, center.Y);
            m.ScaleAt(scale.X, scale.Y, center.X, center.Y);

            imageTransform.Matrix = m;
            RaisePropertyChanged("ImageTransform");

            e.Handled = true;
        }

        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void Image_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            Console.WriteLine(string.Format("Translation : {0}, {1}", trans.X, trans.Y));
            //Console.WriteLine(string.Format("Rotate : {0}", rotate));
            Console.WriteLine(string.Format("Scale : {0}, {1}", scale.X, scale.Y));
        }

        //임시 시작
        private void BtnLeftRotate_Click(object sender, RoutedEventArgs e)
        {
            //myImage.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            //rotate1--;
            //myImage.RenderTransform = new RotateTransform(rotate1, 3.5, 3.5);
        }

        private void BtnRightRotate_Click(object sender, RoutedEventArgs e)
        {
            //myImage.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            //rotate1++;
            //myImage.RenderTransform = new RotateTransform(rotate1, 3.5, 3.5);
        }

        private void Image_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = pbUserImage;
            e.Handled = true;
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {
            Matrix m = new Matrix();

            trans.X = 0;
            trans.Y = 0;

            center.X = 0.5;
            center.Y = 0.5;
            center = m.Transform(center);
            //rotate = 0;
            scale.X = 1;
            scale.Y = 1;

            m.Translate(trans.X, trans.Y);
            //m.RotateAt(rotate, center.X, center.Y);
            m.ScaleAt(scale.X, scale.Y, center.X, center.Y);

            imageTransform.Matrix = m;
            RaisePropertyChanged("ImageTransform");
            e.Handled = true;
        }
    }

}

