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
    /// Q200_QRScan.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Q200_QRScan : UserControl
    {
        public Q200_QRScan()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtScanText.Focus();
        }

        public void doFocus(string msg)
        {
            if (msg == "focus")
                this.txtScanText.Focus();
        }
    }
}
