namespace AvitoMerchShop.Core.DTOs
{

    public class InfoResponse
    {
        public int Coins { get; set; }
        public List<InventoryItemDto> Inventory { get; set; }
        public CoinHistoryDto CoinHistory { get; set; }
    }

    public class InventoryItemDto
    {
        public string Type { get; set; }
        public int Quantity { get; set; }
    }

    public class CoinHistoryDto
    {
        public List<TransactionDto> Received { get; set; }
        public List<TransactionDto> Sent { get; set; }
    }

    public class TransactionDto
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
