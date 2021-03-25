using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Interface
{
    public interface INailPrinter : IPrinter
    {
        /// <summary>
        /// MotorOn
        /// </summary>
        void MotorOn();

        /// <summary>
        /// MotorOff
        /// </summary>
        void MotorOff();
    }
}
