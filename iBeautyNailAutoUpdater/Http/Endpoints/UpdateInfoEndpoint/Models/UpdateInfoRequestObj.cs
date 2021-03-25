using Newtonsoft.Json;
using System;

namespace iBeautyNail.Http.Endpoints.UpdateInfoEndpoint.Models
{
    public class UpdateInfoRequestObj
    {
        [JsonProperty("p_machine_id")]
        public int MachineID { get; set; }

        [JsonProperty("p_file_name")]
        public string FileName { get; set; }

        [JsonProperty("p_file_type")]
        public string FileType { get; set; }

        [JsonProperty("p_last_upd_dt")]
        public DateTimeOffset LastUpdDt { get; set; }

        [JsonProperty("p_version_info")]
        public string VersionInfo { get; set; }

        [JsonProperty("p_file_info_list")]
        public string FileInfoList { get; set; }
    }
}
