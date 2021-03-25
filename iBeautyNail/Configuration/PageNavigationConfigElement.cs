using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Configuration
{
    public class PageNavigationConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        /// <summary>
        /// next
        /// </summary>
        [ConfigurationProperty("next", IsKey = false, IsRequired = false)]
        public string Next
        {
            get { return (string)base["next"]; }
            set { base["next"] = value; }
        }

        /// <summary>
        /// previous
        /// </summary>
        [ConfigurationProperty("prev", IsKey = false, IsRequired = false)]
        public string Previous
        {
            get { return (string)base["prev"]; }
            set { base["prev"] = value; }
        }

        [ConfigurationProperty("demo", IsKey = true, IsRequired = false, DefaultValue = false)]
        public bool IsDemo
        {
            get { return (bool)base["demo"]; }
            set { base["demo"] = value; }
        }

        /// <summary>
        /// className
        /// </summary>
        [ConfigurationProperty("class", IsKey = false, IsRequired = false)]
        public string Class
        {
            get { return (string)base["class"]; }
            set { base["class"] = value; }
        }

        /// <summary>
        /// prevMethod
        /// </summary>
        [ConfigurationProperty("prevMethod", IsKey = false, IsRequired = false)]
        public string PreviousMethod
        {
            get { return (string)base["prevMethod"]; }
            set { base["prevMethod"] = value; }
        }

        /// <summary>
        /// nextMethod
        /// </summary>
        [ConfigurationProperty("nextMethod", IsKey = false, IsRequired = false)]
        public string NextMethod
        {
            get { return (string)base["nextMethod"]; }
            set { base["nextMethod"] = value; }
        }

        [ConfigurationProperty("timeout", IsRequired = false, IsKey = false)]
        public TimeoutConfigElement Timeout
        {
            get { return (TimeoutConfigElement)this["timeout"]; }
            set { this["timeout"] = value; }
        }

        /// <summary>
        /// validation
        /// </summary>
        [ConfigurationProperty("popup", IsKey = false, IsRequired = false)]
        public PopupConfigElement Popup
        {
            get { return (PopupConfigElement)this["popup"]; }
            set { base["popup"] = value; }
        }
    }
}
