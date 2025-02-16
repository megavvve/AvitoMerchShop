using System.Threading.Tasks;
using AvitoMerchShop.Core.Entities;
using AvitoMerchShop.Data;
using Microsoft.EntityFrameworkCore;

namespace AvitoMerchShop.Services
{
    public class ItemMapper : IItemMapper
    {
        private readonly AppDbContext _context;

        public ItemMapper(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Item> GetItemByIdAsync(int itemId)
        {
            return await _context.Items
                                 .FirstOrDefaultAsync(i => i.Id == itemId);
        }
    }

    public interface IItemMapper
    {
        Task<Item> GetItemByIdAsync(int itemId);
    }
}
