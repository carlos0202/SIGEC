using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebTemplateMVC5.Startup))]
namespace WebTemplateMVC5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
