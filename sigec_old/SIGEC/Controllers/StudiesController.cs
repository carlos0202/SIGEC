using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGEC.Models;
using WebMatrix.WebData;
using SIGEC.Models.ViewModels;
using SIGEC.CustomCode;

namespace SIGEC.Controllers
{
    /// <summary>
    /// Controlador que encapsula la lógica utilizada
    /// para manejar la información de los estudios
    /// del consultorio médico.
    /// </summary>
    [IsMenu]
    public class StudiesController : BaseController
    {

        //
        // GET: /Study/
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// principal de los estudios, donde se muestran
        /// todos los tipos de estudios registrados en
        /// la aplicación.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de inicio de
        /// los estudios.
        /// </returns>
        [IsView]
        public ActionResult Index()
        {
            var studies = db.Studies;
            return View(studies.ToList());
        }

        //
        // GET: /Study/Details/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// que contiene los datos en detalle de un
        /// tipo de estudios de la apliación.
        /// </summary>
        /// <param name="id">el ID del estudio</param>
        /// <returns>
        /// Un ViewResult con los datos del estudios
        /// en caso de que este exista en la base de
        /// datos, o de lo contrario un HttpNotFond,
        /// es decir un código 404 de http que indica
        /// que no se ha encontrado ningún resultado
        /// con el criterio de búsqueda.
        /// </returns>
        [IsView]
        public ActionResult Details(int id = 0)
        {
            Study Study = db.Studies.Find(id);
            if (Study == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(Study);
        }

        //
        // GET: /Study/Create
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// de creación de un nuevo tipo de estudio.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de creación
        /// de nuevo tipo de estudio para el consultorio.
        /// </returns>
        [IsView]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Study/Create
        /// <summary>
        /// Acción que captura la petición del usuario
        /// para registar el nuevo tipo de estudio.
        /// Si los datos del estudio son válidos, es
        /// registrado, de lo contrario se le muestra
        /// nuevamente la pantalla de registro al usuario
        /// mostrando los errores en el proceso de registro.
        /// </summary>
        /// <param name="study">datos del estudio.</param>
        /// <returns>datos en detalle del estudio registrado.</returns>
        [HttpPost]
        public ActionResult Create(Study study)
        {
            if (ModelState.IsValid)
            {
                study.createdBy = WebSecurity.CurrentUserId;
                study.status = true;
                db.Studies.Add(study);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = study.ID });
            }

            return View(study);
        }

        //
        // GET: /Study/Edit/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// para actualizar los datos de un tipo de estudio
        /// registrado en el sistema. Si los datos del estudio
        /// no son encontrados, se retornará un resultado con un
        /// HttpNotFound indicando que el criterio de búsqueda no
        /// produjo ningún resultado.
        /// </summary>
        /// <param name="id">ID de estudio</param>
        /// <returns>
        /// un ViewResult con la pantalla para editarlos datos del
        /// estudio.
        /// </returns>
        [IsView]
        public ActionResult Edit(int id = 0)
        {
            Study study = db.Studies.Find(id);
            if (study == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            StudyViewModel model = new StudyViewModel();
            GlobalHelpers.Transfer<Study, StudyViewModel>(study, model);

            return View(model);
        }

        //
        // POST: /Study/Edit/5
        /// <summary>
        /// Acción que captura la petición del usuario de
        /// actualizar los datos de un estudio del sistema.
        /// Si los datos del estudio son inválidos, se le 
        /// mostrará la pantalla de edición nuevamente al
        /// usuario indicando los errores encontrados.
        /// </summary>
        /// <param name="study">datos del estudio</param>
        /// <returns>datos en detalle del estudio actualizado.</returns>
        [HttpPost]
        public ActionResult Edit(StudyViewModel study)
        {
            if (ModelState.IsValid)
            {
                var model = db.Studies.Find(study.ID);
                GlobalHelpers.Transfer<StudyViewModel, Study>(study, model);
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = model.ID });
            }

            return View(study);
        }

        //
        // GET: /Study/Delete/5
        /// <summary>
        /// Acción que muestra al usuario una pantalla
        /// para eliminar los datos de un tipo de estudio
        /// del consultorio. Si el tipo de análisis no es
        /// encontrado se retornará un HttpNotFound indicán-
        /// dolo.
        /// </summary>
        /// <param name="id">ID del tipo de estudio.</param>
        /// <returns>
        /// Un ViewResult con la pantalla para eliminar el
        /// estudio.
        /// </returns>
        [IsView]
        public ActionResult Delete(int id = 0)
        {
            Study study = db.Studies.Find(id);
            if (study == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(study);
        }

        //
        // POST: /Study/Delete/5
        /// <summary>
        /// Acción que captura la confirmación del usuario
        /// para eliminar un tipo de estudio del consultorio.
        /// </summary>
        /// <param name="id">Id del tipo de estudio.</param>
        /// <returns>
        /// Un ViewResult con los datos de los tipos de estudio
        /// del consultorio.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Study study = db.Studies.Find(id);
            db.Studies.Remove(study);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
