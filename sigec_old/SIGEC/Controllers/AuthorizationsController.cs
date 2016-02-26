using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGEC.Models.ViewModels;
using SIGEC.CustomCode;
using SIGEC.Models;
using System.Data;

namespace SIGEC.Controllers
{
    [IsMenu]
    public class AuthorizationsController : BaseController
    {
        //
        // GET: /Authorizations/Edit
        /// <summary>
        /// Acción que muestra a los administradores especiales del sistema
        /// la pantalla para editar los permisos de acceso sobre las panta-
        /// llas del sistema para los diferentes roles definidos.
        /// </summary>
        /// <returns>Un ViewResult con la pantalla de edición de permisos.</returns>
        [IsView]
        public ActionResult Edit()
        {
            ViewBag.RoleId = db.webpages_Roles.ToList()
                .ToSelectListItems(wr => _(wr.RoleName), wr => wr.RoleId.ToString());
            ViewBag.menus = db.Menus.ToList();
            return View();
        }

        //
        // POST: /Authorizations/Edit
        /// <summary>
        /// Acción que captura la petición de los administradores especiales
        /// para editar los permisos de los roles definidos en la aplicación
        /// sobre las diferentes pantallas.
        /// </summary>
        /// <param name="model">modelo con los datos de los permisos actualizados.</param>
        /// <returns>ViewResult de la página principal de la aplicación.</returns>
        [HttpPost]
        public ActionResult Edit(AuthorizationsViewModel model)
        {
            if (ModelState.IsValid)
            {
                webpages_Roles role = db.webpages_Roles.Find(model.RoleId);
                role.Actions.Clear();

                if (model.actions != null)
                {
                    foreach (int action in model.actions)
                    {
                        role.Actions.Add(db.Actions.Find(action));
                    }
                }

                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.RoleId = new SelectList(db.webpages_Roles.ToList(), "RoleID", "RoleName", model.RoleId);
            ViewBag.menus = db.Menus.ToList();
            return View(model);
        }

        /// <summary>
        /// Método utilizado para obtener los datos de los permisos
        /// concedidos a un rol especificado y mostrar dichos datos
        /// para ser editados por parte de un administrador especial
        /// de la aplicación.
        /// </summary>
        /// <param name="id">Id del rol del cual editar permisos.</param>
        /// <returns>
        /// un PartialView con el html necesario para editar
        /// los permisos para dicho rol.
        /// </returns>
        [HttpPost]
        public ActionResult GetRoleAuthorizations(int id)
        {
            var role = db.webpages_Roles.Find(id);
            if (role == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            AuthorizationsRolePartial model = new AuthorizationsRolePartial();
            model.menus = db.Menus.ToList();
            model.SelectedActions = role.Actions.Select(ra => ra.ID).ToArray();

            return PartialView(model);
        }

    }
}
