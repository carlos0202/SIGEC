using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGEC.Models;
using SIGEC.CustomCode;
using PWDTK_DOTNET451;
using System.Data;
using System.Web.Security;
using SIGEC.Models.ViewModels;
using WebMatrix.WebData;

namespace SIGEC.Controllers
{
    /// <summary>
    /// Controlador que encapsula todas las acciones que
    /// se pueden realizar on los pacientes del consultorio. 
    /// </summary>
    [IsMenu]
    public class PatientsController : BaseController
    {
        //
        // GET: /Patients/
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// principal de los pacientes registrados en
        /// el consultorio médico.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de inicio de
        /// los pacientes.
        /// </returns>
        [IsView]
        public ActionResult Index()
        {
            var patients = db.Patients;

            return View(patients.ToList());
        }

        //
        // GET: /Patients/Details/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// que contiene los datos en detalle de un
        /// paciente registrado en el consultorio.
        /// </summary>
        /// <param name="id">el ID del paciente.</param>
        /// <returns>
        /// Un ViewResult con los datos del paciente
        /// en caso de que este exista en la base de
        /// datos, o de lo contrario un HttpNotFond,
        /// es decir un código 404 de http que indica
        /// que no se ha encontrado ningún resultado
        /// con el criterio de búsqueda.
        /// </returns>
        [IsView]
        public ActionResult Details(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(patient);
        }

        //
        // GET: /Patients/Create
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// de creación de un nuevo paciente.
        /// </summary>
        /// <returns>
        /// un ViewResult con la pantalla de creación
        /// de nuevo paciente en el consultorio.
        /// </returns>
        [IsView]
        public ActionResult Create()
        {
            PatientViewModel model = new PatientViewModel();

            return View(model);
        }

        //
        // POST: /Patients/Create
        /// <summary>
        /// Acción que captura la petición del usuario
        /// para registar un nuevo paciente.
        /// Si los datos del paciente son válidos, es
        /// registrado, de lo contrario se le muestra
        /// nuevamente la pantalla de registro al usuario
        /// mostrando los errores en el proceso de registro.
        /// </summary>
        /// <param name="model">datos básicos del paciente.</param>
        /// <param name="address">dirección del paciente.</param>
        /// <param name="Uphones">telefonos del paciente.</param>
        /// <returns>un ViewResult con los pacientes registrados.</returns>
        [HttpPost]
        public ActionResult Create(PatientViewModel model, Address address, string[] Uphones)
        {
            if (ModelState.IsValid)
            {
                db.Addresses.Add(address);
                var user = new User();
                GlobalHelpers.Transfer<PatientViewModel, User>(model, user, "Phones");
                user.gender = ((char)model.gender).ToString();
                user.maritalStatus = ((char)model.maritalStatus).ToString();
                var passwordHelper = new PasswordHelper();
                if (!passwordHelper.HashPassword(user.password))
                {
                    ModelState.AddModelError("", _("lblPasswordPolicyErr"));
                    return View(model);
                }
                user.password = PWDTK.HashBytesToHexString(passwordHelper.Hash);
                user.salt = PWDTK.HashBytesToHexString(passwordHelper.Salt);
                user.Address = address;
                user.status = true;
                user.superUser = false;
                db.Users.Add(user);
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
                var patient = new Patient();
                patient.userID = user.ID;
                patient.createBy = WebSecurity.CurrentUserId;
                db.Patients.Add(patient);
                db.SaveChanges();
                var roleProvider = (SimpleRoleProvider)Roles.Provider;
                roleProvider.AddUsersToRoles(new[] { model.username }, new[] { "Patient" });
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //
        // GET: /Patients/Edit/5
        /// <summary>
        /// Acción que muestra al usuario la pantalla
        /// para actualizar los datos de un paciente
        /// registrado en el sistema. Si los datos del paciente
        /// no son encontrados, se retornará un resultado con un
        /// HttpNotFound indicando que el criterio de búsqueda no
        /// produjo ningún resultado.
        /// </summary>
        /// <param name="id">ID del paciente</param>
        /// <returns>
        /// un ViewResult con la pantalla para editar los datos del
        /// paciente.
        /// </returns>
        [IsView]
        public ActionResult Edit(int id = 0)
        {
            Patient user = db.Patients.Find(id);
            var model = new PatientViewModel();
            if (user == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            GlobalHelpers.Transfer<User, PatientViewModel>(user.UserAccount, model);
            model.password = user.UserAccount.password;
            model.ConfirmPassword = user.UserAccount.password;
            model.maritalStatus = (MaritalStatus)Convert.ToChar(user.UserAccount.maritalStatus);
            model.gender = (Genders)Convert.ToChar(user.UserAccount.gender);
            //Para evitar que se sobreescriba el atributo dentro del model con 
            //el valor de la acción recibido como parámetro
            ModelState.Remove("id");
            return View(model);
        }

        //
        // POST: /Patients/Edit/5
        /// <summary>
        /// Acción que captura la petición del usuario de
        /// actualizar los datos de un paciente del sistema.
        /// Si los datos del paciente son inválidos, se le 
        /// mostrará la pantalla de edición nuevamente al
        /// usuario indicando los errores encontrados.
        /// </summary>
        /// <param name="model">datos básicos del paciente.</param>
        /// <param name="address">dirección del paciente.</param>
        /// <param name="changeLoginInfo">
        /// valor booleano que indica si se cambiaran los datos
        /// de inicio de sesión.
        /// </param>
        /// <returns>un ViewResult con los datos de los pacientes.</returns>
        [HttpPost]
        public ActionResult Edit(PatientViewModel model, Address address, bool changeLoginInfo)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(model.ID);
                GlobalHelpers.Transfer<PatientViewModel, User>(model, user, "Address,Phones,password");
                if (changeLoginInfo)
                {
                    if (user.password != model.password)
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
        // GET: /Patients/Delete/5
        /// <summary>
        /// Acción que muestra al usuario una pantalla
        /// para eliminar los datos de un paciente
        /// del consultorio. Si el paciente no es
        /// encontrado se retornará un HttpNotFound 
        /// indicándolo.
        /// </summary>
        /// <param name="id">ID del paciente.</param>
        /// <returns>
        /// Un ViewResult con la pantalla para eliminar el
        /// paciente.
        /// </returns>
        [IsView]
        public ActionResult Delete(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return this.InvokeHttp404(HttpContext);
            }
            return View(patient);
        }

        //
        // POST: /Patients/Delete/5
        /// <summary>
        /// Acción que captura la confirmación del usuario
        /// para eliminar un paciente del consultorio.
        /// </summary>
        /// <param name="id">Id del paciente.</param>
        /// <returns>
        /// Un ViewResult con los datos de los pacientes
        /// del consultorio.
        /// </returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            var user = db.Users.Find(patient.userID);
            user.Phones.Clear();
            var address = db.Addresses.Find(user.addressID);
            db.Addresses.Remove(address);

            if (user.webpages_Roles.Count() == 1)
            {
                foreach (var role in Roles.GetRolesForUser(user.username))
                {
                    // remover roles del paciente
                    Roles.RemoveUserFromRole(patient.UserAccount.username, role);
                }
                user.webpages_Roles.Clear();
                db.Users.Remove(user);
                db.Patients.Remove(patient);
            }
            else
            {
                var wp_role = db.webpages_Roles.Where(wr => wr.RoleName.ToLower() == "patient").FirstOrDefault();
                wp_role.Users.Remove(user);
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Acción que muestra al usuario la pantalla para
        /// introducir los datos del historial clínino del
        /// paciente. si los datos del paciente especificado
        /// no se encuentran, un HttpNotFound es retornado
        /// indicando que el criterio de búsqueda no produjo
        /// ningún resultado.
        /// </summary>
        /// <param name="id">ID del paiente.</param>
        /// <returns>
        /// un ViewResult con la pantalla para ingresar los
        /// datos del historial médico del paciente.
        /// </returns>
        [IsView]
        public ActionResult MedicalHistory(int id)
        {
            var patient = db.Patients.Find(id);

            if (patient == null)
            {
                return this.InvokeHttp404(HttpContext);
            }

            var history = db.MedicalHistories
                .FirstOrDefault(mh => mh.patientID == id);

            if (history == null)
            {
                history = new MedicalHistory();
                history.patientID = id;
                history.completed = false;
                db.MedicalHistories.Add(history);
                db.SaveChanges();
            }

            var model = new MedicalHistoryViewModel();
            model.patientID = id;
            model.Patient = patient;
            GlobalHelpers.Transfer<MedicalHistory, MedicalHistoryViewModel>(history, model);
            model.medicalhistoryID = history.ID;
            return View(model);
        }

        /// <summary>
        /// Acción que captura la petición del usuario para
        /// guardar los datos del historial médico del paciente.
        /// </summary>
        /// <param name="model">Datos del historial meédico.</param>
        /// <returns>un ViewResult con los pacientes registrados.</returns>
        [HttpPost]
        public ActionResult MedicalHistory(MedicalHistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var history = db.MedicalHistories.Find(model.medicalhistoryID);
                if(history == null){
                    return this.InvokeHttp404(HttpContext);
                }
                GlobalHelpers.Transfer<MedicalHistoryViewModel, MedicalHistory>(model, history, "Patient");
                db.Entry(history).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult AddFamilyDisease(int id, string familyMember, string disease)
        {
            try
            {
                var model = new InheritedDiseas();
                model.medicalHistoryID = id;
                model.disease = disease;
                model.familyMember = familyMember;
                db.InheritedDiseases.Add(model);
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetFamilyDiseases(int id)
        {
            var diseases = db.InheritedDiseases
                .Where(d => d.medicalHistoryID == id).ToList();

            return Json(
                new
                {
                    aaData = diseases.Select(
                        d => new[] { 
                        d.familyMember,
                        d.disease,
                        d.ID.ToString()
                    }
                    )
                },
                JsonRequestBehavior.AllowGet
            );
        }

        public ActionResult EditFamilyDisease(int id)
        {
            var disease = db.InheritedDiseases.Find(id);

            if (disease == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(
                new { disease.familyMember, disease.disease, disease.ID },
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public ActionResult EditFamilyDisease(int fmDiseaseID, string familyMember, string disease)
        {
            try
            {
                var idisease = db.InheritedDiseases.Find(fmDiseaseID);

                if (idisease == null)
                {
                    return Json(false, JsonRequestBehavior.DenyGet);
                }

                idisease.familyMember = familyMember;
                idisease.disease = disease;
                db.Entry(idisease).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteFamilyDisease(int id)
        {
            try
            {
                var disease = db.InheritedDiseases.Find(id);

                if (disease == null)
                {
                    return Json(false, JsonRequestBehavior.DenyGet);
                }

                db.InheritedDiseases.Remove(disease);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

    }
}
