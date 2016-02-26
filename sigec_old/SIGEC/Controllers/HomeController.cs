using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGEC.CustomCode;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using System.Data;
using System.Data.SqlClient;
using WebMatrix.WebData;
using SIGEC.Models;

namespace SIGEC.Controllers
{
    /// <summary>
    /// Controlador con las acciones de las páginas
    /// principales de la aplicación, tales como la
    /// página de inicio, acerca de, y contacto.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// de inicio de la aplicación.
        /// </summary>
        /// <returns></returns>

        private SIGEC.Models.SIGECContext db = new Models.SIGECContext();
        public ActionResult Index()
        {
            ViewBag.Message = Resources.Language.Home_Title;
            var currentUser = db.Users.Find(WebSecurity.CurrentUserId);
            if (GlobalHelpers.isDoctor) {
            
                var doctorID = currentUser.Doctors.FirstOrDefault().ID;
                var consultations = db.Consultations.Where(c => c.doctorID == doctorID && c.ended == false).ToList();
                ViewBag.consultations = (consultations == null) ? Enumerable.Empty<Consultation>() : consultations;
            }
            return View();
        }

        /// <summary>
        /// Acción que muestra al usuario la pantalla de
        /// los datos del consultorio médico.
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            
            return View();
        }

        /// <summary>
        /// Acción que muestra al usuario la pantalla de
        /// contacto del consultorio médico.
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            
            return View();
        }

        public string GetTime()
        {
            var culture = (SIGEC.Code52.i18n.CultureHelper.GetCurrentCulture() == "es") ? "es-DO" : "en-US";

            return string.Format(GlobalHelpers.t("lblHomeTime"), DateTime.Now.ToString(new System.Globalization.CultureInfo(culture)));
        }

        public ActionResult TestReport()
        {
            var reportPath = Server.MapPath("~/Reports/DailyIncome.rpt");
            //DataSet src = new DataSet();
            //using (db)
            //{
            //    db.Database.Connection.Open();
            //    var command = db.Database.Connection.CreateCommand();
            //    command.CommandText = "dbo.UspGetPayments";
            //    command.CommandType = System.Data.CommandType.StoredProcedure;
            //    command.Parameters.Add(new SqlParameter("createDate", DateTime.Now));
            //    var test = (command.ExecuteReader());
            //    src.Load(test, LoadOption.Upsert, "Consultations_Payments", "Consultations", "Patients", "Users");
            //}

            //using (var reportDocument = new ReportDocument())
            //{
            //    reportDocument.Load(reportPath);
            //    //reportDocument.SetDataSource(src);

            //    var response = System.Web.HttpContext.Current.Response;
            //    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, response, false, "ConsultationsPayments");
            //}

            return new EmptyResult();
        }
    }
}
