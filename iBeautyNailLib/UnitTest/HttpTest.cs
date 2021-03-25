using iBeautyNail.Http;
using iBeautyNail.Http.Endpoints.ErrorInfoEndpoint.Models;
using iBeautyNail.Http.Endpoints.ImageEndpoint.Models;
using iBeautyNail.Http.Endpoints.MonitoringInfoEndpoint.Models;
using iBeautyNail.Http.Endpoints.PaymentInfoEndpoint.Models;
using iBeautyNail.Http.Endpoints.PrintInfoEndPoint.Models;
using iBeautyNail.Http.Endpoints.UpdateInfoEndpoint.Models;
using iBeautyNail.Http.Endpoints.UserInfoEndpoint;
using iBeautyNail.Http.Endpoints.UserInfoEndpoint.Models;
using iBeautyNail.Http.Interfaces;
using iBeautyNail.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace iBeautyNail.UnitTest
{
    [TestFixture()]
    public class HttpTest
    {
        private async Task Wait(TimeSpan timeSpan)
        {
            await Task.Delay(timeSpan);
        }

        [Test]
        public async Task ApiTest()
        {
            UserInfoResponseObj res = new UserInfoResponseObj();
            //UserInfoRequestObj req = new UserInfoRequestObj { Username = "admin", Password = "dkdlfhqh!" };
            UserInfoRequestObj req = new UserInfoRequestObj { Username = "irobotech", Password = "dkdlfhqh!" };
            NailApi Api = NailApi.GetDevelopmentInstance("");
            res = await Api.UserInfo.GetUserInfoByLoginAsync(req);
            Assert.IsNotNull(res);

            //BitmapSource bsfromFile = new BitmapImage(new Uri(@"C:\nails\temp\resultTest.png"));
            //string thumb = ImageUtility.Instance.Base64EncodeImage(bsfromFile);
            //Api = NailApi.GetDevelopmentInstance(res.Token);
            //var resSave = await Api.SaveImage.SaveImageAsync(new SaveImageRequestObj() { Thumbnail = thumb });
            //Assert.IsNotNull(resSave);

            Api = NailApi.GetDevelopmentInstance(res.Token);
            var resQRImageData = await Api.QRImageData.QRImageDataTokenAsync(0);
            Assert.IsNotNull(resQRImageData);

            req = new UserInfoRequestObj { Username = "admin", Password = "dkdlfhqh!" };
            res = await Api.UserInfo.GetUserInfoByLoginAsync(req);
            Assert.IsNotNull(res);

            Api = NailApi.GetDevelopmentInstance(res.Token);
            var resImageList = await Api.ImageList.GetImageListTokenAsync(resQRImageData.Datas[0].Token);
            Assert.IsNotNull(resImageList);

            int i = 0;
            foreach (var d in resImageList.Datas)
            {
                //string thumbnail = d.Thumbnail;

                // 실제 이미지 받음
                Api = NailApi.GetDevelopmentInstance(res.Token);
                var resImage = await Api.Image.GetImageAsync(d.Username, d.Index);
                //Assert.IsNotNull(resImage);

                string imagedata = resImage.Datas[0].ImageData;

                BitmapSource bs = ImageUtility.Instance.DecodeBase64Image(imagedata);

                ImageUtility.Instance.SavePNGFile(bs, string.Format(@"C:\nails\temp\testThumbnail_{0}.png", i), PngInterlaceOption.Default);

                FileStream fs = new FileStream(string.Format(@"C:\nails\temp\testThumbnail_{0}.txt", i++), FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.Write(imagedata);
                sw.Flush();
                sw.Close();
            }

            //string CreatedFilePath = Path.Combine(Path.GetDirectoryName(BackupPath), "result_") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        }

        [Test]
        public async Task ApiTestAsync()
        {
            var res = new PaymentInfoResponseObj();
            //PaymentInfoRequestObj req = new PaymentInfoRequestObj { MachineID = 0, CustomerID = 10000, Price = "5000", ApprovalNo = "aa", CardOwner = "bb", CardType = "cc", CardComp = "dd", PaymentDt = "2020-11-10 00:00:00", ErrorCd = "ee", ErrorMsg = "ff"};
            //PaymentInfoRequestObj req = new PaymentInfoRequestObj { MachineID = 0, Price = 10000, ApprovalNo = "aa", CardOwner = "bb", CardType = "cc", CardComp = "dd", PaymentDt = "2020-11-10 00:00:00", ErrorCd = "ee", ErrorMsg = "ff" };
            PaymentInfoRequestObj req = new PaymentInfoRequestObj { MachineID = 1, Price = 5000, ApprovalNo = "06050306", PaymentDt = "2021-01-22 14:26:14", CancelApprovalNo = "000000000" };
            NailApi Api = NailApi.GetDevelopmentInstance("");
            //res = await Api.PaymentInfo.CreatePaymentInfoAsync(req);
            res = await Api.PaymentInfo.UpdateCancelPaymentAsync(req);
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task ApiTestErrorInfoAsync()
        {
            var res = new ErrorInfoResponseObj();
            var req = new ErrorInfoRequestObj { MachineID = 0, ErrorCd = "abc", ErrorMsg = "abc", FixYn = "N" };
            NailApi Api = NailApi.GetDevelopmentInstance("");
            res = await Api.ErrorInfo.CreateErrorInfoAsync(req);
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task ApiTestPrintInfoAsync()
        {
            var res = new PrintInfoResponseObj();
            var req = new PrintInfoRequestObj { MachineID = 0, DesignType = 0 };
            NailApi Api = NailApi.GetDevelopmentInstance("");
            res = await Api.PrintInfo.CreatePrintInfoAsync(req);
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task ApiTestMonitoringInfoAsync()
        {
            var res = new MonitoringInfoObj();
            NailApi Api = NailApi.GetDevelopmentInstance("");
            res = await Api.MonitoringInfo.SelectMonitoringInfoAsync(7);

            Assert.IsNotNull(res);
        }

        [Test]
        public async Task ApiTestUpdateInfoAsync()
        {
            //var res = new AppVersionObj();
            //var res = new UpdateProcessListObj();
            var res = new List<UpdateProcessObj>();
            //var res = new UpdateInfoResponseObj();
            var req = new UpdateInfoRequestObj { MachineID = 0, FileName = @"nailpod\Resources\Data\Video\irobo.mp4", VersionInfo = "0.2" };
            NailApi Api = NailApi.GetDevelopmentInstance("");
            //res = await Api.UpdateInfo.SelectAppVersionAsync();
            //res = await Api.UpdateInfo.SelectUpdateProcessAsync(req);
            //res = await Api.UpdateInfo.DeleteUpdateProcessAsync(req);
            res = await Api.UpdateInfo.SelectDeleteContentsAsync("0.2");
            Assert.IsNotNull(res);
        }
    }

}
