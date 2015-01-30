using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebTemplateMVC5.Models;

namespace WebTemplateMVC5.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public abstract class BaseController : Controller
    {
        // GET: Base
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}