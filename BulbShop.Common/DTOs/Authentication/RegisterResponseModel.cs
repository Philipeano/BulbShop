namespace BulbShop.Common.DTOs.Authentication
{
    public record RegisterResponseModel
    {
        public bool IsSuccessful { get; set; }

        public string Message { get; set; }
    }
}
