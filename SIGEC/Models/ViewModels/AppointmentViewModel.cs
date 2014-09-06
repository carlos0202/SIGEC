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
    /// de una cita.
    /// </summary>
    public class AppointmentViewModel
    {
        /// <summary>
        /// propiedad de referencia al doctor con el 
        /// cual se realizará una cita.
        /// </summary>
        [Required]
        [Display(Name = "lblDoctor", ResourceType = typeof(Language))]
        public int doctorID { get; set; }
        /// <summary>
        /// propiedad de referencia al paciente que 
        /// asistirá a la consulta.
        /// </summary>
        [Required]
        [Display(Name = "lblPatient", ResourceType = typeof(Language))]
        public int patientID { get; set; }
        /// <summary>
        /// propiedad que especifica la fecha de
        /// la cita que se está reservando.
        /// </summary>
        [Required]
        [Display(Name = "lblAppointmentDate", ResourceType = typeof(Language))]
        public DateTime appointmentDate { get; set; }
        /// <summary>
        /// propiedad de clave primaria de la base
        /// de datos.
        /// </summary>
        public int ID { get; set; }
    }
}