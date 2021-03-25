using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Endpoints.ImageEndpoint.Models
{
    public class ResponseObj
    {
        internal ResponseObj() { }

        [JsonProperty("result")]
        public int Result { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("datas")]
        public List<DatasInfo> Datas { get; set; }
    }
}
