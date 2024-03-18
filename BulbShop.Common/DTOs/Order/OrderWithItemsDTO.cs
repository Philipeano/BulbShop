using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Common.DTOs.Order
{
    public record OrderWithItemsDTO : OrderSummaryDTO
    {

        public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}
