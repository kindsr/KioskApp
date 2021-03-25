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
    /// A000_AdminLogin.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class A000_AdminLogin : UserControl
    {
        private StringBuilder Input { get; set; }

        public A000_AdminLogin()
        {
            InitializeComponent();

            this.Input = new StringBuilder();
        }
    }
}
