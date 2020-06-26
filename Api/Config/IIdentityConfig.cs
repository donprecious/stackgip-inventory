namespace StackgipInventory.Config
{
    public interface IIdentityConfig
    {
        string  TokenSecret();
        string ClientSecret();
        string BaseUrl();

        string  ClientId();
    }
}
