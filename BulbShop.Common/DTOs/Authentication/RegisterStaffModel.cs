using System.ComponentModel.DataAnnotations;

namespace BulbShop.Common.DTOs.Authentication
{
    public record RegisterStaffModel : RegisterModel
    {
        [Required(ErrorMessage = "At least one role must be specified.")]
        public string[] Roles { get; set; }
    }
}
