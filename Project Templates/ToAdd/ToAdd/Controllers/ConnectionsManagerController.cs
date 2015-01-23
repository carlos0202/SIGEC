using DGII_PFD.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oracle.ManagedDataAccess.Client;
using CommonTasksLib.Data.ADOExtensions;
using DGII_PFD.Models;
using DGII_PFD.Helpers;

namespace DGII_PFD.Controllers
{

    [IsAuthorized("Administrador")]
    public class ConnectionsManagerController : BaseController
    {
        //
        // GET: /ConnectionsManager/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Create(ConnectionViewModel model)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public ActionResult CheckConnectionString(string ConnectionString)
        {
            using (var dao = new DAO(ConnectionString, InstanceType.Oracle))
            {
                dao.OpenConnection();
                var result = dao.Connection.State == System.Data.ConnectionState.Open;

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
