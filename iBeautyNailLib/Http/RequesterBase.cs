using iBeautyNail.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http
{
    public abstract class RequesterBase
    {
        private readonly HttpClient _httpClient;

        private IniFile serverIni = new IniFile();
        protected string ServerUrl = "dev";
        protected string AppServerUrl = "eane";

        public string ApiKey { get; set; }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="RequesterBase"/> class.
        ///// </summary>
        ///// <param name="apiKey">The API key.</param>
        ///// <exception cref="ArgumentNullException">apiKey</exception>
        protected RequesterBase(string apiKey) : this()
        {
            //if (string.IsNullOrWhiteSpace(apiKey))
            //    throw new ArgumentNullException(nameof(apiKey));
            ApiKey = apiKey;
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="RequesterBase"/> class.
        ///// </summary>
        protected RequesterBase()
        {
            _httpClient = new HttpClient();

            if (File.Exists("Configs\\HttpSetting.ini"))
            {
                serverIni.Load("Configs\\HttpSetting.ini");
                ServerUrl = serverIni["URL"]["MainServer"].ToString();
                AppServerUrl = serverIni["URL"]["AppServer"].ToString();
            }
        }

        #region Protected Methods

        ///// <summary>
        ///// Send a <see cref="HttpRequestMessage"/> asynchronously.
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        ///// <exception cref="NailApiException">Thrown if an Http error occurs. Contains the Http error code and error message.</exception>
        protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                HandleRequestFailure(response); //Pass Response to get status code, Then Dispose Object.
            }
            return response;
        }

        protected HttpRequestMessage PrepareRequest(string host, string relativeUrl, List<string> queryParameters,
            bool useHttps, HttpMethod httpMethod)
        {
            var scheme = useHttps ? "https" : "http";
            var url = queryParameters == null ?
                $"{scheme}://{host}{relativeUrl}" :
                $"{scheme}://{host}{relativeUrl}?{BuildArgumentsString(queryParameters)}";

            var requestMessage = new HttpRequestMessage(httpMethod, url);
            if (!string.IsNullOrEmpty(ApiKey))
            {
                requestMessage.Headers.Add("X-AUTH-TOKEN", ApiKey);
            }
            return requestMessage;
        }

        protected string BuildArgumentsString(List<string> arguments)
        {
            return arguments
                .Where(arg => !string.IsNullOrWhiteSpace(arg))
                .Aggregate(string.Empty, (current, arg) => current + ("&" + arg));
        }

        protected void HandleRequestFailure(HttpResponseMessage response)
        {
            var statusCode = response.StatusCode;
            try
            {
                switch (statusCode)
                {
                    case HttpStatusCode.ServiceUnavailable:
                        throw new NailApiException("503, Service unavailable", statusCode);
                    case HttpStatusCode.InternalServerError:
                        throw new NailApiException("500, Internal server error", statusCode);
                    case HttpStatusCode.Unauthorized:
                        throw new NailApiException("401, Unauthorized", statusCode);
                    case HttpStatusCode.BadRequest:
                        throw new NailApiException("400, Bad request", statusCode);
                    case HttpStatusCode.NotFound:
                        throw new NailApiException("404, Resource not found", statusCode);
                    case HttpStatusCode.Forbidden:
                        throw new NailApiException("403, Forbidden", statusCode);
                    default:
                        throw new NailApiException("Unexpeced failure", statusCode);
                }
            }
            finally
            {
                response.Dispose(); //Dispose Response On Error
            }
        }

        protected async Task<string> GetResponseContentAsync(HttpResponseMessage response)
        {
            using (response)
            using (var content = response.Content)
            {
                return await content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
        #endregion
    }
}
