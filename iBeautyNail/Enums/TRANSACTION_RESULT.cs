using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Enums
{
    public enum TRANSACTION_RESULT
    {
        RESULT_NONE = 0,                // Unknown

        RESULT_CANCEL_PAX,              // Canceled by PAX
        RESULT_CANCEL_ADMIN,            // Canceled by admin Logger-in
        RESULT_CANCEL_OOO,              // Canceled by out-of-order
        RESULT_CANCEL_EXIT,             // Canceled by S/W exit
        RESULT_CANCEL_TIMEOUT,          // Canceled by timeout
        RESULT_FAIL,                    // Failed
    }
}
