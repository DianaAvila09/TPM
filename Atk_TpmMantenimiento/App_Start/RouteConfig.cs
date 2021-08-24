using System.Web.Mvc;
using System.Web.Routing;

namespace Atk_TpmMantenimiento
{
   public class RouteConfig
   {
      public static void RegisterRoutes(RouteCollection routes)
      {
         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
         routes.MapRoute(
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Tpm", action = "DatosTpmBasico", id = UrlParameter.Optional },
            namespaces: new[] { "Atk_TpmMantenimiento.Controllers" }

            // defaults: new { controller = "Tpm", action = "DatosTpmBasico", id = UrlParameter.Optional },
            //namespaces: new[] { "Atk_TpmMantenimiento.Controllers" }
         );
      }
   }
}
