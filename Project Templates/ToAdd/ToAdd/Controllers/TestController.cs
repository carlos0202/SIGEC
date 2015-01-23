using DGII_PFD.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Collections;

namespace DGII_PFD.Controllers
{
    public class TestController : BaseController
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            var procedimiento = new PFD_PROCEDIMIENTOS();
            using (db)
            {
                
                procedimiento = db.PFD_PROCEDIMIENTOS.FirstOrDefault(x => x.ID == 22);
                procedimiento.Parametros = procedimiento.PFD_PARAMETROS.ToList();
            }

            //PFD_PROCEDIMIENTOS procedimiento = new PFD_PROCEDIMIENTOS();
            //procedimiento.NOMBRE = "Sumar cantidades";
            //procedimiento.PROCEDIMIENTO = "SP_SUM_TEST";
            //procedimiento.DESCRIPCION = "Descripción o información adicional";
            //procedimiento.PFD_PARAMETROS = new List<PFD_PARAMETROS>(){
            //new PFD_PARAMETROS{ID = 1, TIPO = "int", REQUERIDO = 1, Required= true, NOMBRE = "Valor 1", PARAMETRO = "valor1", VALOR = 1},
            //new PFD_PARAMETROS{ID = 2, TIPO = "int", REQUERIDO = 0, NOMBRE = "Valor 2", PARAMETRO = "valor2", VALOR = 2},
            //new PFD_PARAMETROS{ID = 3, TIPO = "int", REQUERIDO = 0, NOMBRE = "Valor 3", PARAMETRO = "valor3", VALOR = 3}
            //};
            
            return View(procedimiento);

        }

        [HttpPost]
        public ActionResult Ejecutar(PFD_PROCEDIMIENTOS model)
        {
            var result = DbCommand.EjecutarProcedimiento(model);
            return Json(result);
        }

        public ActionResult CurrentUserProperties()
        {
            string currrent = this.GetCurrentUserName();
            DirectoryEntry directoryEntry = (UserPrincipal.Current.GetUnderlyingObject() as DirectoryEntry);
            List<KeyValuePair<string, string>> userData = new List<KeyValuePair<string, string>>();

            var properties = directoryEntry.Properties;
            foreach (String property in properties.PropertyNames)
            {
                userData.Add(new KeyValuePair<string, string>(property, properties[property].Value.ToString()));
            }

            return View(userData.OrderBy(t => t.Key));
        }

        public ActionResult GetAllUsers()
        {

            return View(ADUsers.Instance.GetUsers());
        }
    }
}
