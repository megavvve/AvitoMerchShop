namespace AvitoMerchShop.Application.Responses
{
    public class InfoResponse
    {
        public int Coins { get; set; }
        public List<ItemInfo> Inventory { get; set; }
        public CoinHistory CoinHistory { get; set; }
    }

    public class ItemInfo
    {
        public string Type { get; set; }
        public int Quantity { get; set; }
    }

    public class CoinHistory
    {
        public List<TransactionInfo> Received { get; set; }
        public List<TransactionInfo> Sent { get; set; }
    }

    public class TransactionInfo
    {
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public int Amount { get; set; }
    }
}
