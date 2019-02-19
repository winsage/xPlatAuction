using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;

namespace xPlatAuction.Backend.Controllers
{
    // Use the MobileAppController attribute for each ApiController you want to use  
    // from your mobile clients 
    [MobileAppController]
    [Authorize]
    public class LoginValidationController : ApiController
    {
        // GET api/values
        public string Get()
        {
            return "Login valid.";
        }

        
    }
}
