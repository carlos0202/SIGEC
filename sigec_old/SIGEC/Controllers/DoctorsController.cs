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
    public class DoctorsController : BaseController
    {
        //
        // GET: /Doctors/
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// principal de los doctores registrados en
        /// la aplicación.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de inicio de
        /// los doctores.
        /// </returns>
        [IsView]
        public ActionResult Index()
        {
            var role = db.webpages_Roles
                .Where(wr => wr.RoleName.ToLower() == "doctor")
                .FirstOrDefault().RoleId;
            var doctors = db.Doctors
                .Where(usr => usr.User.superUser != true && usr.User.status == true
                           && usr.User.webpages_Roles.Select(wr => wr.RoleId).Contains(role));
            return View(doctors.ToList());
        }

        //
        // GET: /Doctors/Details/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// que contiene los datos en detalle de un
        /// doctor registrado en el consultorio.
        /// </summary>
        /// <param name="id">el ID del doctor</param>
        /// <returns>
        /// Un ViewResult con los datos del doctor
        /// en caso de que este exista en la base de
        /// datos, o de lo contrario un HttpNotFond,
        /// es decir un código 404 de http que indica
        /// que no se ha encontrado ningún resultado
        /// con el criterio de búsqueda.
        /// </returns>
        [IsView]
        public ActionResult Details(int id = 0)
        {
            var doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(doctor);
        }

        //
        // GET: /Doctors/Create
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
            DoctorViewModel model = new DoctorViewModel();

            return View(model);
        }

        //
        // POST: /Doctors/Create
        /// <summary>
        /// Acción que captura la petición del usuario
        /// para registar un nuevo doctor.
        /// Si los datos del nuevo doctor son válidos, es
        /// registrada, de lo contrario se le muestra
        /// nuevamente la pantalla de registro de doctor
        /// mostrando los errores en el proceso de registro.
        /// </summary>
        /// <param name="model">
        /// objeto con datos personales y de inicio de sesión del doctor
        /// </param>
        /// <param name="address">dirección del doctor</param>
        /// <param name="Uphones">telefonos a ser registrados</param>
        /// <returns>un ViewResult con los datos de doctores registrados</returns>
        [HttpPost]
        public ActionResult Create(DoctorViewModel model, Address address, string[] Uphones)
        {
            if (ModelState.IsValid)
            {
                db.Addresses.Add(address);
                var user = new User();
                GlobalHelpers.Transfer<DoctorViewModel, User>(model, user);
                user.gender = ((char)model.gender).ToString();
                user.maritalStatus = ((char)model.maritalStatus).ToString();
                // obtener hash de contraseña para almacenar en la bd.
                var passwordHelper = new PasswordHelper();
                if(!passwordHelper.HashPassword(user.password))
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
                var doctor = new Doctor();
                doctor.User = user;
                doctor.speciality = model.speciality;
                db.Doctors.Add(doctor);
                db.SaveChanges();
                var roleProvider = (SimpleRoleProvider)Roles.Provider;
                roleProvider.AddUsersToRoles(new[] { model.username }, new[] { "Doctor" });
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Doctors/Edit/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// para actualizar los datos de un doctor
        /// registrado en el sistema. Si los datos del doctor
        /// no son encontrados, se retornará un resultado con un
        /// HttpNotFound indicando que el criterio de búsqueda no
        /// produjo ningún resultado.
        /// </summary>
        /// <param name="id">ID del doctor</param>
        /// <returns>
        /// un ViewResult con la pantalla para editar los datos del
        /// doctor.
        /// </returns>
        [IsView]
        public ActionResult Edit(int id = 0)
        {
            var doctor = db.Doctors.Find(id);
            var model = new DoctorViewModel();
            if (doctor == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            GlobalHelpers.Transfer<User, DoctorViewModel>(doctor.User, model);
            model.password = doctor.User.password;
            model.ConfirmPassword = doctor.User.password;
            model.maritalStatus = (MaritalStatus)Convert.ToChar(doctor.User.maritalStatus);
            model.gender = (Genders)Convert.ToChar(doctor.User.gender);
            model.speciality = doctor.speciality;
            //Para evitar que se sobreescriba el atributo dentro del model con 
            //el valor de la acción recibido como parámetro
            ModelState.Remove("id");
            return View(model);
        }

        //
        // POST: /Doctors/Edit/5
        /// <summary>
        /// Acción que captura la petición del usuario de
        /// actualizar los datos de un doctor del sistema.
        /// Si los datos del paciente son inválidos, se le 
        /// mostrará la pantalla de edición nuevamente al
        /// usuario indicando los errores encontrados.
        /// </summary>
        /// <param name="model">datos básicos del doctor.</param>
        /// <param name="address">dirección del doctor.</param>
        /// <param name="changeLoginInfo">
        /// valor booleano que indica si se cambiaran los datos
        /// de inicio de sesión.
        /// </param>
        /// <returns>un ViewResult con los datos de los doctores.</returns>
        [HttpPost]
        public ActionResult Edit(DoctorViewModel model, Address address, bool changeLoginInfo)
        {
            if (ModelState.IsValid)
            {
                var doctor = db.Users.Find(model.ID);
                GlobalHelpers.Transfer<DoctorViewModel, User>(model, doctor, "Address,Phones,password");
                if (changeLoginInfo)
                {
                    if (doctor.password != model.password)
                    {
                        var passwordHelper = new PasswordHelper();
                        if (!passwordHelper.HashPassword(model.password))
                        {
                            model.Address = address;
                            ModelState.AddModelError("", _("lblPasswordPolicyErr"));
                            return View(model);
                        }
                        doctor.password = PWDTK.HashBytesToHexString(passwordHelper.Hash);
                        doctor.salt = PWDTK.HashBytesToHexString(passwordHelper.Salt);
                    }
                }

                GlobalHelpers.Transfer<Address, Address>(address, doctor.Address, "ID,Insurers,Users");
                doctor.gender = ((char)model.gender).ToString();
                doctor.maritalStatus = ((char)model.maritalStatus).ToString();
                var doctorData = db.Doctors.FirstOrDefault(d => d.userID == model.ID);
                doctorData.speciality = model.speciality;
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            model.Address = address;

            return View(model);
        }

        //
        // GET: /Doctors/Delete/5
        /// <summary>
        /// Acción que muestra al usuario una pantalla
        /// para eliminar los datos de un doctor
        /// del consultorio. Si el doctor no es
        /// encontrado se retornará un HttpNotFound 
        /// indicándolo.
        /// </summary>
        /// <param name="id">ID del doctor.</param>
        /// <returns>
        /// Un ViewResult con la pantalla para eliminar el
        /// doctor.
        /// </returns>
        [IsView]
        public ActionResult Delete(int id = 0)
        {
            var doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(doctor);
        }

        //
        // POST: /Doctors/Delete/5
        /// <summary>
        /// Acción que captura la confirmación del usuario
        /// para eliminar un doctor del consultorio.
        /// </summary>
        /// <param name="id">Id del doctor.</param>
        /// <returns>
        /// Un ViewResult con los datos de los doctores
        /// del consultorio.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var doctor = db.Doctors.Find(id);
            var user = db.Users.Find(doctor.userID);
            user.Phones.Clear();
            var address = db.Addresses.Find(user.addressID);
            db.Addresses.Remove(address);

            if (user.webpages_Roles.Count() == 1)
            {
                foreach (var role in Roles.GetRolesForUser(user.username))
                {
                    // remover roles del paciente
                    Roles.RemoveUserFromRole(doctor.User.username, role);
                }
                user.webpages_Roles.Clear();
                db.Doctors.Remove(doctor);
                db.Users.Remove(user);
            }
            else
            {
                var wp_role = db.webpages_Roles.Where(wr => wr.RoleName.ToLower() == "doctor").FirstOrDefault();
                wp_role.Users.Remove(user);
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [IsView]
        public ActionResult Rules(bool isDoctor, Nullable<int> id)
        {
            var doctor = new Doctor();
            if (isDoctor)
            {
                id = WebSecurity.CurrentUserId;
                doctor = db.Doctors.FirstOrDefault(d => d.userID == id);
            }
            else
            {
                doctor = db.Doctors.Find(id);
            }

            if (doctor == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            var rules = doctor.DoctorRules.FirstOrDefault();
            var model = new DoctorRulesViewModel();
            if (rules != null)
            {
                GlobalHelpers.Transfer<DoctorRule, DoctorRulesViewModel>(rules, model);
                model.availableDays = rules.availableDays.Split(',').Select(v => Int32.Parse(v)).ToList();
            }
            else
            {
                model.doctorID = doctor.ID;
                model.availableDays = new List<int>();
            }

            ViewBag.availableDays = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(v => new SelectListItem
                {
                    Text = _(v.ToString()),
                    Value = ((int)v).ToString(),
                    Selected = model.availableDays.Contains(((int)v))
                });
            model.doctor = doctor;
            ModelState.Remove("id");

            return View(model);
        }

        [HttpPost]
        public ActionResult Rules(DoctorRulesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctorRules = db.DoctorRules.Find(model.ID);
                GlobalHelpers.Transfer<DoctorRulesViewModel, DoctorRule>(model, doctorRules);
                doctorRules.consultationPrice = Convert.ToDecimal(model.consultationPrice);
                doctorRules.availableDays = string.Join(",", model.availableDays);
                db.Entry(doctorRules).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            model.doctor = db.Doctors.Find(model.doctorID);
            ViewBag.availableDays = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(v => new SelectListItem
            {
                Text = _(v.ToString()),
                Value = ((int)v).ToString(),
                Selected = model.availableDays.Contains(((int)v))
            });

            return View(model);
        }

    }
}
