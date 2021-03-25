using iBeautyNail.Interface;
using Unity;

namespace iBeautyNail.SDK.Device.Payment
{
    public class CardPayment : IPayment
    {
        [Dependency("ICardPayment")]
        public IPayment Payment { get; set; }

        public int Connect()
        {
            return Payment.Connect();
        }

        public int Disconnect()
        {
            return Payment.Disconnect();
        }

        public void Initialize()
        {
            Payment.Initialize();
        }

        public bool IsConnected()
        {
            return Payment.IsConnected();
        }

        public int State()
        {
            return Payment.State();
        }

        public int RequestCardPayment(string amount)
        {
            return Payment.RequestCardPayment(amount);
        }

        public int RequestCardPayment(string amount, ref ReceiptData receiptData)
        {
            return Payment.RequestCardPayment(amount, ref receiptData);
        }

        public int RequestCancelCardPayment(string amount, string approvalDate, string approvalNumber, ref ReceiptData receiptData)
        {
            return Payment.RequestCancelCardPayment(amount, approvalDate, approvalNumber, ref receiptData);
        }
    }
}
