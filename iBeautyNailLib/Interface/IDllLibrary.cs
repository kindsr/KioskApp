using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Interface
{
    public interface IDllLibrary : IDeviceController
    {
        /// <summary>
        /// 이벤트를 받을 윈도우 핸들러 등록. 기타 설정값 Read
        /// </summary>
        int ReadConfig(IntPtr handle);
    }
}
