using SIGEC.CustomCode;
using SIGEC.Models;
using SIGEC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SIGEC.Controllers
{
    /// <summary>
    /// Controlador que encapsula toda la logica presente en las acciones
    /// de pagos de servicios del consultorio médico.
    /// </summary>
    [IsMenu]
    public class PaymentsController : BaseController
    {
        //
        // GET: /Payments/

        public ActionResult Index()
        {
            return View();
        }

        [IsView]
        public ActionResult PayConsultation()
        {
            ViewBag.patientID = db.Patients
                .Where(p => p.User1.status == true
                         && p.Appointments.FirstOrDefault() != null
                         && EntityFunctions.TruncateTime(p.Appointments.FirstOrDefault(a => a.finalStatus == null).appointmentDate)
                         == EntityFunctions.TruncateTime(DateTime.Now)).ToList()
                .ToSelectListItems(p => p.UserAccount.CompleteName, p => p.ID.ToString());
            ViewBag.doctorID = db.Doctors
                .Where(d => d.User.status == true).ToList()
                .ToSelectListItems(d => d.User.CompleteName, d => d.ID.ToString());
            ViewBag.insurerID = db.Insurers
                .Where(i => i.status == true)
                .ToSelectListItems(i => i.name, i => i.ID.ToString());
            ViewBag.paymentForm = ((PaymentForms)0).EnumToList(false);
            ViewBag.patients = db.Patients.Where(p => p.User1.status == true).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult PayConsultation(ConsultationPaymentViewModel model)
        {

            if (model.price < (model.discount + model.insurer))
            {
                ModelState.AddModelError("", _("lblPaymentAmountsErr"));
            }

            var consultation = db.Consultations
                .FirstOrDefault(c => c.patientID == model.patientID && c.doctorID == model.doctorID
                         && EntityFunctions.TruncateTime(c.createDate) == EntityFunctions.TruncateTime(DateTime.Now));

            if (consultation != null && consultation.Consultations_Payment.FirstOrDefault() != null)
            {
                ModelState.AddModelError("alreadyPayed", _("lblPaymentMadeErr"));
            }

            if (ModelState.IsValid)
            {
                var c = new Consultation();
                GlobalHelpers.Transfer<ConsultationPaymentViewModel, Consultation>(model, c);
                var cp = new Consultations_Payment();
                cp.Consultation = c;
                GlobalHelpers.Transfer<ConsultationPaymentViewModel, Consultations_Payment>(model, cp);
                db.Consultations.Add(c);
                db.Consultations_Payment.Add(cp);
                var appointment = db.Appointments.FirstOrDefault(a => a.finalStatus == null && a.patientID == model.patientID);
                if (appointment != null)
                {
                    appointment.finalStatus = AppointmentStatus.Assisted.ToString();
                    db.Entry(appointment).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index", "Consultations");
            }
            ViewBag.patientID = db.Patients
                .Where(p => p.User1.status == true
                         && p.Appointments.FirstOrDefault(a => a.finalStatus == null) != null
                         && EntityFunctions.TruncateTime(p.Appointments.FirstOrDefault(a => a.finalStatus == null).appointmentDate)
                         == EntityFunctions.TruncateTime(DateTime.Now)).ToList()
                .ToSelectListItems(p => p.UserAccount.CompleteName, p => p.ID.ToString(), p => p.ID == model.patientID);
            ViewBag.doctorID = db.Doctors
                .Where(d => d.User.status == true).ToList()
                .ToSelectListItems(d => d.User.CompleteName, d => d.ID.ToString(), d => d.ID == model.doctorID);
            ViewBag.insurerID = db.Insurers
                .Where(i => i.status == true)
                .ToSelectListItems(i => i.name, i => i.ID.ToString(), i => i.ID == model.insurerID);
            ViewBag.paymentForm = (GlobalHelpers.ParseEnum<PaymentForms>(model.paymentForm)).EnumToList(false);
            ViewBag.patients = db.Patients.Where(p => p.User1.status == true).ToList();

            return View(model);
        }

        public ActionResult GetPrice(int doctorID)
        {
            var rules = db.DoctorRules.FirstOrDefault(dr => dr.doctorID == doctorID);
            if (rules == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(
                rules.consultationPrice.ToString("##.00", CultureInfo.InvariantCulture),
                JsonRequestBehavior.AllowGet
            );
        }

        public ActionResult GetPatients(bool hasAppointment)
        {
            dynamic patients = new { };

            if (hasAppointment)
            {
                patients = db.Patients
                .Where(p => p.User1.status == true
                         && p.Appointments.FirstOrDefault(a => a.finalStatus == null) != null
                         && EntityFunctions.TruncateTime(p.Appointments.FirstOrDefault(a => a.finalStatus == null).appointmentDate)
                         == EntityFunctions.TruncateTime(DateTime.Now)).ToList()
                .ToSelectListItems(p => p.UserAccount.CompleteName, p => p.ID.ToString());
            }
            else
            {
                patients = db.Patients
                .Where(p => p.User1.status == true
                         && p.Appointments.FirstOrDefault(a => a.finalStatus == null) == null).ToList()
                .ToSelectListItems(p => p.UserAccount.CompleteName, p => p.ID.ToString());
            }

            return Json(patients, JsonRequestBehavior.AllowGet);
        }
    }
}
