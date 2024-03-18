namespace BulbShop.Common.DTOs.Authentication
{
    public record LoginResponseModel
    {
        public bool IsSuccessful { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}
