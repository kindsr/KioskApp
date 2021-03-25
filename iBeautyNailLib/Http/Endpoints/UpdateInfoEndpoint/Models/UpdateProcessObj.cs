using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace iBeautyNail.Http.Endpoints.UpdateInfoEndpoint.Models
{
    public class UpdateProcessObj
    {
        [JsonProperty("machine_id")]
        public int MachineID { get; set; }

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("file_type")]
        public string FileType { get; set; }

        [JsonProperty("last_upd_dt")]
        public DateTimeOffset? LastUpdDt { get; set; }

        [JsonProperty("version_info")]
        public string VersionInfo { get; set; }

        [JsonProperty("reg_dt")]
        public DateTimeOffset? RegDt { get; set; }
    }
}
