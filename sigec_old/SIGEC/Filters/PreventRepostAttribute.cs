using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace SIGEC.Filters
{
    /// <summary>
    /// Clase del tipo ActionFilterAttribute utilizada para
    /// evitar que se excedan y se realicen peticiones exce-
    /// sivas a una acción específica.
    /// </summary>
    public class PreventRepostAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// variable que establece el tiempo mínimo en segundos
        /// entre peticiones.
        /// </summary>
        public int DelayRequest = 5;
        /// <summary>
        /// Mensaje de error cuando se detecte una cantidad 
        /// excesiva de peticiones.
        /// </summary>
        public string ErrorMessage = "Exceso de peticiones detectado.";
        /// <summary>
        /// Método utilizado para evitar que los usuarios realicen
        /// peticiones excesivas a las acciones, y para almacenar
        /// información sobre acciones ejecutadas en la cache.
        /// </summary>
        /// <param name="filterContext">variable para filtro de acción.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //referencia al objeto request para facil acceso
            var request = filterContext.HttpContext.Request;
            //referencia a la variable cache
            var cache = filterContext.HttpContext.Cache;

            //obtención de la dirección ip que origino la petición
            var originationInfo = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;

            //añadir el User Agent
            originationInfo += request.UserAgent;

            //url requerida
            var targetInfo = request.RawUrl + request.QueryString;

            //Generar hash de la url
            var hashValue = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(originationInfo + targetInfo)).Select(s => s.ToString("x2")));

            //verificara si el hash está en la cache (indicando una repetición de solicitud)
            if (cache[hashValue] != null)
            {
                //Añadir error al modelstate
                filterContext.Controller.ViewData.ModelState.AddModelError("ExcessiveRequests", ErrorMessage);
            }
            else
            {
                //añadir hash a la cache con su tiempo de expiración.
                cache.Add(hashValue, "", null, DateTime.Now.AddSeconds(DelayRequest), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}