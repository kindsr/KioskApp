using iBeautyNail.Http.Caching;
using iBeautyNail.Http.Endpoints.PrintInfoEndPoint.Models;
using iBeautyNail.Http.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Endpoints.PrintInfoEndPoint
{
    public class PrintInfoEndpoint : IPrintInfoEndpoint
    {
        private const string CreatePrintInfoUrl = "/nailpod/createPrintInfo";

        private static IRequester _requester;
        private readonly ICache _cache;

        public PrintInfoEndpoint(IRequester requester, ICache cache)
        {
            _requester = requester;
            _cache = cache;
        }

        public async Task<PrintInfoResponseObj> CreatePrintInfoAsync(PrintInfoRequestObj req)
        {
            var res = new PrintInfoResponseObj();

            var json = await _requester.CreateTestPostRequestAsync(CreatePrintInfoUrl, JsonConvert.SerializeObject(req), null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<PrintInfoResponseObj>(json);
            }

            return res;
        }
    }
}
