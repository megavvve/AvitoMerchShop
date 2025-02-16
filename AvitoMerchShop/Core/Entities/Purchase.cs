namespace AvitoMerchShop.Core.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public int ItemId { get; set; }

        public User User { get; set; }
        public Item Item { get; set; }
    }
}
