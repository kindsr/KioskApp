using iBeautyNail.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace iBeautyNail.SDK.Device.Printer
{
    public class ReceiptPrinter : IPrinter
    {
        [Dependency("IReceiptPrinter")]
        public IPrinter Printer { get; set; }

        public int Connect()
        {
            return Printer.Connect();
        }

        public int Disconnect()
        {
            return Printer.Disconnect();
        }

        public void Initialize()
        {
            Printer.Initialize();
        }

        public bool IsConnected()
        {
            return Printer.IsConnected();
        }

        public int Print(object printData, bool backupStatus = true)
        {
            return Printer.Print(printData, backupStatus);
        }

        public int State()
        {
            return Printer.State();
        }
    }
}
