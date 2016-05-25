using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(P_Layer.Startup))]
namespace P_Layer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
