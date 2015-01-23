using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGII_PFD.Models
{
    

    [MetadataType(typeof(PFD_PARAMETROS_METADATA))]
    public partial class PFD_PARAMETROS
    {
        [NotMapped]
        public object VALOR { get; set; }

        [NotMapped]
        public bool Required { 
            get { return Convert.ToBoolean(this.REQUERIDO); }
            set {} 
        }
    }

    class PFD_PARAMETROS_METADATA
    {
        public string NOMBRE { get; set; }
        public string PARAMETRO { get; set; }
        [DisplayName("Obligatorio?")]
        public short REQUERIDO { get; set; }
        [DisplayName("Tipo de Datos")]
        public decimal TIPO { get; set; }
    }
}
