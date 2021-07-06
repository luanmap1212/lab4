using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(lab4.Startup))]
namespace lab4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
