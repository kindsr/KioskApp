using iBeautyNail.Interface;
using iBeautyNail.SDK;
using iBeautyNail.Windows;
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
    internal interface IRexodReceiptPrinter : IPrinter
    {
        string Port { get; set; }

        int BaudRate { get; set; }

        int FlowControl { get; set; }
    }

    public class RexodReceiptPrinter : IRexodReceiptPrinter
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
            nRes = REXODLib.OpenPortW(Port, BaudRate, FlowControl);
            logger.DebugFormat("RexodReceiptPrinter :: Connect() :: {0}", nRes);

            // 여기서 상태체크 쓰레드 시작
            //logger.Debug("RexodReceiptPrinter :: Thread Start for State()");
            //CancellationToken token = tokenSource.Token;
            //Task.Run(async () =>
            //{
            //    while (!token.IsCancellationRequested)
            //    {
            //        int res = State();
            //        //logger.DebugFormat("RexodReceiptPrinter :: Thread for State() :: {0}", res);
            //        int msg = WindowMessage.WM_REXODPRINTER;
            //        if ((res & REXODLib.REXOD_PAPER_EMPTY) == REXODLib.REXOD_PAPER_EMPTY)   //error
            //            SDKManager.WindowMessageHandler.SendMessage(msg, (IntPtr)ReceiptPrinterWParamType.PaperEmpty, IntPtr.Zero);
            //        else if ((res & REXODLib.REXOD_HEAD_UP) == REXODLib.REXOD_HEAD_UP)   //error
            //            SDKManager.WindowMessageHandler.SendMessage(msg, (IntPtr)ReceiptPrinterWParamType.HeadUp, IntPtr.Zero);
            //        else if ((res & REXODLib.REXOD_CUT_ERROR) == REXODLib.REXOD_CUT_ERROR)   //error
            //            SDKManager.WindowMessageHandler.SendMessage(msg, (IntPtr)ReceiptPrinterWParamType.CutError, IntPtr.Zero);
            //        else if ((res & REXODLib.REXOD_NEAR_END) == REXODLib.REXOD_NEAR_END)  //Warning
            //            SDKManager.WindowMessageHandler.SendMessage(msg, (IntPtr)ReceiptPrinterWParamType.NearEnd, IntPtr.Zero);
            //        else if ((res & REXODLib.REXOD_PR_OUT_SENSOR) == REXODLib.REXOD_PR_OUT_SENSOR)  //Warning, error
            //            SDKManager.WindowMessageHandler.SendMessage(msg, (IntPtr)ReceiptPrinterWParamType.OutSensor, IntPtr.Zero);
            //        Thread.Sleep(TimeSpan.FromSeconds(3));
            //    }
            //}, token);

            return nRes;
        }

        public int Disconnect()
        {
            int nRes = -1;
            nRes = REXODLib.ClosePort();
            logger.DebugFormat("RexodReceiptPrinter :: Disconnect() :: {0}", nRes);
            return nRes;
        }

        public void Initialize()
        {
            
        }

        public bool IsConnected()
        {
            int nRes = -1;
            try
            {
                nRes = REXODLib.PrinterStatus(1);
                //logger.DebugFormat("RexodReceiptPrinter :: IsConnected :: {0}", nRes);
            }
            catch (Exception ex)
            {
                logger.DebugFormat("RexodReceiptPrinter :: IsConnected :: Exception! {0}", nRes);
                return false;
            }
            
            //return nRes == 0 ? true : false;
            return nRes == 0 || nRes == 64 ? true : false;
        }


        public int Print(object printData, bool backupStatus = true)
        {
            ReceiptData receiptData = (ReceiptData)printData;

            int nRes = -1;
            double sumPrice = 0;
            string refundString = "";

            if (receiptData.paymentType == "D4")
            {
                refundString = "취소";
            }
            

            //REXODLib.PrintTextW("RECEIPT\n\n", REXODLib.REXOD_ALIGNMENT_CENTER, REXODLib.REXOD_PRINTMODE_DOUBLE_HEIGHT);
            //REXODLib.PrintTextW(String.Format("Shop : {0}\n", receiptData.companyName), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW(String.Format("Company No : {0}\n", receiptData.companyRegNo), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW(String.Format("Addr : {0}\n", receiptData.companyAddress), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW(String.Format("Tel  : {0}\n", receiptData.companyTel), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW("------------------------------------\n", REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW(String.Format("{0,-22}{1,6}{2,8:NO}\n", "ITEM", "Q'ty", "PRICE"), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW("------------------------------------\n", REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);

            //foreach (var prod in receiptData.prodInfo)
            //{
            //    REXODLib.PrintTextW(String.Format("{0,-22}{1,6}{2,8}\n", prod.dcs, prod.qty, prod.price), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //    sumPrice = sumPrice + prod.price;
            //}

            //REXODLib.PrintTextW("------------------------------------\n", REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW(String.Format("{0,-28}{1,8}\n", "Total", sumPrice), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW("------------------------------------\n", REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW(String.Format("{0,-14}{1,22}\n", "CardNo", receiptData.cardNo), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW(String.Format("{0,-14}{1,22}\n", "Card", receiptData.cardCompany), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW(String.Format("{0,-14}{1:yyyy-MM-dd HH:mm:ss}\n", "Date", receiptData.payDate), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW(String.Format("{0,-14}{1,22}\n", "Approval No", receiptData.receiptNum), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            //REXODLib.PrintTextW("====================================\n", REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);

            REXODLib.PrintTextW("NAIL POD\n\n", REXODLib.REXOD_ALIGNMENT_CENTER, REXODLib.REXOD_PRINTMODE_DOUBLE_HEIGHT);
            REXODLib.PrintTextW(String.Format("가맹점명: {0}\n", receiptData.companyName), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW(String.Format("사업  No: {0}\n", receiptData.companyRegNo), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW(String.Format("주    소: {0}\n", receiptData.companyAddress), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW(String.Format("전화번호: {0}\n", receiptData.companyTel), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW("------------------------------------\n", REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW(String.Format("{0,-18}{1,4}{2,8:NO}\n", "품명", "수량", "금액"), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW("------------------------------------\n", REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);

            foreach (var prod in receiptData.prodInfo)
            {
                REXODLib.PrintTextW(String.Format("{0,-20}{1,6}{2,10}\n", prod.dcs, prod.qty, prod.price), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
                sumPrice = sumPrice + prod.price;
            }

            REXODLib.PrintTextW("------------------------------------\n", REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW(String.Format("{0,-25}{1,8}\n", "총  금액: ", sumPrice), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW("------------------------------------\n", REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW(String.Format("{0,-10}{1,22}\n", "카드번호: ", receiptData.cardNo), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW(String.Format("{0,-2}{1,20}\n", "카드종류: ", receiptData.cardType), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW(String.Format("{0,-11}{1:yyyy-MM-dd HH:mm:ss}\n", refundString+"거래일자: ", receiptData.payDate), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW(String.Format("{0,-8}{1,22}\n", refundString+"승인번호: ", receiptData.receiptNum), REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);
            REXODLib.PrintTextW("====================================\n", REXODLib.REXOD_ALIGNMENT_LEFT, REXODLib.REXOD_PRINTMODE_DEFAULT);

            REXODLib.nLineFeed(6);
            REXODLib.CutPaper(REXODLib.REXOD_PARTIAL_CUT);

            nRes = REXODLib.Printingcomplete(3);
            if (nRes != REXODLib.REXOD_PRINTING_OK)
            {
                logger.DebugFormat("RexodReceiptPrinter :: Print Fail Result :: {0}", nRes);
                return ReceiptPrinterWParamType.Failure;
            }
            logger.DebugFormat("RexodReceiptPrinter :: Print Result :: {0}", nRes);
            return SDKManagerStatusCode.Success;
        }

        public int State()
        {
            int nRes = -1;
            
            try
            {
                nRes = REXODLib.PrinterStatus(1);
                //logger.DebugFormat("RexodReceiptPrinter :: IsConnected :: {0}", nRes);
            }
            catch (Exception ex)
            {
                logger.DebugFormat("RexodReceiptPrinter :: IsConnected :: Exception! {0}", nRes);
            }

            string strMsg = "";

            switch (nRes)
            {
                case 0:
                    return SDKManagerStatusCode.Success;
                case -1:
                    return SDKManagerStatusCode.Disconnected;
                case -2:
                    return SDKManagerStatusCode.ConfigFailed;
                case -3:
                case -4:
                    return ReceiptPrinterWParamType.Timeout;
                default:
                    if ((nRes & REXODLib.REXOD_PAPER_EMPTY) == REXODLib.REXOD_PAPER_EMPTY)   //error
                        //strMsg += (("PAPER_EMPTY\n"));
                        return ReceiptPrinterWParamType.PaperEmpty;

                    if ((nRes & REXODLib.REXOD_HEAD_UP) == REXODLib.REXOD_HEAD_UP)   //error
                        //strMsg += (("HEAD_UP\n"));
                        return ReceiptPrinterWParamType.HeadUp;

                    if ((nRes & REXODLib.REXOD_CUT_ERROR) == REXODLib.REXOD_CUT_ERROR)   //error
                        //strMsg += (("CUT_ERROR\n"));
                        return ReceiptPrinterWParamType.CutError;

                    if ((nRes & REXODLib.REXOD_NEAR_END) == REXODLib.REXOD_NEAR_END)  //Warning
                        //strMsg += (("NEAR_END\n"));
                        return ReceiptPrinterWParamType.NearEnd;


                    //if ((nRes & REXODLib.REXOD_PR_OUT_SENSOR) == REXODLib.REXOD_PR_OUT_SENSOR)  //Warning, error
                    //    strMsg += (("PR_OUT_SENSOR\n"));

                    //logger.DebugFormat("RexodReceiptPrinter :: State() :: {0}", strMsg);

                    return nRes;
            }
        }
    }
}
