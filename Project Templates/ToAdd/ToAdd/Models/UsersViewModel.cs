using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGII_PFD.Models
{
    public class UsersViewModel
    {
        [Required(ErrorMessage="{0} es obligatorio")]
        [DisplayName("Usuario")]
        public string UserId { get; set; }
        [Required(ErrorMessage="{0} es obligatorio")]
        [DisplayName("Rol")]
        public int RoleId { get; set; }

        //Propiedades para generar tabla index.
        public decimal Id { get; set; }
        [DisplayName("Dominio Usuario")]
        public string NOMBRE_USUARIO { get; set; }
        [DisplayName("Nombre Completo")]
        public string Nombre { get; set; }
        [DisplayName("Rol Asignado")]
        public string Roles { get; set; }
    }
}