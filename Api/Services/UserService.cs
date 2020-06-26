using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Refit;
using StackgipInventory.ApiDto;
using StackgipInventory.Config;
using StackgipInventory.Dto;
using StackgipInventory.Handler;
using StackgipInventory.Helpers;
using StackgipInventory.Shared;
using ClaimTypes = StackgipInventory.Shared.ClaimTypes;

namespace StackgipInventory.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IIdentityConfig _identityConfig;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string identityBaseUrl;
        private IIdentityApi identityApi;
        public UserService(IHttpContextAccessor httpContextAccessor, IIdentityConfig identityConfig)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityConfig = identityConfig;


            clientId = _identityConfig.ClientId();
            clientSecret = _identityConfig.ClientSecret();
            identityBaseUrl = _identityConfig.BaseUrl();
            identityApi = RestService.For<IIdentityApi>(
                new HttpClient(new ClientCredientialHttpClientHandler(clientId, clientSecret))
                {
                    BaseAddress = new Uri(identityBaseUrl+"/api/v1"),
                    Timeout =  TimeSpan.FromSeconds(30)
                });

        }

        public UserDto GetCurrentUser()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            token = token.Split(" ")[1].Trim();
            var tokenClaim = JwtDecoder.GetClaims(token).ToList();
            var tokenType = tokenClaim.FirstOrDefault(a => a.Type == ClaimTypes.TokenType)?.Value;

            if (tokenType != TokenType.User)
            {
                return null;
            }

            if (!tokenClaim.Any()) return null;

            //var roles = tokenClaim.Where(a => a.Type == ClaimTypes.Role).Select(a => a.Value).ToList();
            var roles = tokenClaim.Where(claim => claim.Type == "roles").Select(x => x.Value).ToList();
            var userDto = new UserDto() {
                Id = tokenClaim.FirstOrDefault(a => a.Type == "id")?.Value,
                FirstName = tokenClaim.FirstOrDefault(a => a.Type == ClaimTypes.FirstName)?.Value,
                LastName = tokenClaim.FirstOrDefault(a => a.Type == ClaimTypes.LastName)?.Value,
                Email = tokenClaim.FirstOrDefault(a => a.Type == ClaimTypes.Email)?.Value,
                Roles = roles,
                Scopes = tokenClaim.Where(claim => claim.Type == ClaimTypes.Scope)
                                .Select(a => a.Value.ToLower()).ToList()
            };
            //var user = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userDto;
        }

        public async Task<UserApiResponse> GetUser(string userId)
        {
            var response = new UserApiResponse();
            try
            {
                var user = await identityApi.GetUser(userId);
                response.Success = true;
                response.Data = user;
            }
            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content,
                // which is type of ProblemDetails according to RFC 7807
                response.Message = validationException.Content.Detail;

                // If the response contains additional properties on the problem details,
                // they will be added to the validationException.Content.Extensions collection.
            }
            catch (ApiException exception)
            {
                // other exception handling
                response.Message = exception.Content;
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;
            }
            return response;
        }
    }
}
