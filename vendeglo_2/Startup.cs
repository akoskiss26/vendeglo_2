using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(vendeglo_2.Startup))]
namespace vendeglo_2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
