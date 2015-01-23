using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DGII_PFD.Models
{
    public class FormsViewModel
    {
        public decimal ID { get; set; }
        [Required(ErrorMessage="{0} es obligatorio")]
        [DisplayName("Nombre de Formulario")]
        public string NOMBRE { get; set; }
        [Required(ErrorMessage = "{0} es obligatorio")]
        [DisplayName("Nombre del Procedimiento")]
        [Remote("CheckProcedureName", "Forms", ErrorMessage="Procedimiento no encontrado en la Base de Datos.")]
        public string PROCEDIMIENTO { get; set; }
        [DisplayName("Descripción")]
        public string DESCRIPCION { get; set; }
        [DisplayName("¿Formulario Habilitado?")]
        public bool HABILITADO { get; set; }
        public virtual List<PFD_PARAMETROS> PFD_PARAMETROS { get; set; }
        public short ESTANDARIZADO { get; set; }
    }

    public class ParameterViewModel
    {
        public decimal ID { get; set; }
        public decimal ID_PROCEDIMIENTO { get; set; }
        [Required(ErrorMessage = "{0} es obligatorio")]
        [DisplayName("Nombre del Campo")]
        public string NOMBRE { get; set; }
        [Required(ErrorMessage = "{0} es obligatorio")]
        [DisplayName("Nombre del Parámetro")]
        public string PARAMETRO { get; set; }
        [Required(ErrorMessage = "{0} es obligatorio")]
        [DisplayName("¿Parámetro Obligatorio?")]
        public bool REQUERIDO { get; set; }
        [Required(ErrorMessage = "{0} es obligatorio")]
        [DisplayName("Tipo de Datos")]
        public decimal TIPO { get; set; }
    }
}