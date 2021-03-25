using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;     // DLL support
//using DWORD = System.UInt32;

namespace iBeautyNail.Devices.ReceiptPrinter
{
    #region RexodLib class declaration

    public class REXODLib
    {
        // Dll Name.
        public const string REXOD_LIB = "REXOD_LIB.dll";
        public REXODLib()
        { }


        #region Method Declaration

        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "OpenPortW")]
        public static extern int OpenPortW([MarshalAs(UnmanagedType.LPWStr)]string PortName, int nBuadrate, int nFlowControl);

        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "ClosePort")]
        public static extern int ClosePort();

        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "PrintTextW")]
        public static extern int PrintTextW([MarshalAs(UnmanagedType.LPWStr)]string Data, int Alignment, int TextSize);



        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "PrintBarCodeW")]
        public static extern int PrintBarCodeW([MarshalAs(UnmanagedType.LPWStr)]string Data, int symbology, int height, int width, int Alignment, int textPosition);


        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "PrintPDF417W")]
        public static extern int PrintPDF417W([MarshalAs(UnmanagedType.LPWStr)]string Data, int row_number, int column_number);

        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "PrintQRCodeW")]
        public static extern int PrintQRCodeW([MarshalAs(UnmanagedType.LPWStr)]string Data, int xPosition, int rotation, int enlargement);


        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "PrintNvimage")]
        public static extern int PrintNvimage(int number, int Alignment);


        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "PrintBitmapW")]
        public static extern int PrintBitmapW([MarshalAs(UnmanagedType.LPWStr)]string BitmapName, int Alignment);


        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "PrinterStatus")]
        public static extern int PrinterStatus(int Timeout);



        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "nLineFeed")]
        public static extern int nLineFeed(int nline);


        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "CutPaper")]
        public static extern int CutPaper(int Cutmode);

        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "Printingcomplete")]
        public static extern int Printingcomplete(int Timeout);


        [DllImport(REXOD_LIB, SetLastError = true, EntryPoint = "PrinterReboot")]
        public static extern int PrinterReboot();


        #endregion

        #region Constant Declaration
        public const Int32 REXOD_PAPER_EMPTY = 1;
        public const Int32 REXOD_HEAD_UP = 2;
        public const Int32 REXOD_CUT_ERROR = 4;
        public const Int32 REXOD_NEAR_END = 16;
        public const Int32 REXOD_PR_OUT_SENSOR = 64;

        //Text Attribute
        public const Int32 REXOD_PRINTMODE_DEFAULT = 0;
        public const Int32 REXOD_PRINTMODE_FONTB = 1;
        public const Int32 REXOD_PRINTMODE_BOLD = 8;
        public const Int32 REXOD_PRINTMODE_DOUBLE_HEIGHT = 16;
        public const Int32 REXOD_PRINTMODE_DOUBLE_WIDTH = 32;
        public const Int32 REXOD_PRINTMODE_UNDER_LINE = 128;

        //Alignment

        public const Int32 REXOD_ALIGNMENT_LEFT = 0;
        public const Int32 REXOD_ALIGNMENT_CENTER = 1;
        public const Int32 REXOD_ALIGNMENT_RIGHT = 2;

        //Cut Mode
        public const Int32 REXOD_PARTIAL_CUT = 0;
        public const Int32 REXOD_FULL_CUT = 1;


        public const Int32 REXOD_PRINTING_OK = 0;
        public const Int32 REXOD_PRINTING_WRITE_ERROR = -1;
        public const Int32 REXOD_PRINTING_TIMEOUT = -2;
        public const Int32 REXOD_PRINTING_CHECK_ERROR = -3;




        public const Int32 REXOD_BCS_UPCA = 101;
        public const Int32 REXOD_BCS_UPCE = 102;
        public const Int32 REXOD_BCS_EAN13 = 103;
        public const Int32 REXOD_BCS_EAN8 = 104;
        public const Int32 REXOD_BCS_JAN13 = 105;
        public const Int32 REXOD_BCS_JAN8 = 106;

        public const Int32 REXOD_BCS_CODE39 = 107;
        public const Int32 REXOD_BCS_ITF = 108;
        public const Int32 REXOD_BCS_CODABAR = 109;
        public const Int32 REXOD_BCS_CODE93 = 110;
        public const Int32 REXOD_BCS_CODE128 = 111;
        public const Int32 REXOD_BCS_PDF417 = 112;

        //Barcode HRI position
        public const Int32 REXOD_BCS_TEXT_NONE = 0;
        public const Int32 REXOD_BCS_TEXT_ABOVE = 1;
        public const Int32 REXOD_BCS_TEXT_BELOW = 2;
        public const Int32 REXOD_BCS_TEXT_BOTH = 3;

        #endregion
    }
    #endregion
}
