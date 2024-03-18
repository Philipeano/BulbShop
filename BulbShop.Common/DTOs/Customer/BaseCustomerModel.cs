using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Common.DTOs.Customer
{
    public record BaseCustomerModel
    {
        [Required(ErrorMessage = "First Name is required."), StringLength(maximumLength: 50, MinimumLength = 3)]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required."), StringLength(maximumLength: 50, MinimumLength = 3)]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Phone Number is required."), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Email Address is required."), DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
    }
}
