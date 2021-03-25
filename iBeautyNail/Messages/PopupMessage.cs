using GalaSoft.MvvmLight;
using iBeautyNail.Enums;
using iBeautyNail.Messages.Exceptions.Enums;
using iBeautyNail.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace iBeautyNail.Messages
{
    public class PopupMessage : ObservableObject
    {
        public BaseViewModelBase CurrentViewModel { get; set; }
        
        public POPUP_QUANTITY Quantity { get; set; }

        public Visibility Visibility { get; set; }

        public VALIDATION_TITLE_MESSAGE Title { get; set; }

        public VALIDATION_MESSAGE SubTitle { get; set; }

        public VALIDATION_MESSAGE Message { get; set; }

        public POPUP_BUTTON Pushed { get; set; }

        public ICommand Button0 { get; set; }

        public ICommand Button1 { get; set; }

        public ICommand Button2 { get; set; }

        public PopupMessageOption option { get; set; }
    }

    public class PopupMessageOption
    {
        public string Button0Text { get; set; } = "CONFIRM";

        public string Button1Text { get; set; } = "CANCEL";

        public string Button2Text { get; set; } = "NEXT";

        public string Button0Page { get; set; } = null;

        public string Button1Page { get; set; } = null;

        public string Button2Page { get; set; } = null;
    }
}