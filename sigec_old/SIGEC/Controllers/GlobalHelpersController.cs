using SIGEC.CustomCode;
using SIGEC.Models;
using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SIGEC.Controllers
{
    /// <summary>
    /// Controlador utilitario que encapsula métodos globales
    /// empleados para la verificación de datos introducidos
    /// en distintas pantallas de la aplicación, y en otros
    /// casos también para editar datos de secciones generales
    /// de la aplicación, tales como datos que pueden pertenecer
    /// a varios tipos de entidades.
    /// </summary>
    public class GlobalHelpersController : BaseController
    {

        /// <summary>
        /// Método utilitario utilizado para verificar que
        /// los valores de las propiedades a las cuales se
        /// les aplica validación en el cliente sean únicos
        /// en la base de datos, evitando así que estos datos
        /// sean enviados para ser registrados a la base de 
        /// datos si no cumplen con esta condición.
        /// </summary>
        /// <param name="validateUniq">
        /// string compuesto con los datos que se validárán
        /// </param>
        /// <returns>
        /// un valor booleano de tipo javascript indicando 
        /// si existe un registro en la base de datos con 
        /// el valor enviado a validar.
        /// </returns>
        public ActionResult CheckUniq(string validateUniq)
        {
            if (string.IsNullOrEmpty(validateUniq))
            {
                validateUniq = Request.QueryString[1];
            }
            string id = "0";
            string[] parameter = validateUniq.Split('&');
            string formedQuery = "select count(*) from dbo.[{0}] where [{1}]=";
            string entityName = parameter[0], DisplayName = parameter[1];
            string condition = "";
            if (!Request.UrlReferrer.Segments.Contains("Create"))
            {
                id = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Count() - 1];

                condition = string.Format("AND ID!={0}", id);
            }

            string value = Request.QueryString[0];
            string fieldName = Request.QueryString.AllKeys[0];
            string select = string.Format(formedQuery, entityName, fieldName);
            string query = select + "{0} " + condition;
            object[] parameters = { value };
            var result = db.Database.SqlQuery<int>(query, parameters).ToList();
            if (result[0] > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Método utilizatrio utilizado para validar que
        /// el username introducido a la hora de registrar
        /// los uusario de la aplicación no exista previamente
        /// en la base de datos.
        /// </summary>
        /// <param name="username">username a validar.</param>
        /// <param name="ID">
        /// ID de usuario, en caso de que sea un proceso para
        /// editar los datos de un usuario existente.
        /// </param>
        /// <returns>
        /// Si el nombre de usuario ya existe, retorna un mensaje
        /// de error indicándolo, de lo contrario un valor booleano
        /// en formato javascript es retornando indicando que dicho
        /// nombre de usuario no existe en la base de datos.
        /// </returns>
        public JsonResult CheckUsername(string username, int ID = 0)
        {
            MembershipUser usrName = Membership.GetUser(username);

            if (ID != 0)
            {
                var user = db.Users.Find(ID);
                if (user != null)
                {
                    if (user.username == username)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            if (usrName != null)
            {
                return Json(string.Format(Language.lblUsernameTaken, username), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Método utilitario utilizado para verificar que los datos de
        /// un número de teléfono que se intente registrar no exista pre-
        /// viamente en la base de datos.
        /// </summary>
        /// <param name="phoneE">número telefónico a validar.</param>
        /// <param name="phoneID">ID del teléfono (si acción es actualizar).</param>
        /// <returns>
        /// un valor booleano en formato javascript indicando si el teléfono
        /// está registrado en la base de datos.
        /// </returns>
        public ActionResult CheckPhone(string phoneE, int phoneID = 0)
        {
            var matches = 0;
            if (phoneID == 0)
            {
                matches = db.Phones.Where(p => p.number == phoneE).Count();
            }
            else
            {
                matches = db.Phones.Where(p => p.number == phoneE && p.ID != phoneID).Count();
            }

            var returnValue = (matches > 0) ? false : true;

            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Método utilitario utilizado para registrar un nuevo teléfono en la
        /// base de datos.
        /// </summary>
        /// <param name="entityID">Id de entidad dueña del teléfono.</param>
        /// <param name="isUser">valro booleano que indica si es un usuario.</param>
        /// <param name="phoneE">número teléfonico.</param>
        /// <param name="type">tipo de teléfono.</param>
        /// <param name="notes">observaciones para este número telefónico.</param>
        /// <returns>un mensaje indicando que el teléfono ha sido registrado.</returns>
        public string AddPhone(int entityID, bool isUser, string phoneE, PhoneTypes type, string notes)
        {
            Phone number = new Phone();
            number.number = phoneE;
            number.type = (int)type;
            number.notes = notes;
            if (isUser)
            {
                var user = db.Users.Find(entityID);
                user.Phones.Add(number);
            }
            else
            {
                var insurer = db.Insurers.Find(entityID);
                insurer.Phones.Add(number);
            }
            db.SaveChanges();

            return _("lblPhoneAdded");
        }

        /// <summary>
        /// Método utilitario utilizado para eliminar los datos de
        /// un teléfono registrado en la base de datos.
        /// </summary>
        /// <param name="id">ID del teléfono a eliminar.</param>
        /// <returns>Un mensaje indicando que el teléfono ha sido eliminado.</returns>
        public string DeletePhone(int id)
        {
            var phone = db.Phones.Find(id);

            if (phone == null)
            {
                return "Telefono no encontrado";
            }
            phone.Insurers.Clear();
            phone.Users.Clear();
            db.Phones.Remove(phone);
            db.SaveChanges();

            return "Telefono eliminado correctamente";
        }

        /// <summary>
        /// Métodu utilitario utilizado para realizar una búsqueda
        /// en la base de datos de los datos de un paciente utilizando
        /// como criterio de búsqueda la cédula del mismo.
        /// </summary>
        /// <param name="dni">cédula del paciente a buscar.</param>
        /// <returns></returns>
        public ActionResult PatientSearch(string dni)
        {
            var patient = db.Patients.Where(p => p.User1.dni == dni)
                .Select(p => new { p.ID, p.User1.firstName, p.User1.lastName, p.User1.username })
                .FirstOrDefault();

            if (patient == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(patient, JsonRequestBehavior.AllowGet);
        }

    }
}
