namespace SIGEC.Code52.i18n 
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Clase utilizada para la configuración de internacionalización
    /// de la aplicación, gestionando los aspectos de la cultura prefe-
    /// rida de los usuarios.
    /// </summary>
    public static class CultureHelper {
        // Lista de culturas soportadas por la aplicación
        private static readonly IList<string> _cultures = new List<string> {
                                                                               "es",  // Cultura por defecto
                                                                               "en-US",
                                                                               "en-GB",
                                                                               "es-DO",
                                                                               "es-ES"
                                                                           };
        private static readonly ConcurrentDictionary<string, string> _getImplementedCultureCache = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Metodo para obtener la cultura actual implementada para la aplicación de la lista de culturas soportadas.
        /// </summary>
        /// <param name="name">nombre de la cultura. ej. "en", "es", es-DO", "en-US"</param>
        /// <returns>un valor del tipo String con el nombre de la cultura.</returns>
        public static string GetImplementedCulture(string name) {
          if (string.IsNullOrEmpty(name))
            return GetDefaultCulture(); // Obtiene la cultura por defecto
          if (_getImplementedCultureCache.ContainsKey(name))
            return _getImplementedCultureCache[name];   // Si ya tenemos la cultura en la cache almacenada, retornarla.
          if (_cultures.Any(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
            return CacheCulture(name, name); // La cultura especificada esta soportada, aceptarla.
          
          // Si no obtenemos una cultura exacta, intentar obtener la cultura neutral para dicha cultura usando el prefijo.
          var n = GetNeutralCulture(name);
          foreach (var c in _cultures)
            if (c.StartsWith(n))
              return CacheCulture(name, c);
          return CacheCulture(name, GetDefaultCulture()); // retornar cultura por defecto si no se encontro ninguna
        }

        /// <summary>
        /// Metodo utilizado para almacenar en cache o actualizar los datos de la cultura implementada por la aplicación.
        /// </summary>
        /// <param name="originalName"></param>
        /// <param name="implementedName"></param>
        /// <returns></returns>
        private static string CacheCulture(string originalName, string implementedName)
        {
            _getImplementedCultureCache.AddOrUpdate(originalName, implementedName, (_, __) => implementedName);
            return implementedName;
        }

        /// <summary>
        /// Metodo para obtener la cultura por defecto.
        /// </summary>
        /// <returns>un valor del tipo string con la cultura por defecto.</returns>
        public static string GetDefaultCulture() {
            return _cultures[0]; // Obtener cultura por defecto
        }

        /// <summary>
        /// Metodo para obtener la cultura usada actualmente en la aplicación.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentCulture() {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        /// <summary>
        /// Metodo para obtener la cultura neutral actual utilizada por la aplicación.
        /// Ej. "en-US" -> "en".
        /// </summary>
        /// <returns>un valor del tipo string con la cultura neutral actual utilizada.</returns>
        public static string GetCurrentNeutralCulture() {
            return GetNeutralCulture(Thread.CurrentThread.CurrentCulture.Name);
        }

        /// <summary>
        /// Metodo utilizado para extraer la cultura neutral de una cultura especificada
        /// en el parametro name.
        /// </summary>
        /// <param name="name">cultura de la cual extraer la cultura neutral.</param>
        /// <returns></returns>
        public static string GetNeutralCulture(string name) {
            if (name.Length <= 2)
                return name;

            return name.Substring(0, 2); // leer solo los primeros 2 caracteres, ej. "en", "es"
        }
    }
}