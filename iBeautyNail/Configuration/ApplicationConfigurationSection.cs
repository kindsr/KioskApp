using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Configuration
{
    public class ApplicationConfigurationSection : ConfigurationSection
    {
        private static ApplicationConfigurationSection configuration;

        public static ApplicationConfigurationSection Instance
        {
            get
            {
                if (configuration == null)
                {
                    configuration = ConfigurationManager.GetSection("iBeautyNail/application") as ApplicationConfigurationSection;
                }

                return configuration;
            }
        }

        [ConfigurationProperty("machine", IsRequired = false, IsKey = false)]
        public MachineConfigElement Machine
        {
            get { return (MachineConfigElement)this["machine"]; }
            set { this["machine"] = value; }
        }

        [ConfigurationProperty("directories", IsRequired = false)]
        public KeyValueConfigurationCollection Directories
        {
            get { return (KeyValueConfigurationCollection)this["directories"]; }
            set { this["directories"] = value; }
        }
    }
}