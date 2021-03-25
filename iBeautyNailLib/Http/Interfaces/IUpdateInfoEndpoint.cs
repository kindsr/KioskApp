using iBeautyNail.Http.Endpoints.UpdateInfoEndpoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Interfaces
{
    public interface IUpdateInfoEndpoint
    {
        Task<AppVersionObj> SelectAppVersionAsync();
        Task<List<UpdateProcessObj>> SelectUpdateProcessAsync(UpdateInfoRequestObj req);
        Task<UpdateInfoResponseObj> DeleteUpdateProcessAsync(UpdateInfoRequestObj req);
        Task<List<UpdateProcessObj>> SelectDeleteContentsAsync(string version);
        Task UpdateUpdateYNAsync(int machine_id, string updateYn);
        Task<UpdateInfoResponseObj> UpsertUpdateContentsAsync(UpdateInfoRequestObj req);
        Task<UpdateInfoResponseObj> UpdateDelYNAsync(UpdateInfoRequestObj req);
        Task UpdateAppVersionAsync(string version);
    }
}
