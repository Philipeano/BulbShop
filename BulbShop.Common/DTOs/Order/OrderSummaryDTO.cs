using BulbShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Common.DTOs.Order
{
    public record OrderSummaryDTO
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime OrderDate { get; set; }

        public int NumberOfItems { get; set; }

    }
}
