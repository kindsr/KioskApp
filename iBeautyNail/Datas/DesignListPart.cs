using iBeautyNail.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iBeautyNail.Datas
{
    public class DesignListPart : BaseViewModelBase
    {
        private string _line;
        public string Line
        {
            get { return _line; }
            set
            {
                _line = value;
                RaisePropertyChanged();
            }
        }

        private HorizontalAlignment _lineAlign;
        public HorizontalAlignment LineAlign
        {
            get { return _lineAlign; }
            set
            {
                _lineAlign = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<DesignInfo> _designs;
        public ObservableCollection<DesignInfo> Designs
        {
            get { return _designs; }
            set
            {
                _designs = value;
                RaisePropertyChanged();
            }
        }
    }
}
