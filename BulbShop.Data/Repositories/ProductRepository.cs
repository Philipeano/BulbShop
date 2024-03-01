using AutoMapper;
using BulbShop.Common.DTOs.Product;
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
        public BaseProductModel AddProduct(AddProductDto newProduct);

        public BaseProductModel UpdateProduct(UpdateProductDto product);

        public bool DeleteProduct(Guid id);

        public BaseProductModel GetProduct(Guid id);

        public IEnumerable<BaseProductModel> GetAllProducts();
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


        public BaseProductModel AddProduct(AddProductDto newProduct)
        {
            var newlyAddedProduct = new BaseProductModel();
            var productToAdd = _mapper.Map<Product>(newProduct);
            if (productToAdd != null)
            {
                productToAdd.Id = Guid.NewGuid();
                productToAdd.CreatedOn = DateTime.Now;
                productToAdd.ModifiedOn = DateTime.Now;
                _context.Products.Add(productToAdd);
                newlyAddedProduct = _mapper.Map<BaseProductModel>(productToAdd);
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


        public IEnumerable<BaseProductModel> GetAllProducts()
        {
            return _context.Products
                           .Select(p => _mapper.Map<BaseProductModel>(p))
                           .ToList();
        }


        public BaseProductModel GetProduct(Guid id)
        {
            var productToReturn = new BaseProductModel();
            var product = _context.Products.Find(id);
            if (product != null)
            {
                productToReturn = _mapper.Map<BaseProductModel>(product);
            }
            return productToReturn;
        }


        public BaseProductModel UpdateProduct(UpdateProductDto product)
        {
            var newlyUpdatedProduct = new BaseProductModel();
            var productWithNewInfo = _mapper.Map<Product>(product);
            if (productWithNewInfo != null)
            {
                productWithNewInfo.ModifiedOn = DateTime.Now;
                _context.Products.Update(productWithNewInfo);
                newlyUpdatedProduct = _mapper.Map<BaseProductModel>(productWithNewInfo);
            }
            return newlyUpdatedProduct;
        }
    }

}
