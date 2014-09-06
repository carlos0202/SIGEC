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
    [IsMenu]
    public class MedicamentsController : BaseController
    {

        //
        // GET: /Medicaments/
        /// <summary>
        /// Acción que muestra al usuario la página principal
        /// con los medicamentos registrados en este consultorio
        /// disponibles para las recetas.
        /// </summary>
        /// <returns>ViewReesult con los medicamentos de la aplicación.</returns>
        [IsView]
        public ActionResult Index()
        {
            return View(db.Medicines.ToList());
        }

        //
        // GET: /Medicaments/Details/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// que contiene los datos en detalle de un
        /// medicamento registrado en el consultorio.
        /// </summary>
        /// <param name="id">el ID del medicamento.</param>
        /// <returns>
        /// Un ViewResult con los datos del medicamento
        /// en caso de que este exista en la base de
        /// datos, o de lo conrario un HttpNotFond,
        /// es decir un código 404 de http que indica
        /// que no se ha encontrado ningún resultado
        /// con el criterio de búsqueda.
        /// </returns>
        [IsView]
        public ActionResult Details(int id = 0)
        {
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(medicine);
        }

        //
        // GET: /Medicaments/Create
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// de creación de un nuevo medicamento que será
        /// utilizado en el consultorio.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de creación
        /// de nuevo medicamento para el consultorio.
        /// </returns>
        [IsView]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Medicaments/Create
        /// Acción que captura la petición del usuario
        /// para registar el nuevo tipo de análisis.
        /// Si los datos del análisis son válidos, es
        /// registrado, de lo contrario se le muestra
        /// nuevamente la pantalla de registro al usuario
        /// mostrando los errores en el proceso de registro.
        /// </summary>
        /// <param name="medicine">datos del medicamento.</param>
        /// <returns>
        /// un ViewResult con los datos del medicamento registrado.
        /// </returns>
        [HttpPost]
        public ActionResult Create(Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                medicine.createdBy = WebSecurity.CurrentUserId;
                medicine.status = true;
                db.Medicines.Add(medicine);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = medicine.ID });
            }

            return View(medicine);
        }

        //
        // GET: /Medicaments/Edit/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// para actualizar los datos de un medicamento
        /// registrado en el sistema. Si los datos del 
        /// medicamento no son encontrados, se retornará 
        /// un HttpNotFound indicando que el criterio de 
        /// búsqueda no produjo ningún resultado.
        /// </summary>
        /// <param name="id">ID del medicamento</param>
        /// <returns>
        /// un ViewResult con la pantalla para editarlos datos
        /// del medicamento.
        /// </returns>
        [IsView]
        public ActionResult Edit(int id = 0)
        {
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            MedicineViewModel model = new MedicineViewModel();
            GlobalHelpers.Transfer<Medicine, MedicineViewModel>(medicine, model);
            return View(model);
        }

        //
        // POST: /Medicaments/Edit/5
        /// <summary>
        /// Acción que captura la petición del usuario de
        /// actualizar los datos de un medicamento registrado.
        /// Si los datos del análisis son inválidos, se le 
        /// mostrará la pantalla de edición nuevamente al
        /// usuario indicando los errores encontrados.
        /// </summary>
        /// <param name="model">
        /// Objeto con los dátos del medicamento a actualizar.
        /// </param>
        /// <returns>
        /// Un ViewResult con los medicamentos registrados.
        /// </returns>
        [HttpPost]
        public ActionResult Edit(MedicineViewModel model)
        {
            if (ModelState.IsValid)
            {
                var medicine = db.Medicines.Find(model.ID);
                GlobalHelpers.Transfer<MedicineViewModel, Medicine>(model, medicine);
                db.Entry(medicine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //
        // GET: /Medicaments/Delete/5
        /// <summary>
        /// Acción que muestra al usuario una pantalla
        /// para eliminar los datos de un medicamento
        /// del consultorio. Si el medicamento no es
        /// encontrado se retornará un HttpNotFound 
        /// indicándolo.
        /// </summary>
        /// <param name="id">ID del medicamento.</param>
        /// <returns>
        /// Un ViewResult con la pantalla para eliminar el
        /// medicamento.
        /// </returns>
        [IsView]
        public ActionResult Delete(int id = 0)
        {
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(medicine);
        }

        //
        // POST: /Medicaments/Delete/5
        /// <summary>
        /// Acción que captura la confirmación del usuario
        /// para eliminar un medicamento del consultorio.
        /// </summary>
        /// <param name="id">Id del medicamento.</param>
        /// <returns>
        /// Un ViewResult con los datos de los medicamentos
        /// del consultorio.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicine medicine = db.Medicines.Find(id);
            db.Medicines.Remove(medicine);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}