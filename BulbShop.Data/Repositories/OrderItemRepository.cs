using AutoMapper;
using BulbShop.Common.DTOs.Order;
using BulbShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Data.Repositories
{
    public interface IOrderItemRepository
    {
        public IEnumerable<OrderItemDTO> GetOrderItems(Guid orderId);

        public OrderItemDTO AddOrderItem(Guid orderId, OrderItemDTO orderItem);

        public OrderItemDTO ChangeItemQuantity(Guid orderId, Guid productId, int newQuantity);

        public bool DeleteOrderItem(Guid orderId, Guid productId);
    }

    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly BulbShopContext _context;
        private readonly IMapper _mapper;

        public OrderItemRepository(BulbShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public OrderItemDTO AddOrderItem(Guid orderId, OrderItemDTO orderItem)
        {
            /* 
             * Check if the order already contains the product being added
             * If yes, simply change the quantity
             * If no, then include the product with the specified quantity
             */
            OrderItemDTO orderItemToReturn = null;
            var existingOrderItem = _context.OrderItems
                                            .AsNoTracking()
                                            .SingleOrDefault(i => i.OrderId == orderId && i.ProductId == orderItem.ProductId);

            if (existingOrderItem == null)  // Add new
            {               
                var newOrderItem = new OrderItem
                {
                    OrderId = orderId,
                    ProductId = orderItem.ProductId,
                    Quantity = orderItem.Quantity,
                };
                _context.OrderItems.Add(newOrderItem);
                orderItemToReturn = _mapper.Map<OrderItemDTO>(newOrderItem);
            }
            else // Change quantity
            {                
                existingOrderItem.Quantity = orderItem.Quantity;
                _context.OrderItems.Update(existingOrderItem);
                orderItemToReturn = _mapper.Map<OrderItemDTO>(existingOrderItem);
            }

            return orderItemToReturn;
        }

        public OrderItemDTO ChangeItemQuantity(Guid orderId, Guid productId, int newQuantity)
        {
            /* 
             * Check if the order currently contains the product whose quantity is being changed
             * If yes, simply change the quantity
             * If no, abort the update and return null
             */
            OrderItemDTO orderItemToReturn = null;
            var existingOrderItem = _context.OrderItems
                                       .SingleOrDefault(i => i.OrderId == orderId && i.ProductId == productId);

            if (existingOrderItem != null)  
            {
                existingOrderItem.Quantity = newQuantity;
                _context.OrderItems.Update(existingOrderItem);
                orderItemToReturn = _mapper.Map<OrderItemDTO>(existingOrderItem);
            }

            return orderItemToReturn;
        }

        public bool DeleteOrderItem(Guid orderId, Guid productId)
        {
            var orderItemToDelete = _context.OrderItems
                                       .SingleOrDefault(i => i.OrderId == orderId && i.ProductId == productId);

            if (orderItemToDelete != null)
            {
                _context.OrderItems.Remove(orderItemToDelete);
                return true;
            }

            return false;
        }

        public IEnumerable<OrderItemDTO> GetOrderItems(Guid orderId)
        {
            return _context.OrderItems
                           .AsNoTracking()
                           .Select(i => _mapper.Map<OrderItemDTO>(i))
                           .ToList();
        }
    }
}
