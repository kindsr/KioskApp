using iBeautyNail.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Devices.ReceiptPrinter.Datas
{
    public class ReceiptModel : INotifyPropertyChanged
    {
        public ReceiptModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyUpdate(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string companyLogoPath;
        public string CompanyLogoPath
        {
            get
            {
                return companyLogoPath;
            }
            set
            {
                companyLogoPath = value;
                OnPropertyUpdate("CompanyLogoPath");
            }
        }

        private string companyName;
        public string CompanyName
        {
            get
            {
                return companyName;
            }
            set
            {
                companyName = value;
                OnPropertyUpdate("CompanyName");
            }
        }

        private string companyRegNo;
        public string CompanyRegNo
        {
            get
            {
                return companyRegNo;
            }
            set
            {
                companyRegNo = value;
                OnPropertyUpdate("CompanyRegNo");
            }
        }

        private string companyAddress;
        public string CompanyAddress
        {
            get
            {
                return companyAddress;
            }
            set
            {
                companyAddress = value;
                OnPropertyUpdate("CompanyAddress");
            }
        }

        private string companyTel;
        public string CompanyTel
        {
            get
            {
                return companyTel;
            }
            set
            {
                companyTel = value;
                OnPropertyUpdate("CompanyTel");
            }
        }

        private string refSo;
        public string RefSo
        {
            get
            {
                return refSo;
            }
            set
            {
                refSo = value;
                OnPropertyUpdate("RefSo");
            }
        }

        private string receiptNum;
        public string ReceiptNum
        {
            get
            {
                return receiptNum;
            }
            set
            {
                receiptNum = value;
                OnPropertyUpdate("ReceiptNum");
            }
        }

        private DateTime payDate;
        public DateTime PayDate
        {
            get
            {
                return payDate;
            }
            set
            {
                payDate = value;
                OnPropertyUpdate("PayDate");
            }
        }

        private string store;
        public string Store
        {
            get
            {
                return store;
            }
            set
            {
                store = value;
                OnPropertyUpdate("Store");
            }
        }

        private string assoc;
        public string Assoc
        {
            get
            {
                return assoc;
            }
            set
            {
                assoc = value;
                OnPropertyUpdate("Assoc");
            }
        }

        private string cashier;
        public string Cashier
        {
            get
            {
                return cashier;
            }
            set
            {
                cashier = value;
                OnPropertyUpdate("Cashier");
            }
        }

        private string billTo;
        public string BillTo
        {
            get
            {
                return billTo;
            }
            set
            {
                billTo = value;
                OnPropertyUpdate("BillTo");
            }
        }

        private ProductInfo prodInfo;
        public ProductInfo ProdInfo
        {
            get
            {
                return prodInfo;
            }
            set
            {
                prodInfo = value;
                OnPropertyUpdate("ProdInfo");
            }
        }

        private string subTotal;
        public string SubTotal
        {
            get
            {
                return subTotal;
            }
            set
            {
                subTotal = value;
                OnPropertyUpdate("SubTotal");
            }
        }

        private string taxRate;
        public string TaxRate
        {
            get
            {
                return taxRate;
            }
            set
            {
                taxRate = value;
                OnPropertyUpdate("TaxRate");
            }
        }

        private string receiptTotal;
        public string ReceiptTotal
        {
            get
            {
                return receiptTotal;
            }
            set
            {
                receiptTotal = value;
                OnPropertyUpdate("ReceiptTotal");
            }
        }

        private string extraMessage;
        public string ExtraMessage
        {
            get
            {
                return extraMessage;
            }
            set
            {
                extraMessage = value;
                OnPropertyUpdate("ExtraMessage");
            }
        }
    }
}
