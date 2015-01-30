using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTemplateMVC5.App.Shared;

namespace WebTemplateMVC5.Controllers
{
    public class ExamplesController : BaseController
    {
        // GET: Examples
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Success()
        {
            TempData["Message"] = "Success message sent";
            TempData["MessageCode"] = 1;

            return View("Index");
        }
        public ActionResult Error()
        {
            TempData["Message"] = "Error message sent";
            TempData["MessageCode"] = 0;

            return View("Index");
        }

        public JsonResult Ajax()
        {
            throw new Exception("Ajax error message sent");

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Tables()
        {
            return View();
        }

        public ActionResult DynamicObj()
        {
            return View(SharedRepository.RuntimeProps);
        }

        public ActionResult DeleteObj(string key)
        {
            SharedRepository.RuntimeAsDict.Remove(key);

            return RedirectToAction("DynamicObj");
        }

        [HttpPost]
        public ActionResult AddObj(string key, string value)
        {
            SharedRepository.RuntimeAsDict.Add(key, value);

            return RedirectToAction("DynamicObj");
        }
    }
}