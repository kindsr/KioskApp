using iBeautyNail.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Datas
{
    public class DesignListViewModel : BaseViewModelBase
    {
        private ObservableCollection<DesignListPart> _designCompleteList;

        public DesignListViewModel(List<DesignListPart> designCompleteList)
        {
            DesignCompleteList = new ObservableCollection<DesignListPart>(designCompleteList);
        }

        public ObservableCollection<DesignListPart> DesignCompleteList
        {
            get { return _designCompleteList; }
            set { Set(ref _designCompleteList, value); }
        }
    }
}
