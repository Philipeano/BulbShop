using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Data.Entities
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }


        [Required, StringLength(maximumLength: 50, MinimumLength = 3)]
        public string FirstName { get; set; }


        [Required, StringLength(maximumLength: 50, MinimumLength = 3)]
        public string LastName { get; set; }


        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }


        [Required, DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }


        public DateTime CreatedOn { get; set; }


        public DateTime ModifiedOn { get; set; }
    }
}
