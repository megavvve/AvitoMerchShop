namespace AvitoMerchShop.Models
{
    public class CoinTransaction
    {
        public int Id { get; set; }  
        public int Amount { get; set; }  
        public int FromUserId { get;  set; }  
        public int ToUserId { get;  set; }
    }
}
