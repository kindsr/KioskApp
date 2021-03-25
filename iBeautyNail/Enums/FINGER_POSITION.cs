using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Enums
{
    public enum FINGER_POSITION
    {
        [Description("LeftPinky")]
        LEFT_PINKY,

        [Description("LeftRing")]
        LEFT_RING,

        [Description("LeftMiddle")]
        LEFT_MIDDLE,

        [Description("LeftIndex")]
        LEFT_INDEX,

        [Description("LeftThumb")]
        LEFT_THUMB,

        [Description("RightThumb")]
        RIGHT_THUMB,

        [Description("RightIndex")]
        RIGHT_INDEX,

        [Description("RightMiddle")]
        RIGHT_MIDDLE,

        [Description("RightRing")]
        RIGHT_RING,

        [Description("RightPinky")]
        RIGHT_PINKY,
    }
}
