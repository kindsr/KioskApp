using GalaSoft.MvvmLight;
using System.Windows.Media.Imaging;

namespace iBeautyNail.Datas
{
    public class QRImageListInfo : ObservableObject
    {
        public QRImageListInfo()
        {
        }

        private string printImage;
        public string PrintImage
        {
            get { return printImage; }
            set
            {
                if (Equals(value, printImage)) return;
                printImage = value;
            }

        }

        private string tempImagePath;
        public string TempImagePath
        {
            get { return tempImagePath; }
            set
            {
                if (Equals(value, tempImagePath)) return;
                tempImagePath = value;
                RaisePropertyChanged();
            }

        }

        private int printCount;
        public int PrintCount
        {
            get { return printCount; }
            set
            {
                // 5만원........
                if (value < 0) return;
                if (GlobalVariables.Instance.MyProduct.qty >= 3 && value > printCount) return;
                if (GlobalVariables.Instance.MyProduct.qty <= 0 && value < printCount) return;
                if (Equals(value, printCount)) return;

                // +/- 구분
                if (value > printCount)
                    GlobalVariables.Instance.MyProduct.qty++;
                else
                    GlobalVariables.Instance.MyProduct.qty--;

                printCount = value;
                RaisePropertyChanged();
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (Equals(value, isSelected)) return;
                isSelected = value;
                RaisePropertyChanged();
            }
        }
    }
}
