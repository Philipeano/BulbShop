using AutoMapper;
using BulbShop.Common.DTOs.Product;
using BulbShop.Data;
using BulbShop.Data.Entities;
using BulbShop.Data.MappingProfiles;
using BulbShop.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("BulbShopConnStr");
builder.Services.AddDbContext<BulbShopContext>(options => options.UseSqlServer(connectionString));

// Configure mappings for AutoMapper
builder.Services.AddAutoMapper(typeof(ProductProfile));

// Register unit of work and repository classes
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
