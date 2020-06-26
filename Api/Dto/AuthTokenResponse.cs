using System;

namespace StackgipInventory.Dto
{
    public class AuthTokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
