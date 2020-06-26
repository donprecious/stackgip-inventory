using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackgipInventory.Helpers;

namespace StackgipInventory.Filters
{
    [Obsolete]
    public class AuthorizeAccessActionFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _config;
        private readonly string _roles;
        public AuthorizeAccessActionFilter(IConfiguration config, string roles)
        {
            _config = config;
            _roles = roles;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var bearer = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (bearer == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var token = bearer.Split(" ")[1];

            if (token == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var valid = ValidateCurrentToken(token);
            if (!valid)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!string.IsNullOrEmpty(_roles))
            {
                var rolesList = _roles.Split(",").Select(a => a.Trim()).ToList();

                var claims = JwtDecoder.GetClaims(token);
                if (claims == null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                if (claims != null)
                {
                    bool hasRole = false;
                    var userRoles = claims.Where(claim => claim.Type == "roles").ToList();

                    foreach (var r in userRoles)
                    {
                        if (rolesList.Contains(r.Value))
                        {
                            hasRole = true;
                            break;
                        }
                    }

                    if (hasRole == false)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                }
            }
        }

        public bool ValidateCurrentToken(string token)
        {
            try
            {
                var mySecret = _config.GetSection("TefIdentity.TokenSecret").Value;
                var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    IssuerSigningKey = mySecurityKey,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }

    [Obsolete]
    public class AuthorizeAccessAttribute : TypeFilterAttribute
    {
        public AuthorizeAccessAttribute(string roles)
            : base(typeof(AuthorizeAccessActionFilter))
        {
            Arguments = new object[] { roles };
        }
    }
}
