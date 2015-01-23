using DGII_PFD.Filters;
using DGII_PFD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DGII_PFD.Controllers
{
    public class ProcedimientosController : BaseController
    {
        //
        // GET: /Procedimientos/

        public ActionResult Index()
        {
            var current = this.GetCurrentUserName();
            var usuarioActual = db.PFD_USUARIOS.FirstOrDefault(u => u.NOMBRE_USUARIO == current);
            var rol = usuarioActual.PFD_ROLES.FirstOrDefault().ID;
            var procedimientos = db.PFD_PROCEDIMIENTOS
                .Where(x => x.HABILITADO == 1 && x.PFD_ROLES.Select(r => r.ID).Contains(rol))
                .ToList();
            return View(procedimientos);
        }

        [CheckExecuteRights]
        public ActionResult DetalleProcedimiento(int id)
        {
            var procedimiento = db.PFD_PROCEDIMIENTOS.FirstOrDefault(x => x.ID == id);
            procedimiento.Parametros = procedimiento.PFD_PARAMETROS.ToList();
            return View(procedimiento);
        }

        [CheckExecuteRights]
        public ActionResult EjecutarProcedimiento(int id)
        {
            var procedimiento = db.PFD_PROCEDIMIENTOS.FirstOrDefault(x => x.ID == id);
            procedimiento.Parametros = procedimiento.PFD_PARAMETROS.ToList();
            return View(procedimiento);
        }
        [HttpPost]
        public ActionResult Ejecutar(PFD_PROCEDIMIENTOS model)
        {
            MensajeResultado result = DbCommand.EjecutarProcedimiento(model);
            TempData["Resultado"] = result;
            return RedirectToAction("Index");
        }
    }
}
