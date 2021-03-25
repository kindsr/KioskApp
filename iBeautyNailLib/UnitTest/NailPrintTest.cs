using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iBeautyNail.Devices.NailPrinter;

namespace iBeautyNail.UnitTest
{
    [TestFixture]
    public class NailPrintTest
    {
        [Test]
        public void Create()
        {
            NailPrinterLib np = new NailPrinterLib();
            np.Create();
            np.Open(callbackPrintStatus, callbackMotorStatus, callbackInkVol);
        }

        NailPrinterLib.PrintStatus callbackPrintStatus =
            (value) =>
            {
                Console.WriteLine("PrintStatus = {0}", value);
                        /*
                        if (myForm.InvokeRequired)
                        {
                            myForm.Invoke(new MethodInvoker(delegate ()
                            {
                                string Msg = string.Format(Msg = "PrintStatus = {0}", value);

                                myForm.textBox1.Text = myForm.textBox1.Text + Environment.NewLine + " >> " + Msg;

                            }));
                        }
                        */

                GC.Collect();
            };

        NailPrinterLib.MotorStatus callbackMotorStatus =
            (value) =>
            {
                Console.WriteLine("MotorStatus = {0}", value);
                GC.Collect();
            };

        NailPrinterLib.InkVol callbackInkVol =
            (id, value) =>
            {
                Console.WriteLine("InkVol[{0}] = {1}", id, value);
                GC.Collect();
            };
    }
}
