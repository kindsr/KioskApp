using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace iBeautyNail.Http.Endpoints.MonitoringInfoEndpoint.Models
{
    public class MonitoringInfoObj
    {
        [JsonProperty("machine_id")]
        public int MachineID { get; set; }

        [JsonProperty("prog_status")]
        public string ProgStatus { get; set; }

        [JsonProperty("uv_status")]
        public string UvStatus { get; set; }

        [JsonProperty("ink_status")]
        public string InkStatus { get; set; }

        [JsonProperty("motor_status")]
        public string MotorStatus { get; set; }

        [JsonProperty("epson_printer_status")]
        public string EpsonPrinterStatus { get; set; }

        [JsonProperty("receipt_printer_status")]
        public string ReceiptPrinterStatus { get; set; }

        [JsonProperty("print_cnt")]
        public int PrintCnt { get; set; }

        [JsonProperty("ink_remain_c")]
        public string InkRemainC { get; set; }

        [JsonProperty("ink_remain_m")]
        public string InkRemainM { get; set; }

        [JsonProperty("ink_remain_y")]
        public string InkRemainY { get; set; }

        [JsonProperty("ink_remain_k")]
        public string InkRemainK { get; set; }

        [JsonProperty("ink_remain_w")]
        public string InkRemainW { get; set; }

        [JsonProperty("hdd_used")]
        public string HddUsed { get; set; }

        [JsonProperty("ram_used")]
        public string RamUsed { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("reg_dt")]
        public DateTimeOffset? RegDt { get; set; }

        [JsonProperty("upd_dt")]
        public DateTimeOffset? UpdDt { get; set; }
    }
}
