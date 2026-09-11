
using EcommerceBackend.Data;
using EcommerceBackend.Entities;
using EcommerceBackend.Repositories;
using EcommerceBackend.Services;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;     

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
IServiceCollection serviceCollection = builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>)) ;
// Register the ProductServices and CategoryService with the DI container
builder.Services.AddScoped<IProductService, ProductServices>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!db.Categories.Any())
    {
        var electronics = new Category { Name = "Electronics" };
        var books = new Category { Name = "Books" };
        db.Categories.AddRange(electronics, books);
        db.SaveChanges();
        db.Products.AddRange(
        new Product { ProductName = "Wireless Mouse", ProductPrice = 19.99m, ProductQuantity = 50, ProductCategoryId = electronics.Id },
        new Product { ProductName = "Mechanical Keyboard", ProductPrice = 79.99m, ProductQuantity = 20, ProductCategoryId = electronics.Id },
        new Product { ProductName = "Clean Code", ProductPrice = 34.99m, ProductQuantity = 15, ProductCategoryId = books.Id }
        );
        db.SaveChanges();
    }
}

app.Run();