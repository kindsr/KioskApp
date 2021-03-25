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
    public class SaveImageEndpoint : ISaveImageEndpoint
    {
        private const string SaveImageRootUrl = "/rest/v1/saveImage";

        private static IRequester _requester;
        private readonly ICache _cache;

        /// <inheritdoc />
        public SaveImageEndpoint(IRequester requester, ICache cache)
        {
            _requester = requester;
            _cache = cache;
        }

        public async Task<ResponseObj> SaveImageAsync(SaveImageRequestObj req)
        {
            var res = new ResponseObj();

            var json = await _requester.CreatePostRequestAsync(SaveImageRootUrl, JsonConvert.SerializeObject(req), null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<ResponseObj>(json);
            }

            return res;
        }
    }
}
