using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Enums
{
    public enum NAVIGATION_TYPE
    {
        [Description("Previous")]
        Previous,

        [Description("Next")]
        Next,

        [Description("Direct")]
        Direct,

        [Description("Back")]
        Back,
    }
}
