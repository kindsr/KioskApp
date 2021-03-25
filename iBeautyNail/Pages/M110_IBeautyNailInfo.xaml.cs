using System;
using System.Collections.Generic;
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

namespace iBeautyNail.Pages
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M110_IBeautyNailInfo : UserControl
    {
        public M110_IBeautyNailInfo()
        {
            InitializeComponent();
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            meVideo.Position = TimeSpan.Zero;
            meVideo.Volume = 0;
            meVideo.Play();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            meVideo.Volume = 0;
            meVideo.Play();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            meVideo.Volume = 0;
            meVideo.Stop();
        }
    }
}
