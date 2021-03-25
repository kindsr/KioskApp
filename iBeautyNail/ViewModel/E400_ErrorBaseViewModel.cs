using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.ViewModel
{
    class E400_ErrorBaseViewModel : BaseViewModelBase
    {
        public E400_ErrorBaseViewModel()
        {
            BaseViewModelBase.CanNavigate = true;

            HomeButtonVisible = false;
            PrevButtonVisible = false;
            NextButtonVisible = false;
        }
    }
}
