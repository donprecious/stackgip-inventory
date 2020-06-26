using System.Threading.Tasks;
using StackgipInventory.ApiDto;
using StackgipInventory.Dto;

namespace StackgipInventory.Services
{
    public interface IUserService
   {
        UserDto GetCurrentUser();
        Task<UserApiResponse> GetUser(string userId);
   }
}
