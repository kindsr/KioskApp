using GalaSoft.MvvmLight.CommandWpf;
using iBeautyNail.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

namespace iBeautyNail.ViewModel
{
    class M110_IBeautyNailInfoViewModel : BaseViewModelBase
    {
        public ICommand ClickCommand { get; }

        public M110_IBeautyNailInfoViewModel()
        {
            ClickCommand = new RelayCommand(MouseDown);
        }
        public void MouseDown()
        {
            CommandAction(NAVIGATION_TYPE.Next);
        }
    }
}
