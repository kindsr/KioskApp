using Newtonsoft.Json;
using System.Collections.Generic;

namespace iBeautyNail.Http.Endpoints.PaymentInfoEndpoint.Models
{
    public class PaymentInfoResponseObj
    {
        [JsonProperty("o_result")]
        public int Result { get; set; }

        [JsonProperty("o_msg")]
        public string Msg { get; set; }
    }
}
