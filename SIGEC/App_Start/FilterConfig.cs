using System.Web;
using System.Web.Mvc;

namespace SIGEC
{
    public class FilterConfig
    {
        /// <summary>
        /// Filtros utilizados para el mecanismo de control de acciones.
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //Filtro utilizado para cuando se invoca en producción
            //el view general de errores. ~/Views/Shared/Error.cshtml
            //filters.Add(new HandleErrorAttribute());
        }
    }
}