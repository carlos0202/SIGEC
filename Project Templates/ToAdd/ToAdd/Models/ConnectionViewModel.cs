using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DGII_PFD.Models
{
    public class ConnectionViewModel
    {
        [DisplayName("Nombre de la conexión")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Data Source")]
        [Required]
        public string DataSource { get; set; }
        [DisplayName("User Id")]
        [Required]
        public string UserId { get; set; }
        [DisplayName("Password")]
        [Required]
        public string Password { get; set; }
    }
}