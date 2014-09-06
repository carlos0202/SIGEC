using SIGEC.CustomCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;
using WebMatrix.WebData;
using SIGEC.Models.ViewModels;
using SIGEC.Models;
using SIGEC.Code52.i18n;
using System.Data;
//using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Data.SqlClient;
using SIGEC.Reports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace SIGEC.Controllers
{
    /// <summary>
    /// Controlador que contiene las acciones básicas que
    /// se pueden realizar con las consultas del consultorio
    /// médico.
    /// </summary>
    [IsMenu]
    public class ConsultationsController : BaseController
    {
        //
        // GET: /Consultations/
        [IsView]
        public ActionResult Index()
        {
            ViewBag.patientID = db.Consultations
                .Where(p => EntityFunctions.TruncateTime(p.createDate) ==
                            EntityFunctions.TruncateTime(DateTime.Now))
                .Select(p => p.Patient).ToList().ToSelectListItems(p => p.UserAccount.CompleteName, p => p.ID.ToString());
            var consultations = db.Consultations.Where(c => c.ended == true).ToList();

            var currentUser = db.Users.Find(WebSecurity.CurrentUserId);
            if (currentUser.isDoctor)
            {
                var doctorID = currentUser.Doctors.FirstOrDefault().ID;
                consultations = db.Consultations.Where(c => c.doctorID == doctorID).ToList();
            }

            if (GlobalHelpers.isPatient)
            {
                var patientID = db.Patients.FirstOrDefault(p => p.userID == WebSecurity.CurrentUserId).ID;
                consultations = db.Consultations.Where(c => c.patientID == patientID && c.ended == true).ToList();
            }

            return View(consultations);
        }

        //
        // GET: /Consultations/Details/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// que contiene los datos en detalle de una
        /// consulta registrada en el consultorio.
        /// </summary>
        /// <param name="id">el ID de la consulta</param>
        /// <returns>
        /// Un ViewResult con los datos de la consulta
        /// en caso de que este exista en la base de
        /// datos, o de lo contrario un HttpNotFond,
        /// es decir un código 404 de http que indica
        /// que no se ha encontrado ningún resultado
        /// con el criterio de búsqueda.
        /// </returns>
        [IsView]
        public ActionResult Details(int id = 0)
        {
            var consultation = db.Consultations.Find(id);
            if (consultation == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            var analysisID = consultation.Analyses.Select(a => a.ID).ToList();
            var studyID = consultation.Studies.Select(a => a.ID).ToList();
            var procedureID = consultation.Procedures.Select(a => a.ID).ToList();
            ViewBag.procedureID = db.Procedures.Where(p => p.status == true)
               .ToSelectListItems(p => p.name, p => p.ID.ToString(), p => procedureID != null && procedureID.Contains(p.ID));
            ViewBag.studyID = db.Studies.Where(p => p.status == true)
                .ToSelectListItems(p => p.name, p => p.ID.ToString(), s => studyID != null && studyID.Contains(s.ID));
            ViewBag.analysisID = db.Analyses.Where(p => p.status == true)
                .ToSelectListItems(p => p.name, p => p.ID.ToString(), a => analysisID != null && analysisID.Contains(a.ID));

            return View(consultation);
        }

        [IsView]
        public ActionResult Edit(int id)
        {
            var doctor = db.Doctors.FirstOrDefault(d => d.userID == WebSecurity.CurrentUserId);

            var consultation = db.Consultations
                .FirstOrDefault(c => c.ID == id
                                  && (c.ended == false || EntityFunctions.TruncateTime(c.createDate)
                                  == EntityFunctions.TruncateTime(DateTime.Now)));
            if (consultation == null)
            {
                return this.InvokeHttp404(HttpContext);
            }

            var model = new ConsultationsViewModel();
            GlobalHelpers.Transfer<Consultation, ConsultationsViewModel>(consultation, model);
            model.comments = consultation.observations;
            model.analysisID = consultation.Analyses.Select(a => a.ID).ToList();
            model.studyID = consultation.Studies.Select(a => a.ID).ToList();
            model.procedureID = consultation.Procedures.Select(a => a.ID).ToList();
            ViewBag.procedureID = db.Procedures.Where(p => p.status == true)
               .ToSelectListItems(p => p.name, p => p.ID.ToString(), p => model.procedureID != null && model.procedureID.Contains(p.ID));
            ViewBag.studyID = db.Studies.Where(p => p.status == true)
                .ToSelectListItems(p => p.name, p => p.ID.ToString(), s => model.studyID != null && model.studyID.Contains(s.ID));
            ViewBag.analysisID = db.Analyses.Where(p => p.status == true)
                .ToSelectListItems(p => p.name, p => p.ID.ToString(), a => model.analysisID != null && model.analysisID.Contains(a.ID));
            ViewBag.medicineID = db.Medicines.Where(p => p.status == true)
                .ToSelectListItems(p => p.name, p => p.ID.ToString());
            ViewBag.diagnoseCode = db.ICD10
                .ToSelectListItems(
                  i => i.Code + " - " + ((CultureHelper.GetCurrentNeutralCulture() == "es") ? i.Description_es : i.Description_en),
                  i => i.Code
                );

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ConsultationsViewModel model)
        {
            var doctor = db.Doctors.Find(model.doctorID);
            var patient = db.Patients.Find(model.patientID);

            if (model.nextAppointment != null)
            {
                // verificar si el doctor no ha definido sus reglas para consulta
                if (doctor.DoctorRules.Count() == 0)
                {
                    ModelState.AddModelError("nextAppointment", _("lblDcotorAPErr")); // Agregar error al modelo.
                }
                // verificar si el doctor consulta ese día.
                else if (!doctor.DoctorRules
                    .ElementAt(0).availableDays.Split(',')
                    .Contains(((int)((DateTime)model.nextAppointment).DayOfWeek).ToString()))
                {
                    ModelState.AddModelError("nextAppointment", _("lblDoctorConsDayErr"));
                }
                // verificar la cantidad de pacientes que han reservado cita ese día.
                else if (doctor.Appointments
                    .Where(a => a.appointmentDate.Date == ((DateTime)model.nextAppointment).Date).Count() >=
                    doctor.DoctorRules.ElementAt(0).maxPatients)
                {
                    ModelState.AddModelError(
                        "nextAppointment", string.Format(
                              _("lblDoctorMaxPatientsErr"),
                              doctor.DoctorRules.ElementAt(0).maxPatients
                            )
                    );
                }
                // verificar si el paciente tiene una cita con este doctor.
                else if (patient.Appointments.Where(a => a.status == true && a.doctorID == model.doctorID && a.finalStatus == null).Count() > 0)
                {
                    ModelState.AddModelError("nextAppointment", _("lblPatientAASErr"));
                }
            }

            if (ModelState.IsValid)
            {
                var c = db.Consultations.Find(model.ID);
                GlobalHelpers.Transfer<ConsultationsViewModel, Consultation>(model, c, "Patient");
                c.observations = model.comments;
                c.ended = true;
                c.Analyses.Clear();
                c.Studies.Clear();
                c.Procedures.Clear();

                if (model.analysisID != null)
                    foreach (int ID in model.analysisID)
                    {
                        c.Analyses.Add(db.Analyses.Find(ID));
                    }

                if (model.studyID != null)
                    foreach (int ID in model.studyID)
                    {
                        c.Studies.Add(db.Studies.Find(ID));
                    }

                if (model.procedureID != null)
                    foreach (int ID in model.procedureID)
                    {
                        c.Procedures.Add(db.Procedures.Find(ID));
                    }

                if (model.nextAppointment != null)
                {
                    var appointment = new Appointment();
                    appointment.doctorID = model.doctorID;
                    appointment.patientID = model.patientID;
                    appointment.createUser = WebSecurity.CurrentUserId;
                    appointment.appointmentDate = (DateTime)model.nextAppointment;
                    appointment.status = true;
                    db.Appointments.Add(appointment);
                    c.Appointment = appointment;
                }
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            var consultation = db.Consultations
                .FirstOrDefault(c => c.ID == model.ID
                                  && (c.ended == false || EntityFunctions.TruncateTime(c.createDate)
                                  == EntityFunctions.TruncateTime(DateTime.Now)));
            if (consultation == null)
            {
                return this.InvokeHttp404(HttpContext);
            }

            model.Patient = consultation.Patient;

            ViewBag.procedureID = db.Procedures.Where(p => p.status == true)
               .ToSelectListItems(p => p.name, p => p.ID.ToString(), p => model.procedureID != null && model.procedureID.Contains(p.ID));
            ViewBag.studyID = db.Studies.Where(p => p.status == true)
                .ToSelectListItems(p => p.name, p => p.ID.ToString(), s => model.studyID != null && model.studyID.Contains(s.ID));
            ViewBag.analysisID = db.Analyses.Where(p => p.status == true)
                .ToSelectListItems(p => p.name, p => p.ID.ToString(), a => model.analysisID != null && model.analysisID.Contains(a.ID));
            ViewBag.medicineID = db.Medicines.Where(p => p.status == true)
                .ToSelectListItems(p => p.name, p => p.ID.ToString());
            ViewBag.diagnoseCode = db.ICD10
                .ToSelectListItems(
                  i => i.Code + " - " + ((CultureHelper.GetCurrentNeutralCulture() == "es") ? i.Description_es : i.Description_en),
                  i => i.Code
                );

            return View(model);
        }

        public ActionResult AddDiagnostic(int ID, string observations, string diagnoseCode)
        {
            try
            {
                var disease = new Disease();
                disease.consultationID = ID;
                disease.observations = observations;
                disease.diagnoseCode = diagnoseCode;
                var consultation = db.Consultations.Find(ID);
                disease.patientID = consultation.patientID;

                db.Diseases.Add(disease);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateDiagnosticTable(int ID)
        {
            var diseases = db.Diseases
                .Where(d => d.consultationID == ID).ToList();

            return Json(
                new
                {
                    aaData = diseases.Select(
                        d => new[]{
                            d.diagnoseCode + " - " + ((CultureHelper.GetCurrentNeutralCulture() == "es") ? d.ICD10.Description_es : d.ICD10.Description_en),
                            d.observations,
                            d.ID.ToString()
                        }
                    )
                }, JsonRequestBehavior.AllowGet
            );

        }

        public ActionResult EditDiagnostic(int id)
        {
            var disease = db.Diseases.Find(id);
            if (disease == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(
                new
                {
                    disease.diagnoseCode,
                    disease.observations,
                    disease.ID
                }, JsonRequestBehavior.AllowGet
             );
        }

        [HttpPost]
        public ActionResult EditDiagnostic(int diseaseID, string diagnoseCode, string observations)
        {
            try
            {
                var disease = db.Diseases.Find(diseaseID);
                disease.diagnoseCode = diagnoseCode;
                disease.observations = observations;
                db.Entry(disease).State = EntityState.Modified;
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteDiagnostic(int id)
        {
            try
            {
                var disease = db.Diseases.Find(id);
                db.Diseases.Remove(disease);
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult AddMedicine(int ID, int medicineID, string administration)
        {
            try
            {
                var consultation = db.Consultations.Find(ID);
                var prescription = db.Prescriptions.FirstOrDefault(p => p.consultationID == ID);

                if (prescription == null)
                {
                    prescription = new Prescription();
                    prescription.consultationID = ID;
                    prescription.notes = "";
                    prescription.patientID = consultation.patientID;
                    db.Prescriptions.Add(prescription);
                    db.SaveChanges();
                }

                Prescriptions_Medicines pm = new Prescriptions_Medicines();
                pm.prescriptionID = prescription.ID;
                pm.medicineID = medicineID;
                pm.patientID = prescription.patientID;
                pm.consultationID = ID;
                pm.administration = administration;
                db.Prescriptions_Medicines.Add(pm);
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult EditMedicine(int id)
        {
            var prescriptionMedicine = db.Prescriptions_Medicines.Find(id);
            if (prescriptionMedicine == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(
                new
                {
                    prescriptionMedicine.medicineID,
                    prescriptionMedicine.administration,
                    prescriptionMedicine.ID
                }, JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public ActionResult EditMedicine(int pmID, string administration, int medicineID)
        {
            try
            {
                var pm = db.Prescriptions_Medicines.Find(pmID);
                pm.administration = administration;
                pm.medicineID = medicineID;
                db.Entry(pm).State = EntityState.Modified;
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteMedicine(int id)
        {
            try
            {
                var pm = db.Prescriptions_Medicines.Find(id);
                db.Prescriptions_Medicines.Remove(pm);
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult UpdateMedicinesTable(int ID)
        {
            var precriptionMedicines = db.Prescriptions_Medicines
                .Where(d => d.consultationID == ID).ToList();

            return Json(
                new
                {
                    aaData = precriptionMedicines.Select(
                        d => new[]{
                            d.Medicine.name,
                            d.administration,
                            d.ID.ToString()
                        }
                    )
                }, JsonRequestBehavior.AllowGet
            );
        }

        public ActionResult CheckNextAppointment(int ID, int patientID, int doctorID, Nullable<DateTime> nextAppointment)
        {
            if (nextAppointment == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            var nAppointment = (DateTime)nextAppointment;
            var doctor = db.Doctors.Find(doctorID);
            var patient = db.Patients.Find(patientID);
            // verificar si el doctor no ha definido sus reglas para consulta
            if (doctor.DoctorRules.Count() == 0)
            {
                return Json(("lblDcotorAPErr"), JsonRequestBehavior.AllowGet); // Agregar error al modelo.
            }
            // verificar si el doctor consulta ese día.
            else if (!doctor.DoctorRules
                .ElementAt(0).availableDays.Split(',')
                .Contains(((int)nAppointment.DayOfWeek).ToString()))
            {
                return Json(_("lblDoctorConsDayErr"), JsonRequestBehavior.AllowGet);
            }
            // verificar la cantidad de pacientes que han reservado cita ese día.
            else if (doctor.Appointments
                .Where(a => a.appointmentDate.Date == nAppointment.Date).Count() >=
                doctor.DoctorRules.ElementAt(0).maxPatients)
            {
                return Json(
                    string.Format(
                        _("lblDoctorMaxPatientsErr"),
                        doctor.DoctorRules.ElementAt(0).maxPatients
                    ), JsonRequestBehavior.AllowGet
                );
            }
            // verificar si el paciente tiene una cita con este doctor.
            else if (patient.Appointments.Where(a => a.status == true && a.doctorID == doctorID && a.finalStatus == null).Count() > 0)
            {
                return Json(_("lblPatientAASErr"), JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public ActionResult printAnalysis(int ID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("analysis", typeof(string));
            dt.Columns.Add("fullName", typeof(string));
            dt.Columns.Add("age", typeof(int));
            dt.Columns.Add("creationDate", typeof(DateTime));
            dt.Columns.Add("nextAppointment", typeof(DateTime));

            using (SIGECEntities db1 = new SIGECEntities())
            {
                var result = db1.PrintAnalysisIndication(ID).ToList();
                foreach (var a in result)
                {
                    dt.Rows.Add(a.analysis, a.fullName, a.age, a.creationDate, a.nextAppointnment);
                }
            }

            using (var reportDocument = new ReportDocument())
            {
                reportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "PrintableAnalysisIndication.rpt"));
                reportDocument.SetDataSource(dt);

                var response = System.Web.HttpContext.Current.Response;
                response.Buffer = false;
                response.ClearContent();
                response.ClearHeaders();
                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, response, false, "IndicacionAnalisis");
            }

            return new EmptyResult();
        }


        public ActionResult printProcedure(int ID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("procedureName", typeof(string));
            dt.Columns.Add("fullName", typeof(string));
            dt.Columns.Add("age", typeof(int));
            dt.Columns.Add("creationDate", typeof(DateTime));
            dt.Columns.Add("nextAppointment", typeof(DateTime));

            using (SIGECEntities db1 = new SIGECEntities())
            {
                var result = db1.PrintProcedureIndication(ID).ToList();
                foreach (var a in result)
                {
                    dt.Rows.Add(a.procedureName, a.fullName, a.age, a.creationDate, a.nextAppointnment);
                }

                ReportDocument rep = new ReportDocument();
                rep.Load(Path.Combine(Server.MapPath("~/Reports"), "PrintableProcedureIndication.rpt"));
                rep.SetDataSource(dt);

                try
                {
                    var response = System.Web.HttpContext.Current.Response;
                    rep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, response, false, "IndicacionProcedimiento");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return new EmptyResult();
        }


        public ActionResult printStudy(int ID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("studyName", typeof(string));
            dt.Columns.Add("fullName", typeof(string));
            dt.Columns.Add("age", typeof(int));
            dt.Columns.Add("creationDate", typeof(DateTime));
            dt.Columns.Add("nextAppointment", typeof(DateTime));

            using (SIGECEntities db1 = new SIGECEntities())
            {
                var result = db1.PrintStudyIndication(ID).ToList();

                foreach (var a in result)
                {
                    dt.Rows.Add(a.studyName, a.fullName, a.age, a.creationDate, a.nextAppointnment);
                }

                ReportDocument rep = new ReportDocument();
                rep.Load(Path.Combine(Server.MapPath("~/Reports"), "PrintableStudiesIndication.rpt"));
                rep.SetDataSource(dt);

                try
                {
                    var response = System.Web.HttpContext.Current.Response;
                    rep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, response, false, "IndicacionEstudios");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return new EmptyResult();
        }


        public ActionResult printPrescription(int ID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("medicine", typeof(string));
            dt.Columns.Add("dosage", typeof(string));
            dt.Columns.Add("fullName", typeof(string));
            dt.Columns.Add("age", typeof(int));
            dt.Columns.Add("creationDate", typeof(DateTime));
            dt.Columns.Add("nextAppointment", typeof(DateTime));

            using (SIGECEntities db1 = new SIGECEntities())
            {
                var result = db1.PrintPrescription(ID).ToList();

                foreach (var a in result)
                {
                    dt.Rows.Add(a.medicine, a.dosage, a.fullName, a.age, a.creationDate, a.nextAppointment);
                }

                ReportDocument rep = new ReportDocument();
                rep.Load(Path.Combine(Server.MapPath("~/Reports"), "PrintablePrescription.rpt"));
                rep.SetDataSource(dt);

                try
                {
                    var response = System.Web.HttpContext.Current.Response;
                    rep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, response, false, "Receta");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return new EmptyResult();
        }


        public ActionResult printIncomes(DateTime date)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("fullName", typeof(string));
            dt.Columns.Add("price", typeof(double));
            dt.Columns.Add("discount", typeof(double));
            dt.Columns.Add("insurer", typeof(double));
            dt.Columns.Add("total", typeof(double));
            dt.Columns.Add("reportDate", typeof(DateTime));

            using (SIGECEntities db1 = new SIGECEntities())
            {
                var result = db1.PrintIncomes(date).ToList();

                foreach (var a in result)
                {
                    dt.Rows.Add(a.No, a.fullName, a.price, a.discount, a.insurer, a.total, a.reportDate);
                }

                ReportDocument rep = new ReportDocument();
                rep.Load(Path.Combine(Server.MapPath("~/Reports"), "Incomes.rpt"));
                rep.SetDataSource(dt);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                try
                {
                    var response = System.Web.HttpContext.Current.Response;
                    rep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, response, false, "Receta");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return new EmptyResult();
        }
    }

}
