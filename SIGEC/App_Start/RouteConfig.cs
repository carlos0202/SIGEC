using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIGEC
{
    /// <summary>
    /// Clase para la configuracion de enrutamiento
    /// de peticiones http hacia las acciones de los 
    /// controladores de la aplicación.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Metodo utilizado para registrar las rutas utilizadas para mapear las solicitudes
        /// entrantes desde el navegador y encontrar la acción específica a ejecutar.
        /// </summary>
        /// <param name="routes">Collección de rutas por defecto definidas por ASP.Net</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute("NotFound", "{*url}",
            new { controller = "CustomErrors", action = "NotFound" });
        }
    }
}