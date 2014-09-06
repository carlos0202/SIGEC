using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SIGEC
{
    public static class WebApiConfig
    {
        /// <summary>
        /// Metodo para registrar el mecanismo de rutas para el web api.
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
