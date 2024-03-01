using AutoMapper;
using BulbShop.Common.DTOs.Product;
using BulbShop.Data;
using BulbShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("BulbShopConnStr");
builder.Services.AddDbContext<BulbShopContext>(options => options.UseSqlServer(connectionString));

// TODO: Register repository classes here 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<AddProductDto, Product>().ForMember(dest => dest.QuantityInStock, act => act.MapFrom(src => src.InitialQuantity));
    cfg.CreateMap<UpdateProductDto, Product>();
    cfg.CreateMap<Product, BaseProductModel>();
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
