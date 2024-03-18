using AutoMapper;
using BulbShop.Common.DTOs.Product;
using BulbShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Data.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductDto, Product>().ForMember(dest => dest.QuantityInStock, act => act.MapFrom(src => src.InitialQuantity));
            CreateMap<ProductDto, Product>().ForMember(dest => dest.CreatedOn, act => act.Ignore());
            CreateMap<Product, ProductDto>();
        }
    }
}
