using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Interface
{
    public class ProductInfo
    {
        public string dcs;
        public string itemNum;
        public int qty;
        public double price;
        public double extPrice;
        public bool isPaid;
    }

    public class ReceiptData
    {
        public ReceiptData()
        {
            _prodInfo = new List<ProductInfo>();
        }

        public string paymentType;
        public string companyLogoPath;
        public string companyName;
        public string companyRegNo;
        public string companyAddress;
        public string companyTel;
        //public string refSo;
        public string receiptNum;
        public DateTime payDate;
        //public string store;
        //public string assoc;
        //public string cashier;
        //public string billTo;
        private List<ProductInfo> _prodInfo;
        public List<ProductInfo> prodInfo
        {
            get
            {
                return _prodInfo;
            }
            set
            {
                _prodInfo = value;
            }
        }
        //public double subTotal;
        //public double taxRate;
        //public double receiptTotal;
        public string extraMessage;
        public string cardNo;
        public string cardCompany;
        public string cardType;
    }
}
