using DataAnnotationsExtensions;
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
    /// la información de los pacientes registrados
    /// en la aplicación.
    /// </summary>
    public class PatientViewModel
    {
        /// <summary>
        /// propiedad de clave primaria de la base de datos.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// propiedad de la fecha de nacimiento del paciente.
        /// </summary>
        [Required]
        [Display(Name = "lblBornDate", ResourceType = typeof(Language))]
        public System.DateTime bornDate { get; set; }

        /// <summary>
        /// propiedad de la cédula del paciente.
        /// </summary>
        [Display(Name = "lblDni", ResourceType = typeof(Language))]
        public string dni { get; set; }

        /// <summary>
        /// propiedad de la dirección electrónica del paciente.
        /// </summary>
        [Required]
        [Email(ErrorMessageResourceName = "lblEmailErr", ErrorMessageResourceType = typeof(Language))]
        [Display(Name = "lblEmail", ResourceType = typeof(Language))]
        public string email { get; set; }

        /// <summary>
        /// propiedad con el(los) nombre(s) del paciente.
        /// </summary>
        [Required]
        [Display(Name = "lblFirstName", ResourceType = typeof(Language))]
        public string firstName { get; set; }

        /// <summary>
        /// propiedad con el(los) apellido(s) del paciente.
        /// </summary>
        [Required]
        [Display(Name = "lblLastName", ResourceType = typeof(Language))]
        public string lastName { get; set; }

        /// <summary>
        /// propiedad del género del paciente.
        /// </summary>
        [Required]
        [Display(Name = "lblGender", ResourceType = typeof(Language))]
        [UIHint("EnumRadioButtonList")]
        public Genders gender { get; set; }

        /// <summary>
        /// propiedad con el estado civil del paciente.
        /// </summary>
        [Required]
        [Display(Name = "lblMaritalStatus", ResourceType = typeof(Language))]
        [UIHint("EnumRadioButtonList")]
        public MaritalStatus maritalStatus { get; set; }

        /// <summary>
        /// propiedad con el nombre de usuario para iniciar
        /// sesión en la aplicación para el paciente.
        /// </summary>
        [Required]
        [Display(Name = "lblUsername", ResourceType = typeof(Language))]
        [System.Web.Mvc.Remote("CheckUsername", "GlobalHelpers", AdditionalFields="ID")]
        public string username { get; set; }

        /// <summary>
        /// propiedad con el estatus de la cuenta del
        /// paciente.
        /// </summary>
        [Display(Name = "lblStatus", ResourceType = typeof(Language))]
        public bool status { get; set; }


        /// <summary>
        /// propiedad con la ocupación (empleo) del paciente.
        /// </summary>
        [Required]
        [Display(Name = "lblOccupation", ResourceType = typeof(Language))]
        public string occupation { get; set; }

        /// <summary>
        /// propiedad con la dirección física del paciente.
        /// </summary>
        public virtual Address Address { get; set; }

        /// <summary>
        /// propiedad con la contraseña de inicio de sesión del paciente.
        /// </summary>
        [Required]
        [StringLength(256, ErrorMessageResourceName = "StringLengthErr", ErrorMessageResourceType = typeof(Language), MinimumLength = 6)]
        [Display(Name = "lblPassword", ResourceType = typeof(Language))]
        public string password { get; set; }

        /// <summary>
        /// propiedad para validar la contraseña inicial introducida en el registro del paciente.
        /// </summary>
        [Display(Name = "lblConfirmPass", ResourceType = typeof(Language))]
        [Compare("password", ErrorMessageResourceName = "PassConfirmErr", ErrorMessageResourceType = typeof(Language))]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// propiedad para agregar mensajes localizados en las vistas
        /// para el campo de número telefónico.
        /// </summary>
        [Display(Name = "lblPhone", ResourceType = typeof(Language))]
        public string phoneNumber { get; set; }

        /// <summary>
        /// propiedad para agregar mensajes localizados en las vistas
        /// para el campo de tipo de teléfono.
        /// </summary>
        [Display(Name = "lblPhoneType", ResourceType = typeof(Language))]
        public Nullable<PhoneTypes> type { get; set; }

        /// <summary>
        /// propiedad para agregar mensajes localizados en las vistas
        /// para el campo observaciones del número telefónico.
        /// </summary>
        [Display(Name = "lblPhoneNotes", ResourceType = typeof(Language))]
        public string notes { get; set; }

        /// <summary>
        /// propiedad con los números telefónicos del empleado.
        /// </summary>
        public virtual ICollection<Phone> Phones { get; set; }

        /// <summary>
        /// propiedad que almacena la religión preferida del paciente.
        /// </summary>
        [Display(Name = "lblReligion", ResourceType = typeof(Language))]
        public string religion { get; set; }
    }
}