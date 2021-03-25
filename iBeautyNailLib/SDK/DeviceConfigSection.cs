using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.SDK
{
    /// <summary>
    /// DeviceConfigSection configuration
    /// </summary>
    public class DeviceConfigSection : ConfigurationSection
    {
        private static DeviceConfigSection configuration;

        public static DeviceConfigSection Instance
        {
            get
            {
                if (configuration == null)
                {
                    configuration = ConfigurationManager.GetSection("iBeautyNail/devices") as DeviceConfigSection;
                }

                return configuration;
            }
        }

        [ConfigurationProperty("receiptPrinter", IsRequired = false, IsKey = false)]
        public DeviceConfigElement ReceiptPrinter
        {
            get { return (DeviceConfigElement)this["receiptPrinter"]; }
            set { this["receiptPrinter"] = value; }
        }

        [ConfigurationProperty("nailPrinter", IsRequired = false, IsKey = false)]
        public DeviceConfigElement NailPrinter
        {
            get { return (DeviceConfigElement)this["nailPrinter"]; }
            set { this["nailPrinter"] = value; }
        }

        [ConfigurationProperty("payment", IsRequired = false, IsKey = false)]
        public DeviceConfigElement CardPayment
        {
            get { return (DeviceConfigElement)this["payment"]; }
            set { this["payment"] = value; }
        }
    }
}
