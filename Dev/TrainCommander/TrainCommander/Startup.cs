using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrainCommander.Startup))]
namespace TrainCommander
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
