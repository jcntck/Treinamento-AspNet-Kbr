using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TreinamentoAspNet02.Startup))]
namespace TreinamentoAspNet02
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
