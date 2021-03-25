using iBeautyNail.Http.Caching;
using iBeautyNail.Http.Endpoints.UpdateInfoEndpoint.Models;
using iBeautyNail.Http.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Endpoints.UpdateInfoEndpoint
{
    public class UpdateInfoEndpoint : IUpdateInfoEndpoint
    {
        private const string SelectAppVersion = "/nailpod/selectAppVersion";
        private const string SelectUpdateProcess = "/nailpod/selectUpdateProcess";
        private const string DeleteUpdateProcess = "/nailpod/deleteUpdateProcess";
        private const string SelectDeleteContents = "/nailpod/selectDeleteContents/{0}";
        private const string UpdateUpdateYN = "/nailpod/updateUpdateYN/{0}/{1}";
        private const string UpsertUpdateContents = "/nailpod/upsertUpdateContents";
        private const string UpdateDelYN = "/nailpod/updateDelYN";
        private const string UpdateAppVersion = "/nailpod/updateAppVersion/{0}";

        private static IRequester _requester;
        private readonly ICache _cache;

        public UpdateInfoEndpoint(IRequester requester, ICache cache)
        {
            _requester = requester;
            _cache = cache;
        }

        public async Task<UpdateInfoResponseObj> DeleteUpdateProcessAsync(UpdateInfoRequestObj req)
        {
            var res = new UpdateInfoResponseObj();

            var json = await _requester.CreateTestPostRequestAsync(DeleteUpdateProcess, JsonConvert.SerializeObject(req), null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<UpdateInfoResponseObj>(json);
            }

            return res;
        }

        public async Task<AppVersionObj> SelectAppVersionAsync()
        {
            var res = new AppVersionObj();

            var json = await _requester.CreateTestPostRequestAsync(SelectAppVersion, "", null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<AppVersionObj>(json);
            }

            return res;
        }

        public async Task<List<UpdateProcessObj>> SelectUpdateProcessAsync(UpdateInfoRequestObj req)
        {
            var res = new List<UpdateProcessObj>();

            var json = await _requester.CreateTestPostRequestAsync(SelectUpdateProcess, JsonConvert.SerializeObject(req), null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<List<UpdateProcessObj>>(json);
            }

            return res;
        }

        public async Task<List<UpdateProcessObj>> SelectDeleteContentsAsync(string version)
        {
            var res = new List<UpdateProcessObj>();

            var json = await _requester.CreateTestPostRequestAsync(string.Format(SelectDeleteContents, version), "", null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<List<UpdateProcessObj>>(json);
            }

            return res;
        }

        public async Task UpdateUpdateYNAsync(int machine_id, string updateYn)
        {
            await _requester.CreateTestPostRequestAsync(string.Format(UpdateUpdateYN, updateYn, machine_id), "", null, false);
        }

        public async Task<UpdateInfoResponseObj> UpsertUpdateContentsAsync(UpdateInfoRequestObj req)
        {
            var res = new UpdateInfoResponseObj();

            var json = await _requester.CreateTestPostRequestAsync(UpsertUpdateContents, JsonConvert.SerializeObject(req), null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<UpdateInfoResponseObj>(json);
            }

            return res;
        }

        public async Task<UpdateInfoResponseObj> UpdateDelYNAsync(UpdateInfoRequestObj req)
        {
            var res = new UpdateInfoResponseObj();

            var json = await _requester.CreateTestPostRequestAsync(UpdateDelYN, JsonConvert.SerializeObject(req), null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<UpdateInfoResponseObj>(json);
            }

            return res;
        }

        public async Task UpdateAppVersionAsync(string version)
        {
            await _requester.CreateTestPostRequestAsync(string.Format(UpdateAppVersion, version), "", null, false);
        }
    }
}
