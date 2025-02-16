using System.Net.Http;
using System.Threading.Tasks;
using AvitoMerchShop.Core.Entities;
using AvitoMerchShop.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace AvitoMerchShop.Tests
{
    public class PurchaseFlowTests : IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly AppDbContext _context;

        public PurchaseFlowTests()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services => {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<AppDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDB");
                        });
                    });

                });

            _client = _factory.CreateClient();
            _context = _factory.Services.GetRequiredService<AppDbContext>();

          
            var item = new Item { Name = "t-shirt", Price = 80 };
            var user = new User { Username = "testuser", CoinsBalance = 1000 };
            _context.Items.Add(item);
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        [Fact]
        public async Task Purchase_ReducesBalance()
        {
          
            var user = _context.Users.First();
            var item = _context.Items.First();

       
            var response = await _client.GetAsync($"/api/buy/{item.Name}");


            response.EnsureSuccessStatusCode();

            var updatedUser = await _context.Users
                .Include(u => u.Purchases)
                .FirstAsync(u => u.Id == user.Id);

            Assert.Equal(920, updatedUser.CoinsBalance);
            Assert.Single(updatedUser.Purchases);
        }

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}