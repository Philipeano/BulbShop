using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Data.Entities
{
    public class OrderItem
    {
        [Required]
        public Guid ProductId { get; set; }


        [Required, ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }


        [Required]
        public Guid OrderId { get; set; }


        [Required, ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }


        [Required]
        public int Quantity { get; set; }
    }
}
