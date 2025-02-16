
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using AvitoMerchShop.Data;
using Microsoft.Extensions.Configuration;
using AvitoMerchShop.Data.Services;
using AvitoMerchShop.Core.Entities;
using AvitoMerchShop.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AvitoMerchShop.Tests;
public class UserServiceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly UserService _service;
    private readonly IConfiguration _config;

    public UserServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Jwt:SecretKey"] = "supersecretkeywith32characters1234567890",
                ["Jwt:Issuer"] = "test",
                ["Jwt:Audience"] = "test"
            }).Build();

        _service = new UserService(_context, _config);

        // Инициализация тестовых данных
        SeedTestData();
    }

    private void SeedTestData()
    {
        _context.Items.AddRange(
            new Item { Name = "t-shirt", Price = 80 },
            new Item { Name = "cup", Price = 20 }
        );
        _context.SaveChanges();
    }

    [Fact]
    public async Task NewUser_Has1000Coins()
    {

        var user = await _service.Authenticate("new_user", "pass");

    
        Assert.Equal(1000, user.CoinsBalance);
    }

    [Fact]
    public async Task PurchaseItem_ReducesBalanceCorrectly()
    {
      
        var user = await _service.Authenticate("user1", "pass");
        var item = _context.Items.First(i => i.Name == "t-shirt");


        var result = await _service.PurchaseItem(user.Id, item.Id, 2);

    
        Assert.True(result);
        Assert.Equal(1000 - 80 * 2, user.CoinsBalance);
    }

    [Fact]
    public async Task TransferCoins_UpdatesBothUsers()
    {
      
        var user1 = await _service.Authenticate("user2", "pass");
        var user2 = await _service.Authenticate("user3", "pass");


        var result = await _service.TransferCoins(user1.Id, user2.Id, 300);

    
        Assert.True(result);
        Assert.Equal(1000 - 300, user1.CoinsBalance);
        Assert.Equal(1000 + 300, user2.CoinsBalance);
    }
    [Fact]
    public async Task Authenticate_ExistingUser_ReturnsUserWithSamePassword()
    {
      
        var existingUser = new User { Username = "existing", Password = "pass123" };
        _context.Users.Add(existingUser);
        await _context.SaveChangesAsync();


        var result = await _service.Authenticate("existing", "pass123");

    
        Assert.NotNull(result);
        Assert.Equal(existingUser.Id, result.Id);
    }

  

    [Fact]
    public void GenerateJwtToken_ValidUser_ReturnsTokenWithClaims()
    {
      
        var user = new User { Id = 1, Username = "testuser" };


        var token = _service.GenerateJwtToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

    
        Assert.Equal("testuser", jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value);
        Assert.Equal("1", jwt.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }

    public void Dispose() => _context.Dispose();
}