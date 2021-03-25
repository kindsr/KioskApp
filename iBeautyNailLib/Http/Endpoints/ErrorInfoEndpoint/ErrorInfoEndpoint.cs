using iBeautyNail.Http.Caching;
using iBeautyNail.Http.Endpoints.ErrorInfoEndpoint.Models;
using iBeautyNail.Http.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Endpoints.ErrorInfoEndpoint
{
    public class ErrorInfoEndpoint : IErrorInfoEndpoint
    {
        private const string CreatePaymentInfoUrl = "/nailpod/createErrorInfo";

        private static IRequester _requester;
        private readonly ICache _cache;

        public ErrorInfoEndpoint(IRequester requester, ICache cache)
        {
            _requester = requester;
            _cache = cache;
        }

        public async Task<ErrorInfoResponseObj> CreateErrorInfoAsync(ErrorInfoRequestObj req)
        {
            var res = new ErrorInfoResponseObj();

            var json = await _requester.CreateTestPostRequestAsync(CreatePaymentInfoUrl, JsonConvert.SerializeObject(req), null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<ErrorInfoResponseObj>(json);
            }

            return res;
        }
    }
}
