using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.SDK.Device
{
    internal class DeviceStatus
    {
        private static DeviceStatus deviceStatus;
        private static object lockObj = new object();
        public static DeviceStatus Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (deviceStatus == null)
                        deviceStatus = new DeviceStatus();
                }
                return deviceStatus;
            }
        }

        public DeviceStatus()
        {
            //this.BaggageTagPrinterStatus = new CustomPrinterStatus<BTP_AEA_CODE_ERROR, BaggageTagPrinterPaperStatus>();
        }

        /// <summary>
        /// 수하물태그 프린터 버퍼
        /// </summary>
        //public CustomPrinterStatus<BTP_AEA_CODE_ERROR, BaggageTagPrinterPaperStatus> BaggageTagPrinterStatus { get; set; }
        
        public void Initialize()
        {
            // Buffer Init
            //DeviceStatus.Instance.BaggageTagPrinterStatus.PrinterError = BTP_AEA_CODE_ERROR.NOERROR;
            //DeviceStatus.Instance.BaggageTagPrinterStatus.PaperStatus = BaggageTagPrinterPaperStatus.Normal;
        }
    }
}
