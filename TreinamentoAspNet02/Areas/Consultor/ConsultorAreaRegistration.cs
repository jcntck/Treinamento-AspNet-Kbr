using System.Web.Mvc;

namespace TreinamentoAspNet02.Areas.Consultor
{
    public class ConsultorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Consultor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Consultor_default",
                url: "Consultor/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TreinamentoAspNet02.Areas.Consultor.Controllers" }
            );
        }
    }
}