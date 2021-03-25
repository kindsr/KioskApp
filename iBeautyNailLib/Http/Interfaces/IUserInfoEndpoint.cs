using iBeautyNail.Http.Endpoints.UserInfoEndpoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Interfaces
{
    public interface IUserInfoEndpoint
    {
        Task<UserInfoResponseObj> GetUserInfoByLoginAsync(UserInfoRequestObj reqUserInfo);
    }
}
