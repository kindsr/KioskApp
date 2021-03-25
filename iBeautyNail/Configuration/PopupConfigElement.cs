using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Configuration
{
    public class PopupConfigElement : ConfigurationElement
    {
        /// <summary>
        /// button0에서 이동할 페이지
        /// </summary>
        [ConfigurationProperty("button0", IsKey = false, IsRequired = false)]
        public string Button0
        {
            get { return (string)base["button0"]; }
            set { base["button0"] = value; }
        }

        /// <summary>
        /// button1에서 이동할 페이지
        /// </summary>
        [ConfigurationProperty("button1", IsKey = false, IsRequired = false)]
        public string Button1
        {
            get { return (string)base["button1"]; }
            set { base["button1"] = value; }
        }

        /// <summary>
        /// button2에서 이동할 페이지
        /// </summary>
        [ConfigurationProperty("button2", IsKey = false, IsRequired = false)]
        public string Button2
        {
            get { return (string)base["button2"]; }
            set { base["button2"] = value; }
        }

        /// <summary>
        /// popup time
        /// </summary>
        [ConfigurationProperty("popuptime", IsKey = false, IsRequired = false)]
        public string Popuptime
        {
            get { return (string)base["popuptime"]; }
            set { base["popuptime"] = value; }
        }

        /// <summary>
        /// popup의 버튼 갯수 설정(1 = 버튼1개, 2 = 버튼 2개, 3 = 버튼 3개)
        /// </summary>
        [ConfigurationProperty("quantity", IsKey = false, IsRequired = false)]
        public string Quantity
        {
            get { return (string)base["quantity"]; }
            set { base["quantity"] = value; }
        }
    }
}
