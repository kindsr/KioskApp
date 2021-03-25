namespace iBeautyNail.Interface
{
    public interface IPayment
    {
        /// <summary>
        /// Initialize
        /// </summary>
        void Initialize();

        /// <summary>
        /// Connect
        /// </summary>
        int Connect();

        /// <summary>
        /// Disconnect
        /// </summary>
        int Disconnect();

        /// <summary>
        /// IsConnected
        /// </summary>
        bool IsConnected();

        /// <summary>
        /// 카드리더기 상태를 반환한다.
        /// </summary>
        int State();

        /// <summary>
        /// 카드 결제 요청
        /// </summary>
        int RequestCardPayment(string amount);

        /// <summary>
        /// 카드 결제 요청
        /// </summary>
        int RequestCardPayment(string amount, ref ReceiptData receiptData);

        /// <summary>
        /// 카드 결제 취소 요청
        /// </summary>
        int RequestCancelCardPayment(string amount, string approvalDate, string approvalNumber, ref ReceiptData receiptData);
    }
}
