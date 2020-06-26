using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StackgipInventory.Helpers
{
    public static class JwtDecoder
    {
        public static IEnumerable<Claim> GetClaims(string token)
        {
            var decoded = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            return decoded?.Claims;
        }
    }
}
