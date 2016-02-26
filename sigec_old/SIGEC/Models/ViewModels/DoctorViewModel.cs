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
    /// la información de los doctores registrados
    /// en la aplicación.
    /// </summary>
    public class DoctorViewModel
    {
        /// <summary>
        /// propiedad de clave primaria del registro del doctor.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// propiedad de la fecha de nacimiento del doctor.
        /// </summary>
        [Required]
        [Display(Name = "lblBornDate", ResourceType = typeof(Language))]
        public System.DateTime bornDate { get; set; }

        /// <summary>
        /// propiedad de la cédula del empleaedo.
        /// </summary>
        [Required]
        [Display(Name = "lblDni", ResourceType = typeof(Language))]
        public string dni { get; set; }

        /// <summary>
        /// propiedad de la dirección electrónica del doctor.
        /// </summary>
        [Required]
        [Email(ErrorMessageResourceName = "lblEmailErr", ErrorMessageResourceType = typeof(Language))]
        [Display(Name = "lblEmail", ResourceType = typeof(Language))]
        public string email { get; set; }

        /// <summary>
        /// propiedad con el(los) nombre(s) del doctor.
        /// </summary>
        [Required]
        [Display(Name = "lblFirstName", ResourceType = typeof(Language))]
        public string firstName { get; set; }

        /// <summary>
        /// propiedad con el(los) apellido(s) del doctor.
        /// </summary>
        [Required]
        [Display(Name = "lblLastName", ResourceType = typeof(Language))]
        public string lastName { get; set; }

        /// <summary>
        /// propiedad del género del doctor.
        /// </summary>
        [Required]
        [Display(Name = "lblGender", ResourceType = typeof(Language))]
        [UIHint("EnumRadioButtonList")]
        public Genders gender { get; set; }

        /// <summary>
        /// propiedad con el estado civil del doctor.
        /// </summary>
        [Required]
        [Display(Name = "lblMaritalStatus", ResourceType = typeof(Language))]
        [UIHint("EnumRadioButtonList")]
        public MaritalStatus maritalStatus { get; set; }

        /// <summary>
        /// propiedad con el nombre de usuario para iniciar
        /// sesión en la aplicación para el doctor.
        /// </summary>
        [Required]
        [Display(Name = "lblUsername", ResourceType = typeof(Language))]
        [System.Web.Mvc.Remote("CheckUsername", "GlobalHelpers", AdditionalFields = "ID")]
        public string username { get; set; }

        /// <summary>
        /// propiedad con el estatus de la cuenta del
        /// doctor.
        /// </summary>
        [Display(Name = "lblStatus", ResourceType = typeof(Language))]
        public bool status { get; set; }

        /// <summary>
        /// propiedad con la dirección física del doctor.
        /// </summary>
        public virtual Address Address { get; set; }

        /// <summary>
        /// propiedad con la contraseña para el inicio de sesión
        /// del doctor.
        /// </summary>
        [Required]
        [StringLength(256, ErrorMessageResourceName = "StringLengthErr", ErrorMessageResourceType = typeof(Language), MinimumLength = 6)]
        [Display(Name = "lblPassword", ResourceType = typeof(Language))]
        public string password { get; set; }

        /// <summary>
        /// propiedad para validar la contraseña introducida
        /// al momento de registro con una confirmación de la
        /// misma.
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
        /// propiedad con los números telefónicos del doctor.
        /// </summary>
        public virtual ICollection<Phone> Phones { get; set; }

        [Required]
        [Display(Name = "lblSpeciality", ResourceType = typeof(Language))]
        public string speciality { get; set; }
    }
}