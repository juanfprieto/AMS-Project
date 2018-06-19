using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AMS.Carga.Startup))]
namespace AMS.Carga
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
