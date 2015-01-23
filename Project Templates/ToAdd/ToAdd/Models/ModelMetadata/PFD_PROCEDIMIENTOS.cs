using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DGII_PFD.Models
{
    [MetadataType(typeof(PFD_PROCEDIMIENTOS_METADATA))]
    public partial class PFD_PROCEDIMIENTOS
    {
        [NotMapped]
        public List<PFD_PARAMETROS> Parametros { get; set; }

        [NotMapped]
        public bool Enabled
        {
            get { return Convert.ToBoolean(this.HABILITADO); }
            set { this.HABILITADO = Convert.ToDecimal(value); }
        }
    }

    class PFD_PROCEDIMIENTOS_METADATA
    {
        public decimal ID { get; set; }
        [Required(ErrorMessage = "{0} es obligatorio")]
        [DisplayName("Nombre de Formulario")]
        public string NOMBRE { get; set; }
        [Required(ErrorMessage = "{0} es obligatorio")]
        [DisplayName("Nombre del Procedimiento")]
        [Remote("CheckProcedureName", "Forms", ErrorMessage = "Procedimiento no encontrado en la Base de Datos.")]
        public string PROCEDIMIENTO { get; set; }
        [DisplayName("Descripción")]
        public string DESCRIPCION { get; set; }
        [DisplayName("¿Formulario Habilitado?")]
        public bool HABILITADO { get; set; }
    }
}