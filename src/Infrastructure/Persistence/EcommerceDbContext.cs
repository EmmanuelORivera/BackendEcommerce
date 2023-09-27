using Ecommerce.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Persistence;

public class EcommerceDbContext : IdentityDbContext<User>
{
    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().Property(x => x.Id).HasMaxLength(36);
        builder.Entity<User>().Property(x => x.NormalizedUserName).HasMaxLength(90);
        builder.Entity<IdentityRole>().Property(x => x.Id).HasMaxLength(36);
        builder.Entity<IdentityRole>().Property(x => x.NormalizedName).HasMaxLength(90);
    }

    public DbSet<Product>? Products { get; set; }
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Image>? Images { get; set; }
    public DbSet<Address>? Addresses { get; set; }
    public DbSet<Order>? Orders { get; set; }
    public DbSet<OrderItem>? OrderItems { get; set; }
    public DbSet<Review>? Reviews { get; set; }
    public DbSet<ShoppingCart>? ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem>? ShoppingCartItems { get; set; }
    public DbSet<Country>? Countries { get; set; }
    public DbSet<OrderAddress>? OrderAddresses { get; set; }
}