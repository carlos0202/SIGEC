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

    [MetadataType(typeof(PFD_USUARIOS_METADATA))]
    public partial class PFD_USUARIOS
    {
        [NotMapped]
        public string DisplayName
        {
            get
            {
                return ADUsers.Instance.GetDisplayName(this.NOMBRE_USUARIO);
            }
        }
    }

    class PFD_USUARIOS_METADATA
    {
        [DisplayName("Nombre de Usuario")]
        public string NOMBRE_USUARIO { get; set; }

        public virtual ICollection<PFD_ROLES> PFD_ROLES { get; set; }
        public virtual ICollection<PFD_LOG> PFD_LOG { get; set; }
    }
}