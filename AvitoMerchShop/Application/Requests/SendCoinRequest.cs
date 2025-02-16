namespace AvitoMerchShop.Application.Requests
{
    public class SendCoinRequest
    {
        public string ToUser { get; set; }
        public int Amount { get; set; }
    }
}
