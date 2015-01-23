using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenericDAL;
using Oracle.ManagedDataAccess.Client;
using DGII_PFD.Models;
using CommonTasksLib.Data;
using CommonTasksLib.Collections;
using System.Data;
using System.Data.Common;

namespace DGII_PFD.Controllers
{
    using OracleGenericDao = GenericDAO<OracleCommand, OracleConnection, OracleDataAdapter>;
    using DGII_PFD.Filters;
    
    [IsAuthorized("Administrador")]
    public class FormsController : BaseController
    {
        //
        // GET: /Forms/

        public ActionResult Index()
        {
            var forms = db.PFD_PROCEDIMIENTOS.ToList();

            return View(forms);
        }

        public ActionResult Create()
        {
            ViewBag.TIPO = DataTypes.Texto.EnumToSelectList<DataTypes>(true);

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var procedimiento = new PFD_PROCEDIMIENTOS();
                model.Transfer(ref procedimiento);
                procedimiento.HABILITADO = Convert.ToDecimal(model.HABILITADO);
                db.PFD_PROCEDIMIENTOS.Add(procedimiento);
                db.SaveChanges();
                TempData["Message"] = "Formulario Agregado Satisfactoriamente";
                return RedirectToAction("Index");

            }
            ViewBag.TIPO = DataTypes.Texto.EnumToSelectList<DataTypes>(true);

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var procedimiento = db.PFD_PROCEDIMIENTOS.Find(id);

            if (procedimiento == null)
            {
                return HttpNotFound();
            }

            return View(procedimiento);
        }

        public ActionResult Edit(int id)
        {
            var procedimiento = db.PFD_PROCEDIMIENTOS.Find(id);

            if (procedimiento == null)
            {
                return HttpNotFound();
            }
            ViewBag.TIPO = DataTypes.Texto.EnumToSelectList<DataTypes>(true);

            return View(procedimiento);
        }

        [HttpPost]
        public ActionResult Edit(PFD_PROCEDIMIENTOS model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Formulario actualizado satisfactoriamente";

                return RedirectToAction("Index");
            }
            ViewBag.TIPO = DataTypes.Texto.EnumToSelectList<DataTypes>(true);

            return View(model);

        }

        public ActionResult Delete(int id)
        {
            var procedimiento = db.PFD_PROCEDIMIENTOS.Find(id);

            if (procedimiento == null)
            {
                return HttpNotFound();
            }

            return View(procedimiento);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var procedimiento = db.PFD_PROCEDIMIENTOS.Find(id);

            if (procedimiento == null)
            {
                return HttpNotFound();
            }

            foreach (var parametro in procedimiento.PFD_PARAMETROS.ToList())
            {
                db.PFD_PARAMETROS.Remove(parametro);
            }
            procedimiento.PFD_ROLES.Clear();
            db.PFD_PROCEDIMIENTOS.Remove(procedimiento);
            db.SaveChanges();
            TempData["Message"] = "Formulario Eliminado Satisfactoriamente.";

            return RedirectToAction("Index");
        }

        public ActionResult Assign(int id)
        {
            var procedimiento = db.PFD_PROCEDIMIENTOS.Find(id);
            var actualRoles = procedimiento.PFD_ROLES.Select(r => r.ID).ToList();
            if (procedimiento == null)
            {
                return HttpNotFound();
            }
            ViewBag.Roles = db.PFD_ROLES.ToList()
                .ToSelectListItems(r => r.NOMBRE, r => r.ID.ToString(), r => actualRoles.Contains(r.ID));

            return View(procedimiento);
        }

        [HttpPost]
        public ActionResult Assign(int ID, int[] Roles)
        {
            var procedimiento = db.PFD_PROCEDIMIENTOS.Find(ID);
            procedimiento.PFD_ROLES.Clear();
            if (Roles != null)
            {
                foreach (var role in Roles)
                {
                    procedimiento.PFD_ROLES.Add(
                        db.PFD_ROLES.Find(role));
                }
            }
            db.Entry(procedimiento).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Message"] = "Asignación realizada satisfactoriamente";

            return RedirectToAction("Index");
        }

        public ActionResult CheckProcedureName(string PROCEDIMIENTO)
        {
            string connection = GetConnectionString("OracleConn");

            if (string.IsNullOrEmpty(connection))
            {
                return Json(JsonResponseBase.ErrorResponse("Cadena de conexion no encontrada"));
            }
            OracleGenericDao dao = new OracleGenericDao(connection);
            dao.OpenConnection();
            Object[] prms = { PROCEDIMIENTO, 0 };
            Object[] dirs = { "Input", "Output" };
            string names = "P_NOMBRE_PROCEDIMIENTO,P_EXISTE";
            dao.ExecuteNonQuery("pfd_procedimientos_internos.check_procedurename", prms, names, dirs);
            var existe = dao.Command.Parameters["P_EXISTE"].Value;
            dao.CloseConnection();

            return Json(Convert.ToBoolean(existe), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetParametersInfo(string PROCEDIMIENTO)
        {
            string connection = GetConnectionString("OracleConn");
            if (string.IsNullOrEmpty(connection))
            {
                return Json(JsonResponseBase.ErrorResponse("Cadena de conexion no encontrada"));
            }
            OracleGenericDao dao = new OracleGenericDao(connection);
            dao.OpenConnection();
            Object[] prms = { PROCEDIMIENTO, null };
            Object[] dirs = { "Input", "Output" };
            string names = "P_NOMBRE_PROCEDIMIENTO,C_PARAMETERS";
            dao.FillCommand("pfd_procedimientos_internos.GET_PROCEDURE_PARAMS", prms, names, dirs);
            dao.Command.Parameters[1].OracleDbType = OracleDbType.RefCursor;
            OracleDataReader reader = dao.Command.ExecuteReader();

            var result = reader.Select(
                r => new {
                    NAME = r["ARGUMENT_NAME"], POSITION = r["POSITION"],
                    DATA_TYPE = r["DATA_TYPE"], IN_OUT = r["IN_OUT"] }).ToArray();
            dao.CloseConnection();

            return Json(JsonResponseBase.SuccessResponse(result, "Datos Obtenidos"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteParameters(decimal procedureID)
        {
            var procedure = db.PFD_PROCEDIMIENTOS.Find(procedureID);
            if (procedure == null)
            {
                return Json(JsonResponseBase.ErrorResponse("Procedimiento no encontrado"), JsonRequestBehavior.AllowGet);
            }
            foreach (var item in procedure.PFD_PARAMETROS.ToList())
            {
                db.PFD_PARAMETROS.Remove(item);
            }
            procedure.PFD_PARAMETROS.Clear();
            db.SaveChanges();

            return Json(new{ Result = true, Count = procedure.PFD_PARAMETROS.Count() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddParameter(ParameterViewModel parameter, int procedureID)
        {
            var model = new PFD_PARAMETROS();
            parameter.Transfer(ref model);
            model.REQUERIDO = Convert.ToInt16(parameter.REQUERIDO);
            model.ID_PROCEDIMIENTO = procedureID;
            db.PFD_PARAMETROS.Add(model);
            db.SaveChanges();
            var message = "Parámetro agregado satisfactoriamente";

            return Json(JsonResponseBase.SuccessResponse(message), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditParameter(int ID)
        {
            var param = db.PFD_PARAMETROS.Find(ID);
            var model = new ParameterViewModel();
            param.Transfer(ref model);
            model.REQUERIDO = param.Required;
            ViewBag.Mode = "editParameterModal";
            var type = (DataTypes)((int)model.TIPO);
            ViewBag.TIPO = type.EnumToSelectList<DataTypes>(true);

            return Json(RenderRazorViewToString("_ParameterPartial", model), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditParameter(ParameterViewModel param)
        {
            var model = db.PFD_PARAMETROS.Find(param.ID);
            param.Transfer(ref model);
            model.REQUERIDO = Convert.ToInt16(param.REQUERIDO);
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            var message = "Parámetro actualizado satisfactoriamente";

            return Json(JsonResponseBase.SuccessResponse(message));
        }

        public ActionResult UpdateParamsTable(decimal procedureID) 
        {
            var procedure = db.PFD_PROCEDIMIENTOS.Find(procedureID);
            var prms = procedure.PFD_PARAMETROS.ToList();
            return Json(new{
                aaData = prms.Select(n =>
                    new[]{
                        n.NOMBRE,
                        n.PARAMETRO,
                        (n.REQUERIDO == 1) ? "SI" : "NO",
                        (((DataTypes)((int)n.TIPO)).ToString().Replace('_', ' ')),
                        n.ID.ToString()
                    }),
                    JsonRequestBehavior.AllowGet});
        }

        [HttpPost]
        public ActionResult DeleteParameter(int ID)
        {
            var param = db.PFD_PARAMETROS.Find(ID);
            db.PFD_PARAMETROS.Remove(param);
            db.SaveChanges();
            var message = "Parámetro eliminado satisfactoriamente";

            return Json(JsonResponseBase.SuccessResponse(message));
        }

        public ActionResult GetLogs(int ID)
        {
            var procedure = db.PFD_PROCEDIMIENTOS.Find(ID);
            var model =procedure.PFD_LOG.ToList();

            return Json(RenderRazorViewToString("_FormLogsPartial", model), JsonRequestBehavior.AllowGet);
        }

    }
}
