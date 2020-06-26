using System.Threading.Tasks;
using StackgipInventory.Dto;

namespace StackgipInventory.Providers.Emailing
{
    public interface IEmailService
    {
        Task<bool> Send(EmailMessage emailMessage);
    }
}