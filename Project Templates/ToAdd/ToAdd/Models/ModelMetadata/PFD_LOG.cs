using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DGII_PFD.Models
{
    [MetadataType(typeof(PFD_LOG_METADATA))]
    public partial class PFD_LOG { }

    class PFD_LOG_METADATA
    {
        [DisplayName("Usuario")]
        public decimal ID_USUARIO { get; set; }
        [DisplayName("Procedimiento")]
        public decimal ID_PROCEDIMIENTO { get; set; }
        [DisplayName("Código Respuesta")]
        [DisplayFormat(DataFormatString = "{0:#####}")]
        public decimal CODIGO_RESPUESTA { get; set; }
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public System.DateTime FECHA { get; set; }
    }
}
