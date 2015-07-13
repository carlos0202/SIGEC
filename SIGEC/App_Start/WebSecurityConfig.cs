using PWDTK_DOTNET451;
using SIGEC.CustomCode;
using SIGEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace SIGEC
{
    public static class WebSecurityConfig
    {
        /// <summary>
        /// Metodo utilizado para inicializar la configuración del
        /// plugin WebSecurity, y la inicialización de la conexión
        /// de la base de datos, tomando en cuenta la tabla de la 
        /// base de datos que almacenará los datos de los usuarios
        /// de la aplicación. También en este método se insertan los
        /// datos por defecto requeridos para el mínimo funcionamiento
        /// de la aplicación, tales como roles y un usuario por defecto.
        /// </summary>
        public static void RegisterWebSec()
        {
            ///inizializar el websecurity, especificando la tabla
            ///que guardará los datos básicos de usuario.
            WebSecurity.InitializeDatabaseConnection
                (
                 "SIGECContext",
                 "Users",
                 "ID",
                 "username",
                 autoCreateTables: true
                );

            ///creación de la cuenta por defecto de administrador.
            if (!WebSecurity.UserExists("admin"))
            {
                
                using (SIGECContext db = new SIGECContext())
                {
                    Address a = new Address();
                    a.city = "Ciudad";
                    a.municipality = "Municipio";
                    a.sector = "Sector";
                    a.street = "Calle";
                    a.number = "Numero";
                    a.country = "Republica Dominicana";
                    var passwordHelper = new PasswordHelper();
                    passwordHelper.HashPassword("123456");
                    db.Addresses.Add(a);
                    var user = new User();
                    user.username = "admin";
                    user.password = PWDTK.HashBytesToHexString(passwordHelper.Hash);
                    user.salt = PWDTK.HashBytesToHexString(passwordHelper.Salt);
                    user.bornDate = DateTime.Now;
                    user.createDate = DateTime.Now;
                    user.email = "mail@SIGEC.com";
                    user.status = true;
                    user.gender = "M";
                    user.maritalStatus = "S";
                    user.dni = "00000000000";
                    user.firstName = "admin";
                    user.lastName = "istrador";
                    user.occupation = "Super Admin" ;
                    db.Users.Add(user);
                    user.Address = a;
                    
                    db.SaveChanges();
                }
            }

            var roles = (SimpleRoleProvider)Roles.Provider;
            if (!roles.RoleExists("Admin"))
            {
                roles.CreateRole("Admin");
            }

            if (!roles.GetRolesForUser("admin").Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { "admin" }, new[] { "Admin" });
            }

            ///insertar datos de menús y acciones en la base de datos
            ///para el manejo de permisos.
            //GlobalHelpers.InsertMenusAndActions();

            ///asignar permisos sobre todas las acciones al rol Admin
            using (var db = new SIGECContext())
            {
                var adminRole = db.webpages_Roles.FirstOrDefault(r => r.RoleName == "Admin");
                foreach (SIGEC.Models.Action a in db.Actions)
                {
                    if (!adminRole.Actions.Contains(a))
                        adminRole.Actions.Add(a);
                }
                db.Entry(adminRole).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}