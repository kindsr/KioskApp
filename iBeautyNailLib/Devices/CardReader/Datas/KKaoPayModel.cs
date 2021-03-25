using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Devices.CardReader.Datas
{
    class KKaoPayModel
    {
        internal KKaoPayModel() { }

        [JsonProperty("SUC")]
        public string Suc { get; set; }

        [JsonProperty("MSG")]
        public string Msg { get; set; }

        [JsonProperty("S01")]
        public string S01 { get; set; }

        [JsonProperty("S02")]
        public string S02 { get; set; }

        [JsonProperty("S03")]
        public string S03 { get; set; }

        [JsonProperty("S04")]
        public string S04 { get; set; }

        [JsonProperty("S05")]
        public string S05 { get; set; }

        [JsonProperty("S06")]
        public string S06 { get; set; }

        //[JsonProperty("S07")]
        //public string S07 { get; set; }

        [JsonProperty("S08")]
        public string S08 { get; set; }

        [JsonProperty("S09")]
        public string S09 { get; set; }

        //[JsonProperty("S10")]
        //public string S10 { get; set; }

        [JsonProperty("S11")]
        public string S11 { get; set; }

        [JsonProperty("S12")]
        public string S12 { get; set; }

        //[JsonProperty("S13")]
        //public string S13 { get; set; }

        [JsonProperty("S14")]
        public string S14 { get; set; }

        [JsonProperty("S15")]
        public string S15 { get; set; }

        //[JsonProperty("S16")]
        //public string S16 { get; set; }

        [JsonProperty("S17")]
        public string S17 { get; set; }

        [JsonProperty("S18")]
        public string S18 { get; set; }

        [JsonProperty("S19")]
        public string S19 { get; set; }

        [JsonProperty("S30")]
        public string S30 { get; set; }

        [JsonProperty("S34")]
        public string S34 { get; set; }

        [JsonProperty("R01")]
        public string R01 { get; set; }

        [JsonProperty("R02")]
        public string R02 { get; set; }

        [JsonProperty("R03")]
        public string R03 { get; set; }

        [JsonProperty("R04")]
        public string R04 { get; set; }

        [JsonProperty("R05")]
        public string R05 { get; set; }

        [JsonProperty("R06")]
        public string R06 { get; set; }

        [JsonProperty("R07")]
        public string R07 { get; set; }

        [JsonProperty("R08")]
        public string R08 { get; set; }

        [JsonProperty("R09")]
        public string R09 { get; set; }

        [JsonProperty("R10")]
        public string R10 { get; set; }

        [JsonProperty("R11")]
        public string R11 { get; set; }

        [JsonProperty("R12")]
        public string R12 { get; set; }

        [JsonProperty("R13")]
        public string R13 { get; set; }

        [JsonProperty("R14")]
        public string R14 { get; set; }

        [JsonProperty("R15")]
        public string R15 { get; set; }

        [JsonProperty("R16")]
        public string R16 { get; set; }

        [JsonProperty("R17")]
        public string R17 { get; set; }

        [JsonProperty("R18")]
        public string R18 { get; set; }

        [JsonProperty("R19")]
        public string R19 { get; set; }

        [JsonProperty("R20")]
        public string R20 { get; set; }

        [JsonProperty("R21")]
        public string R21 { get; set; }

        [JsonProperty("R22")]
        public string R22 { get; set; }

        [JsonProperty("R23")]
        public string R23 { get; set; }

        [JsonProperty("R24")]
        public string R24 { get; set; }

        public override string ToString()
        {
            return "S01=" + S01 + ";" + "S02=" + S02 + ";" + "S03=" + S03 + ";" + "S04=" + S04 + ";" + "S05=" + S05 + ";" + "S06=" + S06 + ";" + "S08=" + S08 + ";" +
                   "S09=" + S09 + ";" + "S11=" + S11 + ";" + "S12=" + S12 + ";" + "S14=" + S14 + ";" + "S15=" + S15 + ";" + "S17=" + S17 + ";" + "S18=" + S18 + ";" +
                   "S19=" + S19 + ";" + "S30=" + S30 + ";" + "S34=" + S34 + ";";
        }
    }
}
