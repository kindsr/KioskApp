using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iBeautyNail.Extensions.Attributes
{
    public class TimeoutAttribute : Attribute
    {
        public int Milliseconds { get; private set; }

        public bool UseConfig { get; private set; }

        public TimeoutAttribute(bool useConfig = true)
        {
            UseConfig = useConfig;
        }

        public TimeoutAttribute(int milliseconds) : this(false)
        {
            this.Milliseconds = milliseconds;
        }
    }
}
