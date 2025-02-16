using System.Threading.Tasks;
using AvitoMerchShop.Core.DTOs;
using AvitoMerchShop.Core.Entities;


namespace AvitoMerchShop.Core.Interfaces
{
    public interface IUserService
    {
        Task<InfoResponse> GetUserInfo(int userId);  
        Task<User> Authenticate(string username, string password);
        string GenerateJwtToken(User user);
        Task<User> GetUserById(int userId);
        Task<bool> TransferCoins(int fromUserId, int toUserId, int amount);
        Task<bool> PurchaseItem(int userId, int itemId, int quantity);
        Task<bool> PurchaseItemByName(int userId, string item);
    }
}
