//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DGII_PFD.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    [MetadataType(typeof(PFD_ROLES_METADATA))]
    public partial class PFD_ROLES
    {  }

    class PFD_ROLES_METADATA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal ID { get; set; }
        [DisplayName("Nombre del Rol")]
        [Required(ErrorMessage="{0} es obligatorio")]
        public string NOMBRE { get; set; }
    }
}
