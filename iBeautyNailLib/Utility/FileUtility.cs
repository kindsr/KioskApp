using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Utility
{
    public class FileUtility
    {
        #region SINGLETON
        private static FileUtility _instance;
        private static object lockObj = new object();
        public static FileUtility Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (_instance == null)
                        _instance = new FileUtility();
                }
                return _instance;
            }
        }
        #endregion SINGLETON

        private FileUtility()
        {

        }

        public List<string> DirSearch(string sDir, bool subSearch = true, bool dirOnly = false)
        {
            List<string> dirList = new List<string>();

            try
            {
                foreach (var directory in Directory.GetDirectories(sDir))
                {
                    dirList.Add(directory);

                    if (!dirOnly)
                    {
                        foreach (var filename in Directory.GetFiles(directory))
                        {
                            dirList.Add(filename);
                        }
                    }
                    
                    if (subSearch)
                        DirSearch(directory);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dirList;
        }

        public void RunTabTip()
        {
            string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
            string keyboardPath = Path.Combine(progFiles, "TabTip.exe");
            Process.Start(keyboardPath);
        }

        public void CloseTabTip()
        {
            Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
            foreach (Process p in oskProcessArray)
            {
                p.Kill();
            }
        }
    }
}
