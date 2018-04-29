using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnYourWay.Startup))]
namespace OnYourWay
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
