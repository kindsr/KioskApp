using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace iBeautyNail.Extensions.Converters
{
    class DataConverter
    {
        public static T CastType<T>(IntPtr lParam)
        {
            GCHandle gch = GCHandle.FromIntPtr(lParam);
            return (T)gch.Target;
        }

        public static string String2md5(string s)
        {
            try
            {
                // calculate MD5
                MD5 m = MD5.Create();
                byte[] b = Encoding.ASCII.GetBytes(s);
                byte[] h = m.ComputeHash(b);

                // byte to hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < h.Length; i++) sb.Append(h[i].ToString("X2"));

                return sb.ToString();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
