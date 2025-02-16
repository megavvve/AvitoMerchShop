namespace AvitoMerchShop.Application.Requests
{
    public class CoinTransactionRequest
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public int Amount { get; set; }
    }

}
