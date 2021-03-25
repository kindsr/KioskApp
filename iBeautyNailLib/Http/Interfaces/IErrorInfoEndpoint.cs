using iBeautyNail.Http.Endpoints.ErrorInfoEndpoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Interfaces
{
    public interface IErrorInfoEndpoint
    {
        Task<ErrorInfoResponseObj> CreateErrorInfoAsync(ErrorInfoRequestObj reqErrorInfo);
    }
}
