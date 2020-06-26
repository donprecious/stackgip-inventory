using Microsoft.Extensions.Configuration;

namespace StackgipInventory.Config
{
    public class Config:IConfig
    {
        private IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string MSSqlConnection()
        {
            return GetValue(ConfigConstants.MsSqlConnection);
        }

        public string GetValue(string prop)
        {
            return _configuration.GetSection(prop).Value;
        }
    }

    public static class ConfigConstants
    {
        public const string MsSqlConnection = "MSSql.Connection";
        public const string IdentitySecret = "Identity.TokenSecret";
        public const string IdentityClientId = "Identity.ClientId";
        public const string IdentityClientSecret= "Identity.ClientSecret";
        public const string IdentityBaseUrl= "Identity.BaseUrl";
    }
}
