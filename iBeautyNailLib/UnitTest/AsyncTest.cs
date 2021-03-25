using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.UnitTest
{
    [TestFixture()]
    public class AsyncTest
    {
        private async Task Wait(TimeSpan timeSpan)
        {
            await Task.Delay(timeSpan);
        }

        [Test]
        [Order(0)]
        public void WaitTest()
        {
            Task task = Wait(new TimeSpan(0, 0, 0, 0, 5000));
            task.Wait();
        }
    }
}
