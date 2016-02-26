using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGEC.Controllers
{
    public class HelpController : Controller
    {
        //
        // GET: /Help/

        public ActionResult Index()
        {

            ViewBag.Title = Resources.Language.Help_Title;
            ViewBag.Message = Resources.Language.Help_Msg;
    return View();
        }

        

    }
}
