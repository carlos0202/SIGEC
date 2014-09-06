using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGEC.Models;
using SIGEC.CustomCode;
using WebMatrix.WebData;
using SIGEC.Models.ViewModels;

namespace SIGEC.Controllers
{
    /// <summary>
    /// Controlador que posee las acciones que
    /// se pueden realizar a los datos de las 
    /// aseguradoras que serán manejadas en el
    /// consultorio médico.
    /// </summary>
    [IsMenu]
    public class InsurersController : BaseController
    {
        //
        // GET: /Insurers/
        /// <summary>
        /// Acción que muestra al usuario la pantalla principal
        /// de la sección de aseguradoras, donde se muestran todas
        /// las aseguradoras registradas en el consuotorio médico.
        /// </summary>
        /// <returns>un ViewResult con la pantalla de las aseguraoras.</returns>
        [IsView]
        public ActionResult Index()
        {
            var insurers = db.Insurers.Include(i => i.Address);
            return View(insurers.ToList());
        }

        //
        // GET: /Insurers/Details/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// que contiene los datos en detalle de una
        /// aseguradora registrada en el consultorio.
        /// </summary>
        /// <param name="id">el ID de la aseguradora</param>
        /// <returns>
        /// Un ViewResult con los datos de la aseguradora
        /// en caso de que este exista en la base de
        /// datos, o de lo contrario un HttpNotFond,
        /// es decir un código 404 de http que indica
        /// que no se ha encontrado ningún resultado
        /// con el criterio de búsqueda.
        /// </returns>
        [IsView]
        public ActionResult Details(int id = 0)
        {
            Insurer insurer = db.Insurers.Find(id);
            if (insurer == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(insurer);
        }

        //
        // GET: /Insurers/Create
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// de creación de un nueva aseguradora.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de creación
        /// de nueva aseguradora en el consultorio.
        /// </returns>
        [IsView]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Insurers/Create
        /// <summary>
        /// Acción que captura la petición del usuario
        /// para registar la nueva aseguradora.
        /// Si los datos de la nueva aseguradora son válidos, es
        /// registrada, de lo contrario se le muestra
        /// nuevamente la pantalla de registro de aseguradora
        /// mostrando los errores en el proceso de registro.
        /// </summary>
        /// <param name="insurer">Datos de la aseguradora</param>
        /// <param name="address">dirección de la aseguradora.</param>
        /// <param name="Uphones">teléfonos de la aseguradora.</param>
        /// <returns>ViewResult con los datos de aseguradoras registradas.</returns>
        [HttpPost]
        public ActionResult Create(Insurer insurer, Address address, string[] Uphones)
        {
            if (ModelState.IsValid)
            {
                if (Uphones != null)
                {
                    foreach (string n in Uphones)
                    {
                        var phone = new Phone();
                        var data = n.Split('|');
                        phone.number = data[0];
                        phone.type = (int)GlobalHelpers.ParseEnum<PhoneTypes>(data[1]);
                        phone.notes = data[2];
                        db.Phones.Add(phone);
                        insurer.Phones.Add(phone);
                    }
                }
                insurer.createdBy = WebSecurity.CurrentUserId;
                insurer.status = true;
                insurer.Address = address;
                db.Insurers.Add(insurer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insurer);
        }

        //
        // GET: /Insurers/Edit/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// para actualizar los datos de una aseguradora
        /// registrada en el sistema. Si los datos de la aseguradora
        /// no son encontrados, se retornará un resultado con un
        /// HttpNotFound indicando que el criterio de búsqueda no
        /// produjo ningún resultado.
        /// </summary>
        /// <param name="id">ID de la aseguradora</param>
        /// <returns>
        /// un ViewResult con la pantalla para editarlos datos de 
        /// la aseguradora.
        /// </returns>
        [IsView]
        public ActionResult Edit(int id = 0)
        {
            Insurer insurer = db.Insurers.Find(id);
            if (insurer == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            InsurerViewModel model = new InsurerViewModel();
            GlobalHelpers.Transfer<Insurer, InsurerViewModel>(insurer, model);

            return View(model);
        }

        //
        // POST: /Insurers/Edit/5
        /// <summary>
        /// Acción que captura la petición del usuario de
        /// actualizar los datos de una aseguradora del sistema.
        /// Si los datos de la aseguradora son inválidos, se le 
        /// mostrará la pantalla de edición nuevamente al
        /// usuario indicando los errores encontrados.
        /// </summary>
        /// <param name="model">
        /// Objeto con los dátos de la aseguradora a actualizar.
        /// </param>
        /// <param name="address">dirección de la aseguradora</param>
        /// <returns>
        /// Un ViewResult con las aseguradoras registradas.
        /// </returns>
        [HttpPost]
        public ActionResult Edit(InsurerViewModel model, Address address)
        {
            if (ModelState.IsValid)
            {
                var insurer = db.Insurers.Find(model.ID);
                GlobalHelpers.Transfer<InsurerViewModel, Insurer>(model, insurer, "ID,Address,addressID");
                GlobalHelpers.Transfer<Address, Address>(address, insurer.Address, "ID,Insurers,Users");
                db.Entry(insurer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Insurers/Delete/5
        /// <summary>
        /// Acción que muestra al usuario una pantalla
        /// para eliminar los datos de una aseguradora
        /// registrada. Si los datos de la aseguradora no 
        /// son encontrado se retornará un HttpNotFound 
        /// indicándolo.
        /// </summary>
        /// <param name="id">ID de la aseguradora.</param>
        /// <returns>
        /// Un ViewResult con la pantalla para eliminar la
        /// aseguradora.
        /// </returns>
        [IsView]
        public ActionResult Delete(int id = 0)
        {
            Insurer insurer = db.Insurers.Find(id);
            if (insurer == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(insurer);
        }

        //
        // POST: /Insurers/Delete/5
        /// <summary>
        /// Acción que captura la confirmación del usuario
        /// para eliminar una aseguradora registrada.
        /// </summary>
        /// <param name="id">Id de la aseguradora. </param>
        /// <returns>
        /// Un ViewResult con los datos de las aseguradoras
        /// registradas en el consultorio.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Insurer insurer = db.Insurers.Find(id);
            insurer.Phones.Clear();
            db.Insurers.Remove(insurer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}