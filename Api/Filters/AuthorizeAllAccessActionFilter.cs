using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackgipInventory.Helpers;
using StackgipInventory.Shared;
using ClaimTypes = StackgipInventory.Shared.ClaimTypes;

namespace StackgipInventory.Filters
{
    public class AuthorizeAllAccessActionFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _config;
        private readonly string _roles;
        private readonly string _scopes;

        public AuthorizeAllAccessActionFilter(IConfiguration config,  string scopes,string roles)
        {
            _config = config;
            _roles = roles;
            _scopes = scopes;

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var filters = context.Filters;
            for (var i = 0; i < filters.Count; i++)
            {
                if (filters[i] is IAllowAnonymousFilter)
                {
                    return ;
                }
            }

            var bearer = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(bearer))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var token = bearer.Split(" ")[1];
            var valid = ValidateCurrentToken(token);
            if (!valid)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var claims = JwtDecoder.GetClaims(token).ToList();
            if (!claims.Any())
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var tokenType = claims.FirstOrDefault(a => a.Type == "tokenType");
            if (tokenType == null)
            {
                context.Result = new CustomUnauthorizedResult("unauthorize access");
                return;
            }
          
            //required roles to get access 
            var requiredRoles = _roles.Split(',').ToList();

            // user roles 
            var userRole = claims
                .Where(claim => claim.Type == ClaimTypes.Role)
                .Select(a=>a.Value).Select(a=>a.ToLower()).ToList(); 
            var userRoles = new List<string>();
            userRole.ForEach(a =>
            {
                var scopes = a.ToLower().Split(',').ToList();
                scopes.ForEach(c =>
                {
                    userRoles.Add(c);
                });
            });
            //user has the required roles to get access 
            var userAccessRoles =  requiredRoles.Intersect(userRoles).ToList();
            bool hasRoleAcess = userAccessRoles.Any();

            //required scopes to get access 
            var requiredScopes = _scopes.ToLower().Split(',').ToList();
            // user scopes 
            var userScopes = claims
                .Where(claim => claim.Type == ClaimTypes.Scope)
                .Select(a => a.Value);
            var userScope = new List<string>();
            userScopes.ForEach(a =>
            {
                var scopes = a.ToLower().Split(',').ToList();
                scopes.ForEach(c =>
                {
                    userScope.Add(c);
                });
            });
            //user has the required scope to get access 
            var userAccessScopes =  requiredScopes.Intersect(userScope).ToList();
            var hasScopeAccess = userAccessScopes.Any();
            
            if (string.IsNullOrEmpty(_roles) && string.IsNullOrEmpty(_scopes))
            {
                return;
            }

            if (hasRoleAcess || hasScopeAccess)
            {
                return;
            }

            if (!hasRoleAcess && !hasScopeAccess)
            {
                context.Result = new CustomUnauthorizedResult("Forbidden you dont have the privilege to access this resources");
                return;
            }


        }

        public bool ValidateCurrentToken(string token)
        {
            var mySecret = _config.GetSection("TefIdentity.TokenSecret").Value;
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

    public class AuthorizeAllAccessAttribute : TypeFilterAttribute
    {
     
        public AuthorizeAllAccessAttribute( string scope = "",   string roles = "")
            : base(typeof(AuthorizeAllAccessActionFilter))
        {
            Arguments = new object[] { scope, roles  };
        }
    }
}