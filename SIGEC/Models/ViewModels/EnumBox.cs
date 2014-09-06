using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SIGEC.Models.ViewModels
{
    /// <summary>
    /// Clase utilizada para generar objetos html
    /// a partir de objetos enumerados.
    /// </summary>
    public class EnumBox
    {
        /// <summary>
        /// Constructor para inicializar los datos
        /// de un objeto del tipo EnumBox.
        /// </summary>
        /// <param name="enumObj">tipo del objeto enumeración.</param>
        /// <param name="selected">valor seleccianado para la enumeración.</param>
        /// <param name="name">nombre para el objeto html resultante.</param>
        public EnumBox(Type enumObj, object selected, string name)
        {
            this.EnumObject = enumObj;
            this.Selected = selected;
            this.name = name;
        }

        /// <summary>
        /// propiedad con el tipo del objeto enumerado.
        /// </summary>
        public Type EnumObject { get; set; }

        /// <summary>
        /// propiedad con el valor seleccionado del objeto
        /// html resultante.
        /// </summary>
        public object Selected { get; set; }

        /// <summary>
        /// propiedad con el nombre del objeto html resultante.
        /// </summary>
        public string name { get; set; } 
    }
}