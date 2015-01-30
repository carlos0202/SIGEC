using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebTemplateMVC5.Controllers
{
    public class CustomErrorsController : Controller
    {
        //
        // GET: /CustomErrors/NotFound

        public ActionResult NotFound()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.NotFound;

            return View("NotFound");
        }
    }
}
