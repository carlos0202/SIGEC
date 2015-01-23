using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonTasksLib.Data;
using DGII_PFD.Models;
using DGII_PFD.Filters;
using DGII_PFD.Helpers;

namespace DGII_PFD.Controllers
{
    [IsAuthorized("Administrador")]
    public class UsersController : BaseController
    {
        //
        // GET: /Users/

        public ActionResult Index()
        {
            var model = new List<UsersViewModel>();
            var usr = db.PFD_USUARIOS.ToList();
            if (!AsyncWorker.Instance.Completed)
            {
                AsyncWorker.Instance.RunningTask.Wait(-1);
            }
            usr.ForEach(u =>
                    {
                        model.Add(new UsersViewModel
                        {
                            Id = u.ID,
                            NOMBRE_USUARIO = u.NOMBRE_USUARIO,
                            Nombre = u.DisplayName,
                            Roles = (u.PFD_ROLES.FirstOrDefault() != null) ? u.PFD_ROLES.FirstOrDefault().NOMBRE : "Error en los Datos"
                        });
                    });


            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.UserId = GetADUsers().ToSelectListItems(
                t => t.Value, t => t.Key);
            ViewBag.RoleId = db.PFD_ROLES.ToList().ToSelectListItems(
                r => r.NOMBRE, r => r.ID.ToString());

            return View();
        }

        [HttpPost]
        public ActionResult Create(UsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new PFD_USUARIOS();
                var role = db.PFD_ROLES.Find(model.RoleId);
                user.NOMBRE_USUARIO = string.Format("DGII\\{0}", model.UserId);
                db.PFD_USUARIOS.Add(user);
                user.PFD_ROLES.Add(role);
                role.PFD_USUARIOS.Add(user);
                db.Entry(role).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Usuario Registrado Satisfactoriamente";
                return RedirectToAction("Index");
            }
            ViewBag.UserId = GetADUsers().ToSelectListItems(
                t => t.Value, t => t.Key, t => t.Key == model.UserId);
            ViewBag.RoleId = db.PFD_ROLES.ToList().ToSelectListItems(
                r => r.NOMBRE, r => r.ID.ToString(), r => r.ID == model.RoleId);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var user = db.PFD_USUARIOS.Find(id);
            if (user == null) 
            {
                return HttpNotFound(); 
            }
            var model = new UsersViewModel();
            user.Transfer(ref model);
            model.UserId = user.NOMBRE_USUARIO;
            model.RoleId = (int)user.PFD_ROLES.First().ID;
            ViewBag.RoleId = db.PFD_ROLES.ToList().ToSelectListItems(
                r => r.NOMBRE, r => r.ID.ToString(), r => r.ID == (decimal)model.RoleId);
            model.Nombre = ADUsers.Instance.GetDisplayName(model.NOMBRE_USUARIO);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.PFD_USUARIOS.Find(model.Id);
                if (model.RoleId != user.PFD_ROLES.First().ID)
                {
                    user.PFD_ROLES.Clear();
                    user.PFD_ROLES.Add(db.PFD_ROLES.Find(model.RoleId));
                    db.Entry(user).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }
                TempData["Message"] = "Datos de usuario actualizados exitosamente.";
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = db.PFD_ROLES.ToList().ToSelectListItems(
                r => r.NOMBRE, r => r.ID.ToString(), r => r.ID == (decimal)model.RoleId);

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var user = db.PFD_USUARIOS.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var model = new UsersViewModel();
            user.Transfer(ref model);
            model.UserId = user.NOMBRE_USUARIO;
            model.RoleId = (int)user.PFD_ROLES.First().ID;
            model.Nombre = ADUsers.Instance.GetDisplayName(model.NOMBRE_USUARIO);
            model.Roles = user.PFD_ROLES.First().NOMBRE;

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.PFD_USUARIOS.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.PFD_ROLES.Clear();
            db.PFD_USUARIOS.Remove(user);
            db.SaveChanges();
            TempData["Message"] = "Usuario eliminado correctamente";

            return RedirectToAction("Index");
        }

        public ActionResult GetUsers()
        {
            return this.RefreshADUsers();
        }

    }
}
