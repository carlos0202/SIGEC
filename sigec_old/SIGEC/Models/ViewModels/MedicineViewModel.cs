using SIGEC.CustomCode;
using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIGEC.Models.ViewModels
{
    /// <summary>
    /// Clase modelo para los views que manejan
    /// la información de las medicinas registradas
    /// en la aplicación.
    /// </summary>
    public partial class MedicineViewModel
    {
        /// <summary>
        /// propiedad de clave primaria de la base de datos.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// propiedad con el nombre del medicamento.
        /// </summary>
        [Display(Name = "lblName", ResourceType = typeof(Language))]
        public string name { get; set; }

        /// <summary>
        /// propiedad con el tipo de medicamento.
        /// </summary>
        [Display(Name = "lblType", ResourceType = typeof(Language))]
        public string type { get; set; }

        /// <summary>
        /// propiedad con el uso del medicamento.
        /// </summary>
        [Display(Name = "lblUsage", ResourceType = typeof(Language))]
        public string usage { get; set; }

        /// <summary>
        /// propiedad con la dosificación del medicamento.
        /// </summary>
        [Display(Name = "lblDosage", ResourceType = typeof(Language))]
        public string dosage { get; set; }

        /// <summary>
        /// propiedad con el nombre genérico del medicamento.
        /// </summary>
        [Display(Name = "lblGenericName", ResourceType = typeof(Language))]
        public string genericName { get; set; }

        /// <summary>
        /// propiedad con el estatus del medicamento.
        /// </summary>
        [Display(Name = "lblStatus", ResourceType = typeof(Language))]
        public bool status { get; set; }
    }
}
