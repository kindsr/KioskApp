using iBeautyNail.Devices.CardReader.Datas;
using iBeautyNail.Interface;
using iBeautyNail.SDK;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using Unity;
using WinHttp;

namespace iBeautyNail.Devices.CardReader
{
    internal interface IEasyCheckCardReader : IPayment
    {
        string Url { get; set; }
        string Method { get; set; }
        string Callback { get; set; }
    }

    public class EasyCheckCardReader : IEasyCheckCardReader
    {
        protected readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string url;
        [Dependency("Url")]
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private string method;
        [Dependency("Method")]
        public string Method
        {
            get { return method; }
            set { method = value; }
        }

        private string callback;
        [Dependency("Callback")]
        public string Callback
        {
            get { return callback; }
            set { callback = value; }
        }

        public int Connect()
        {
            WinHttpRequest winHttp = new WinHttpRequest();

            try
            {
                winHttp.Open(Method, Url, true);
                winHttp.Send(string.Format("callback={0}&REQ=CC", Callback));
                winHttp.WaitForResponse(60000); //60초후 timeout
                object resp = winHttp.ResponseBody;
                logger.Debug("Payment Connect OK!");
                return SDKManagerStatusCode.Success;
            }
            catch (Exception ex)
            {
                logger.DebugFormat("Payment Connect Exception Error! :: {0} :: {1}", url, ex.ToString());
                return CardPaymentWParamType.Failure;
            }
        }

        public int Disconnect()
        {
            return SDKManagerStatusCode.Success;
        }

        public void Initialize()
        {

        }

        public bool IsConnected()
        {
            return true;
        }

        public int State()
        {
            WinHttpRequest winHttp = new WinHttpRequest();

            try
            {
                winHttp.Open(Method, Url, true);
                winHttp.Send(string.Format("callback={0}&REQ=CR^FB^11^32^01^", Callback));
                winHttp.WaitForResponse(60000); //60초후 timeout
                object resp = winHttp.ResponseBody;
                string encStr = Encoding.Default.GetString((Byte[])resp);
                var json = JObject.Parse(encStr.Replace(Callback + "(", "").Replace(")", ""));

                if (json["SUC"].ToString() == "00" && json.ContainsKey("RDATA"))
                {
                    if (json["RDATA"].ToString() == "010")
                        return CardPaymentWParamType.CardInserted;
                    else if (json["RDATA"].ToString() == "011")
                        return CardPaymentWParamType.CardEmpty;
                }

                return SDKManagerStatusCode.Success;
            }
            catch (Exception ex)
            {
                logger.DebugFormat("Payment Exception Error! :: {0} :: {1}", url, ex.ToString());
                return CardPaymentWParamType.Failure;
            }
        }

        public int RequestCardPayment(string amount)
        {
            if (string.IsNullOrEmpty(amount)) return CardPaymentWParamType.Failure;
            if (int.Parse(amount) == 0) return CardPaymentWParamType.Failure;

            PaymentModel pm = new PaymentModel();
            pm.Rq01 = "D1";
            pm.Rq03 = amount;
            pm.Rq04 = "00";
            pm.Rq12 = "30";
            pm.Rq13 = "A";

            string url = Url;
            url += "?callback=";
            url += Callback;
            url += "&REQ=";
            url += pm.ToString();

            WinHttpRequest winHttp = new WinHttpRequest();

            try
            {
                winHttp.Open(Method, url, true); //뒤에 bool값이 동기/비동기방식을 나타냄(Async)
                winHttp.Send(""); // string.Format("callback={0}&REQ=CC", "jsonp12345678983543344"));
                winHttp.WaitForResponse(120000); //120초후 timeout
                object resp = winHttp.ResponseBody;
                string encStr = Encoding.Default.GetString((Byte[])resp);

                var json = JObject.Parse(encStr.Replace(Callback + "(", "").Replace(")", ""));

                if (json["SUC"].ToString() != "00")
                {
                    Console.WriteLine(string.Format("Payment Fail :: {0} :: {1}", json["SUC"].ToString(), json["MSG"].ToString()));
                    logger.DebugFormat("Payment Fail :: {0} :: {1}", json["SUC"].ToString(), json["MSG"].ToString());
                    return CardPaymentWParamType.Failure;
                }

                return SDKManagerStatusCode.Success;
            }
            catch (Exception ex)
            {
                logger.DebugFormat("Payment Exception Error! :: {0} :: {1}", url, ex.ToString());
                return CardPaymentWParamType.Failure;
            }
        }

        public int RequestCardPayment(string amount, ref ReceiptData receiptData)
        {
            if (string.IsNullOrEmpty(amount)) return CardPaymentWParamType.Failure;
            if (int.Parse(amount) == 0) return CardPaymentWParamType.Failure;

            PaymentModel pm = new PaymentModel();
            pm.Rq01 = "D1";
            pm.Rq03 = amount;
            pm.Rq04 = "00";
            pm.Rq12 = "30";
            pm.Rq13 = "A";

            string url = Url;
            url += "?callback=";
            url += Callback;
            url += "&REQ=";
            url += pm.ToString();


            WinHttpRequest winHttp = new WinHttpRequest();

            try
            {
                winHttp.Open(Method, url, true); //뒤에 bool값이 동기/비동기방식을 나타냄(Async)
                winHttp.Send(""); // string.Format("callback={0}&REQ=CC", "jsonp12345678983543344"));
                winHttp.WaitForResponse(120000); //120초후 timeout
                object resp = winHttp.ResponseBody;
                string encStr = Encoding.Default.GetString((Byte[])resp);

                var json = JObject.Parse(encStr.Replace(Callback + "(", "").Replace(")", ""));

                if (json["SUC"].ToString() != "00")
                {
                    Console.WriteLine(string.Format("Payment Fail :: {0} :: {1}", json["SUC"].ToString(), json["MSG"].ToString()));
                    logger.DebugFormat("Payment Fail :: {0} :: {1}", json["SUC"].ToString(), json["MSG"].ToString());
                    receiptData.extraMessage = json["MSG"].ToString();
                    return CardPaymentWParamType.Failure;
                }

                if (json["RS04"].ToString() != "0000")
                {
                    Console.WriteLine(string.Format("Payment Fail :: {0} :: {1}", json["RS04"].ToString(), json["RS16"].ToString()));
                    logger.DebugFormat("Payment Fail :: {0} :: {1}", json["RS04"].ToString(), json["RS16"].ToString());
                    receiptData.extraMessage = json["RS16"].ToString();
                    return CardPaymentWParamType.Failure;
                }

                receiptData.cardNo = json["RQ04"].ToString();
                receiptData.cardCompany = json["RS14"].ToString();
                receiptData.cardType = json["RS12"].ToString();
                receiptData.payDate = DateTime.ParseExact("20" + json["RS07"].ToString().Substring(0, 12), "yyyyMMddHHmmss", null);
                receiptData.receiptNum = json["RS09"].ToString();
                logger.DebugFormat("Payment Success :: {0} :: {1} :: {2}", receiptData.cardNo, receiptData.payDate, receiptData.receiptNum);
                return SDKManagerStatusCode.Success;
            }
            catch (Exception ex)
            {
                logger.DebugFormat("Payment Exception Error! :: {0} :: {1}", url, ex.ToString());
                return CardPaymentWParamType.Failure;
            }
        }


        public int RequestCancelCardPayment(string amount, string approvalDate, string approvalNumber, ref ReceiptData receiptData)
        {


            if (string.IsNullOrEmpty(amount)) return CardPaymentWParamType.Failure;
            if (int.Parse(amount) == 0) return CardPaymentWParamType.Failure;

            PaymentModel pm = new PaymentModel();
            pm.Rq01 = "D4";// D1 :  승인, D4 : 환불
            pm.Rq03 = amount;
            pm.Rq04 = "00";
            pm.Rq05 = approvalDate;
            pm.Rq06 = approvalNumber;
            pm.Rq12 = "30";
            pm.Rq13 = "A";

            string url = Url;
            url += "?callback=";
            url += Callback;
            url += "&REQ=";
            url += pm.ToString();


            WinHttpRequest winHttp = new WinHttpRequest();

            try
            {
                winHttp.Open(Method, url, true); //뒤에 bool값이 동기/비동기방식을 나타냄(Async)
                winHttp.Send(""); // string.Format("callback={0}&REQ=CC", "jsonp12345678983543344"));
                winHttp.WaitForResponse(120000); //120초후 timeout
                object resp = winHttp.ResponseBody;
                string encStr = Encoding.Default.GetString((Byte[])resp);

                var json = JObject.Parse(encStr.Replace(Callback + "(", "").Replace(")", ""));

                if (json["SUC"].ToString() != "00")
                {
                    Console.WriteLine(string.Format("Fail :: {0} :: {1}", json["SUC"].ToString(), json["MSG"].ToString()));
                    logger.DebugFormat("Card Payment Fail :: {0} :: {1}", json["SUC"].ToString(), json["MSG"].ToString());
                    receiptData.extraMessage = json["MSG"].ToString();
                    return CardPaymentWParamType.Failure;
                }

                if (json["RS04"].ToString() != "0000")
                {
                    Console.WriteLine(string.Format("Card Payment Fail :: {0} :: {1}", json["RS04"].ToString(), json["RS16"].ToString()));
                    logger.DebugFormat("Payment Fail :: {0} :: {1}", json["RS04"].ToString(), json["RS16"].ToString());
                    receiptData.extraMessage = json["RS16"].ToString();
                    return CardPaymentWParamType.Failure;
                }

                receiptData.cardNo = json["RQ04"].ToString();
                receiptData.cardCompany = json["RS14"].ToString();
                receiptData.cardType = json["RS12"].ToString();
                receiptData.payDate = DateTime.ParseExact("20" + json["RS07"].ToString().Substring(0, 12), "yyyyMMddHHmmss", null);
                receiptData.receiptNum = json["RS09"].ToString();
                logger.DebugFormat("Payment Success :: {0} :: {1} :: {2}", receiptData.cardNo, receiptData.payDate, receiptData.receiptNum);
                return SDKManagerStatusCode.Success;
            }
            catch (Exception ex)
            {
                logger.DebugFormat("Payment Exception Error! :: {0} :: {1}", url, ex.ToString());
                return CardPaymentWParamType.Failure;
            }
        }
    }
}
