using BulbShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Data.Entities
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }


        [Required, ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }


        public OrderStatus Status { get; set; }


        public DateTime CreatedOn { get; set; }


        public DateTime ModifiedOn { get; set; }


        public virtual ICollection<OrderItem> Items { get; set; }

    }
}
