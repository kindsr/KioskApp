using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.SDK
{
    public class DeviceConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("enable", IsKey = false, IsRequired = true)]
        public bool Enable
        {
            get { return (bool)base["enable"]; }
            set { base["enable"] = value; }
        }

        [ConfigurationProperty("useHistory", IsKey = false, IsRequired = false, DefaultValue = false)]
        public bool UseHistory
        {
            get { return (bool)base["useHistory"]; }
            set { base["useHistory"] = value; }
        }

        [ConfigurationProperty("variables", IsRequired = false)]
        public KeyValueConfigurationCollection Variables
        {
            get { return (KeyValueConfigurationCollection)this["variables"]; }
            set { this["variables"] = value; }
        }
    }
}
