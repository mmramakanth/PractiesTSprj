using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PracticeTS.Startup))]
namespace PracticeTS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
