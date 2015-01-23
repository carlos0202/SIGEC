using DGII_PFD.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using CommonTasksLib.Data;
using System.Data.Common;
using DGII_PFD.Filters;
using System.Threading;
using DGII_PFD.Helpers;

namespace DGII_PFD.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    [IsAuthorized]
    public class BaseController : Controller
    {

        protected PFDContext db = new PFDContext();

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
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //some logic
                filterContext.ExceptionHandled = true;
                filterContext.Result = new JsonResult()
                {
                    Data = JsonResponseBase.ErrorResponse(filterContext.Exception.GetBaseException().Message),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
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

        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected IOrderedEnumerable<KeyValuePair<string, string>> GetADUsers()
        {
            var toExclude = db.PFD_USUARIOS.Select(u => u.NOMBRE_USUARIO).ToList();
            if(!AsyncWorker.Instance.Completed)
            {
                AsyncWorker.Instance.RunningTask.Wait(-1);
            }
            var users = ADUsers.Instance.GetUsers().Where(u => !toExclude.Contains(string.Format("DGII\\{0}", u.Key))).OrderBy(u => u.Key);

            return users;
        }

        protected ActionResult RefreshADUsers()
        {
            var toExclude = db.PFD_USUARIOS.Select(u => u.NOMBRE_USUARIO).ToList();
            ADUsers.Instance.ReInstanciate(toExclude);
            var model = ADUsers.Instance.GetUsers().ToSelectListItems(
                t => t.Value, t => t.Key);

            return Json(JsonResponseBase.SuccessResponse(model, "Datos Actualizados"), JsonRequestBehavior.AllowGet);
        }

        protected string GetCurrentUserName()
        {
            return HttpContext.User.Identity.Name;
            //return ((System.DirectoryServices.AccountManagement.Principal)(UserPrincipal.Current)).SamAccountName; 
        }

        protected string GetConnectionString(string ConnectionName)
        {
            System.Configuration.Configuration rootWebConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/DGII_PFD");
            System.Configuration.ConnectionStringSettings connString;
            connString = rootWebConfig.ConnectionStrings.ConnectionStrings[ConnectionName];

            return connString.ConnectionString;
        }

    }
}
