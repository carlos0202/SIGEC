using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using SIGEC.Filters;
using SIGEC.Models;
using PWDTK_DOTNET451;
using SIGEC.CustomCode;
using SIGEC.Models.ViewModels;
using System.Data;
using System.Net.Mail;
using System.Net;
using SIGEC.Properties;
using SIGEC.Resources;

namespace SIGEC.Controllers
{
    /// <summary>
    /// Controlador que contiene las acciones básicas para los usuarios
    /// de la aplicación, tales como iniciar sesión, cerrar sesión,
    /// registrarse, actualizar datos de cuenta, etc.
    /// </summary>

    public class AccountController : BaseController
    {

        // GET: /Account/Login
        /// <summary>
        /// Acción para el inicio de sesión de los usuarios.
        /// </summary>
        /// <param name="returnUrl">
        /// Url local hacia la cual redireccionar luego de que
        /// el inicio de sesión se produzca.
        /// </param>
        /// <returns>
        /// un ViewResult que representa la pantalla de
        /// inicio de sesión.
        /// </returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl; //dejar disponible la url al view resultante.
            return View();
        }

        //
        // POST: /Account/Login
        /// <summary>
        /// Acción para verificaar los datos de inicio de sesión
        /// introducidos por el usuario.
        /// </summary>
        /// <param name="model">
        /// objeto del tipo LoginModel que contiene los datos de
        /// inicio de sesión. 
        /// </param>
        /// <param name="returnUrl">
        /// Url local hacia la cual redireccionar luego de que
        /// el inicio de sesión se produzca.
        /// </param>
        /// <returns>
        /// Si los datos de inicio de sesión son correctos, redirecciona
        /// al usuario a la dirección especificada en la variable returnUrl,
        /// si returnUrl no es una dirección válida, redirecciona al usuario
        /// a la acción definida por defecto en el RouteConfig. (Home/Index).
        /// Si los datos son incorrectos, muestra nuevamente la pantalla de
        /// inicio de sesión indicando los errores encontrados.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (Session["attempts"] != null && ((int)Session["attempts"] >= Settings.Default.MaxLoginAttempts))
            {
                ModelState.AddModelError("", _("lblMaxLoginsAttemptsErr"));

                return View(model);
            }
            Session["attempts"] = (Session["attempts"] == null) ? 1 : (int)Session["attempts"] + 1;
            var userID = WebSecurity.GetUserId(model.UserName);
            var user = db.Users.Find(userID);
            var valid = (userID != -1) ? WebSecurity.Login(user.username, model.Password, persistCookie: model.RememberMe) : false;
            if (user == null)
            {
                ModelState.AddModelError(
                    "",
                    string.Format(_("lblFailedAttemptsCount"), (int)Session["attempts"], Settings.Default.MaxLoginAttempts)
                );
                ModelState.AddModelError("", _("lblIncorrectPasswordErr"));
            }
            else if (!(bool)user.status)
            {
                ModelState.AddModelError(
                    "",
                    string.Format(_("lblFailedAttemptsCount"), (int)Session["attempts"], Settings.Default.MaxLoginAttempts)
                );
                ModelState.AddModelError("", _("lblAccountDisabledErr"));
                if (Session["attempts"] != null && ((int)Session["attempts"] >= Settings.Default.MaxLoginAttempts - 1))
                {
                    ModelState.AddModelError("", _("lblMaxLoginsAttemptsErr"));
                }
            }

            else if (valid)
            {
                user.lastVisit = DateTime.Now;
                db.SaveChanges();
                Session["attempts"] = 0;

                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError(
                    "",
                    string.Format(_("lblFailedAttemptsCount"), (int)Session["attempts"], Settings.Default.MaxLoginAttempts)
                );
                
                ModelState.AddModelError("", _("lblIncorrectPasswordErr"));
            }

            return View(model);
        }

        //
        // POST: /Account/LogOff
        /// <summary>
        /// Acción de cierre de sesión de los usuarios
        /// de la aplicación.
        /// </summary>
        /// <returns>un ViewResult de la pagina principal de la aplicación</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage()
        {
            dynamic model = new { };

            if (isDoctor)
            {
                var doctor = db.Doctors.FirstOrDefault(d => d.userID == WebSecurity.CurrentUserId);
                model = new DoctorViewModel();
                GlobalHelpers.Transfer<User, DoctorViewModel>(doctor.User, model);
                model.password = doctor.User.password;
                model.ConfirmPassword = doctor.User.password;
                model.maritalStatus = (MaritalStatus)Convert.ToChar(doctor.User.maritalStatus);
                model.gender = (Genders)Convert.ToChar(doctor.User.gender);
                model.speciality = doctor.speciality;

                return View("ManageDoctor", model);
            }
            else if (isPatient)
            {
                Patient patient = db.Patients.FirstOrDefault(d => d.userID == WebSecurity.CurrentUserId);
                model = new PatientViewModel();
                GlobalHelpers.Transfer<User, PatientViewModel>(patient.UserAccount, model);
                model.password = patient.UserAccount.password;
                model.ConfirmPassword = patient.UserAccount.password;
                model.maritalStatus = (MaritalStatus)Convert.ToChar(patient.UserAccount.maritalStatus);
                model.gender = (Genders)Convert.ToChar(patient.UserAccount.gender);

                return View("ManagePatient", model);
            }
            else if (isEmployee || isAdmin)
            {
                User user = db.Users.Find(WebSecurity.CurrentUserId);
                model = new EmployeeViewModel();
                GlobalHelpers.Transfer<User, EmployeeViewModel>(user, model);
                model.password = user.password;
                model.ConfirmPassword = user.password;
                model.maritalStatus = (MaritalStatus)Convert.ToChar(user.maritalStatus);
                model.gender = (Genders)Convert.ToChar(user.gender);

                return View("ManageEmployee", model);
            }
            else
            {
                return this.InvokeHttp404(HttpContext);
            }


        }

        [HttpPost]
        public ActionResult ManageDoctor(DoctorViewModel model, Address address, bool changeLoginInfo)
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
                return RedirectToAction("Index", "Home");
            }
            model.Address = address;

            return View(model);
        }

        [HttpPost]
        public ActionResult ManageEmployee(EmployeeViewModel model, Address address, bool changeLoginInfo)
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
                return RedirectToAction("Index", "Home");
            }
            model.Address = address;

            return View(model);
        }

        [HttpPost]
        public ActionResult ManagePatient(PatientViewModel model, Address address, bool changeLoginInfo)
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
                return RedirectToAction("Index", "Home");
            }
            model.Address = address;

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users
                    .FirstOrDefault(u => u.email == model.email);

                if (user == null)
                {
                    ModelState.AddModelError("", _("lblInvalidMailErr"));
                }
                else
                {
                    try
                    {
                        var fromAddress = new MailAddress(Settings.Default.SMTP_Mail, Settings.Default.SMTP_FromName);
                        var toAddress = new MailAddress(model.email, user.firstName);
                        string fromPassword = Settings.Default.SMTP_Password;
                        string subject = Language.ResetPasword_SubjectMsg;
                        var passwordHelper = new PasswordHelper();
                        var password = GlobalHelpers.CreateRandomPassword(10);
                        passwordHelper.HashGeneratedPassword(password);
                        user.password = PWDTK.HashBytesToHexString(passwordHelper.Hash);
                        user.salt = PWDTK.HashBytesToHexString(passwordHelper.Salt);
                        db.SaveChanges();
                        string body = string.Format(
                            Language.ResetPassword_BodyMsg, user.CompleteName,
                            user.username, password
                        );

                        var smtp = new SmtpClient
                        {
                            Host = Settings.Default.SMTP_Host,
                            Port = Convert.ToInt16(Settings.Default.SMTP_Port),
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                        };
                        using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body, IsBodyHtml = true })
                        { smtp.Send(message); }
                        TempData["success"] = _("lblSendMailSuccess");
                    }
                    catch
                    {
                        ModelState.AddModelError("", _("lblSendMailErr"));

                    }
                }
            }
            return View(model);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
