using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace iBeautyNail.Devices.ReceiptPrinter
{
    public class HwaLib
    {
        // Dll Name.
        public const string HWA_LIB = "HwaUSB.dll";
        public HwaLib()
        { }

        #region Method Declaration
        [DllImport(HWA_LIB)]
        public static extern int UsbOpen(string ModelName);
        
        [DllImport(HWA_LIB)]
        public static extern int PrintStr(string Str);
        
        [DllImport(HWA_LIB)]
        public static extern int PrintCmd(short data);
        
        [DllImport(HWA_LIB)]
        public static extern int NewRealRead();
        
        [DllImport(HWA_LIB)]
        public static extern void UsbClose();
        #endregion

        public const Int32 HWA_PRINTING_OK = 0;
        public const Int32 HWA_PAPEROUT = 1;
        public const Int32 HWA_HEADOPEN = 2;
        public const Int32 HWA_PAPEROUT_HEADOPEN = 3;
        public const Int32 HWA_PAPERJAM = 4;
        public const Int32 HWA_PAPEROUT_PAPERJAM = 5;
        public const Int32 HWA_HEADOPEN_PAPERJAM = 6;
        public const Int32 HWA_PAPEROUT_HEADOPEN_PAPERJAM = 7;
        public const Int32 HWA_NEAREND = 8;
        public const Int32 HWA_PAPEROUT_NEAREND = 9;
        public const Int32 HWA_HEADOPEN_NEAREND = 10;
        public const Int32 HWA_PAPEROUT_HEADOPEN_NEAREND = 11;
        public const Int32 HWA_PAPERJAM_NEAREND = 12;
        public const Int32 HWA_PAPEROUT_PAPERJAM_NEAREND = 13;
        public const Int32 HWA_HEADOPEN_PAPERJAM_NEAREND = 14;
        public const Int32 HWA_PAPEROUT_HEADOPEN_PAPERJAM_NEAREND = 15;
        public const Int32 HWA_PRINTING = 16;
        public const Int32 HWA_CUTTERJAM = 32;
    }
}
