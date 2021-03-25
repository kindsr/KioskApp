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
    public class ImageEndpoint : IImageEndpoint
    {
        private const string ImageRootUrl = "/rest/admin/v1/getImage/{0}/{1}";

        private static IRequester _requester;
        private readonly ICache _cache;

        /// <inheritdoc />
        public ImageEndpoint(IRequester requester, ICache cache)
        {
            _requester = requester;
            _cache = cache;
        }

        public async Task<ImageResponseObj> GetImageAsync(string username, int idx)
        {
            var resImage = new ImageResponseObj();

            var json = await _requester.CreateGetRequestAsync(string.Format(ImageRootUrl, username, idx), null, false);

            if (json != null)
            {
                resImage = JsonConvert.DeserializeObject<ImageResponseObj>(json);
            }

            return resImage;
        }
    }
}
