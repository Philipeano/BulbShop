using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Common.DTOs.Customer
{
    public record CustomerDto : BaseCustomerModel
    {
        public Guid Id { get; set; }

    }
}
