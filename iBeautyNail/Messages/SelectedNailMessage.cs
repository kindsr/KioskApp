using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Messages
{
    class SelectedNailMessage : MessageBase
    {
        public string DesignPath { get; set; }
    }
}
