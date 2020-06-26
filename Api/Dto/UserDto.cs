using System.Collections.Generic;

namespace StackgipInventory.Dto
{
    public class UserDto
    {
        public  string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Scopes { get; set; }
    }
}
