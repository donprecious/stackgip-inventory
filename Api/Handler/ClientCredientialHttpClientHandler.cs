using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StackgipInventory.Handler
{
    public class ClientCredientialHttpClientHandler :HttpClientHandler
    {
         private readonly string _clientId;
         private readonly string _clientsecret;

    public ClientCredientialHttpClientHandler(string clientId, string clientsecret)
    {
        if (clientId == null || clientsecret == null) throw new ArgumentNullException("clientId, or clientSecret not set");
        _clientId = clientId;
        _clientsecret = clientsecret;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
     
        string auth = _clientId + ":" + _clientsecret;
        var encodeByte = Encoding.ASCII.GetBytes(auth);
        var token = Convert.ToBase64String(encodeByte);

        request.Headers.Authorization = new AuthenticationHeaderValue("basic", token);
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
    }
}
