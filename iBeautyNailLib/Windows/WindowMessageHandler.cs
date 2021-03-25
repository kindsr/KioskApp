using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Windows
{
    public class WindowMessageHandler
    {
        private IntPtr hWnd;
        public IntPtr WindowHandle
        {
            get
            {
                return hWnd;
            }
            private set
            {
                hWnd = value;
            }
        }

        public WindowMessageHandler(IntPtr hWnd)
        {
            this.hWnd = hWnd;
        }

        public IntPtr SendMessage(int msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr result = (IntPtr)0;

            if (hWnd != null)
            {
                result = Win32API.SendMessage(hWnd, msg, wParam, lParam);
            }

            return result;
        }

        public IntPtr SendMessage(int msg, IntPtr wparam, object lparamObj)
        {
            GCHandle gch = GCHandle.Alloc(lparamObj);
            IntPtr lparam = GCHandle.ToIntPtr(gch);

            IntPtr result = SendMessage(msg, wparam, lparam);

            gch.Free();

            return result;
        }

        public static IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr result = (IntPtr)0;

            if (hWnd != null)
            {
                result = Win32API.SendMessage(hWnd, msg, wParam, lParam);
            }

            return result;
        }

        public static IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wparam, object lparamObj)
        {
            GCHandle gch = GCHandle.Alloc(lparamObj);
            IntPtr lparam = GCHandle.ToIntPtr(gch);

            IntPtr result = WindowMessageHandler.SendMessage(hWnd, msg, wparam, lparam);

            gch.Free();

            return result;
        }

    }
}
