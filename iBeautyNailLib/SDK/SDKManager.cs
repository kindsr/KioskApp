using iBeautyNail.Extensions;
using iBeautyNail.Interface;
using iBeautyNail.SDK.Device.Payment;
using iBeautyNail.SDK.Device.Printer;
using iBeautyNail.Windows;
using System;

namespace iBeautyNail.SDK
{
    public class SDKManager
    {
        internal static WindowMessageHandler WindowMessageHandler { get; private set; }
        internal static int retVal = SDKManagerStatusCode.InitRequired;

        public static string MachineId { get; private set; }

        #region Devices
        public static IPrinter ReceiptPrinter { get; private set; }
        public static INailPrinter NailPrinter { get; private set; }
        public static IPayment CardPayment { get; private set; }
        #endregion

        static SDKManager()
        {
            CreateDeviceObjects();
        }

        /// <summary>
        /// 장비제어 인스턴스 생성
        /// </summary>
        private static void CreateDeviceObjects()
        {
            ReceiptPrinter = UnityContainerManager.Instance.Resolve<ReceiptPrinter>("ReceiptPrinter");
            NailPrinter = UnityContainerManager.Instance.Resolve<NailPrinter>("NailPrinter");
            CardPayment = UnityContainerManager.Instance.Resolve<CardPayment>("CardPayment");
        }

        public static void Initialize(string machineId, IntPtr hWnd)
        {
            MachineId = machineId;
            WindowMessageHandler = new WindowMessageHandler(hWnd);

            if (DeviceConfigSection.Instance.ReceiptPrinter.Enable) ReceiptPrinter.Initialize();
            if (DeviceConfigSection.Instance.NailPrinter.Enable) NailPrinter.Initialize();
            if (DeviceConfigSection.Instance.CardPayment.Enable) CardPayment.Initialize();

            retVal = SDKManagerStatusCode.ConnectionRequired;
        }

        public static int Connect()
        {
            retVal = SDKManagerStatusCode.Success;

            // Receipt Printer Connect Area
            if (DeviceConfigSection.Instance.ReceiptPrinter.Enable)
            {
                retVal = ReceiptPrinter.Connect();
                Console.WriteLine(String.Format("Receipt Printer Connect :: {0}", retVal.ToString()));
                if (retVal != SDKManagerStatusCode.Success) return retVal;
            }

            // Nail Printer Connect Area
            if (DeviceConfigSection.Instance.NailPrinter.Enable)
            {
                retVal = NailPrinter.Connect();
                Console.WriteLine(String.Format("Nail Printer Connect :: {0}", retVal.ToString()));
                if (retVal != SDKManagerStatusCode.Success) return retVal;
            }

            // Payment Connect Area
            if (DeviceConfigSection.Instance.CardPayment.Enable)
            {
                retVal = CardPayment.Connect();
                Console.WriteLine(String.Format("Payment Connect :: {0}", retVal.ToString()));
                if (retVal != SDKManagerStatusCode.Success) return retVal;
            }

            return retVal;
        }

        public static int Disconnect()
        {
            // Receipt Printer Disconnect Area
            if (DeviceConfigSection.Instance.ReceiptPrinter.Enable) retVal = ReceiptPrinter.Disconnect();
            if (DeviceConfigSection.Instance.NailPrinter.Enable) retVal = NailPrinter.Disconnect();
            if (retVal != SDKManagerStatusCode.Success) return retVal;

            return retVal;
        }

        public static bool IsConnected
        {
            get
            {
                bool receiptPrinter = true;
                bool nailPrinter = true;

                if (DeviceConfigSection.Instance.ReceiptPrinter.Enable) receiptPrinter = ReceiptPrinter.IsConnected();
                if (DeviceConfigSection.Instance.NailPrinter.Enable) nailPrinter = NailPrinter.IsConnected();

                return receiptPrinter
                    && nailPrinter;
            }
        }

        public static int SDKAvailable()
        {
            if (retVal != SDKManagerStatusCode.Success) return retVal;

            if (!IsConnected)
                retVal = SDKManagerStatusCode.Disconnected;
            else
                retVal = SDKManagerStatusCode.Success;

            return retVal;
        }
    }
}
