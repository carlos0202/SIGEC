using SIGEC.CustomCode;
using SIGEC.Filters;
using SIGEC.Models;
using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;

namespace SIGEC.Controllers
{
    /// <summary>
    /// Clase controlador base utilizada para establecer objetos y métodos comunes
    /// a ser utilizados en todos los controladores de la aplicación.
    /// </summary>
    [Authorize]
    [CustomAuthorize]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class BaseController : Controller
    {
        /// <summary>
        /// objeto de instancia de entity framework para las solicitudes
        /// a la base de datos.
        /// </summary>
        protected SIGECContext db = new SIGECContext();

        /// <summary>
        /// Manejador de recursos, para los textos desde los archivos de recursos.
        /// </summary>
        private static ResourceManager manager = new ResourceManager(typeof(Language));

        private static SimpleRoleProvider roleProvider = (SimpleRoleProvider)Roles.Provider;

        /// <summary>
        /// Método de extensión para tomar los mensajes de la aplicación desde el archivo de recursos.
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public string _(string var)
        {
            var translatedMsg = manager.GetString(var);
            return string.IsNullOrEmpty(translatedMsg) ? var : translatedMsg;
        }

        /// <summary>
        /// propiedad utilizada para saber si el usuario activo es un paciente.
        /// </summary>
        public bool isPatient
        {
            get
            {
                return roleProvider.IsUserInRole(WebSecurity.CurrentUserName, "Patient");
            }
        }

        /// <summary>
        /// propiedad utilizada para saber si el usuario activo es un empleado.
        /// </summary>
        public bool isEmployee
        {
            get
            {
                return roleProvider.IsUserInRole(WebSecurity.CurrentUserName, "Employee");
            }
        }

        /// <summary>
        /// propiedad utilizada para saber si el usuario activo es un doctor.
        /// </summary>
        public bool isDoctor
        {
            get
            {
                return roleProvider.IsUserInRole(WebSecurity.CurrentUserName, "Doctor");
            }
        }

        /// <summary>
        /// propiedad utilizada para saber si el usuario activo es un administrador.
        /// </summary>
        public bool isAdmin
        {
            get
            {
                return roleProvider.IsUserInRole(WebSecurity.CurrentUserName, "Admin");
            }
        }

        /// <summary>
        /// Método utilizado en el momento que se van a liberar los recursos
        /// de los objetos de las clases que hereden de BaseController
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            // Custom error page display
            filterContext.ExceptionHandled = true;
            this.View(
                "Error",
                new HandleErrorInfo(
                    filterContext.Exception,
                    RouteData.GetRequiredString("controller"),
                    RouteData.GetRequiredString("action")
                )
            ).ExecuteResult(this.ControllerContext);
        }

        protected override void HandleUnknownAction(string actionName)
        {
            // If controller is ErrorController dont 'nest' exceptions
            if (this.GetType() != typeof(CustomErrorsController))
                this.InvokeHttp404(HttpContext);
        }

        public ActionResult InvokeHttp404(HttpContextBase httpContext)
        {
            Response.TrySkipIisCustomErrors = true;
            IController errorController = DependencyResolver.Current.GetService<CustomErrorsController>();
            var errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "CustomErrors");
            errorRoute.Values.Add("action", "NotFound");
            errorRoute.Values.Add("url", httpContext.Request.Url.OriginalString);
            errorController.Execute(new RequestContext(
                 httpContext, errorRoute));

            return new EmptyResult();
        }
    }
}
