using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(rmxwebapi.Startup))]

namespace rmxwebapi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
