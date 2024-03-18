using AutoMapper;
using BulbShop.Common.DTOs.Customer;
using BulbShop.Common.DTOs.Product;
using BulbShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Data.MappingProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<AddCustomerDto, Customer>();
            CreateMap<CustomerDto, Customer>().ForMember(dest => dest.CreatedOn, act => act.Ignore());
            CreateMap<Customer, CustomerDto>();
        }
    }
}
