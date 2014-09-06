using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGEC.Models.ViewModels
{
    /// <summary>
    /// Clase modelo utilizada para la pantalla
    /// de editar los permisos de los diferentes
    /// roles de la aplicáción.
    /// </summary>
    public class AuthorizationsViewModel
    {
        /// <summary>
        /// propiedad que hace referencia al rol
        /// al cual se desea editar los permisos.
        /// </summary>
        [Display(Name = "lblRole", ResourceType = typeof(Language))]
        public int RoleId { get; set; }

        /// <summary>
        /// propiedad que establece las acciones
        /// a las cuales tendrá permiso el rol que
        /// se está editando.
        /// </summary>
        public int[] actions { get; set; }
    }

    /// <summary>
    /// Clase utilizada para mostrar los datos
    /// de los menús y las acciones a editar los
    /// permisos en la vista.
    /// </summary>
    public class AuthorizationsRolePartial
    {
        /// <summary>
        /// propiedad de instancia con los datos
        /// del rol al cual se le editarán los
        /// permisos.
        /// </summary>
        public webpages_Roles Role { get; set; }

        /// <summary>
        /// propiedad 
        /// </summary>
        public int[] SelectedActions { get; set; }

        /// <summary>
        /// propiedad con las acciones de la aplicación.
        /// </summary>
        public List<Action> actions { get; set; }

        /// <summary>
        /// propiedad con los menús principales de la
        /// aplicación.
        /// </summary>
        public List<Menu> menus { get; set; }
    }
}