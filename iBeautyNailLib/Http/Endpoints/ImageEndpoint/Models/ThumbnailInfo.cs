using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Endpoints.ImageEndpoint.Models
{
    public class ThumbnailInfo
    {
        internal ThumbnailInfo() { }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("dtCreated")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("dtUploaded")]
        public DateTime UploadedDate { get; set; }

        [JsonProperty("dtUpdated")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("idx")]
        public int Index { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
