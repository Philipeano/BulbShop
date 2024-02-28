using BulbShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Data.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }


        [Required]
        public string BrandName { get; set; }


        [Required]
        public ProductCategory Category { get; set; } // TODO: Extract as separate table


        [Required]
        public string Description { get; set; }


        [Required]
        public Manufacturer Manufacturer { get; set; } // TODO: Extract as separate table


        [Required, DataType(DataType.Currency)]
        public double Price { get; set; }


        [Required]
        public int QuantityInStock { get; set;  }


        public DateTime CreatedOn { get; set; }


        public DateTime ModifiedOn { get; set; }
    }
}
