using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Extensions
{
    public class SystemCommand
    {

        /// <summary>
        /// 윈도우 종료 command
        /// </summary>
        /// <param name="param"></param>
        public static void ShutDown(string param)
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.FileName = "cmd";
            proc.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Arguments = "/C shutdown " + param;
            Process.Start(proc);
        }
    }
}
