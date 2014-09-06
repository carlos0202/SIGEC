using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGEC.Models;
using SIGEC.Models.ViewModels;
using SIGEC.CustomCode;
using WebMatrix.WebData;
using System.Web.Security;
using PWDTK_DOTNET451;

namespace SIGEC.Controllers
{
    [IsMenu]
    public class EmployeesController : BaseController
    {
        //
        // GET: /Employees/
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// principal de los empleados registrados en
        /// la aplicación.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de inicio de
        /// los empleados.
        /// </returns>
        [IsView]
        public ActionResult Index()
        {
            var role = db.webpages_Roles
                .Where(wr => wr.RoleName.ToLower() == "employee")
                .FirstOrDefault().RoleId;
            var users = db.Users
                .Where(usr => usr.superUser != true 
                           && usr.webpages_Roles.Select(wr => wr.RoleId).Contains(role));
            return View(users.ToList());
        }

        //
        // GET: /Employees/Details/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// que contiene los datos en detalle de un
        /// empleado registrado en el consultorio.
        /// </summary>
        /// <param name="id">el ID del empleado</param>
        /// <returns>
        /// Un ViewResult con los datos del empleado
        /// en caso de que este exista en la base de
        /// datos, o de lo contrario un HttpNotFond,
        /// es decir un código 404 de http que indica
        /// que no se ha encontrado ningún resultado
        /// con el criterio de búsqueda.
        /// </returns>
        [IsView]
        public ActionResult Details(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(user);
        }

        //
        // GET: /Employees/Create
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// de registro de un nuevo empleado.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de registro de
        /// nuevo empleado del consultorio.
        /// </returns>
        [IsView]
        public ActionResult Create()
        {
            EmployeeViewModel model = new EmployeeViewModel();

            return View(model);
        }

        //
        // POST: /Employees/Create
        /// <summary>
        /// Acción que captura la petición del usuario
        /// para registar un nuevo empleado.
        /// Si los datos dl nuevo empleado son válidos, es
        /// registrada, de lo contrario se le muestra
        /// nuevamente la pantalla de registro de empleado
        /// mostrando los errores en el proceso de registro.
        /// </summary>
        /// <param name="model">
        /// objeto con datos personales y de inicio de sesión del empleado
        /// </param>
        /// <param name="address">dirección del empleado</param>
        /// <param name="Uphones">telefonos a ser registrados</param>
        /// <returns>un ViewResult con los datos de empleados registrados</returns>
        [HttpPost]
        public ActionResult Create(EmployeeViewModel model, Address address, string[] Uphones)
        {
            if (ModelState.IsValid)
            {
                db.Addresses.Add(address);
                var user = new User();
                GlobalHelpers.Transfer<EmployeeViewModel, User>(model, user);
                user.gender = ((char)model.gender).ToString();
                user.maritalStatus = ((char)model.maritalStatus).ToString();
                // obtener hash de contraseña para almacenar en la bd. 
                var passwordHelper = new PasswordHelper();
                if (!passwordHelper.HashPassword(user.password))
                {
                    ModelState.AddModelError("", _("lblPasswordPolicyErr"));
                    return View(model);
                }
                user.password = PWDTK.HashBytesToHexString(passwordHelper.Hash);
                user.salt = PWDTK.HashBytesToHexString(passwordHelper.Salt);
                user.Address = address;
                user.superUser = false;
                user.status = true;
                db.Users.Add(user);
                // Agregar telefonos
                if (Uphones != null)
                {
                    foreach (string n in Uphones)
                    {
                        var phone = new Phone();
                        var data = n.Split('|');
                        phone.number = data[0];
                        phone.type = (int)GlobalHelpers.ParseEnum<PhoneTypes>(data[1]);
                        phone.notes = data[2];
                        db.Phones.Add(phone);
                        user.Phones.Add(phone);
                    }
                }
                db.SaveChanges();
                var roleProvider = (SimpleRoleProvider)Roles.Provider;
                roleProvider.AddUsersToRoles(new[] { model.username }, new[] { "Employee" });
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Employees/Edit/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// para actualizar los datos de un empleado
        /// registrada en el sistema. Si los datos del empleado
        /// no son encontrados, se retornará un resultado con un
        /// HttpNotFound indicando que el criterio de búsqueda no
        /// produjo ningún resultado.
        /// </summary>
        /// <param name="id">ID del empleado</param>
        /// <returns>
        /// un ViewResult con la pantalla para editar los datos del
        /// empleado
        /// </returns>
        [IsView]
        public ActionResult Edit(int id = 0)
        {
            User user = db.Users.Find(id);
            var model = new EmployeeViewModel();
            if (user == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            GlobalHelpers.Transfer<User, EmployeeViewModel>(user, model);
            model.password = user.password;
            model.ConfirmPassword = user.password;
            model.maritalStatus = (MaritalStatus)Convert.ToChar(user.maritalStatus);
            model.gender = (Genders)Convert.ToChar(user.gender);
            return View(model);
        }

        //
        // POST: /Employees/Edit/5
        /// <summary>
        /// Acción que captura la petición del usuario de
        /// actualizar los datos de un empleado registrado.
        /// Si los datos del empleado son inválidos, se le 
        /// mostrará la pantalla de edición nuevamente al
        /// usuario indicando los errores encontrados.
        /// </summary>
        /// <param name="model">Datos personales y de inicio de sesión del empleado</param>
        /// <param name="address">Dirección del empleado</param>
        /// <param name="changeLoginInfo">valor que indica si se desea cambiar datos de inicio de sesión</param>
        /// <returns>un ViewResult con la pantalla de los empleados registrados del consultorio.</returns>
        [HttpPost]
        public ActionResult Edit(EmployeeViewModel model, Address address, bool changeLoginInfo)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(model.ID);
                // transferir propiedaades entre el modelo de empleado y el objeto de usuario
                // exceptuando las propiedades especificadas en el último argumento.
                GlobalHelpers.Transfer<EmployeeViewModel, User>(model, user, "Address,Phones,password");
                if (changeLoginInfo) // si se especificó cambiar los datos de inicio de sesión
                {
                    if (user.password != model.password) // si se cambió la contraseña
                    {
                        var passwordHelper = new PasswordHelper();
                        if (!passwordHelper.HashPassword(model.password))
                        {
                            model.Address = address;
                            ModelState.AddModelError("", _("lblPasswordPolicyErr"));
                            return View(model);
                        }
                        user.password = PWDTK.HashBytesToHexString(passwordHelper.Hash);
                        user.salt = PWDTK.HashBytesToHexString(passwordHelper.Salt);
                    }
                }

                GlobalHelpers.Transfer<Address, Address>(address, user.Address, "ID,Insurers,Users");
                user.gender = ((char)model.gender).ToString();
                user.maritalStatus = ((char)model.maritalStatus).ToString();
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            model.Address = address;

            return View(model);
        }

        //
        // GET: /Employees/Delete/5
        /// <summary>
        /// Acción que muestra al usuario una pantalla
        /// para eliminar los datos de un empleado registrado
        /// en el consultorio. Si el empleado no es
        /// encontrada se retornará un HttpNotFound indicán-
        /// dolo.
        /// </summary>
        /// <param name="id">ID del empleado.</param>
        /// <returns>
        /// Un ViewResult con la pantalla para eliminar el
        /// empleado registrado.
        [IsView]
        public ActionResult Delete(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(user);
        }

        //
        // POST: /Employees/Delete/5
        /// <summary>
        /// Acción que captura la confirmación del usuario
        /// para eliminar un empleado registrado en el consultorio.
        /// </summary>
        /// <param name="id">Id del empleado.</param>
        /// <returns>
        /// Un ViewResult con los datos de los empleado reigstrados
        /// en el consultorio.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            user.Phones.Clear();
            db.Addresses.Remove(user.Address);
            foreach (var role in Roles.GetRolesForUser(user.username))
            {
                Roles.RemoveUserFromRole(user.username, role);
            }
            user.webpages_Roles.Clear();
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Método utilitario que busca en la base de datos
        /// los datos de un teléfono registrado usando como
        /// criterio el valor del id especificado como argu-
        /// mento. Si el teléfono es encontrado, se retorna
        /// como un objeto en notación javascript (Json), de
        /// lo contrario se retorna un mensaje indicando que
        /// no ha sido encontrado.
        /// </summary>
        /// <param name="id">ID del teléfono a buscar.</param>
        /// <returns>un arreglo javascript (Json) con los datos del teléfono</returns>
        public ActionResult EditPhone(int id)
        {
            var phone = db.Phones.Find(id);
            if (phone == null)
            {
                return Json("No encontrado", JsonRequestBehavior.AllowGet);
            }

            return Json(new { phone.ID, phone.number, type = ((PhoneTypes)phone.type).ToString(), phone.notes }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Método utilitario que actualiza en la base de datos
        /// los datos de un teléfono registrado usando como cri-
        /// terio el id del teléfono especificado como argumento.
        /// </summary>
        /// <param name="phoneID">Id del teléfono a actualizar.</param>
        /// <param name="phone">número de teléfono.</param>
        /// <param name="type">tipo de teléfono.</param>
        /// <param name="notes">observaciones para dicho teléfono.</param>
        /// <returns>un mensaje que indica que se ha actualizado correctamente el teléfono.</returns>
        [HttpPost]
        public string UpdatePhone(int phoneID, string phone, PhoneTypes type, string notes)
        {
            var p = db.Phones.Find(phoneID);
            p.number = phone;
            p.type = (int)type;
            p.notes = notes;
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();

            return "Numero actualizado correctamente";
        }

        /// <summary>
        /// Método utilitario que obtiene los datos de lso teléfonos
        /// registrados en la base de datos de un usuario o de una
        /// aseguradora, y los retorna en notación javascript para
        /// ser mostrados en una tabla html.
        /// </summary>
        /// <param name="id">
        /// ID de la entidad a la que pertenecen dichos teléfonos (Usuario, Aseguradora)
        /// </param>
        /// <param name="isUser">valor que indica si los teléfonos son de un usuario.</param>
        /// <returns>un JsonResult de los datos de los teléfonos en notación javascript.</returns>
        public ActionResult UpdatePhonesTable(int id, bool isUser)
        {
            var phones = new List<Phone>();
            if (isUser)
            {
                phones = db.Users.Find(id).Phones.ToList();
            }
            else
            {
                phones = db.Insurers.Find(id).Phones.ToList();
            }
            return Json(
                new
                {
                    aaData = phones.Select(
                      p => new[] {
                           p.number.CFormat("phone"),
                           GlobalHelpers.t(((PhoneTypes)p.type).ToString()),
                           p.notes,
                           p.ID.ToString()
                       }
                    )
                },
                JsonRequestBehavior.AllowGet
             );
        }

    }
}