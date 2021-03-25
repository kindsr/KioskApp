using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Configuration
{
    public class TimeoutConfigElement : ConfigurationElement
    {
        /// <summary>
        /// 타임아웃 사용여부
        /// </summary>
        [ConfigurationProperty("enable", IsKey = false, IsRequired = false, DefaultValue = true)]
        public bool Enable
        {
            get { return (bool)base["enable"]; }
            set { base["enable"] = value; }
        }

        /// <summary>
        /// 타임아웃 시간 (seconds)
        /// </summary>
        [ConfigurationProperty("timeout", IsKey = false, IsRequired = false)]
        public int Timeout
        {
            get { return (int)base["timeout"]; }
            set { base["timeout"] = value; }
        }
        
        /// <summary>
        /// 타임아웃 발생시 이동할 페이지
        /// </summary>
        [ConfigurationProperty("page", IsKey = false, IsRequired = false)]
        public string Page
        {
            get { return (string)base["page"]; }
            set { base["page"] = value; }
        }
    }
}
