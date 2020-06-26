using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Refit;
using StackgipInventory.ApiDto;
using StackgipInventory.Config;
using StackgipInventory.Handler;

namespace StackgipInventory.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IIdentityConfig _identityConfig;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string identityBaseUrl;
        private IIdentityApi identityApi;
        private IIdentityApi identityOAuthApi;
        public IdentityService(IHttpContextAccessor httpContextAccessor, IIdentityConfig identityConfig
         )
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
            identityOAuthApi = RestService.For<IIdentityApi>(
                new HttpClient(new ClientCredientialHttpClientHandler(clientId, clientSecret))
                {
                    BaseAddress = new Uri(identityBaseUrl),
                    Timeout =  TimeSpan.FromSeconds(30)
                });
           
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

        public  async Task<AuthApiRefitResponse> GenerateClientToken()
        {
            var response = new AuthApiRefitResponse();

            try
            {

                var auth = await identityOAuthApi.GenerateClientToken(clientId, clientSecret);
                response.Success = true;
                response.Data = auth;
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
