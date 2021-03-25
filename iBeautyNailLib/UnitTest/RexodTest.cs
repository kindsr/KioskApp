using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iBeautyNail.Devices.ReceiptPrinter;

namespace iBeautyNail.UnitTest
{
    [TestFixture]
    public class RexodTest
    {
        [Test]
        public void PortOpen()
        {
            REXODLib receiptPrinter = new REXODLib();

            string sComm = "";
            int nRes = -1;
            int nBaud_rate = 0;
            int nFlow_control = 0;

            sComm = "USB";
            nBaud_rate = 115200;
            nFlow_control = 1;
            nRes = REXODLib.OpenPortW(sComm, nBaud_rate, nFlow_control);
            Console.WriteLine("ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁCONNECTEDㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ " + sComm);

            Assert.That(nRes, Is.EqualTo((int)0));
            if (nRes != 0)
            {
                Console.WriteLine("Printer Connect failed. ERORR");
                return;
            }

        }
    }
}
