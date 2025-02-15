namespace AvitoMerchShop.Models
{
    public class InfoResponse
    {
        public int Coins { get; set; }  
        public List<ItemInventory> Inventory { get; set; }  
        public CoinHistory CoinHistory { get; set; }  
    }

    public class ItemInventory
    {
        public string Type { get; set; }  
        public int Quantity { get; set; }  
    }

    public class CoinHistory
    {
        public List<CoinTransaction> Received { get; set; }  
        public List<CoinTransaction> Sent { get; set; }
    }

    public class SendCoinRequest
    {
        public string ToUser { get; set; }
        public int Amount { get; set; } 
    }

    public class ErrorResponse
    {
        public string Errors { get; set; }  
    }

    public class AuthRequest
    {
        public string Username { get; set; }  
        public string Password { get; set; }  
    }

    public class AuthResponse
    {
        public string Token { get; set; }
    }
}
