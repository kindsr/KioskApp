using iBeautyNail.Interface;
using iBeautyNail.SDK;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace iBeautyNail.Devices.ReceiptPrinter
{
    internal interface IHwasungReceiptPrinter : IPrinter
    {
        string Port { get; set; }

        int BaudRate { get; set; }

        int FlowControl { get; set; }
    }

    public class HwasungReceiptPrinter : IHwasungReceiptPrinter
    {
        protected readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        CancellationTokenSource tokenSource = new CancellationTokenSource();

        private string port;
        [Dependency("Port")]
        public string Port
        {
            get { return port; }
            set { port = value; }
        }

        private int baudRate;
        [Dependency("BaudRate")]
        public int BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }

        private int flowControl;
        [Dependency("FlowControl")]
        public int FlowControl
        {
            get { return flowControl; }
            set { flowControl = value; }
        }


        public int Connect()
        {
            int nRes = -1;
            nRes = HwaLib.UsbOpen("HMK-056");
            logger.DebugFormat("HwasungReceiptPrinter :: Connect() :: {0}", nRes);
            return nRes;
        }

        public int Disconnect()
        {
            return SDKManagerStatusCode.Success;
        }

        public void Initialize()
        {

        }

        public bool IsConnected()
        {
            int nRes = HwaLib.UsbOpen("HMK-056");
            return nRes == 0 ? true : false;
        }

        public int Print(object printData, bool backupStatus = true)
        {
            ReceiptData receiptData = (ReceiptData)printData;

            int nRes = -1;
            double sumPrice = 0;

            nRes = HwaLib.PrintCmd(0x1B);
            nRes = HwaLib.PrintStr("a");
            nRes = HwaLib.PrintCmd(0x01);                       //Text Align center

            nRes = HwaLib.PrintStr("NAILPOD");
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintCmd(0x0A);

            nRes = HwaLib.PrintCmd(0x1B);
            nRes = HwaLib.PrintStr("M");
            nRes = HwaLib.PrintCmd(0x00);                       //한글 폰트 16x16 10   24x24 00

            nRes = HwaLib.PrintCmd(0x1B);
            nRes = HwaLib.PrintStr("a");
            nRes = HwaLib.PrintCmd(0x00);                       //Text Align left

            nRes = HwaLib.PrintCmd(0x1A);
            nRes = HwaLib.PrintStr("x");
            nRes = HwaLib.PrintCmd(0x00);                       // 한글

            nRes = HwaLib.PrintStr(string.Format("가맹점명: {0}", receiptData.companyName));
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintStr(string.Format("사업자No: {0}", receiptData.companyRegNo));
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintStr(string.Format("주   소: {0}", receiptData.companyAddress));
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintStr(string.Format("전화번호: {0}", receiptData.companyTel));
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintStr("---------------------------------");
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintStr(string.Format("{0,-12}{1,6}{2,8:NO}", "품명", "수량", "금액"));
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintStr("---------------------------------");
            nRes = HwaLib.PrintCmd(0x0A);

            foreach (var prod in receiptData.prodInfo)
            {
                HwaLib.PrintStr(string.Format("{0,-16}{1,6}{2,10}", prod.dcs, prod.qty, prod.price));
                nRes = HwaLib.PrintCmd(0x0A);
                sumPrice = sumPrice + prod.price;
            }

            nRes = HwaLib.PrintStr("---------------------------------");
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintStr(string.Format("{0,-21}{1,8}", "총 금액: ", sumPrice));
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintStr("---------------------------------");
            nRes = HwaLib.PrintCmd(0x0A);

            nRes = HwaLib.PrintStr(string.Format("{0,-8}{1,20}", "카드번호: ", receiptData.cardNo));
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintStr(string.Format("{0}{1,16}", "카드종류: ", receiptData.cardType));
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintStr(string.Format("{0,-9}{1:yyyy-MM-dd HH:mm:ss}", "거래일자: ", receiptData.payDate));
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintStr(string.Format("{0,-8}{1,20}", "승인번호: ", receiptData.receiptNum));
            nRes = HwaLib.PrintCmd(0x0A);

            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintCmd(0x0A);
            nRes = HwaLib.PrintCmd(0x0A);

            nRes = HwaLib.PrintCmd(0x1B);
            //nRes = HwaLib.PrintStr("i");                                       //Full Cut
            nRes = HwaLib.PrintStr("m");                                       //Patial Cut

            //nRes = HwaLib.NewRealRead();

            logger.DebugFormat("HwasungReceiptPrinter :: Print Result :: {0}", nRes);
            return SDKManagerStatusCode.Success;
        }

        public int State()
        {
            int nRes = HwaLib.NewRealRead();

            switch (nRes)
            {
                case 0:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Normal Status");
                    nRes = SDKManagerStatusCode.Success;
                    break;
                case 1:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Paper out");
                    nRes = ReceiptPrinterWParamType.PaperEmpty;
                    break;
                case 2:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Head open");
                    nRes = ReceiptPrinterWParamType.HeadUp;
                    break;
                case 3:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Paper out && Head open");
                    break;
                case 4:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Paper Jam");
                    nRes = ReceiptPrinterWParamType.Failure;
                    break;
                case 5:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Paper out && Paper Jam");
                    break;
                case 6:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Head open && Paper Jam");
                    break;
                case 7:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Paper out && Head open && Paper Jam");
                    break;
                case 8:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Near End");
                    nRes = ReceiptPrinterWParamType.NearEnd;
                    break;
                case 9:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Paper out && Near end");
                    nRes = ReceiptPrinterWParamType.NearEnd;
                    break;
                case 10:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Head open && Near end");
                    nRes = ReceiptPrinterWParamType.NearEnd;
                    break;
                case 11:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Paper out && Head open && Near end");
                    break;
                case 12:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Paper Jam && Near end");
                    break;
                case 13:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Paper out && Paper Jam && Near end");
                    break;
                case 14:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Head open && Paper Jam && Near end");
                    break;
                case 15:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Paper out && Head open && Paper Jam && Near end");
                    break;
                case 16:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Print Running");
                    break;
                case 32:
                    logger.DebugFormat("HwasungReceiptPrinter :: Status :: Cutter Jam");
                    break;
            }

            return nRes;
        }
    }
}
