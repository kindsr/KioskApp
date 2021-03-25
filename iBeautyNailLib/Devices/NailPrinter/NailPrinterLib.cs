using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace iBeautyNail.Devices.NailPrinter
{
    public class NailPrinterLib
    {
        protected readonly static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void PrintStatus(int value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void MotorStatus(int value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InkVol(int id, int value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void PrinterStatus(int value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void EmptyPaper();


        [DllImport("NailPrinterDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateNailPrint();


        [DllImport("NailPrinterDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void OpenNailPrint(IntPtr pNailPrintObject
            , [MarshalAs(UnmanagedType.FunctionPtr)] PrintStatus callbackPS
            , [MarshalAs(UnmanagedType.FunctionPtr)] MotorStatus callbackMS
            , [MarshalAs(UnmanagedType.FunctionPtr)] InkVol callbackIV);

        [DllImport("NailPrinterDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPrinterStatusEvnet(IntPtr pNailPrintObject
            , [MarshalAs(UnmanagedType.FunctionPtr)] PrinterStatus callbackPS);

        [DllImport("NailPrinterDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetEmptyPaperEvent(IntPtr pNailPrintObject
            , [MarshalAs(UnmanagedType.FunctionPtr)] EmptyPaper callbackEP);

        [DllImport("NailPrinterDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MotorOnNailPrint(IntPtr pNailPrintObject);

        [DllImport("NailPrinterDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MotorOffNailPrint(IntPtr pNailPrintObject);

        [DllImport("NailPrinterDll.dll", CallingConvention = CallingConvention.Cdecl)]
        static public extern void PrinterHeadCleanNailPrint(IntPtr pNailPrintObject);

        [DllImport("NailPrinterDll.dll", CallingConvention = CallingConvention.Cdecl)]
        static public extern void PrinterQueueCleanNailPrint(IntPtr pNailPrintObject);

        [DllImport("NailPrinterDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetInkVolNailPrint(IntPtr pNailPrintObject);

        [DllImport("NailPrinterDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CloseNailPrint(IntPtr pNailPrintObject);

        [DllImport("NailPrinterDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReleaseNailPrint(IntPtr pNailPrintObject);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);


        private IntPtr _pCublClass;
        static private IntPtr _hWnd;
        public const int PCU_MSG_STATE = 0x4321;
        public const int PCU_MSG_STATE_MOTOR = 0x01;
        public const int PCU_MSG_STATE_PRINT = 0x02;
        public const int PCU_MSG_STATE_PRINTER = 0x03;
        public const int PCU_MSG_STATE_EMPTY_PAPER = 0x04;

        public const int PCU_MSG_INK_VALUE = 0x4322;

        public NailPrinterLib()
        {

        }

        public void Create()
        {
            _pCublClass = CreateNailPrint();
            Console.WriteLine(string.Format("NailPrinterLib :: Create() :: CreateNailPrint {0}", _pCublClass.ToInt32()));
            logger.DebugFormat("NailPrinterLib :: Create() :: CreateNailPrint {0}", _pCublClass.ToInt32());
        }

        public void Open(IntPtr hWnd)
        {
            _hWnd = hWnd;
            Console.WriteLine(string.Format("NailPrinterLib :: Open :: OpenNailPrint {0}", _hWnd.ToInt32()));
            logger.DebugFormat("NailPrinterLib :: Open :: OpenNailPrint {0}", _hWnd.ToInt32());
            OpenNailPrint(_pCublClass, CallBackInterPrintStatus, CallBackInterMotorStatus, CallBackInterInkVol);
            SetPrinterStatusEvnet(_pCublClass, CallBackInterPrinterStatus);
            SetEmptyPaperEvent(_pCublClass, CallBackInterEmptyPaper);
        }

        public void Open(PrintStatus callbackPrintStatus, MotorStatus callbackMotorStatus, InkVol callbackInkVol)
        {
            OpenNailPrint(_pCublClass, callbackPrintStatus, callbackMotorStatus, callbackInkVol);
        }

        public void setPrinterEvent(PrinterStatus callbackPrinterStatus)
        {
            SetPrinterStatusEvnet(_pCublClass, callbackPrinterStatus);
        }

        public void setEmptyPaper(EmptyPaper callbackEmptyPaper)
        {
            SetEmptyPaperEvent(_pCublClass, callbackEmptyPaper);
        }

        public void MotorOn()
        {
            MotorOnNailPrint(_pCublClass);
        }

        public void MotorOff()
        {
            MotorOffNailPrint(_pCublClass);
        }

        public void GetInkValues()
        {
            GetInkVolNailPrint(_pCublClass);
        }

        public void PrinterHeadClean()
        {
            PrinterHeadCleanNailPrint(_pCublClass);
        }

        public void PrintQueueClean()
        {
            PrinterQueueCleanNailPrint(_pCublClass);
        }

        public void Close()
        {
            Console.WriteLine(string.Format("NailPrinterLib :: Close() :: CloseNailPrint {0}", _pCublClass.ToInt32()));
            logger.DebugFormat("NailPrinterLib :: Close() :: CloseNailPrint {0}", _pCublClass.ToInt32());
            CloseNailPrint(_pCublClass);
        }


        public void Release()
        {
            Console.WriteLine(string.Format("NailPrinterLib :: Release() :: CloseNailPrint {0}", _pCublClass.ToInt32()));
            logger.DebugFormat("NailPrinterLib :: Release() :: CloseNailPrint {0}", _pCublClass.ToInt32());
            ReleaseNailPrint(_pCublClass);
        }

        PrintStatus CallBackInterPrintStatus =
           (value) =>
           {
               Console.WriteLine(string.Format("Callback :: PrintStatus :: SendMessage {0}", value));
               logger.DebugFormat("Callback :: PrintStatus :: SendMessage {0}", value);
               SendMessage(_hWnd, PCU_MSG_STATE, PCU_MSG_STATE_PRINT, value);
           };

        MotorStatus CallBackInterMotorStatus =
            (value) =>
            {
                Console.WriteLine(string.Format("Callback :: MotorStatus :: SendMessage {0}", value));
                logger.DebugFormat("Callback :: MotorStatus :: SendMessage {0}", value);
                SendMessage(_hWnd, PCU_MSG_STATE, PCU_MSG_STATE_MOTOR, value);
            };

        InkVol CallBackInterInkVol =
            (id, value) =>
            {
                Console.WriteLine(string.Format("Callback :: InkVol :: SendMessage {0} / {1}", id, value));
                logger.DebugFormat("Callback :: InkVol :: SendMessage {0} / {1}", id, value);
                SendMessage(_hWnd, PCU_MSG_INK_VALUE, id, value);
            };

        PrinterStatus CallBackInterPrinterStatus =
            (value) =>
            {
                Console.WriteLine(string.Format("Callback :: InterPrinterStatus :: SendMessage {0}", value));
                logger.DebugFormat("Callback :: InterPrinterStatus :: SendMessage {0}", value);
                // lParam
                /*
                 if (((Status & PRINTER_STATUS_ERROR) == PRINTER_STATUS_ERROR)
                  || ((Status & PRINTER_STATUS_PAPER_JAM) == PRINTER_STATUS_PAPER_JAM)
                  || ((Status & PRINTER_STATUS_PAPER_OUT) == PRINTER_STATUS_PAPER_OUT)
                  || ((Status & PRINTER_STATUS_PAPER_PROBLEM) == PRINTER_STATUS_PAPER_PROBLEM)
                  || ((Status & PRINTER_STATUS_OFFLINE) == PRINTER_STATUS_OFFLINE))   
                 */
                SendMessage(_hWnd, PCU_MSG_STATE, PCU_MSG_STATE_PRINTER, value);
            };

        EmptyPaper CallBackInterEmptyPaper =
            () =>
            {
                Console.WriteLine(string.Format("Callback :: InterEmptyPaper :: SendMessage"));
                logger.DebugFormat("Callback :: InterEmptyPaper :: SendMessage");
                SendMessage(_hWnd, PCU_MSG_STATE, PCU_MSG_STATE_EMPTY_PAPER, 1);
            };
    }
}
