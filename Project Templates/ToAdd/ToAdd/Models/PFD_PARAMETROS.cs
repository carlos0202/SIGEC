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
    
    public partial class PFD_PARAMETROS
    {
        public decimal ID { get; set; }
        public decimal ID_PROCEDIMIENTO { get; set; }
        public string NOMBRE { get; set; }
        public string PARAMETRO { get; set; }
        public short REQUERIDO { get; set; }
        public decimal TIPO { get; set; }
    
        public virtual PFD_PROCEDIMIENTOS PFD_PROCEDIMIENTOS { get; set; }
    }
}
