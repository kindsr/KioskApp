using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class A010_AdminBase : UserControl
    {
        public A010_AdminBase()
        {
            InitializeComponent();
        }

        private void ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<decimal> e)
        {
            TotalPrintCount.Text = iBeautyNail.Datas.GlobalVariables.Instance.MyProduct.qty.ToString();
        }



        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsNumeric(e.Text);

        }


        private bool IsNumeric(string source)
        {
            Regex regex = new Regex("[a-zA-Z]");
            return !regex.IsMatch(source);
        }
    }
}        