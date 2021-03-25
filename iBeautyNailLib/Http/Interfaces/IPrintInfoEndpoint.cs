using iBeautyNail.Http.Endpoints.PrintInfoEndPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Interfaces
{
    public interface IPrintInfoEndpoint
    {
        Task<PrintInfoResponseObj> CreatePrintInfoAsync(PrintInfoRequestObj req);
    }
}
