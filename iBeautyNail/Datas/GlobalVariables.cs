using System;
using System.Collections.Generic;

using log4net;
using iBeautyNail.Extensions.Enums;
using System.Diagnostics;
using iBeautyNail.Interface;
using iBeautyNail.Devices.NailPrinter;
using System.IO;

namespace iBeautyNail.Datas
{
    public class GlobalVariables
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static object lockObj = new object();
        private static GlobalVariables singletonObj;

        private GlobalVariables()
        {
            ElapsedTime = new PageElapsedTime();
            _myDesigns = new List<Nail>();
            _myProduct = new ProductInfo();
            _qrData = new List<QRImageListInfo>();
            _printerInfo = new PrinterInfo();
            _receiptData = new ReceiptData();
            InkVol = new int[7];
        }

        public static GlobalVariables Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (singletonObj == null)
                    {
                        singletonObj = new GlobalVariables();
                    }
                }
                return singletonObj;
            }
        }

        #region Member Variables

        /// <summary>
        /// 선택한 디자인 담는 변수
        /// </summary>
        private List<Nail> _myDesigns;
        public List<Nail> MyDesigns
        {
            get { return _myDesigns; }
            set
            {
                if (_myDesigns != value)
                    _myDesigns = value;
            }
        }

        private ProductInfo _myProduct;
        public ProductInfo MyProduct
        {
            get { return _myProduct; }
            set
            {
                if (_myProduct != value)
                    _myProduct = value;
            }
        }

        private List<QRImageListInfo> _qrData;
        public List<QRImageListInfo> QRData
        {
            get { return _qrData; }
            set
            {
                if (_qrData != value)
                    _qrData = value;
            }
        }

        private PrinterInfo _printerInfo;
        public PrinterInfo PrinterInfo
        {
            get { return _printerInfo; }
            set
            {
                if (_printerInfo != value)
                    PrinterInfo = value;
            }
        }

        private ReceiptData _receiptData;
        public ReceiptData ReceiptData
        {
            get { return _receiptData; }
            set
            {
                if (_receiptData != value)
                    _receiptData = value;
            }
        }

        public PageElapsedTime ElapsedTime { get; set; }
        public bool LanguagePopup { get; set; }
        public int HeadCleaning { get; set; }
        public TimeSpan HeadCleanTime { get; set; }
        public double DesignPrice { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }
        //네일 이미지 하단에 보여질 서명
        public string Sign1 { get; set; }
        public string Sign2 { get; set; }
        public int[] InkVol { get; set; }
        public bool IsTTSOn { get; set; }
        public bool IsPaymentOn { get; set; }

        // 하드코딩 없애고싶다..
        public string DesignRootPath = @"C:\nails";
        public string TempResultPath = @"C:\nails\temp\result.png";
        public string ResultPath = @"C:\nails\result\result.png";
        public string BackupPath = @"C:\nails\backup\result.png";
        public string BackgroundPath = @"C:\\nails\\bgsticker.png";
        public string PrintInkLimitPath = @"C:\PartnerDRV\Setting\print.ini";
        public string PrintTemplatePath = @"C:\PartnerDRV\Setting\Template.ini";


        #endregion //Member Variables

        #region public methods
        public void Clear()
        {
            ElapsedTime.Clear();
            MyDesigns.Clear();

            // QR데이터파일 삭제
            foreach (var d in QRData)
            {
                try
                {
                    if (File.Exists(d.PrintImage)) File.Delete(d.PrintImage);
                    if (File.Exists(d.TempImagePath)) File.Delete(d.TempImagePath);
                }
                catch (Exception ex)
                {
                    logger.DebugFormat("{0} :: Delete files exception : {1}", "GlobalVariables Clear", ex.ToString());
                }
            }

            QRData.Clear();

            ReceiptData.cardNo = "";
            ReceiptData.cardCompany = "";
            ReceiptData.cardType = "";
            ReceiptData.payDate = DateTime.Now;
            ReceiptData.receiptNum = "";
            ReceiptData.extraMessage = "";

            MyProduct.qty = 0;
            MyProduct.price = 0;
            MyProduct.isPaid = false;

            Token = string.Empty;
        }

        #endregion //public methods
    }
}
