using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Extensions
{
    public static class StringHelper
    {
        public static string PrintBytes(this byte[] byteArray)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < byteArray.Length; i++)
            {
                var b = byteArray[i];
                sb.Append(b);
                if (i < byteArray.Length - 1)
                {
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }
    }
}
