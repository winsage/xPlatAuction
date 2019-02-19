using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(xPlatAuction.Backend.Startup))]

namespace xPlatAuction.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}