using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DGII_PFD.Models;
using System.Web.Routing;

namespace DGII_PFD.Filters
{
    public class CheckExecuteRightsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var id = Convert.ToInt32(filterContext.ActionParameters["id"]);
            var allowed = false;
            using(var db = new PFDContext()){
                var procedimiento = db.PFD_PROCEDIMIENTOS.FirstOrDefault(p => p.ID == id);
                if (procedimiento == null)
                {
                    filterContext.Result = new HttpNotFoundResult();
                    return;
                }
                var current = HttpContext.Current.User.Identity.Name;
                var usuarioActual = db.PFD_USUARIOS.FirstOrDefault(u => u.NOMBRE_USUARIO == current);
                var rol = usuarioActual.PFD_ROLES.FirstOrDefault().ID;
                allowed = procedimiento.PFD_ROLES.Select(r => r.ID).Contains(rol);                
            }

            if (allowed)
            {
                return;
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary
                 {
                     { "controller", "Home" },
                     { "action", "ErrorUnauthorized" }
                 });
            }
        }
    }
}