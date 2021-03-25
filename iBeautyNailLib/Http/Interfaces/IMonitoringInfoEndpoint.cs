using iBeautyNail.Http.Endpoints.MonitoringInfoEndpoint.Models;
using iBeautyNail.Http.Endpoints.UserInfoEndpoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Interfaces
{
    public interface IMonitoringInfoEndpoint
    {
        Task<MonitoringInfoResponseObj> UpdateMonitoringInfoAsync(MonitoringInfoRequestObj reqMonitoringInfo);
        Task<MonitoringInfoObj> SelectMonitoringInfoAsync(int machine_id);
        Task UpdateMachineStatusAsync(int machine_id, string status);
    }
}
