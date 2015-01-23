using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DGII_PFD.Models;
using DGII_PFD.Filters;

namespace DGII_PFD.Controllers
{
    [IsAuthorized("Administrador")]
    public class RolesController : BaseController
    {
        //
        // GET: /Roles/

        public ActionResult Index()
        {
            var roles = db.PFD_ROLES.ToList();

            return View(roles);
        }

        [HttpPost]
        public ActionResult Create(PFD_ROLES model)
        {
            if (ModelState.IsValid)
            {
                db.PFD_ROLES.Add(model);
                db.SaveChanges();
                var message = "Rol registrado satisfactoriamente.";
                return Json(JsonResponseBase.SuccessResponse(message));
            }

            var roles = db.PFD_ROLES.ToList();
            var error = "Rol no registrado satisfactoriamente. Revisar datos.";

            return Json(JsonResponseBase.ErrorResponse(error));
        }

        public ActionResult Edit(int id)
        {
            var role = db.PFD_ROLES.Find(id);
            if (role.NOMBRE.ToLower() == "administrador")
            {
                var error = "No puede editar los roles predefinidos [Administrador]";
                throw new Exception(error);
            }
            ViewBag.Mode = "editRoleModal";

            return Json(RenderRazorViewToString("_CreateRolePartial", role), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(PFD_ROLES model)
        {
            if (ModelState.IsValid)
            {
                var role = db.PFD_ROLES.Find(model.ID);
                role.NOMBRE = model.NOMBRE;
                db.Entry(role).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                var message = "Rol actualizado satisfactoriamente.";

                return Json(JsonResponseBase.SuccessResponse(message));
            }
            var error = "Error al actualizar los datos del rol.";

            return Json(JsonResponseBase.ErrorResponse(error));
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var role = db.PFD_ROLES.Find(id);

            if (role == null)
            {
                return HttpNotFound();
            }
            else if (role.NOMBRE.ToLower() == "administrador")
            {
                var error = "No puede eliminar los roles predefinidos [Administrador]";
                return Json(JsonResponseBase.ErrorResponse(error));
            }
            else if (role.PFD_USUARIOS.Count > 0)
            {
                var error = "No puede eliminar un rol con usuarios asignados.";
                return Json(JsonResponseBase.ErrorResponse(error));
            }

            db.PFD_ROLES.Remove(role);
            db.SaveChanges();

            var message = "Rol eliminado satisfactoriamente.";
            return Json(JsonResponseBase.SuccessResponse(message));
        }

        public ActionResult UpdateRolesTable()
        {
            var roles = db.PFD_ROLES.ToList();

            return Json(
                new
                {
                    aaData = roles.Select(
                        d => new[]{
                            d.NOMBRE,
                            d.ID.ToString()
                        }
                    )
                }, JsonRequestBehavior.AllowGet
            );
        }

    }
}
