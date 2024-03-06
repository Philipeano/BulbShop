using BulbShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Common.DTOs.Product
{
    public record ProductDto : BaseProductModel
    {
        public Guid Id { get; set; }


        [Required(ErrorMessage = "Indicate how many units of this product are now available in the store.")]
        public int QuantityInStock { get; set; }
    }
}
