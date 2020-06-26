using System.Threading.Tasks;
using Refit;
using StackgipInventory.ApiDto;

namespace StackgipInventory.Services
{
 public interface IIdentityApi
    {
        [Get("/users/{userid}")]
        Task<UserResponse> GetUser(string userid);

        [Headers("GrantType: client_credentials")]
        [Post("/OAuth/token")]
        Task<AuthApiResponse> GenerateClientToken([Header("X-ClientId")] string clientId, [Header("X-ClientSecret")] string clientSecret  );
    }
}
