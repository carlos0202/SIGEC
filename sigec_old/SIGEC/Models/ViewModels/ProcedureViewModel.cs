using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGEC.Models.ViewModels
{
    /// <summary>
    /// Clase modelo para los views que manejan
    /// la información de los procedimientos 
    /// en la aplicación.
    /// </summary>
    public class ProcedureViewModel
    {
        /// <summary>
        /// propiedad de clave primaria de la base de datos.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// propiedad para el nombre del procedimiento.
        /// </summary>
        [Required]
        [Display(Name = "lblName", ResourceType = typeof(Language))]
        public string name { get; set; }

        /// <summary>
        /// propiedad para la descripción del procedimiento.
        /// </summary>
        [Display(Name = "lblDescription", ResourceType = typeof(Language))]
        [Required]
        public string description { get; set; }

        /// <summary>
        /// propiedad para el estatus del procedimiento.
        /// </summary>
        [Display(Name = "lblStatus", ResourceType = typeof(Language))]
        public bool status { get; set; }
    }
}