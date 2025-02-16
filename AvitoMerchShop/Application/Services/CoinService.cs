using AvitoMerchShop.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AvitoMerchShop.Core.Entities;

namespace AvitoMerchShop.Application.Services
{
    public class CoinService
    {
        private readonly AppDbContext _context;

        public CoinService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TransferCoins(int fromUserId, int toUserId, int amount)
        {
            var fromUser = await _context.Users.FindAsync(fromUserId);
            var toUser = await _context.Users.FindAsync(toUserId);

            if (fromUser == null || toUser == null || fromUser.CoinsBalance < amount)
                return false;

            fromUser.CoinsBalance -= amount;
            toUser.CoinsBalance += amount;

            var transaction = new CoinTransaction
            {
                Amount = amount,
                FromUserId = fromUserId,
                ToUserId = toUserId,
                FromUser = fromUser,
                ToUser = toUser
            };

            _context.CoinTransactions.Add(transaction);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
