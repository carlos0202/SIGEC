using SIGEC.Filters;
using SIGEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using WebMatrix.WebData;

namespace SIGEC.CustomCode
{
    public class MenuAuthorize
    {
        SIGECContext db = new SIGECContext();
        /// <summary>
        /// Constructor para sacar los permisos que tiene un usuario y almacenarlos en la variable Menu
        /// </summary>
        public MenuAuthorize()
        {
            try
            {
                if (WebSecurity.IsAuthenticated)
                {
                    db = new SIGECContext();
                    var user = db.Users.FirstOrDefault(usr => usr.ID == WebSecurity.CurrentUserId);

                    foreach (webpages_Roles role in user.webpages_Roles)
                    {
                        foreach (SIGEC.Models.Action action in role.Actions)
                        {
                            if (!Menu.ContainsKey(
                                        string.Format("{0}-{1}", action.Menu.name.Replace("Controller", ""),
                                        action.name)
                                ))
                            {
                                Menu.Add(
                                    string.Format("{0}-{1}", action.Menu.name.Replace("Controller", ""), action.name),
                                    action.name
                                );
                            }
                        }
                    }
                }
            }
            catch
            {
                new AuthorizationContext().Result = new RedirectToRouteResult(
                     new RouteValueDictionary
                 {
                     { "controller", "Account" },
                     { "action", "Login" }
                 });
            }
        }

        //Variables privadas
        Dictionary<string, string> Menu = new Dictionary<string, string>();

        /// <summary>
        /// Retorna true si el usuario loged acutal tiene permiso al controlador-acción
        /// </summary>
        /// <param name="actionName">Nombre de la acción</param>
        /// <param name="ControllerName">Nombre del controlador</param>
        /// <returns></returns>
        public bool HasPermission(string actionName, string ControllerName)
        {
            if (Menu.Where(d => d.Key == (ControllerName + "-" + actionName) && d.Value == actionName).Count() > 0)
                return true;
            return false;
        }
    }
}

