using iBeautyNail.Http.Caching;
using iBeautyNail.Http.Endpoints.ImageEndpoint.Models;
using iBeautyNail.Http.Endpoints.PaymentInfoEndpoint.Models;
using iBeautyNail.Http.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Endpoints.PaymentInfoEndpoint
{
    public class PaymentInfoEndpoint : IPaymentInfoEndpoint
    {
        private const string CreatePaymentInfoUrl = "/nailpod/createPaymentInfo";
        private const string UpdateCancelPaymentUrl = "/nailpod/updateCancelPayment";

        private static IRequester _requester;
        private readonly ICache _cache;

        public PaymentInfoEndpoint(IRequester requester, ICache cache)
        {
            _requester = requester;
            _cache = cache;
        }

        public async Task<PaymentInfoResponseObj> CreatePaymentInfoAsync(PaymentInfoRequestObj req)
        {
            var res = new PaymentInfoResponseObj();

            var json = await _requester.CreateTestPostRequestAsync(CreatePaymentInfoUrl, JsonConvert.SerializeObject(req), null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<PaymentInfoResponseObj>(json);
            }

            return res;
        }

        public async Task<PaymentInfoResponseObj> UpdateCancelPaymentAsync(PaymentInfoRequestObj req)
        {
            var res = new PaymentInfoResponseObj();

            var json = await _requester.CreateTestPostRequestAsync(UpdateCancelPaymentUrl, JsonConvert.SerializeObject(req), null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<PaymentInfoResponseObj>(json);
            }

            return res;
        }
    }
}
