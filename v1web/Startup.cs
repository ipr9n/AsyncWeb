using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(v1web.Startup))]
namespace v1web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
