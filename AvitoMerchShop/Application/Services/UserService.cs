using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using AvitoMerchShop.Core.DTOs;
using InfoResponse = AvitoMerchShop.Core.DTOs.InfoResponse;
using AvitoMerchShop.Core.Interfaces;
using AvitoMerchShop.Core.Entities;


namespace AvitoMerchShop.Data.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<InfoResponse> GetUserInfo(int userId)
        {
            var user = await _context.Users
        .AsSplitQuery()
        .Include(u => u.Purchases)
            .ThenInclude(p => p.Item)
        .Include(u => u.SentTransactions)
            .ThenInclude(t => t.ToUser)
        .Include(u => u.ReceivedTransactions)
            .ThenInclude(t => t.FromUser)
        .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return new InfoResponse
            {
                Coins = user.CoinsBalance,
                Inventory = user.Purchases
                    .GroupBy(p => p.Item.Name)
                    .Select(g => new InventoryItemDto
                    {
                        Type = g.Key,
                        Quantity = g.Sum(p => p.Quantity)
                    }).ToList(),
                CoinHistory = new CoinHistoryDto
                {
                    Received = user.ReceivedTransactions.Select(t => new TransactionDto
                    {
                        FromUser = t.FromUser?.Username,
                        Amount = t.Amount,
                        Date = t.TransactionDate
                    }).ToList(),
                    Sent = user.SentTransactions.Select(t => new TransactionDto
                    {
                        ToUser = t.ToUser?.Username,
                        Amount = t.Amount,
                        Date = t.TransactionDate
                    }).ToList()
                }
            };
        }

     
        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            

            if (user == null)
            {
                user = new User
                {
                    Username = username,
                    Password = password,
                    CoinsBalance = 1000,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                Console.WriteLine($"New user created: {user.Id}");
            }
            else if (user.Password != password)
            {
                Console.WriteLine("Invalid password");
                return null;
            }

            return user;
        }

        public string GenerateJwtToken(User user)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);

            if (keyBytes.Length < 32)
                throw new ArgumentException("JWT secret key must be at least 256 bits (32 bytes)");


            var claims = new[]
    {
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Name, user.Username)
};

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


       
        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }


        public async Task<bool> TransferCoins(int fromUserId, int toUserId, int amount)
        {
            var fromUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == fromUserId);
            var toUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == toUserId);

            if (fromUser == null || toUser == null || fromUser.CoinsBalance < amount)
                return false;

            fromUser.CoinsBalance -= amount;
            toUser.CoinsBalance += amount;

            var transaction = new CoinTransaction
            {
                Amount = amount,
                FromUserId = fromUserId,
                ToUserId = toUserId
            };

            _context.CoinTransactions.Add(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PurchaseItem(int userId, int itemId, int quantity)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);

            if (user == null || item == null || user.CoinsBalance < item.Price * quantity)
                return false;

            user.CoinsBalance -= item.Price * quantity;

            var purchase = new Purchase
            {
                UserId = userId,
                ItemId = itemId,
                Quantity = quantity
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> PurchaseItemByName(int userId, string itemName)
        {
            var user = await _context.Users
                .Include(u => u.Purchases)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var item = await _context.Items
         .FirstOrDefaultAsync(i =>
             EF.Functions.ILike(i.Name, itemName));

            if (user == null || item == null)
                return false;

            if (user.CoinsBalance < item.Price)
                return false;

            user.CoinsBalance -= item.Price;

            var purchase = new Purchase
            {
                UserId = userId,
                ItemId = item.Id,
                Quantity = 1,
                PurchaseDate = DateTime.UtcNow
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            return true;
        }

    }

}