using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
    public class TransferFlowTests : IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly AppDbContext _context;

        public TransferFlowTests()
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

            var user1 = new User { Username = "user1", CoinsBalance = 1000 };
            var user2 = new User { Username = "user2", CoinsBalance = 1000 };
            _context.Users.AddRange(user1, user2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task Transfer_UpdatesBalances()
        {
            var fromUser = _context.Users.First(u => u.Username == "user1");
            var toUser = _context.Users.First(u => u.Username == "user2");

            var response = await _client.PostAsJsonAsync("/api/user/transfer", new
            {
                FromUserId = fromUser.Id,
                ToUserId = toUser.Id,
                Amount = 300
            });

         
            response.EnsureSuccessStatusCode();

            var updatedFrom = await _context.Users.FindAsync(fromUser.Id);
            var updatedTo = await _context.Users.FindAsync(toUser.Id);

            Assert.Equal(700, updatedFrom.CoinsBalance);
            Assert.Equal(1300, updatedTo.CoinsBalance);
        }

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}