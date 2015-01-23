using DGII_PFD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DGII_PFD.Filters
{
    public class IsAuthorizedAttribute : AuthorizeAttribute
    {
        public string RequiredRoles { get; set; }
        private PFDContext db;

        public IsAuthorizedAttribute(string RequiredRoles)
        {
            this.RequiredRoles = RequiredRoles;
        }

        public IsAuthorizedAttribute()
        {
            this.RequiredRoles = "*";
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.ActionDescriptor.ActionName == "ErrorUnauthorized")
            {
                return;
            }
            var allowed = false;
            var currentUser = this.GetCurrentUserName();
            using (db = new PFDContext())
            {
                var user = db.PFD_USUARIOS.FirstOrDefault(u => u.NOMBRE_USUARIO == currentUser);
                allowed = (user == null) ? false : true;
                var allAvailable = RequiredRoles.Equals("*");
                if (allowed && !allAvailable)
                {
                    var roles = RequiredRoles.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim());

                    allowed = roles.Contains(user.PFD_ROLES.First().NOMBRE);
                }
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