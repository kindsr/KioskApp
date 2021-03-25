using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Configuration
{
    public class PageNavigationConfigSection : ConfigurationSection
    {
        private static PageNavigationConfigSection configuration;

        public static PageNavigationConfigSection Instance
        {
            get
            {
                configuration = ConfigurationManager.GetSection("iBeautyNail/pageNavigation") as PageNavigationConfigSection;

                if (configuration != null)
                    return configuration;

                return new PageNavigationConfigSection();
            }
        }

        [ConfigurationProperty("pages", IsRequired = true, IsKey = false)]
        public PageNavigationConfigCollection Page
        {
            get { return (PageNavigationConfigCollection)this["pages"]; }
            set { this["pages"] = value; }
        }

        [ConfigurationProperty("defaultTimeout", IsRequired = true, IsKey = false)]
        public TimeoutConfigElement DefaultTimeout
        {
            get { return (TimeoutConfigElement)this["defaultTimeout"]; }
            set { this["defaultTimeout"] = value; }
        }
        
    }
}
