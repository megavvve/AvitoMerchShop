namespace AvitoMerchShop.Core.Entities
{
    public class CoinTransaction
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }

        public User FromUser { get; set; }
        public User ToUser { get; set; }
    }
}
