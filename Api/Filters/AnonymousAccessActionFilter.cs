using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace StackgipInventory.Filters
{
    public class AnonymousAccessActionFilter : IAllowAnonymousFilter, IAuthorizationFilter, IOrderedFilter
    {
        private readonly IConfiguration _config;
        public int Order { get; set; }

        public AnonymousAccessActionFilter(IConfiguration config)
        {
            _config = config;
           
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
           // todo any special requirement needed 
            return ;
        }

        public bool ValidateCurrentToken(string token)
        {
            var mySecret = _config.GetSection("TokenSecret").Value;
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    IssuerSigningKey = mySecurityKey,
                    ValidateAudience = false,
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

    }




    public class AnonymousAccessAttribute : TypeFilterAttribute
    {
        public AnonymousAccessAttribute()
            : base(typeof(AnonymousAccessActionFilter))
        {
            base.Order = int.MinValue;
        }
    }
}