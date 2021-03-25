using iBeautyNail.Http.Caching;
using iBeautyNail.Http.Endpoints.ImageEndpoint.Models;
using iBeautyNail.Http.Endpoints.MonitoringInfoEndpoint.Models;
using iBeautyNail.Http.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Endpoints.MonitoringInfoEndpoint
{
    public class MonitoringInfoEndpoint : IMonitoringInfoEndpoint
    {
        private const string UpdateMonitoringInfoUrl = "/nailpod/upsertMonitoringInfo";
        private const string SelectMonitoringInfoUrl = "/nailpod/selectMonitoringInfo/{0}";
        private const string UpdateMachineStatusUrl = "/nailpod/updateMachineStatus/{0}/{1}";

        private static IRequester _requester;
        private readonly ICache _cache;

        public MonitoringInfoEndpoint(IRequester requester, ICache cache)
        {
            _requester = requester;
            _cache = cache;
        }

        public async Task<MonitoringInfoResponseObj> UpdateMonitoringInfoAsync(MonitoringInfoRequestObj req)
        {
            var res = new MonitoringInfoResponseObj();

            var json = await _requester.CreateTestPostRequestAsync(UpdateMonitoringInfoUrl, JsonConvert.SerializeObject(req), null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<MonitoringInfoResponseObj>(json);
            }

            return res;
        }

        public async Task<MonitoringInfoObj> SelectMonitoringInfoAsync(int machine_id)
        {
            var res = new MonitoringInfoObj();

            var json = await _requester.CreateTestPostRequestAsync(string.Format(SelectMonitoringInfoUrl, machine_id), "", null, false);

            if (json != null)
            {
                res = JsonConvert.DeserializeObject<MonitoringInfoObj>(json);
            }

            return res;
        }

        public async Task UpdateMachineStatusAsync(int machine_id, string status)
        {
            await _requester.CreateTestPostRequestAsync(string.Format(UpdateMachineStatusUrl, machine_id, status), "", null, false);
        }
    }
}
