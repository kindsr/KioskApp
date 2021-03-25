using Newtonsoft.Json;
using System.Collections.Generic;

namespace iBeautyNail.Http.Endpoints.ImageEndpoint.Models
{
    public class SaveImageRequestObj
    {
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
    }
}
