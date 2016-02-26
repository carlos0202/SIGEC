using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGEC.Models.ViewModels
{
    /// <summary>
    /// Clase utilizada como modelo para
    /// los views que editan la información
    /// de un Análisis.
    /// </summary>
    public class AnalysisViewModel
    {
        /// <summary>
        /// propiedad ID (clave primaria bd).
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// propiedad para el nombre del análisis.
        /// </summary>
        [Required]
        [Display(Name = "lblName", ResourceType = typeof(Language))]
        public string name { get; set; }

        /// <summary>
        /// propiedad para la descripción del análisis.
        /// </summary>
        [Display(Name = "lblDescription", ResourceType = typeof(Language))]
        [Required]
        public string description { get; set; }

        /// <summary>
        /// propiedad para el estatus del análisis.
        /// </summary>
        [Display(Name = "lblStatus", ResourceType = typeof(Language))]
        public bool status { get; set; }
    }
}