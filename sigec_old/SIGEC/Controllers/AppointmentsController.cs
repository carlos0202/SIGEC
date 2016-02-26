using SIGEC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGEC.CustomCode;
using SIGEC.Models;
using WebMatrix.WebData;

namespace SIGEC.Controllers
{
    [IsMenu]
    public class AppointmentsController : BaseController
    {
        //
        // GET: /Appointments/
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// principal de las citas, donde se muestran
        /// todas las citas registradas en la
        /// aplicación.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de inicio de
        /// las citas.
        /// </returns>
        [IsView]
        public ActionResult Index()
        {
            var appointments = db.Appointments.ToList();

            if (GlobalHelpers.isPatient)
            {
                var patient = db.Patients
                    .FirstOrDefault(p => p.userID == WebSecurity.CurrentUserId);
                appointments = db.Appointments
                    .Where(a => a.patientID == patient.ID).ToList();
            }

            return View(appointments);
        }

        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// que contiene los datos en detalle de una
        /// cita reservada en el consultorio.
        /// </summary>
        /// <param name="id">el ID de la cita</param>
        /// <returns>
        /// Un ViewResult con los datos de la cita
        /// en caso de que este exista en la base de
        /// datos, o de lo contrario un HttpNotFond,
        /// es decir un código 404 de http que indica
        /// que no se ha encontrado ningún resultado
        /// con el criterio de búsqueda.
        /// </returns>
        [IsView]
        public ActionResult Details(int id)
        {
            var appointment = db.Appointments.Find(id);

            if (appointment == null)
            {
                return this.InvokeHttp404(HttpContext);
            }

            return View(appointment);
        }

        //
        // GET: /Appointments/Create
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// de creación de un nueva cita.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de creación
        /// de nueva cita en el consultorio.
        /// </returns>
        [IsView]
        public ActionResult Create()
        {
            AppointmentViewModel model = new AppointmentViewModel();
            var patients = db.Patients.Where(p => p.User1.status == true).ToList();
            var doctors = db.Doctors.Where(d => d.User.status == true).ToList();
            ViewBag.patientID = patients.ToSelectListItems(p => p.User1.CompleteName, p => p.ID.ToString());
            ViewBag.doctorID = doctors.ToSelectListItems(d => d.User.CompleteName, d => d.ID.ToString());
            ViewBag.patients = patients;

            return View(model);
        }

        //
        // POST: /Appointments/Create
        /// <summary>
        /// Acción que captura la petición del usuario
        /// para registar la nueva cita médica.
        /// Si los datos de la nueva cita son válidos, es
        /// registrada, de lo contrario se le muestra
        /// nuevamente la pantalla de registro cita de usuario
        /// mostrando los errores en el proceso de registro.
        /// </summary>
        /// <param name="model">Datos de la cita</param>
        /// <returns>ViewResult con los datos de citas registradas.</returns>
        [HttpPost]
        public ActionResult Create(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = db.Doctors.Find(model.doctorID);
                var patient = db.Patients.Find(model.patientID);
                // verificar si el doctor no ha definido sus reglas para consulta
                if (doctor.DoctorRules.Count() == 0) 
                {
                    ModelState.AddModelError("", _("lblDcotorAPErr")); // Agregar error al modelo.
                }
                // verificar si el doctor consulta ese día.
                else if (!doctor.DoctorRules
                    .ElementAt(0).availableDays.Split(',') 
                    .Contains(((int)model.appointmentDate.DayOfWeek).ToString()))
                {
                    ModelState.AddModelError("", _("lblDoctorConsDayErr"));
                }
                // verificar la cantidad de pacientes que han reservado cita ese día.
                else if (doctor.Appointments
                    .Where(a => a.appointmentDate.Date == model.appointmentDate.Date).Count() >=
                    doctor.DoctorRules.ElementAt(0).maxPatients)
                {
                    ModelState.AddModelError(
                        "", string.Format(
                              _("lblDoctorMaxPatientsErr"),
                              doctor.DoctorRules.ElementAt(0).maxPatients
                            )
                    );
                }
                // verificar si el paciente tiene una cita con este doctor.
                else if (patient.Appointments.Where(a => a.status == true && a.doctorID == model.doctorID && a.finalStatus == null).Count() > 0)
                {
                    ModelState.AddModelError("", _("lblPatientAASErr"));
                }
                else
                {
                    var appointment = new Appointment();
                    GlobalHelpers.Transfer<AppointmentViewModel, Appointment>(model, appointment);
                    appointment.createUser = WebSecurity.CurrentUserId;
                    appointment.status = true;
                    db.Appointments.Add(appointment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            var patients = db.Patients.Where(p => p.User1.status == true).ToList();
            var doctors = db.Doctors.Where(d => d.User.status == true).ToList();
            ViewBag.patientID = patients.ToSelectListItems(p => p.User1.CompleteName, p => p.ID.ToString());
            ViewBag.doctorID = doctors.ToSelectListItems(d => d.User.CompleteName, d => d.ID.ToString());
            ViewBag.patients = patients;

            return View(model);
        }

        //
        // GET: Appointments/ScheduleAppointment
        /// <summary>
        /// Acción que muestra a los pacientes la pantalla
        /// para registrar una nueva cita en el consultorio.
        /// </summary>
        /// <returns>un ViewResult con la pantalla de registro de cita.</returns>
        [IsView]
        public ActionResult ScheduleAppointment()
        {
            AppointmentViewModel model = new AppointmentViewModel();
            var patients = db.Patients.Where(p => p.User1.status == true).ToList();
            var doctors = db.Doctors.Where(d => d.User.status == true).ToList();
            int id = WebSecurity.CurrentUserId;
            model.patientID = db.Patients.Where(p => p.userID == id).FirstOrDefault().ID;
            ViewBag.doctorID = doctors.ToSelectListItems(d => d.User.CompleteName, d => d.ID.ToString());

            return View(model);
        }

        //
        // POST: /Appointments/ScheduleAppointment
        /// <summary>
        /// Acción que captura la petición del paciente
        /// para registar la nueva cita médica.
        /// Si los datos de la nueva cita son válidos, es
        /// registrada, de lo contrario se le muestra
        /// nuevamente la pantalla de registro cita al paciente
        /// mostrando los errores en el proceso de registro.
        /// </summary>
        /// <param name="model">Datos de la cita</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ScheduleAppointment(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = db.Doctors.Find(model.doctorID);
                var patient = db.Patients.Find(model.patientID);
                // verificar si el doctor no ha definido sus reglas para consulta
                if (doctor.DoctorRules.Count() == 0)
                {
                    ModelState.AddModelError("", _("lblDcotorAPErr"));
                }
                // verificar si el doctor consulta ese día.
                else if (!doctor.DoctorRules
                    .ElementAt(0).availableDays.Split(',')
                    .Contains(((int)model.appointmentDate.DayOfWeek).ToString()))
                {
                    ModelState.AddModelError("", _("lblDoctorConsDayErr"));
                }
                // verificar la cantidad de pacientes que han reservado cita ese día.
                else if (doctor.Appointments
                    .Where(a => a.appointmentDate.Date == model.appointmentDate.Date).Count() >=
                    doctor.DoctorRules.ElementAt(0).maxPatients)
                {
                    ModelState.AddModelError(
                        "", string.Format(
                              _("lblDoctorMaxPatientsErr"),
                              doctor.DoctorRules.ElementAt(0).maxPatients
                            )
                    );
                }
                // verificar si el paciente tiene una cita con este doctor.
                else if (patient.Appointments.Where(a => a.status == true && a.doctorID == model.doctorID && a.finalStatus == null).Count() > 0)
                {
                    ModelState.AddModelError("", _("lblPatientSAAErr"));
                }
                else
                {
                    var appointment = new Appointment();
                    GlobalHelpers.Transfer<AppointmentViewModel, Appointment>(model, appointment);
                    appointment.createUser = WebSecurity.CurrentUserId;
                    appointment.status = true;
                    db.Appointments.Add(appointment);
                    db.SaveChanges();
                    return RedirectToAction("ScheduleDetails", new { id = appointment.ID });
                }
            }
            var patients = db.Patients.Where(p => p.User1.status == true).ToList();
            var doctors = db.Doctors.Where(d => d.User.status == true).ToList();
            ViewBag.patientID = patients.ToSelectListItems(p => p.User1.CompleteName, p => p.ID.ToString());
            ViewBag.doctorID = doctors.ToSelectListItems(d => d.User.CompleteName, d => d.ID.ToString());
            return View(model);
        }

        /// <summary>
        /// Acción que muestra al paciente la pantalla
        /// que contiene los datos en detalle de una
        /// cita reservada en el consultorio.
        /// </summary>
        /// <param name="id">el ID de la cita</param>
        /// <returns>
        /// Un ViewResult con los datos de la cita
        /// en caso de que este exista en la base de
        /// datos, o de lo contrario un HttpNotFond,
        /// es decir un código 404 de http que indica
        /// que no se ha encontrado ningún resultado
        /// con el criterio de búsqueda.
        /// </returns>
        [IsView]
        public ActionResult ScheduleDetails(int id)
        {
            var appointment = db.Appointments.Find(id);

            if (appointment == null)
            {
                return this.InvokeHttp404(HttpContext);
            }

            return View(appointment);
        }

        //
        // GET: /Appointments/Edit/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// para actualizar los datos de una cita
        /// registrada en el sistema. Si los datos de la cita
        /// no son encontrados, se retornará un resultado con un
        /// HttpNotFound indicando que el criterio de búsqueda no
        /// produjo ningún resultado.
        /// </summary>
        /// <param name="id">ID de la cita</param>
        /// <returns>
        /// un ViewResult con la pantalla para editar los datos de
        /// la cita.
        /// </returns>
        [IsView]
        public ActionResult Edit(int id = 0)
        {
            var appointment = db.Appointments.Find(id);

            if (appointment == null)
            {
                return this.InvokeHttp404(HttpContext);
            }

            var patients = db.Patients.Where(p => p.User1.status == true).ToList();
            var doctors = db.Doctors.Where(d => d.User.status == true).ToList();
            ViewBag.patientID = patients
                .ToSelectListItems(p => p.User1.CompleteName, p => p.ID.ToString(), p => p.ID == appointment.patientID);
            ViewBag.doctorID = doctors
                .ToSelectListItems(d => d.User.CompleteName, d => d.ID.ToString(), d => d.ID == appointment.doctorID);
            ViewBag.patients = patients;
            var model = new AppointmentViewModel();
            GlobalHelpers.Transfer<Appointment, AppointmentViewModel>(appointment, model);

            return View(model);
        }

        //
        // POST: /Appointments/Edit/5
        /// <summary>
        /// Acción que captura la petición del usuario de
        /// actualizar los datos de una cita registrada.
        /// Si los datos de la cita son inválidos, se le 
        /// mostrará la pantalla de edición nuevamente al
        /// usuario indicando los errores encontrados.
        /// </summary>
        /// <param name="model">
        /// Objeto con los dátos de la cita a actualizar.
        /// </param>
        /// <returns>
        /// Un ViewResult con las citas registradas.
        /// </returns>
        [HttpPost]
        public ActionResult Edit(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = db.Doctors.Find(model.doctorID);
                // verificar si el doctor no ha definido sus reglas para consulta
                if (!doctor.DoctorRules
                    .ElementAt(0).availableDays.Split(',')
                    .Contains(((int)model.appointmentDate.DayOfWeek).ToString()))
                {
                    ModelState.AddModelError("", _("lblDoctorConsDayErr"));
                }
                // verificar si el doctor consulta ese día.
                else if (doctor.Appointments
                    .Where(a => a.appointmentDate.Date == model.appointmentDate.Date).Count() >=
                    doctor.DoctorRules.ElementAt(0).maxPatients)
                {
                    ModelState.AddModelError(
                        "", string.Format(
                              _("lblDoctorMaxPatientsErr"),
                              doctor.DoctorRules.ElementAt(0).maxPatients
                            )
                    );
                }
                else
                {
                    var appointment = db.Appointments.Find(model.ID);
                    GlobalHelpers.Transfer<AppointmentViewModel, Appointment>(model, appointment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            var patients = db.Patients.Where(p => p.User1.status == true).ToList();
            var doctors = db.Doctors.Where(d => d.User.status == true).ToList();
            ViewBag.patientID = patients.ToSelectListItems(p => p.User1.CompleteName, p => p.ID.ToString());
            ViewBag.doctorID = doctors.ToSelectListItems(d => d.User.CompleteName, d => d.ID.ToString());
            ViewBag.patients = patients;
            return View(model);
        }

        //
        // GET: /Appointments/Delete/5
        /// <summary>
        /// Acción que muestra al usuario una pantalla
        /// para eliminar los datos de una cita registrada
        /// en el consultorio. Si la cita no es
        /// encontrada se retornará un HttpNotFound indicán-
        /// dolo.
        /// </summary>
        /// <param name="id">ID de la cita.</param>
        /// <returns>
        /// Un ViewResult con la pantalla para eliminar la
        /// cita registrada.
        /// <summary>
        /// Acción que captura la confirmación del usuario
        /// para eliminar una cita programada en el consultorio.
        /// </summary>
        /// <param name="id">Id de la cita registrada.</param>
        /// <returns>
        /// Un ViewResult con los datos de las citas reigstradas
        /// en el consultorio.
        /// </returns>
        [IsView]
        public ActionResult Delete(int id)
        {
            var appointment = db.Appointments.Find(id);

            if (appointment == null)
            {
                return this.InvokeHttp404(HttpContext);
            }

            return View(appointment);
        }

        //
        // POST: /Appointments/Delete/5
        /// <summary>
        /// Acción que captura la confirmación del usuario
        /// para eliminar una cita programada en el consultorio.
        /// </summary>
        /// <param name="id">Id de la cita registrada.</param>
        /// <returns>
        /// Un ViewResult con los datos de las citas reigstradas
        /// en el consultorio.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var appointment = db.Appointments.Find(id);

            if (appointment == null)
            {
                return this.InvokeHttp404(HttpContext);
            }

            appointment.finalStatus = AppointmentStatus.Cancelled.ToString();
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
