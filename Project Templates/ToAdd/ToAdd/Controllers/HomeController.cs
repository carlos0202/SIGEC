using DGII_PFD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DGII_PFD.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Exception()
        {
            string a = null;
            return View(a);
        }

        [AllowAnonymous]
        public ActionResult ErrorUnauthorized()
        {
            return View();
        }

        public ActionResult TestTable()
        {
            ViewBag.roles = db.PFD_ROLES.ToList();
            
            return View();
        }

        [HttpPost]
        public ActionResult TestTable(PFD_ROLES model)
        {
            ViewBag.roles = db.PFD_ROLES.ToList();
            ModelState.AddModelError("", "Error 1");
            ModelState.AddModelError("", "Error 2");
            ModelState.AddModelError("ExceptionError", "Custon <i>named</i> <b>Exception</b>");

            return View(model);
        }
    }
}
