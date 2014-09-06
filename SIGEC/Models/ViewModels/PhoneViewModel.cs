using SIGEC.CustomCode;
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
    /// la información de los números telefónicos
    /// de las diferentes entidades (Paciente, Empleado, etc.)
    /// en la aplicación.
    /// </summary>
    public class PhoneViewModel
    {
        /// <summary>
        /// propiedad para el número de teléfono.
        /// </summary>
        [Display(Name = "lblPhone", ResourceType = typeof(Language))]
        public string phoneNumber { get; set; }

        /// <summary>
        /// propiedad para el tipo de teléfono.
        /// </summary>
        [Display(Name = "lblPhoneType", ResourceType = typeof(Language))]
        public PhoneTypes type { get; set; }

        /// <summary>
        /// propiedad para las observaciones del número de teléfono.
        /// </summary>
        [Display(Name = "lblPhoneNotes", ResourceType = typeof(Language))]
        public string notes { get; set; }
    }
}