using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Payment
{
    class FirstData
    {
        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Status", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Status();

        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Create", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Create(string p_pszServer, string p_pszPort);

        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Destroy", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Destroy();

        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_MkTranId", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_MkTranId(byte[] p_pszOutValueBuffer, int p_inOutValueBufferLen);

        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Init", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Init(string p_pszTranId, string p_pszMsgCmd, string p_pszMsgCert);

        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Term", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Term();

        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Input", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Input(byte[] p_pszKey, string p_pszVal);

        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Input", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Input_char(char[] p_pszKey, string p_pszVal);

        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Execute", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Execute();

        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Output", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Output(string p_pszKey, byte[] p_pszOutValueBuffer, int p_inOutValueBufferLen);

        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_GetNotifyMsg", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_GetNotifyMsg(byte[] p_pszOutNotifyMsgBuffer, int p_inOutNotifyMsgBufferLen);


        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_UserStop", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_UserStop();

        [DllImport(@"Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Check", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Check(string p_pszServer, string p_pszPort);

    }
}
