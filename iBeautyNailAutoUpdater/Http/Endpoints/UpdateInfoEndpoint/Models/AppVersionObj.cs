using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace iBeautyNail.Http.Endpoints.UpdateInfoEndpoint.Models
{
    public class AppVersionObj
    {
        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
