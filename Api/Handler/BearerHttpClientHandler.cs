using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace StackgipInventory.Handler
{
    public class BearerHttpClientHandler :HttpClientHandler
    {
        private  string _token;
        
        public BearerHttpClientHandler(string token)
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException("token is empty");
            _token = token;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
