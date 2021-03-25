using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Endpoints.ImageEndpoint.Models
{
    public class QRImageDataTokenInfo
    {
        internal QRImageDataTokenInfo() { }

        [JsonProperty("dtCreated")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("dtExpired")]
        public DateTime ExpiredDate { get; set; }

        [JsonProperty("dtUpdated")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("token_type")]
        public string Token_Type { get; set; }

        [JsonProperty("idx")]
        public int Index { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
