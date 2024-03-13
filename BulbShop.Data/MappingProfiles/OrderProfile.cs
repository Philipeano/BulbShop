using AutoMapper;
using BulbShop.Common.DTOs.Customer;
using BulbShop.Common.DTOs.Order;
using BulbShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Data.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddOrderDTO, Order>();

            CreateMap<Order, OrderSummaryDTO>()
                .ForMember(dest => dest.OrderDate, act => act.MapFrom(src => src.CreatedOn))
                .ForPath(dest => dest.NumberOfItems, opt => opt.MapFrom(src => GetItemCount(src.Items)));

            CreateMap<Order, OrderWithItemsDTO>()
                .ForMember(dest => dest.OrderDate, act => act.MapFrom(src => src.CreatedOn))
                .ForPath(dest => dest.NumberOfItems, opt => opt.MapFrom(src => GetItemCount(src.Items)));

            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<OrderItemDTO, OrderItem>();
        }

        private int GetItemCount(IEnumerable<OrderItem> items)
        {
            int result = 0;
            foreach (var item in items)
            {
                result = result + item.Quantity;
            }
            return result;
        }
    }
}
