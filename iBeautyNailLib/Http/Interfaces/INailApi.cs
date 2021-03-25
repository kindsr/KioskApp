using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Http.Interfaces
{
    public interface INailApi
    {
        IUserInfoEndpoint UserInfo { get; }
        IImageListEndpoint ImageList { get; }
        IImageEndpoint Image { get; }
        IQRImageDataEndpoint QRImageData { get; }
        ISaveImageEndpoint SaveImage { get; }
        IMonitoringInfoEndpoint MonitoringInfo { get; }
        IPaymentInfoEndpoint PaymentInfo { get; }
        IErrorInfoEndpoint ErrorInfo { get; }
        IPrintInfoEndpoint PrintInfo { get; }
        IUpdateInfoEndpoint UpdateInfo { get; }
    }
}
