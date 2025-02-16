using AvitoMerchShop.Data;
using Microsoft.EntityFrameworkCore;

namespace AvitoMerchShop.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int CoinsBalance { get; set; }
        public string Password { get;  set; }



        public ICollection<CoinTransaction> SentTransactions { get; set; }
        public ICollection<CoinTransaction> ReceivedTransactions { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    
    }
}
