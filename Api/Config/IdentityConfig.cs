namespace StackgipInventory.Config
{
    public class IdentityConfig:IIdentityConfig
    {
        private IConfig _config;

        public IdentityConfig(IConfig config)
        {
            _config = config;
        }

        public string TokenSecret()
        {
            return _config.GetValue(ConfigConstants.IdentitySecret);
        }

        public string ClientSecret()
        {
            return _config.GetValue(ConfigConstants.IdentityClientSecret);

        }

        public string BaseUrl()
        {
            return _config.GetValue(ConfigConstants.IdentityBaseUrl);

        }

        public string ClientId()
        {
            return _config.GetValue(ConfigConstants.IdentityClientId);
        }
    }
}
