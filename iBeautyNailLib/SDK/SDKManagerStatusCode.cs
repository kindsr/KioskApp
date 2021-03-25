using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.SDK
{
    #region BaseStatusCode
    public class BaseStatusCode
    {
        public const int Success = 0;
    }
    #endregion  // BaseStatusCode

    public class SDKManagerStatusCode : BaseStatusCode
    {
        /// <summary>
        /// device library disconnected
        /// </summary>
        public const int Disconnected = 1001;

        /// <summary>
        /// device library initialization required
        /// </summary>
        public const int InitRequired = 1002;

        /// <summary>
        /// device library connection required
        /// </summary>
        public const int ConnectionRequired = 1003;

        /// <summary>
        /// device library connect failed
        /// </summary>
        public const int ConnectFailed = 1004;

        /// <summary>
        /// device library disconnect failed
        /// </summary>
        public const int DisconnectFailed = 1005;

        /// <summary>
        /// 설정값 오류
        /// </summary>
        public const int ConfigFailed = 1008;

        /// <summary>
        /// 연결된 장비 없음
        /// </summary>
        public const int NoDeviceConnected = 1009;
    }

    #region  Receipt Printer
    public class ReceiptPrinterWParamType
    {
        /// <summary>
        /// 프린터 완료
        /// </summary>
        public const int Printed = 1;

        /// <summary>
        /// 프린터 용지 없음
        /// </summary>
        public const int PaperEmpty = 11;

        /// <summary>
        /// 프린터 헤드 오류
        /// </summary>
        public const int HeadUp = 12;

        /// <summary>
        /// 프린터 커팅오류
        /// </summary>
        public const int CutError = 13;

        /// <summary>
        /// 프린터 용지 거의끝남
        /// </summary>
        public const int NearEnd = 14;

        /// <summary>
        /// 프린터 센서 오류
        /// </summary>
        public const int OutSensor = 15;

        /// <summary>
        /// 프린트 타임아웃
        /// </summary>
        public const int Timeout = 98;

        /// <summary>
        /// 프린트 실패
        /// </summary>
        public const int Failure = 99;
    }
    #endregion

    #region  Nail Printer
    public class NailPrinterWParamType
    {
        /// <summary>
        /// 프린터 완료
        /// </summary>
        public const int Printed = 1;

        /// <summary>
        /// 프린트 시작
        /// </summary>
        public const int Printing = 2;

        /// <summary>
        /// 파일없음
        /// </summary>
        public const int Nofile = 3;

        /// <summary>
        /// 파일있음
        /// </summary>
        public const int FileExist = 4;

        /// <summary>
        /// 프린트 타임아웃
        /// </summary>
        public const int Timeout = 98;

        /// <summary>
        /// 프린트 실패
        /// </summary>
        public const int Failure = 99;
    }
    #endregion

    #region Card Payment
    public class CardPaymentWParamType
    {
        /// <summary>
        /// 결제 취소 완료
        /// </summary>
        public const int CancelCompleted = 1;

        /// <summary>
        /// 카드 리더기 내 카드 있음
        /// </summary>
        public const int CardInserted = 2;

        /// <summary>
        /// 카드 리더기 내 카드 없음
        /// </summary>
        public const int CardEmpty = 3;

        /// <summary>
        /// 카드 결제 실패
        /// </summary>
        public const int Failure = 99;
    }
    #endregion
}
