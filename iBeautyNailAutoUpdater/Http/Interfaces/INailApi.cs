using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Interfaces
{
    public interface INailApi
    {
        IUpdateInfoEndpoint UpdateInfo { get; }
    }
}
