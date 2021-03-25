using Newtonsoft.Json;
using System;
using System.IO;

namespace iBeautyNailAutoUpdater
{
    public class FileInfoModel
    {
        [JsonProperty("filename")]
        public string FileName { get; set; }

        [JsonProperty("lastwritetime")]
        public DateTime LastWriteTime { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
