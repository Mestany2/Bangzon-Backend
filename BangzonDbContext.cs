using Microsoft.EntityFrameworkCore;
using Bangzon.Models;
using System.Runtime.CompilerServices;

public class BangzonDbContext : DbContext
{

    public DbSet<User> Users { get; set; }
    public DbSet<UserPaymentType> UserPaymentTypes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<PaymentType> PaymentTypes { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Category> Categories { get; set; }

    public BangzonDbContext(DbContextOptions<BangzonDbContext> context) : base(context)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // seed data with Users
        modelBuilder.Entity<User>().HasData(new User[]
        {
        new User {Id = 1, UserName = "Jack Smith", IsSeller = true},
        new User {Id = 2, UserName = "Chris Lee", IsSeller = false},
        new User {Id = 3, UserName = "Steve Wilson", IsSeller = false},
        new User {Id = 4, UserName = "Mike Larry", IsSeller = true},

        });

        modelBuilder.Entity<UserPaymentType>().HasData(new UserPaymentType[]
{
         new UserPaymentType {Id = 1, UserId = 4, PaymentTypeId = 2},
         new UserPaymentType {Id = 2, UserId = 2, PaymentTypeId = 4},
         new UserPaymentType {Id = 3, UserId = 3, PaymentTypeId = 1},
         new UserPaymentType {Id = 4, UserId = 1, PaymentTypeId = 3},
});
        modelBuilder.Entity<Product>().HasData(new Product[]
{
        new Product { Id = 1, ProductName = "Shoes", UserId = 2, ProductPrice = 11.99M, CategoryId = 1, ProductDescription = "Pair of shoes"},
        new Product { Id = 2, ProductName = "Shirt", UserId = 1, ProductPrice = 10.49M, CategoryId = 3, ProductDescription = "White Polo" },
        new Product { Id = 3, ProductName = "Pants", UserId = 3, ProductPrice = 5.79M, CategoryId = 2, ProductDescription = "Black Jeans" },

});
        modelBuilder.Entity<PaymentType>().HasData(new PaymentType[]
{
         new PaymentType {Id = 1, PaymentTypeName = "Credit Card"},
         new PaymentType {Id = 2, PaymentTypeName = "Gift Card"},
         new PaymentType {Id = 3, PaymentTypeName = "PayPal"},

});
        modelBuilder.Entity<OrderStatus>().HasData(new OrderStatus[]
{
         new OrderStatus {Id = 1, Status = "Completed"},
         new OrderStatus {Id = 2, Status = "Shipped"},
         new OrderStatus {Id = 3, Status = "Ordered"},


});
        modelBuilder.Entity<Order>().HasData(new Order[]
{
         new Order {Id = 1, UserId = 1, OrderStatusId=3},
         new Order {Id = 2, UserId = 3, OrderStatusId=1},
         new Order {Id = 3, UserId = 2, OrderStatusId=2},

});

        modelBuilder.Entity<Category>().HasData(new Category[]
{
         new Category {Id = 1, CategoryName="Shoes"},
         new Category {Id = 2, CategoryName="Tops"},
         new Category {Id = 3, CategoryName="Bottoms"},

});
        var OrderProduct = modelBuilder.Entity("OrderProduct");
        OrderProduct.HasData(new []
            {
            new { OrdersId = 1, ProductsId = 1 },
            new { OrdersId = 1, ProductsId = 2 },
            new { OrdersId = 1, ProductsId = 3 },
            });

    }
}