using System.ComponentModel.DataAnnotations;

namespace BulbShop.Common.DTOs.Authentication
{
    public record RegisterModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }


        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required."), MinLength(6)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Password confirmation is required."), MinLength(6)]
        public string ConfirmPassword { get; set; }
    }
}
