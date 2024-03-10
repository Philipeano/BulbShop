using AutoMapper;
using BulbShop.Common.DTOs.Product;
using BulbShop.Common.Enums;
using BulbShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Data.Repositories
{
    public interface IProductRepository
    {
        public ProductDto AddProduct(AddProductDto newProduct);

        public ProductDto UpdateProduct(ProductDto product);

        public bool DeleteProduct(Guid id);

        public ProductDto GetProduct(Guid id);

        public IEnumerable<ProductDto> GetAllProducts();

        public IEnumerable<ProductDto> GetFilteredProducts(string? brandName, string? categoryName, string? manufacturerName);
    }


    public class ProductRepository : IProductRepository
    {
        private readonly BulbShopContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(BulbShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public ProductDto AddProduct(AddProductDto newProduct)
        {
            ProductDto newlyAddedProduct = null;
            var productToAdd = _mapper.Map<Product>(newProduct);
            if (productToAdd != null)
            {
                productToAdd.Id = Guid.NewGuid();
                productToAdd.CreatedOn = DateTime.Now;
                productToAdd.ModifiedOn = DateTime.Now;
                _context.Products.Add(productToAdd);
                newlyAddedProduct = _mapper.Map<ProductDto>(productToAdd);
            }
            return newlyAddedProduct;
        }


        public bool DeleteProduct(Guid id)
        {
            var productToDelete = _context.Products.Find(id);
            if (productToDelete != null)
            {
                _context.Products.Remove(productToDelete);
                return true;
            }
            return false;
        }


        public IEnumerable<ProductDto> GetAllProducts()
        {
            return _context.Products
                           .AsNoTracking()
                           .Select(p => _mapper.Map<ProductDto>(p))
                           .ToList();
        }


        public IEnumerable<ProductDto> GetFilteredProducts(string? brandName, string? categoryName, string? manufacturerName)
        {
            var filteredProducts = new List<ProductDto>();
            var allProducts = _context.Products.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(brandName))
            {
                allProducts = allProducts.Where(p => p.BrandName.Contains(brandName));
            }

            if (!string.IsNullOrWhiteSpace(categoryName) 
                && Enum.TryParse(typeof(ProductCategory), categoryName, true, out object? category)
                && (ProductCategory)category != ProductCategory.Unknown)
            {
                allProducts = allProducts.Where(p => p.Category == (ProductCategory)category);
            }

            if (!string.IsNullOrWhiteSpace(manufacturerName)
                && Enum.TryParse(typeof(Manufacturer), manufacturerName, true, out object? manufacturer)
                && (Manufacturer)manufacturer != Manufacturer.Unknown)
            {
                allProducts = allProducts.Where(p => p.Manufacturer == (Manufacturer)manufacturer);
            }

            filteredProducts = allProducts.Select(p => _mapper.Map<ProductDto>(p)).ToList();
            return filteredProducts;
        }


        public ProductDto GetProduct(Guid id)
        {
            ProductDto productToReturn = null;
            var product = _context.Products
                                  .AsNoTracking()
                                  .SingleOrDefault(p => p.Id == id);
            if (product != null)
            {
                productToReturn = _mapper.Map<ProductDto>(product);
            }
            return productToReturn;
        }


        public ProductDto UpdateProduct(ProductDto product)
        {
            ProductDto newlyUpdatedProduct = null;
            var existingProduct = _context.Products.Find(product.Id);
            var productWithNewInfo = _mapper.Map(product, existingProduct);
            if (productWithNewInfo != null)
            {
                productWithNewInfo.ModifiedOn = DateTime.Now;
                _context.Entry(productWithNewInfo).State = EntityState.Modified;
                _context.Products.Update(productWithNewInfo);
                newlyUpdatedProduct = _mapper.Map<ProductDto>(productWithNewInfo);
            }
            return newlyUpdatedProduct;
        }
    }

}
