namespace SIGEC.Code52.i18n
{
	using System.Threading;
	using System.Web.Mvc;
    /// <summary>
    /// Clase utilizada para establecer los valores de la cultura actual
    /// tomando como base la preferencia del navegador del usuario, o
    /// un cookie almacenado. Esta clase esta implementada como un ActionFilter
    /// para permitir establecer la preferencia de cultura al ejecutar una 
    /// acción.
    /// </summary>
    public class LanguageFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            string cultureName = null;
            var cultureCookie = request.Cookies["_culture"];
            if (request.UserLanguages != null)
                cultureName = cultureCookie != null ? cultureCookie.Value : request.UserLanguages[0];
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            base.OnActionExecuting(filterContext);
        }
    }
}