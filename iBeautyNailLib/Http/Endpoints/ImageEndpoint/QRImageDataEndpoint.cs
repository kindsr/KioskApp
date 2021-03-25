using iBeautyNail.Http.Caching;
using iBeautyNail.Http.Endpoints.ImageEndpoint.Models;
using iBeautyNail.Http.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Endpoints.ImageEndpoint
{
    public class QRImageDataEndpoint : IQRImageDataEndpoint
    {
        private const string ImageListRootUrl = "/rest/v1/QRImageData/{0}";

        private static IRequester _requester;
        private readonly ICache _cache;

        /// <inheritdoc />
        public QRImageDataEndpoint(IRequester requester, ICache cache)
        {
            _requester = requester;
            _cache = cache;
        }

        public async Task<QRImageDataResponseObj> QRImageDataTokenAsync(int idx)
        {
            var resImage = new QRImageDataResponseObj();

            var json = await _requester.CreateGetRequestAsync(string.Format(ImageListRootUrl, idx), null, false);

            if (json != null)
            {
                resImage = JsonConvert.DeserializeObject<QRImageDataResponseObj>(json);
            }

            return resImage;
        }
    }
}
