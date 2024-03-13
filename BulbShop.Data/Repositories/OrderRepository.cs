using AutoMapper;
using BulbShop.Common.DTOs.Order;
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
    public interface IOrderRepository
    {
        public OrderSummaryDTO AddOrder(AddOrderDTO newOrder);

        public OrderSummaryDTO UpdateOrderStatus(Guid id, OrderStatus newStatus);

        public bool DeleteOrder(Guid id);

        public OrderWithItemsDTO GetOrder(Guid id);

        public IEnumerable<OrderSummaryDTO> GetAllOrders();
    }


    public class OrderRepository : IOrderRepository
    {
        private readonly BulbShopContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(BulbShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public OrderSummaryDTO AddOrder(AddOrderDTO newOrder)
        {
            OrderSummaryDTO newlyAddedOrder = null;
            var orderToAdd = _mapper.Map<Order>(newOrder);
            if (orderToAdd != null)
            {
                orderToAdd.Id = Guid.NewGuid();
                orderToAdd.CreatedOn = DateTime.Now;
                orderToAdd.ModifiedOn = DateTime.Now;
                orderToAdd.Status = OrderStatus.Pending;
                _context.Orders.Add(orderToAdd);
            }
            newlyAddedOrder = _mapper.Map<OrderSummaryDTO>(orderToAdd);            
            return newlyAddedOrder;
        }

        public bool DeleteOrder(Guid id)
        {
            var orderToDelete = _context.Orders.Find(id);
            if (orderToDelete != null)
            {
                _context.Orders.Remove(orderToDelete);
                return true;
            }
            return false;
        }

        public IEnumerable<OrderSummaryDTO> GetAllOrders()
        {
            return _context.Orders
                           .Include(o => o.Items)
                           .AsNoTracking()
                           .Select(o => _mapper.Map<OrderSummaryDTO>(o))
                           .ToList();
        }

        public OrderWithItemsDTO GetOrder(Guid id)
        {
            OrderWithItemsDTO orderToReturn = null;
            var order = _context.Orders
                                .Include(o => o.Items)
                                .AsNoTracking()
                                .SingleOrDefault(o => o.Id == id);
            if (order != null)
            {
                orderToReturn = _mapper.Map<OrderWithItemsDTO>(order);
            }
            return orderToReturn;
        }

        public OrderSummaryDTO UpdateOrderStatus(Guid id, OrderStatus newStatus)
        {
            OrderSummaryDTO newlyUpdatedOrder = null;
            var existingOrder = _context.Orders.Find(id);
            if (existingOrder != null)
            {
                existingOrder.Status = newStatus;
                existingOrder.ModifiedOn = DateTime.Now;
                _context.Entry(existingOrder).State = EntityState.Modified;
                _context.Orders.Update(existingOrder);
                newlyUpdatedOrder = _mapper.Map<OrderSummaryDTO>(existingOrder);
            }
            return newlyUpdatedOrder;
        }
    }
}
