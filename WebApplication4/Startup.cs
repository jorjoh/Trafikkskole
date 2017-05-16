using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SkakkjørtTrafikkPrøve.Startup))]
namespace SkakkjørtTrafikkPrøve
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
