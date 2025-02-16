namespace AvitoMerchShop.Application.Requests
{
    public class PurchaseRequest
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
