using System;
using Newtonsoft.Json;
using StackgipInventory.Dto;

namespace StackgipInventory.ApiDto
{
 
    public class AuthResponse
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
        public DateTime refreshTokenExpiry { get; set; }
    }

    public class AuthApiResponse : ResponseDto
    {
        [JsonProperty("data")]
        public new AuthResponse Data { get; set; }
    }

    public class AuthApiRefitResponse : RefitApiResponse
    {

        public new AuthApiResponse Data { get; set; }
    }
}
