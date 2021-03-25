using iBeautyNail.Enums;
using iBeautyNail.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Messages
{
    public class NavigationMessage
    {
        public BaseViewModelBase CurrentViewModel { get; set; }

        public NAVIGATION_TYPE NavigationType { get; set; }

        public string ViewModelName { get; set; }
    }
}