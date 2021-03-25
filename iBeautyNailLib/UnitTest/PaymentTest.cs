using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iBeautyNail.Devices.ReceiptPrinter;
using WinHttp;
using System.Security.Policy;
using iBeautyNail.Devices.CardReader.Datas;
using Newtonsoft.Json.Linq;
using iBeautyNail.Devices.CardReader;

namespace iBeautyNail.UnitTest
{
    [TestFixture]
    public class PaymentTest
    {
        [Test]
        public void PortOpen()
        {
            string url = "http://127.0.0.1:8090/";
            string cb = "callback=jsonp12345678983543344";
            string cbValue = "jsonp12345678983543344";

            PaymentModel pm = new PaymentModel();
            pm.Rq01 = "D1";
            pm.Rq03 = "1004";
            pm.Rq04 = "00";
            pm.Rq12 = "30";
            pm.Rq13 = "A";


            string req = "REQ=" + pm.ToString();

            WinHttpRequest winHttp = new WinHttpRequest();
            winHttp.Open("POST", url + "?" + cb + "&" + req, true); //뒤에 bool값이 동기/비동기방식을 나타냄(Async)
            winHttp.Send(""); // string.Format("callback={0}&REQ=CC", "jsonp12345678983543344"));
            winHttp.WaitForResponse(120000); //120초후 timeout
            object resp = winHttp.ResponseBody;
            string encStr = Encoding.Default.GetString((Byte[])resp);

            Console.WriteLine("ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁResponseㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ \n" + encStr);

            var json = JObject.Parse(encStr.Replace(cbValue + "(", "").Replace(")", ""));
        }

        [Test]
        public void Pay()
        {
            string url = "http://127.0.0.1:8090/";
            string cb = "callback=jsonp12345678983543344";
            string cbValue = "jsonp12345678983543344";

            KKaoPayModel km = new KKaoPayModel();
            km.S01 = "EX";
            km.S02 = "D1";
            km.S03 = " ";
            km.S04 = "40";
            km.S05 = "0700081";
            km.S06 = "B L";
            km.S08 = "B";
            km.S09 = "281006024188771830854621";
            km.S11 = "00";
            km.S12 = "1004";
            km.S18 = "91";
            km.S19 = "N";
            km.S34 = "KKAO=KS#";


            string req = km.ToString();

            WinHttpRequest winHttp = new WinHttpRequest();
            winHttp.Open("POST", url + "?" + cb + "&" + req, true); //뒤에 bool값이 동기/비동기방식을 나타냄(Async)
            winHttp.Send(""); // string.Format("callback={0}&REQ=CC", "jsonp12345678983543344"));
            winHttp.WaitForResponse(120000); //120초후 timeout
            object resp = winHttp.ResponseBody;
            string encStr = Encoding.Default.GetString((Byte[])resp);

            Console.WriteLine("ㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁResponseㅁㅁㅁㅁㅁㅁㅁㅁㅁㅁ \n" + encStr);

            var json = JObject.Parse(encStr.Replace(cbValue + "(", "").Replace(")", ""));
        }

        [Test]
        public void Status()
        {
            string url = "http://127.0.0.1:8090/";
            string cb = "callback=jsonp12345678983543344";
            string cbValue = "jsonp12345678983543344";

            WinHttpRequest winHttp = new WinHttpRequest();
            winHttp.Open("POST", url, true);
            winHttp.Send(string.Format("callback={0}&REQ=CR^FB^11^32^01^", cbValue));
            winHttp.WaitForResponse(60000); //60초후 timeout
            object resp = winHttp.ResponseBody;
            string encStr = Encoding.Default.GetString((Byte[])resp);
        }

    }
}
