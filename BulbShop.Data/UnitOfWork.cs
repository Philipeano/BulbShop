﻿using BulbShop.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Data
{
    public interface IUnitOfWork //: IDisposable
    {
        // A method for persisting all changes
        public void Commit();

        // A method for discarding all changes
        public void Rollback();

        // A method for disposing the object and freeing system resources
        // public void Dispose();

        // A number of repository properties to handle data access
        public IProductRepository ProductRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderItemRepository OrderItemRepository { get; }
    }



    public class UnitOfWork : IUnitOfWork
    {
        // private bool _disposedValue;
        private readonly BulbShopContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public UnitOfWork(BulbShopContext context, 
                          IProductRepository productRepository, 
                          ICustomerRepository customerRepository, 
                          IOrderRepository orderRepository, 
                          IOrderItemRepository orderItemRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public IProductRepository ProductRepository => _productRepository;

        public ICustomerRepository CustomerRepository => _customerRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public IOrderItemRepository OrderItemRepository => _orderItemRepository;

        public void Commit()
        {
            _context.SaveChanges();
        }


        public void Rollback()
        {
            // Check if there are any pending changes being tracked by the DbContext, then revert them
            if (_context.ChangeTracker.HasChanges())
            {
                var modifiedEntities = _context.ChangeTracker.Entries()
                    .Where(e => e.State != EntityState.Unchanged);

                foreach (var entity in modifiedEntities)
                {
                    if (entity.State == EntityState.Added)
                        entity.State = EntityState.Detached;
                    else
                        entity.Reload();
                }
            }
        }


        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!_disposedValue)
        //    {
        //        if (disposing)
        //        {
        //            // TODO: dispose managed state (managed objects)
        //        }

        //        // TODO: free unmanaged resources (unmanaged objects) and override finalizer
        //        // TODO: set large fields to null
        //        _disposedValue = true;
        //    }
        //}

        //// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        //// ~UnitOfWork()
        //// {
        ////     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        ////     Dispose(disposing: false);
        //// }

        //public void Dispose()
        //{
        //    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //    Dispose(disposing: true);
        //    GC.SuppressFinalize(this);
        //}
    }
}
