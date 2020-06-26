using System.Threading.Tasks;
using StackgipInventory.Dto;

namespace StackgipInventory.Providers.Emailing
{
    public class EmailSender : IEmailSender
    {
        private IEmailService _emailService;
        private IEmailTemplateProvider _emailTemplateProvider;
        public EmailSender(
            IEmailTemplateProvider emailTemplateProvider,
            IEmailService emailService)
        {
            _emailTemplateProvider = emailTemplateProvider;
            _emailService = emailService;
        }
        public async Task<bool> SendEmailMessage(EmailMessage emailMessage, string callbackUrl)
        {
            var mailTemp ="";

            var body = "Congratulations!!! Thank you on your completion of the Application Process";

            mailTemp.Replace("{header}", "Application Completion");
            mailTemp.Replace("{body}", body);
            mailTemp.Replace("{url}", callbackUrl);
            mailTemp.Replace("{btn-text}", "TEF Connect");

            emailMessage.Body = mailTemp;

            return await _emailService.Send(emailMessage);
        }
    }
}
