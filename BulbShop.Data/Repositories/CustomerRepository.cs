using AutoMapper;
using BulbShop.Common.DTOs.Customer;
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
    public interface ICustomerRepository
    {
        public CustomerDto AddCustomer(AddCustomerDto newCustomer);

        public CustomerDto UpdateCustomer(CustomerDto customer);

        public bool DeleteCustomer(Guid id);

        public CustomerDto GetCustomer(Guid id);

        public IEnumerable<CustomerDto> GetAllCustomers();
    }


    public class CustomerRepository : ICustomerRepository
    {
        private readonly BulbShopContext _context;
        private readonly IMapper _mapper;

        public CustomerRepository(BulbShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public CustomerDto AddCustomer(AddCustomerDto newCustomer)
        {
            CustomerDto newlyAddedCustomer = null;
            var customerToAdd = _mapper.Map<Customer>(newCustomer);
            if (customerToAdd != null)
            {
                customerToAdd.Id = Guid.NewGuid();
                customerToAdd.CreatedOn = DateTime.Now;
                customerToAdd.ModifiedOn = DateTime.Now;
                _context.Customers.Add(customerToAdd);
                newlyAddedCustomer = _mapper.Map<CustomerDto>(customerToAdd);
            }
            return newlyAddedCustomer;
        }


        public bool DeleteCustomer(Guid id)
        {
            var customerToDelete = _context.Customers.Find(id);
            if (customerToDelete != null)
            {
                _context.Customers.Remove(customerToDelete);
                return true;
            }
            return false;
        }


        public IEnumerable<CustomerDto> GetAllCustomers()
        {
            return _context.Customers
                           .AsNoTracking()
                           .Select(c => _mapper.Map<CustomerDto>(c))
                           .ToList();
        }


        public CustomerDto GetCustomer(Guid id)
        {
            CustomerDto customerToReturn = null;
            var customer = _context.Customers
                                  .AsNoTracking()
                                  .SingleOrDefault(c => c.Id == id);
            if (customer != null)
            {
                customerToReturn = _mapper.Map<CustomerDto>(customer);
            }
            return customerToReturn;
        }


        public CustomerDto UpdateCustomer(CustomerDto customer)
        {
            CustomerDto newlyUpdatedCustomer = null;
            var existingCustomer = _context.Customers.Find(customer.Id);
            var customerWithNewInfo = _mapper.Map(customer, existingCustomer);
            if (customerWithNewInfo != null)
            {
                customerWithNewInfo.ModifiedOn = DateTime.Now;
                _context.Entry(customerWithNewInfo).State = EntityState.Modified;
                _context.Customers.Update(customerWithNewInfo);
                newlyUpdatedCustomer = _mapper.Map<CustomerDto>(customerWithNewInfo);
            }
            return newlyUpdatedCustomer;
        }
    }
}
