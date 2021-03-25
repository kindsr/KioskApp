using iBeautyNail.Http.Endpoints.ImageEndpoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Interfaces
{
    public interface IImageListEndpoint
    {
        Task<ImageListResponseObj> GetImageListTokenAsync();
        Task<ImageListResponseObj> GetImageListTokenAsync(string token);
    }
}
