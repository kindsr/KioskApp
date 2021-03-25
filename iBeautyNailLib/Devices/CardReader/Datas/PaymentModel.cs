using Newtonsoft.Json;

namespace iBeautyNail.Devices.CardReader.Datas
{
    public class PaymentModel
    {
        internal PaymentModel() { }

        [JsonProperty("SUC")]
        public string Suc { get; set; }

        [JsonProperty("MSG")]
        public string Msg { get; set; }

        [JsonProperty("RQ01")]
        public string Rq01 { get; set; }

        [JsonProperty("RQ02")]
        public string Rq02 { get; set; }

        [JsonProperty("RQ03")]
        public string Rq03 { get; set; }

        [JsonProperty("RQ04")]
        public string Rq04 { get; set; }

        [JsonProperty("RQ05")]
        public string Rq05 { get; set; }

        [JsonProperty("RQ06")]
        public string Rq06 { get; set; }

        [JsonProperty("RQ07")]
        public string Rq07 { get; set; }

        [JsonProperty("RQ08")]
        public string Rq08 { get; set; }

        [JsonProperty("RQ09")]
        public string Rq09 { get; set; }

        [JsonProperty("RQ10")]
        public string Rq10 { get; set; }

        [JsonProperty("RQ11")]
        public string Rq11 { get; set; }

        [JsonProperty("RQ12")]
        public string Rq12 { get; set; }

        [JsonProperty("RQ13")]
        public string Rq13 { get; set; }

        [JsonProperty("RQ14")]
        public string Rq14 { get; set; }

        [JsonProperty("RQ15")]
        public string Rq15 { get; set; }

        [JsonProperty("RQ16")]
        public string Rq16 { get; set; }

        [JsonProperty("RQ17")]
        public string Rq17 { get; set; }

        [JsonProperty("RS01")]
        public string RS01 { get; set; }

        [JsonProperty("RS02")]
        public string RS02 { get; set; }

        [JsonProperty("RS03")]
        public string RS03 { get; set; }

        [JsonProperty("RS04")]
        public string RS04 { get; set; }

        [JsonProperty("RS05")]
        public string RS05 { get; set; }

        [JsonProperty("RS06")]
        public string RS06 { get; set; }

        [JsonProperty("RS07")]
        public string RS07 { get; set; }

        [JsonProperty("RS08")]
        public string RS08 { get; set; }

        [JsonProperty("RS09")]
        public string RS09 { get; set; }

        [JsonProperty("RS10")]
        public string RS10 { get; set; }

        [JsonProperty("RS11")]
        public string RS11 { get; set; }

        [JsonProperty("RS12")]
        public string RS12 { get; set; }

        [JsonProperty("RS13")]
        public string RS13 { get; set; }

        [JsonProperty("RS14")]
        public string RS14 { get; set; }

        [JsonProperty("RS15")]
        public string RS15 { get; set; }

        [JsonProperty("RS16")]
        public string RS16 { get; set; }

        [JsonProperty("RS17")]
        public string RS17 { get; set; }

        [JsonProperty("RS18")]
        public string RS18 { get; set; }

        [JsonProperty("RS19")]
        public string RS19 { get; set; }

        [JsonProperty("RS20")]
        public string RS20 { get; set; }

        [JsonProperty("RS21")]
        public string RS21 { get; set; }

        [JsonProperty("RS22")]
        public string RS22 { get; set; }

        [JsonProperty("RS23")]
        public string RS23 { get; set; }

        [JsonProperty("RS24")]
        public string RS24 { get; set; }

        [JsonProperty("RS25")]
        public string RS25 { get; set; }

        [JsonProperty("RS26")]
        public string RS26 { get; set; }

        [JsonProperty("RS27")]
        public string RS27 { get; set; }

        [JsonProperty("RS28")]
        public string RS28 { get; set; }

        [JsonProperty("RS29")]
        public string RS29 { get; set; }

        [JsonProperty("RS30")]
        public string RS30 { get; set; }

        [JsonProperty("RS31")]
        public string RS31 { get; set; }

        [JsonProperty("RS32")]
        public string RS32 { get; set; }

        [JsonProperty("RS33")]
        public string RS33 { get; set; }

        [JsonProperty("RS34")]
        public string RS34 { get; set; }

        public override string ToString()
        {
            return Rq01 + "^" + Rq02 + "^" + Rq03 + "^" + Rq04 + "^" + Rq05 + "^" + Rq06 + "^" + Rq07 + "^" + Rq08 + "^" +
                   Rq09 + "^" + Rq10 + "^" + Rq11 + "^" + Rq12 + "^" + Rq13 + "^" + Rq14 + "^" + Rq15 + "^" + Rq16 + "^" +
                   Rq17 + "^";
        }
    }
}
