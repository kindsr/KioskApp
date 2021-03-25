using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace iBeautyNail.Extensions.Controls
{
    public class ImageButton : DependencyObject
    {
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.RegisterAttached("ImagePath", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

        public static readonly DependencyProperty BackgroundImagePathProperty =
            DependencyProperty.RegisterAttached("BackgroundImagePath", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

        public static ImageSource GetImagePath(DependencyObject obj) { return (ImageSource)obj.GetValue(ImagePathProperty); }
        public static void SetImagePath(DependencyObject obj, ImageSource value) { obj.SetValue(ImagePathProperty, value); }

        public static ImageSource GetBackgroundImagePath(DependencyObject obj) { return (ImageSource)obj.GetValue(BackgroundImagePathProperty); }
        public static void SetBackgroundImagePath(DependencyObject obj, ImageSource value) { obj.SetValue(BackgroundImagePathProperty, value); }
    }

    public class ImageRec : DependencyObject
    {
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.RegisterAttached("ImagePath", typeof(ImageSource), typeof(ImageRec), new UIPropertyMetadata(null));

        public static ImageSource GetImagePath(DependencyObject obj) { return (ImageSource)obj.GetValue(ImagePathProperty); }
        public static void SetImagePath(DependencyObject obj, ImageSource value) { obj.SetValue(ImagePathProperty, value); }
    }
}
