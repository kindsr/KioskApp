using GalaSoft.MvvmLight;
using System.Windows.Media.Imaging;

namespace iBeautyNail.Language
{
    public class LanguageInfo : ObservableObject
    {
        private string culture;
        public string Culture
        {
            get { return culture; }
            set { Set(() => Culture, ref culture, value); }
        }

        private string country;
        public string Country
        {
            get { return country; }
            set { Set(() => Country, ref country, value); }
        }
    }

}