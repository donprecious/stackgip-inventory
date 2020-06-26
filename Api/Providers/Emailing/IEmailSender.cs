using System.Threading.Tasks;
using StackgipInventory.Dto;

namespace StackgipInventory.Providers.Emailing
{
    public interface IEmailSender
    {
        Task<bool> SendEmailMessage(EmailMessage emailMessage,  string url);
    }
}
