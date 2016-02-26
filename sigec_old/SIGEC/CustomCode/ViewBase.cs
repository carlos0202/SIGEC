using SIGEC.Resources;
using System;
using System.Resources;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace SIGEC.CustomCode
{
    /// <summary>
    /// Clase base utilizada para encapsular genéricamente métodos y propiedades
    /// que serán utilizados en todos los Views de la apliación.
    /// </summary>
    /// <typeparam name="TModel">Clase modelo abstracta del View en cuestión en tiempo de ejecución</typeparam>
    public abstract class ViewBase<TModel> : System.Web.Mvc.WebViewPage<TModel> where TModel : class
    {
        /// <summary>
        /// Manejador de recursos, para los textos desde los archivos de recursos.
        /// </summary>
        private static ResourceManager manager = new ResourceManager(typeof(Language));

        public MenuAuthorize menu = new MenuAuthorize();

        private static SimpleRoleProvider roleProvider = (SimpleRoleProvider)Roles.Provider;

        /// <summary>
        /// Método de extensión para tomar los mensajes de la aplicación desde el archivo de recursos.
        /// </summary>
        /// <param name="var">clave para buscar en el diccionario de mensajes.</param>
        /// <returns>un String con el mensaje localizado para dicha clave.</returns>
        public string _(string var)
        {
            var translatedMsg = manager.GetString(var);
            return string.IsNullOrEmpty(translatedMsg) ? var : translatedMsg;
        }

        /// <summary>
        /// propiedad utilizada para saber si el usuario activo es un paciente.
        /// </summary>
        public bool isPatient
        {
            get
            {
                return roleProvider.IsUserInRole(WebSecurity.CurrentUserName, "Patient");
            }
        }

        /// <summary>
        /// propiedad utilizada para saber si el usuario activo es un empleado.
        /// </summary>
        public bool isEmployee
        {
            get
            {
                return roleProvider.IsUserInRole(WebSecurity.CurrentUserName, "Employee");
            }
        }

        /// <summary>
        /// propiedad utilizada para saber si el usuario activo es un doctor.
        /// </summary>
        public bool isDoctor
        {
            get
            {
                return roleProvider.IsUserInRole(WebSecurity.CurrentUserName, "Doctor");
            }
        }

        /// <summary>
        /// propiedad utilizada para saber si el usuario activo es un administrador.
        /// </summary>
        public bool isAdmin
        {
            get
            {
                return roleProvider.IsUserInRole(WebSecurity.CurrentUserName, "Admin");
            }
        }
    }
}