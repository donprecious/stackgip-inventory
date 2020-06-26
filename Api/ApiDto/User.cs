using System.Collections.Generic;
using Newtonsoft.Json;
using StackgipInventory.Dto;

namespace StackgipInventory.ApiDto
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]

        public string Email { get; set; }

        [JsonProperty("firstName")]

        public string FirstName { get; set; }
        [JsonProperty("lastName")]

        public string LastName { get; set; }
    }

    public class UserResponse : ResponseDto
    {
        [JsonProperty("data")]
        public new User Data  {get;set;}
    }
 

    public class UserApiResponse : RefitApiResponse
    {
        public new UserResponse Data  {get;set;}

    }
    public class UserListResponse : ResponseDto
    {
        [JsonProperty("data")]
        public new List<User> Data  {get;set;}
    }
    public class UserListApiResponse : RefitApiResponse
    {
        public new UserListResponse Data  {get;set;}

    }

}
