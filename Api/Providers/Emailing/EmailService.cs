using System.Net.Http;
using System.Threading.Tasks;
using StackgipInventory.Dto;

namespace StackgipInventory.Providers.Emailing
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
      
        public async Task<bool> Send(EmailMessage emailMessage)
        {
          

            return false;
        }
    }
}