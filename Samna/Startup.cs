using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Samna.Startup))]
namespace Samna
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
