using System.Threading.Tasks;
using StackgipInventory.ApiDto;

namespace StackgipInventory.Services.Identity
{
   public interface IIdentityService
   {

       Task<UserApiResponse> GetUser(string userId);
       Task<AuthApiRefitResponse> GenerateClientToken();

   }
}
