using Microsoft.AspNetCore.Mvc;

namespace StackgipInventory.Shared
{
    public static class Responses
    {
        // todo create generic responses for Ok, Created, Unauthorize, BadRequest
        public static OkResult Ok()
        {
         
            var x = new  OkResult();
            return x;
        }
    }
}
