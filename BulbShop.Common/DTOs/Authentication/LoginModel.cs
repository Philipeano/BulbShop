using System.ComponentModel.DataAnnotations;

namespace BulbShop.Common.DTOs.Authentication
{
    public record LoginModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
