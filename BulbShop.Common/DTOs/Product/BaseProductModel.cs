using BulbShop.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Common.DTOs.Product
{
    public record BaseProductModel
    {
        [Required(ErrorMessage = "Product brand name is required.")]
        public string BrandName { get; set; }


        [Required(ErrorMessage = "Product category must be specified.")]
        public ProductCategory Category { get; set; }


        [Required(ErrorMessage = "Provide a description of the product.")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Product manufacturer must be specified.")]
        public Manufacturer Manufacturer { get; set; }


        [Required(ErrorMessage = "Product price must be specified."), DataType(DataType.Currency)]
        public double Price { get; set; }
    }
}
