using Microsoft.AspNetCore.Hosting;

namespace StackgipInventory.Providers.Emailing
{
    public class EmailTemplateProvider : IEmailTemplateProvider
    {
        private IWebHostEnvironment _hostingEnvironment;

        public EmailTemplateProvider(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string GetEmailTemplate()
        {
            return "";
        }
    }
}
