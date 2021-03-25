using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Interface
{
    /// <summary>
    /// 통신 인터페이스
    /// </summary>
    public interface ISendCommand
    {
        /// <summary>
        /// 송신 인터페이스
        /// </summary>
        int SendCommand(string command);
        /// <summary>
        /// 송신 인터페이스
        /// </summary>
        int SendCommand(byte[] command);
        /// <summary>
        /// 데이터 전달을 위한 EventHandler
        /// </summary>
        event EventHandler<ExEventArgs> ReceiveCompleted;
    }

    /// <summary>
    /// 데이터 전달을 위한 Event Args
    /// </summary>
    public class ExEventArgs : EventArgs
    {
        public string StringData { get; set; }
        public byte[] ByteData { get; set; }
    }
}
