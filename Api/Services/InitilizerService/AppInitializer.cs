using System.Threading.Tasks;
using Extensions.Hosting.AsyncInitialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace StackgipInventory.Services.InitilizerService
{
    public class AppInitializer: IAsyncInitializer
    {


        private IWebHostEnvironment _webHost;
 
        public AppInitializer(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public async Task InitializeAsync()
        {

            if (_webHost.IsDevelopment())
            {
                
            }
        }
    }
}
