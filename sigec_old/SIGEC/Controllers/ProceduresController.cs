using SIGEC.CustomCode;
using SIGEC.Models;
using SIGEC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace SIGEC.Controllers
{
    /// <summary>
    /// Controlador que encapsula toda la lógica utilizada
    /// para manejar la información de los procedimientos
    /// del consultorio médico.
    /// </summary>
    [IsMenu]
    public class ProceduresController : BaseController
    {
        //
        // GET: /Procedure/
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// principal de los procedimientos, donde se muestran
        /// todos los tipos de procedimientos registrados en
        /// la aplicación.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de inicio de
        /// los procedimientos.
        /// </returns>
        [IsView]
        public ActionResult Index()
        {
            var procedures = db.Procedures;
            return View(procedures.ToList());
        }

        //
        // GET: /Procedure/Details/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// que contiene los datos en detalle de un
        /// tipo de procedimiento de la apliación.
        /// </summary>
        /// <param name="id">el ID del procedimiento</param>
        /// <returns>
        /// Un ViewResult con los datos del procedimiento
        /// en caso de que este exista en la base de
        /// datos, o de lo contrario un HttpNotFond,
        /// es decir un código 404 de http que indica
        /// que no se ha encontrado ningún resultado
        /// con el criterio de búsqueda.
        /// </returns>
        [IsView]
        public ActionResult Details(int id = 0)
        {
            Procedure procedure = db.Procedures.Find(id);
            if (procedure == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(procedure);
        }

        //
        // GET: /Procedure/Create
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// de creación de un nuevo tipo de procedimiento.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de creación
        /// de nuevo tipo de procedimiento para el consultorio.
        /// </returns>
        [IsView]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Procedure/Create
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// de creación de un nuevo tipo de procedimiento.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de creación
        /// de nuevo tipo de procedimiento para el consultorio.
        /// </returns>
        /// <param name="procedure">datos del procedimiento.</param>
        /// <returns>un ViewResult con los detalles del procedimiento.</returns>
        [HttpPost]
        public ActionResult Create(Procedure procedure)
        {
            if (ModelState.IsValid)
            {
                procedure.createdBy = WebSecurity.CurrentUserId;
                procedure.status = true;
                db.Procedures.Add(procedure);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = procedure.ID });
            }

            return View(procedure);
        }

        //
        // GET: /Procedure/Edit/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// para actualizar los datos de un tipo de procedimiento
        /// registrado en el sistema. Si los datos del procedimiento
        /// no son encontrados, se retornará un resultado con un
        /// HttpNotFound indicando que el criterio de búsqueda no
        /// produjo ningún resultado.
        /// </summary>
        /// <param name="id">ID del procedimiento</param>
        /// <returns>
        /// un ViewResult con la pantalla para editar los datos
        /// del procedimiento.
        /// </returns>
        [IsView]
        public ActionResult Edit(int id = 0)
        {
            Procedure procedure = db.Procedures.Find(id);
            if (procedure == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            ProcedureViewModel model = new ProcedureViewModel();
            GlobalHelpers.Transfer<Procedure, ProcedureViewModel>(procedure, model);

            return View(model);
        }

        //
        // POST: /Procedure/Edit/5
        /// <summary>
        /// Acción que captura la petición del usuario de
        /// actualizar los datos de un procedimiento del sistema.
        /// Si los datos del procedimiento son inválidos, se le 
        /// mostrará la pantalla de edición nuevamente al
        /// usuario indicando los errores encontrados.
        /// </summary>
        /// <param name="procedure">datos del procedimiento.</param>
        /// <returns>ViewResult con los procedimientos registrados.</returns>
        [HttpPost]
        public ActionResult Edit(ProcedureViewModel procedure)
        {
            if (ModelState.IsValid)
            {
                var model = db.Procedures.Find(procedure.ID);
                GlobalHelpers.Transfer<ProcedureViewModel, Procedure>(procedure, model);
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(procedure);
        }

        //
        // GET: /Procedure/Delete/5
        /// <summary>
        /// Acción que muestra al usuario una pantalla
        /// para eliminar los datos de un tipo de procedimiento
        /// del consultorio. Si el tipo de procedimiento no es
        /// encontrado se retornará un HttpNotFound indicán-
        /// dolo.
        /// </summary>
        /// <param name="id">ID del tipo de procedimiento.</param>
        /// <returns>
        /// Un ViewResult con la pantalla para eliminar el
        /// procedimiento.
        /// </returns>
        [IsView]
        public ActionResult Delete(int id = 0)
        {
            Procedure procedure = db.Procedures.Find(id);
            if (procedure == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(procedure);
        }

        //
        // POST: /Procedure/Delete/5
        /// <summary>
        /// Acción que captura la confirmación del usuario
        /// para eliminar un tipo de procedimiento del consultorio.
        /// </summary>
        /// <param name="id">Id del tipo de procedimiento.</param>
        /// <returns>
        /// Un ViewResult con los datos de los tipos de procedimiento
        /// del consultorio.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Procedure procedure = db.Procedures.Find(id);
            db.Procedures.Remove(procedure);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
