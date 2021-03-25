using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Configuration
{
    public class MachineConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public string ID
        {
            get { return (string)base["id"]; }
            set { base["id"] = value; }
        }

        [ConfigurationProperty("defaultLanguage", IsKey = false, IsRequired = true)]
        public string DefaultLanguage
        {
            get { return (string)base["defaultLanguage"]; }
            set { base["defaultLanguage"] = value; }
        }

        [ConfigurationProperty("appversion", IsKey = true, IsRequired = true)]
        public string AppVersion
        {
            get { return (string)base["appversion"]; }
            set { base["appversion"] = value; }
        }
    }
}
