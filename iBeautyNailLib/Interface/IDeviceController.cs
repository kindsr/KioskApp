using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Interface
{
    /// <summary>
    /// 각 장비의 기본 동작을 선언하는 인터페이스
    /// </summary>
    public interface IDeviceController
    {
        /// <summary>
        /// 장비 초기화 구현
        /// </summary>
        void Initialize(string filepath);

        /// <summary>
        /// 장비 연결 구현
        /// </summary>
        int Connect();

        /// <summary>
        /// 장비 연결 해제 구현
        /// </summary>
        int Disconnect();

        /// <summary>
        /// 장비 접속 여부를 반환하게 구현
        /// true : 연결. false : 연결 안됨.
        /// </summary>
        bool IsConnected { get; }

    }
}
