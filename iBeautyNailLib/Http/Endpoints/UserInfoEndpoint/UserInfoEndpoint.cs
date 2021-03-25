//using iBeautyNail.Http.Caching;
using iBeautyNail.Http.Caching;
using iBeautyNail.Http.Endpoints.UserInfoEndpoint.Models;
using iBeautyNail.Http.Interfaces;
//using iBeautyNail.Http.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Endpoints.UserInfoEndpoint
{
    public class UserInfoEndpoint : IUserInfoEndpoint
    {
        private const string UserInfoRootUrl = "/rest/v1/login";
        private const string UserInfoCache = "userinfo-{0}";

        private static IRequester _requester;
        private readonly ICache _cache;

        /// <inheritdoc />
        public UserInfoEndpoint(IRequester requester, ICache cache)
        {
            _requester = requester;
            _cache = cache;
        }

        public async Task<UserInfoResponseObj> GetUserInfoByLoginAsync(UserInfoRequestObj reqUserInfo)
        {
            var resUserInfo = new UserInfoResponseObj();

            var json = await _requester.CreatePostRequestAsync(UserInfoRootUrl, JsonConvert.SerializeObject(reqUserInfo), null, false);

            if (json != null)
            {
                resUserInfo = JsonConvert.DeserializeObject<UserInfoResponseObj>(json);
            }

            return resUserInfo;
        }
    }
}