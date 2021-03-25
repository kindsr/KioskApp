using iBeautyNail.Http.Endpoints.PaymentInfoEndpoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Interfaces
{
    public interface IPaymentInfoEndpoint
    {
        Task<PaymentInfoResponseObj> CreatePaymentInfoAsync(PaymentInfoRequestObj reqMonitoringInfo);
        Task<PaymentInfoResponseObj> UpdateCancelPaymentAsync(PaymentInfoRequestObj reqMonitoringInfo);
    }
}
