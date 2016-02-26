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
    /// Controlador que encapsula todas las acciones básicas
    /// a realizar en la entidad Analysis (CRUD, Manetnimiento).
    /// </summary>
    [Authorize]
    [IsMenu]
    public class AnalysisController : BaseController
    {
        
        //
        // GET: /Analysis/
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// principal de los análisis, donde se muestran
        /// todos los tipos de análisis registrados en
        /// la aplicación.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de inicio de
        /// los análisis.
        /// </returns>
        [IsView]
        public ActionResult Index()
        {
            var analyses = db.Analyses;
            return View(analyses.ToList());
        }

        //
        // GET: /Analysis/Details/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// que contiene los datos en detalle de un
        /// tipo de análisis de la apliación.
        /// </summary>
        /// <param name="id">el ID del análisis</param>
        /// <returns>
        /// Un ViewResult con los datos del análisis
        /// en caso de que este exista en la base de
        /// datos, o de lo contrario un HttpNotFond,
        /// es decir un código 404 de http que indica
        /// que no se ha encontrado ningún resultado
        /// con el criterio de búsqueda.
        /// </returns>
        [IsView]
        public ActionResult Details(int id = 0)
        {
            Analysis analysis = db.Analyses.Find(id);
            if (analysis == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(analysis);
        }

        //
        // GET: /Analysis/Create
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// de creación de un nuevo tipo de análisis.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de creación
        /// de nuevo tipo de Análisis para el consultorio.
        /// </returns>
        [IsView]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Analysis/Create
        /// <summary>
        /// Acción que captura la petición del usuario
        /// para registar el nuevo tipo de análisis.
        /// Si los datos del análisis son válidos, es
        /// registrado, de lo contrario se le muestra
        /// nuevamente la pantalla de registro al usuario
        /// mostrando los errores en el proceso de registro.
        /// </summary>
        /// <param name="analysis"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Analysis analysis)
        {
            if (ModelState.IsValid)
            {
                analysis.createdBy = WebSecurity.CurrentUserId;
                analysis.status = true;
                db.Analyses.Add(analysis);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = analysis.ID });
            }

            return View(analysis);
        }

        //
        // GET: /Analysis/Edit/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// para actualizar los datos de un tipo de análisis
        /// registrado en el sistema. Si los datos del análisis
        /// no son encontrados, se retornará un resultado con un
        /// HttpNotFound indicando que el criterio de búsqueda no
        /// produjo ningún resultado.
        /// </summary>
        /// <param name="id">ID de análisis</param>
        /// <returns>
        /// un ViewResult con la pantalla para editarlos datos del
        /// análisis.
        /// </returns>
        public ActionResult Edit(int id = 0)
        {
            Analysis analysis = db.Analyses.Find(id);
            if (analysis == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            AnalysisViewModel model = new AnalysisViewModel();
            //Copia de propiedades entre el ViewModel y el modelo real.
            GlobalHelpers.Transfer<Analysis, AnalysisViewModel>(analysis, model);

            return View(model);
        }

        //
        // POST: /Analysis/Edit/5
        /// <summary>
        /// Acción que captura la petición del usuario de
        /// actualizar los datos de un análisis del sistema.
        /// Si los datos del análisis son inválidos, se le 
        /// mostrará la pantalla de edición nuevamente al
        /// usuario indicando los errores encontrados.
        /// </summary>
        /// <param name="analysis">
        /// Objeto con los dátos del análisis a actualizar.
        /// </param>
        /// <returns>
        /// Un ViewResult con los tipos de análisis registrados.
        /// </returns>
        [HttpPost]
        [IsView]
        public ActionResult Edit(AnalysisViewModel analysis)
        {
            if (ModelState.IsValid)
            {
                var model = db.Analyses.Find(analysis.ID);
                //Copia de propiedades entre el ViewModel y el modelo real.
                GlobalHelpers.Transfer<AnalysisViewModel, Analysis>(analysis, model);
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(analysis);
        }

        //
        // GET: /Analysis/Delete/5
        /// <summary>
        /// Acción que muestra al usuario una pantalla
        /// para eliminar los datos de un tipo de análisis
        /// del consultorio. Si el tipo de análisis no es
        /// encontrado se retornará un HttpNotFound indicán-
        /// dolo.
        /// </summary>
        /// <param name="id">ID del tipo de análisis.</param>
        /// <returns>
        /// Un ViewResult con la pantalla para eliminar el
        /// análisis.
        /// </returns>
        [IsView]
        public ActionResult Delete(int id = 0)
        {
            Analysis analysis = db.Analyses.Find(id);
            if (analysis == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(analysis);
        }

        //
        // POST: /Analysis/Delete/5
        /// <summary>
        /// Acción que captura la confirmación del usuario
        /// para eliminar un tipo de análisis del consultorio.
        /// </summary>
        /// <param name="id">Id del tipo de análisis.</param>
        /// <returns>
        /// Un ViewResult con los datos de los tipos de análisis
        /// del consultorio.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Analysis analysis = db.Analyses.Find(id);
            db.Analyses.Remove(analysis);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}