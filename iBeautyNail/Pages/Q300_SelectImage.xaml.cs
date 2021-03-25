using System.Windows.Controls;

namespace iBeautyNail.Pages
{
    /// <summary>
    /// Q300_SelectImage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Q300_SelectImage : UserControl
    {
        public Q300_SelectImage()
        {
            InitializeComponent();
        }

        private void ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<decimal> e)
        {
            TotalPrintCount.Text = iBeautyNail.Datas.GlobalVariables.Instance.MyProduct.qty.ToString();
        }
    }
}
