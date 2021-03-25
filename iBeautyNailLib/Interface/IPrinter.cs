using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Interface
{
    public interface IPrinter
    {
        /// <summary>
        /// Initialize
        /// </summary>
        void Initialize();

        /// <summary>
        /// Connect
        /// </summary>
        int Connect();

        /// <summary>
        /// Disconnect
        /// </summary>
        int Disconnect();

        /// <summary>
        /// IsConnected
        /// </summary>
        bool IsConnected();

        /// <summary>
        /// 출력
        /// </summary>
        /// <param name="printData">출력 데이터</param>
        int Print(object printData, bool backupStatus = true);
        
        /// <summary>
        /// 프린터의 상태를 반환한다.
        /// </summary>
        int State();
    }
}
