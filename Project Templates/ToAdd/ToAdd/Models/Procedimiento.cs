using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGII_PFD.Models
{
    public class Procedimiento
    {
        //public int ID { get; set; }
        public string Nombre { get; set; }
        public string NombreProcedimiento { get; set; }
        public List<Parametro> Parametros { get; set; }
        public int Tipo { get; set; }
    }
}