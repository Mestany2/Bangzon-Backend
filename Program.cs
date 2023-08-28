using Bangzon.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using System.Linq;
using System.Dynamic;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

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

app.MapPost("/api/Orderlist", (BangzonDbContext db, int oId, List<int> pIds) =>
    {
        var getOrder = db.Orders.FirstOrDefault(o => o.Id == oId);
        var listProductIds = db.Products.Where(p => pIds.Contains(p.Id)).ToList();
        if (getOrder.Products == null)
        {
            getOrder.Products = new List<Product>();
        }
        foreach ( var productId in listProductIds)
        {
            getOrder.Products.Add(productId);
        }

      
           
            db.SaveChanges();
            return getOrder;


    });

//app.MapGet("/api/Orders/{id}", (BangzonDbContext db, int id) =>
//{
//    var products = db.OrderProducts
//    .Where(po => po.OrderId == id)
//    .SelectMany(po => db.Products.Where(p => p.Id == po.ProductId))
//    .ToList();
//    return products;
//});

//app.MapGet("/api/Orders/{id}", (BangzonDbContext db, int id) =>
//{
//    var test = db.Orders
//    .Where(o => o.Id == id)
//    .Select(o => new
//    {
//        Order = o,
//        Products = db.OrderProducts
//        .Where(op => op.OrderId == id)
//        .SelectMany(op => db.Products.Where(p => p.Id == op.ProductId))
//        .ToList()
//    })
//    .FirstOrDefault();


//    return test;
//});

//app.MapGet("/api/Orders/{id}", (BangzonDbContext db, int id) =>
//{
//    var test = db.Orders
//    .Include(o =>o.OrderProducts)
//    .ThenInclude(op => op.Products)
//    .SingleOrDefault(o=>o.Id == id);
//    if (test != null)
//    {
//        var products =
//        test.OrderProducts.Select
//        (x => x.Product).ToList();
//        return products;
//    }

//    return test;
//});




//app.MapGet("/OrderProduct/{id}", (int id) =>
//{
//    ServiceTicket serviceTicket = serviceTicketsList.FirstOrDefault(st => st.Id == id);
//    if (serviceTicket == null)
//    {
//        return Results.NotFound();
//    }
//    serviceTicket.Employee = employeeList.FirstOrDefault(e => e.Id == serviceTicket.EmployeeId);
//    serviceTicket.Customer = customerList.FirstOrDefault(e => e.Id == serviceTicket.CustomerId);
//    return Results.Ok(serviceTicket);
//});
app.Run();
