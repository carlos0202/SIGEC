using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGEC.Models.ViewModels
{
    /// <summary>
    /// Clase modelo utilizada para manejar los datos
    /// de las aseguradoras en la aplicación.
    /// </summary>
    public class InsurerViewModel
    {
        /// <summary>
        /// propiedad con la clave primaria de la base de datos.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// propiedad con el nombre de la aseguradora.
        /// </summary>
        [Display(Name = "lblName", ResourceType = typeof(Language))]
        [Required]
        public string name { get; set; }

        /// <summary>
        /// propiedad con el RNC de la aseguradora.
        /// </summary>
        [Display(Name = "lblRNC", ResourceType = typeof(Language))]
        [Required]
        public string RNC { get; set; }

        /// <summary>
        /// propiedad con la referencia de la dirección física de la aseguradora.
        /// </summary>
        public Nullable<int> addressID { get; set; }

        /// <summary>
        /// propiedad con el estatus de la aseguradora.
        /// </summary>
        [Display(Name = "lblStatus", ResourceType = typeof(Language))]
        public bool status { get; set; }

        /// <summary>
        /// propiedad con la dirección de la aseguradora.
        /// </summary>
        public Address Address { get; set; }
    }
}