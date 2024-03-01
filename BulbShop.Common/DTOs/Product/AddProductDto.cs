using BulbShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Common.DTOs.Product
{
    public record AddProductDto : BaseProductModel
    {
        [Required(ErrorMessage = "Indicate how many units of this product you are adding to the store.")]
        public int InitialQuantity { get; set; }
    }
}
