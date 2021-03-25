using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Enums
{
    /// <summary>
    /// 팝업 창의 스타일 구분
    /// </summary>
    public enum POPUP_QUANTITY
    {
        /// <summary>
        /// 버튼 0개(버튼없는 팝업)
        /// </summary>
        BUTTON_DEFAULT = -1,
        /// <summary>
        /// 버튼 0개(버튼없는 팝업)
        /// </summary>
        BUTTON_ZERO = 0,
        /// <summary>
        /// 버튼 1개(확인창 팝업)
        /// </summary>
        BUTTON_ONE = 1,
        /// <summary>
        /// 버튼 2개(Yes, No 선택 팝업)
        /// </summary>
        BUTTON_TWO = 2,
        /// <summary>
        /// 버튼 3개(지정 시간 후에 사라지는 팝업)
        /// </summary>
        BUTTON_THREE = 3,
    }

    /// <summary>
    /// 팝업창에 사용되는 버튼의 Text
    /// </summary>
    public enum POPUP_BUTTON
    {
        [Description("button0")]
        BUTTON_0 = 0,

        [Description("button1")]
        BUTTON_1 = 1,

        [Description("button2")]
        BUTTON_2 = 2,
    }
}
