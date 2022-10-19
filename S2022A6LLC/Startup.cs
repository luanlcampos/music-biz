using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(S2022A6LLC.Startup))]

namespace S2022A6LLC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
