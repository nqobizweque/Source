using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MySchedule.Startup))]
namespace MySchedule
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
