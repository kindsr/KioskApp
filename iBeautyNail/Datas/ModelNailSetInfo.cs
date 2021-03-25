using iBeautyNail.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace iBeautyNail.Datas
{
    public class ModelNailSetInfo : BaseViewModelBase
    {
        public ModelNailSetInfo()
        {
            modelNailList = new ObservableCollection<DesignInfo>();
            modelNailList2 = new ObservableCollection<DesignInfo>();
        }

        private string modelPath;
        public string ModelPath
        {
            get { return modelPath; }
            set
            {
                modelPath = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<DesignInfo> modelNailList;
        public ObservableCollection<DesignInfo> ModelNailList
        {
            get { return modelNailList; }
            set
            {
                modelNailList = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<DesignInfo> modelNailList2;
        public ObservableCollection<DesignInfo> ModelNailList2
        {
            get { return modelNailList2; }
            set
            {
                modelNailList2 = value;
                RaisePropertyChanged();
            }
        }
    }
}
