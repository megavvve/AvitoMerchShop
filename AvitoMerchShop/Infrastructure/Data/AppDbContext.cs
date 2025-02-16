using AvitoMerchShop.Core.Entities;

using Microsoft.EntityFrameworkCore;

namespace AvitoMerchShop.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<CoinTransaction> CoinTransactions { get; set; }
        public DbSet<Item> Items { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoinTransaction>()
                .HasOne(t => t.FromUser)
                .WithMany(u => u.SentTransactions)
                .HasForeignKey(t => t.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CoinTransaction>()
                .HasOne(t => t.ToUser)
                .WithMany(u => u.ReceivedTransactions)
                .HasForeignKey(t => t.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.User)
                .WithMany(u => u.Purchases)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Item)
                .WithMany()
                .HasForeignKey(p => p.ItemId);
        }
    }
}
