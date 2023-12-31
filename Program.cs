using Bangzon.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using System.Linq;
using System.Dynamic;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<BangzonDbContext>(builder.Configuration["BangzonDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                                "http://localhost:5169")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

var app = builder.Build();


//Add for Cors 
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
//ADD CORS (under Using statements)

app.MapControllers();

//app.MapPost("/api/Order", (BangzonDbContext db, Order newOrder) =>
//    {
//        db.Orders.Add(newOrder);
//        db.SaveChanges();
//        return Results.Created($"/api/Order/{newOrder.Id}", newOrder);
//    };

app.MapPost("/api/Order", (BangzonDbContext db, int oId, int pId) =>
    {
        var getOrder = db.Orders.FirstOrDefault(o => o.Id == oId);
        var getProduct = db.Products.FirstOrDefault(p => p.Id == pId);
        if (getOrder.Products == null)
        {
            getOrder.Products = new List<Product>();
        }
      
            getOrder.Products.Add(getProduct);
            db.SaveChanges();
            return getOrder;


    });


//Orders 
//View an Order with Products
app.MapGet("/api/Orders", (BangzonDbContext db, int id) =>
{
    var orders = db.Orders.Where(o => o.Id == id).Include(x => x.Products).FirstOrDefault();

    return orders;
 }
);
//Delete an Order
app.MapDelete("/api/order/{id}", (BangzonDbContext db, int id) =>
{
    Order order = db.Orders.SingleOrDefault(o => o.Id == id);
    if (order == null)
    {
        return Results.NotFound();
    }
    db.Orders.Remove(order);
    db.SaveChanges();
    return Results.NoContent();

});

//Create an Order with products
app.MapPost("/api/Orderlist", (BangzonDbContext db, int oId, List<int> pIds) =>
{
    var getOrder = db.Orders.FirstOrDefault(o => o.Id == oId);
    var listProductIds = db.Products.Where(p => pIds.Contains(p.Id)).ToList();
    if (getOrder.Products == null)
    {
        getOrder.Products = new List<Product>();
    }
    foreach (var productId in listProductIds)
    {
        getOrder.Products.Add(productId);
    }



    db.SaveChanges();
    return getOrder;


});
//Update an Order
app.MapPut("/api/Order/{id}", (BangzonDbContext db, int id, Order order) =>
{
    Order OrderToUpdate = db.Orders.SingleOrDefault(order => order.Id == id);
    if (OrderToUpdate == null)
    {
        return Results.NotFound();
    }
    OrderToUpdate.OrderStatusId = order.OrderStatusId;

    db.SaveChanges();
    return Results.NoContent();
});
//View an Order
app.MapGet("/api/OrderDetails", (BangzonDbContext db, int oId) =>
{
    var getOrder = db.Orders.FirstOrDefault(o => o.Id == oId);
    return getOrder;
}
);


//Products
//Delete a product
app.MapDelete("/api/product/{id}", (BangzonDbContext db, int id) =>
{
    Product product = db.Products.SingleOrDefault(p => p.Id == id);
    if (product == null)
    {
        return Results.NotFound();
    }
    db.Products.Remove(product);
    db.SaveChanges();
    return Results.NoContent();

});
//Update a product
app.MapPut("/api/Products/{id}", (BangzonDbContext db, int id, Product product) =>
{
    Product productToUpdate = db.Products.SingleOrDefault(product => product.Id == id);
    if (productToUpdate == null)
    {
        return Results.NotFound();
    }
    productToUpdate.ProductName = product.ProductName;
    productToUpdate.ProductPrice = product.ProductPrice;
    productToUpdate.ProductDescription = product.ProductDescription;

    db.SaveChanges();
    return Results.NoContent();
});
//View Seller's products
app.MapGet("/api/sllersproducts/{userId}", (BangzonDbContext db, int id) =>
{
    var product = db.Products.Where(x => x.UserId == id).ToList();
    return product;
});
//Add a product
app.MapPost("/api/products", (BangzonDbContext db, Product product) =>
{
    db.Products.Add(product);
    db.SaveChanges();
    return Results.Created($"/api/products/{product.Id}", product);
});

//Users
//Get user by id
app.MapGet("/api/user/{id}", (BangzonDbContext db, int id) =>
{
    var user= db.Users.Single(u => u.Id == id);
    return user;
});

//Create a User
app.MapPost("/api/user", (BangzonDbContext db, User user) =>
{
    db.Users.Add(user);
    db.SaveChanges();
    return Results.Created($"/api/user/{user.Id}", user);
});

//Delete a User
app.MapDelete("/api/user/{id}", (BangzonDbContext db, int id) =>
{
    User user = db.Users.SingleOrDefault(u => u.Id == id);
    if (user == null)
    {
        return Results.NotFound();
    }
    db.Users.Remove(user);
    db.SaveChanges();
    return Results.NoContent();

});

//Update a User
app.MapPut("/api/user/{id}", (BangzonDbContext db, User user, int id) =>
{
    User userToUpdate = db.Users.SingleOrDefault(u => u.Id == id);
    if (userToUpdate == null)
    {
        return Results.NotFound();
    }
    userToUpdate.UserName = user.UserName;
    userToUpdate.IsSeller = user.IsSeller;

    db.SaveChanges();
    return Results.Created($"/api/user/{user.Id}", user);
});


app.MapGet("/checkuser/{uid}", (BangzonDbContext db, string uid) =>
{
    var user = db.Users.Where(x => x.uid == uid).ToList();
    if(uid == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok (user);
    }
});
app.Run();
