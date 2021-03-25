using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace iBeautyNail.Extensions.Controls
{
    /// <summary>
    /// ModelNailSet.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModelNailSet : UserControl
    {
        public ModelNailSet()
        {
            InitializeComponent();
        }

        private RelayCommand<string> selectModelNailDesignCommand;
        public RelayCommand<string> SelectModelNailDesignCommand
        {
            get
            {
                return new RelayCommand<string>((designpath11) =>
                {
                    Console.Write("Selected Nail Path=>{0}\n", designpath11);
                    Messenger.Default.Send<string>(designpath11);
                });
            }
        }

        //맨 위에 모델 손 사진
        public static DependencyProperty ModelSourceProperty
                = DependencyProperty.Register("ModelSource", typeof(ImageSource), typeof(ModelNailSet), new FrameworkPropertyMetadata(OnModelSourcePropertyChanged));

        public ImageSource ModelSource
        {
            get { return (ImageSource)this.GetValue(ModelSourceProperty); }
            set { this.SetValue(ModelSourceProperty, value); }
        }

        private static void OnModelSourcePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ModelNailSet modelNailSet = obj as ModelNailSet;
            modelNailSet.imgModelSource.Source = args.NewValue as ImageSource;
        }



        //네일 세트 중 1번
        public static DependencyProperty NailSource1Property
        = DependencyProperty.Register("NailSource1", typeof(ImageSource), typeof(ModelNailSet), new FrameworkPropertyMetadata(OnNailSource1PropertyChanged));

        public ImageSource NailSource1
        {
            get { return (ImageSource)this.GetValue(NailSource1Property); }
            set { this.SetValue(NailSource1Property, value); }
        }

        private static void OnNailSource1PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ModelNailSet modelNailSet = obj as ModelNailSet;
            modelNailSet.imgNail1Source.Source = args.NewValue as ImageSource;
        }



        //네일 세트 중 2번
        public static DependencyProperty NailSource2Property
        = DependencyProperty.Register("NailSource2", typeof(ImageSource), typeof(ModelNailSet), new FrameworkPropertyMetadata(OnNailSource2PropertyChanged));

        public ImageSource NailSource2
        {
            get { return (ImageSource)this.GetValue(NailSource2Property); }
            set { this.SetValue(NailSource2Property, value); }
        }

        private static void OnNailSource2PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ModelNailSet modelNailSet = obj as ModelNailSet;
            modelNailSet.imgNail2Source.Source = args.NewValue as ImageSource;
        }




        //네일 세트 중 3번
        public static DependencyProperty NailSource3Property
        = DependencyProperty.Register("NailSource3", typeof(ImageSource), typeof(ModelNailSet), new FrameworkPropertyMetadata(OnNailSource3PropertyChanged));

        public ImageSource NailSource3
        {
            get { return (ImageSource)this.GetValue(NailSource3Property); }
            set { this.SetValue(NailSource3Property, value); }
        }

        private static void OnNailSource3PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ModelNailSet modelNailSet = obj as ModelNailSet;
            modelNailSet.imgNail3Source.Source = args.NewValue as ImageSource;
        }



        //네일 세트 중 4번
        public static DependencyProperty NailSource4Property
        = DependencyProperty.Register("NailSource4", typeof(ImageSource), typeof(ModelNailSet), new FrameworkPropertyMetadata(OnNailSource4PropertyChanged));

        public ImageSource NailSource4
        {
            get { return (ImageSource)this.GetValue(NailSource4Property); }
            set { this.SetValue(NailSource4Property, value); }
        }

        private static void OnNailSource4PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ModelNailSet modelNailSet = obj as ModelNailSet;
            modelNailSet.imgNail4Source.Source = args.NewValue as ImageSource;
        }


        //네일 세트 중 5번
        public static DependencyProperty NailSource5Property
        = DependencyProperty.Register("NailSource5", typeof(ImageSource), typeof(ModelNailSet), new FrameworkPropertyMetadata(OnNailSource5PropertyChanged));

        public ImageSource NailSource5
        {
            get { return (ImageSource)this.GetValue(NailSource5Property); }
            set { this.SetValue(NailSource5Property, value); }
        }

        private static void OnNailSource5PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ModelNailSet modelNailSet = obj as ModelNailSet;
            modelNailSet.imgNail5Source.Source = args.NewValue as ImageSource;
        }

    }
}
