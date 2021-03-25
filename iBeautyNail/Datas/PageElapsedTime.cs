using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Datas
{
    public class PageElapsedTime
    {
        public PageElapsedTime()
        {
            times = new Dictionary<string, Stopwatch>();
        }

        private Dictionary<string, Stopwatch> times;

        public long TotalTime
        {
            get { return times.Sum( s => s.Value.ElapsedMilliseconds); }
        }

        public long this[string viewName]
        {
            get
            {
                return times.ContainsKey(viewName) ? times[viewName].ElapsedMilliseconds : 0L;
            }
            set
            {
                StartElapsedTime(viewName);
            }
        }

        public void StartElapsedTime(string viewName)
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            if (times.ContainsKey(viewName))
            {
                times[viewName] = elapsedTime;
            }
            else
            {
                times.Add(viewName, elapsedTime);
            }
        }

        public void StopElapsedTime(string viewName)
        {
            if (times.ContainsKey(viewName))
                times[viewName].Stop();
        }

        public void Clear()
        {
            times.Clear();
        }
    }
}
