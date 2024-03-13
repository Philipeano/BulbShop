using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Common.DTOs.Order
{
    public record OrderItemDTO
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
